using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using System;
using System.Diagnostics;
using System.Threading;

namespace Alex.WoWRelogger.WoW.States
{
	internal class LoginWowState : State
	{
		private readonly WowManager _wowManager;

		private int _priority = 700;

		private string _cancelText;

		private string _okayText;

		private string GlueDialogButton1Text
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("test=\"\";if GlueDialogButton1 ~= nil and GlueDialogButton1:IsVisible() then test=GlueDialogButton1:GetText(); end", "test");
			}
		}

		private string GlueDialogHtmlFormatText
		{
			get
			{
				return this._wowManager.GetLuaObject("HTML_TEXT");
			}
		}

		private string GlueDialogText
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("test='';if GlueDialogText ~= nil and GlueDialogText:IsVisible() then test=GlueDialogText:GetText(); end", "test");
			}
		}

		private bool IncorrectPassword
		{
			get
			{
				return this.GlueDialogText.Contains("We couldn't log you in");
			}
		}

		private bool IsBanned
		{
			get
			{
				if (this.GlueDialogHtmlFormatText.Contains("account has been closed"))
				{
					return true;
				}
				return this.GlueDialogHtmlFormatText.Contains("has been temporarily disabled");
			}
		}

		private bool IsConnecting
		{
			get
			{
				string glueDialogButton1Text = this.GlueDialogButton1Text;
				if (string.IsNullOrEmpty(glueDialogButton1Text))
				{
					return false;
				}
				if (this._cancelText == null)
				{
					this._cancelText = "Cancel";
				}
				return this._cancelText == glueDialogButton1Text;
			}
		}

		private bool IsDisconnected
		{
			get
			{
				return this.GlueDialogHtmlFormatText.Contains("You have been disc");
			}
		}

		private bool IsErrorDialogVisible
		{
			get
			{
				string glueDialogButton1Text = this.GlueDialogButton1Text;
				if (string.IsNullOrEmpty(glueDialogButton1Text))
				{
					return false;
				}
				if (this._okayText == null)
				{
					this._okayText = "Okay";
				}
				return this._okayText == glueDialogButton1Text;
			}
		}

		private bool IsLocked
		{
			get
			{
				return this.GlueDialogText.Contains("Due to suspicious activity");
			}
		}

		private bool IsSuspended
		{
			get
			{
				return this.GlueDialogHtmlFormatText.Contains("has been temporarily suspended");
			}
		}

		public override bool NeedToRun
		{
			get
			{
				if (this._wowManager.GameProcess == null || this._wowManager.GameProcess.HasExitedSafe() || this._wowManager.InGame)
				{
					return false;
				}
				return this._wowManager.GlueScreen == GlueScreen.Login;
			}
		}

		public override int Priority
		{
			get
			{
				return this._priority;
			}
			set
			{
				this._priority = value;
			}
		}

		private string QueueStatus
		{
			get
			{
				string glueDialogText = this.GlueDialogText;
				if (string.IsNullOrEmpty(glueDialogText))
				{
					return string.Empty;
				}
				return glueDialogText.Replace("\n", ". ");
			}
		}

		public LoginWowState(WowManager wowManager)
		{
			this._wowManager = wowManager;
		}

		private bool EnterTextInEditBox(string editBoxName, string text)
		{
			this._wowManager.LockToken.DoString(string.Format("if {0} ~= nil then {0}:SetText('{1}'); end", editBoxName, text));
			return true;
		}

		public override void Run()
		{
			if (this._wowManager.GlueScreen != GlueScreen.Login)
			{
				return;
			}
			this._wowManager.Profile.Log("Running state: LoginWowState", new object[0]);
			this._wowManager.Profile.Status = "Logging in";
			if (this._wowManager.Throttled)
			{
				Alex.WoWRelogger.Utility.Log.Write("Throttled", new object[0]);
				return;
			}
			if (this._wowManager.StalledLogin)
			{
				this._wowManager.Profile.Log("Failed to log into WoW; Restarting", new object[0]);
				this._wowManager.LockToken.ReleaseLock();
				this._wowManager.GameProcess.Kill();
				return;
			}
			if (this._wowManager.IsConnectiongOrLoading || this.IsConnecting)
			{
				return;
			}
			this._wowManager.LockToken.DoString("hooksecurefunc(GlueDialogHTML, 'SetText', function(self, txt) HTML_TEXT = txt end)");
			bool isBanned = this.IsBanned;
			bool isSuspended = this.IsSuspended;
			bool isLocked = this.IsLocked;
			if (this.IsDisconnected)
			{
				Helper.SendBackgroundKey(this._wowManager.GameProcess.MainWindowHandle, '\r', false);
				return;
			}
			if (this.IncorrectPassword)
			{
				Alex.WoWRelogger.Utility.Log.Write("Account {0} has wrong pass !", new object[] { this._wowManager.Settings.Login });
				AccountManager.WrongPass(this._wowManager.Profile.Account);
				this._wowManager.Profile.Stop();
				this._wowManager.Profile.Start();
				return;
			}
			if (isBanned)
			{
				Alex.WoWRelogger.Utility.Log.Write("Account {0} is banned !", new object[] { this._wowManager.Settings.Login });
				AccountManager.Ban(this._wowManager.Profile.Account);
				this._wowManager.Profile.Stop();
				this._wowManager.Profile.Start();
				return;
			}
			if (isLocked)
			{
				Alex.WoWRelogger.Utility.Log.Write("Account {0} is locked !", new object[] { this._wowManager.Settings.Login });
				AccountManager.Lock(this._wowManager.Profile.Account);
				this._wowManager.Profile.Stop();
				this._wowManager.Profile.Start();
				return;
			}
			if (isSuspended)
			{
				Alex.WoWRelogger.Utility.Log.Write("Account {0} is suspended !", new object[] { this._wowManager.Settings.Login });
				AccountManager.Suspend(this._wowManager.Profile.Account);
				this._wowManager.Profile.Stop();
				this._wowManager.Profile.Start();
				return;
			}
			if (this.IsErrorDialogVisible)
			{
				this._wowManager.Profile.Log("Clicking okay on dialog.", new object[0]);
				Helper.SendBackgroundKey(this._wowManager.GameProcess.MainWindowHandle, '\r', false);
				return;
			}
			if (this._wowManager.ServerHasQueue)
			{
				string queueStatus = this.QueueStatus;
				this._wowManager.Profile.Status = (string.IsNullOrEmpty(queueStatus) ? queueStatus : "Waiting in server queue");
				this._wowManager.Profile.Log("Waiting in server queue", new object[0]);
				return;
			}
			if (this._wowManager.IsConnectiongOrLoading || this.IsConnecting)
			{
				this._wowManager.Profile.Log("Connecting...", new object[0]);
				return;
			}
			this._wowManager.LockToken.DoString(string.Format("C_Login.SelectGameAccount('{0}');", this._wowManager.Settings.AccountName));
			Helper.SetWindowText(this._wowManager.GameProcess.MainWindowHandle, string.Concat(new string[] { this._wowManager.Profile.Settings.WowSettings.ServerName, " - ", this._wowManager.innerProxy, " ", this._wowManager.Profile.Settings.WowSettings.Login }));
			this._wowManager.LockToken.DoString(string.Format("C_Login.SelectGameAccount('{0}');", this._wowManager.Settings.AccountName));
			this.EnterTextInEditBox("AccountLogin.UI.AccountEditBox", this._wowManager.Settings.Login);
			this.EnterTextInEditBox("AccountLogin.UI.PasswordEditBox", this._wowManager.Settings.Password);
			this._wowManager.LockToken.DoString("if AccountLogin.UI.AccountEditBox ~= nil and AccountLogin.UI.AccountEditBox:IsVisible() then AccountLogin_Login() end");
			Thread.Sleep(15000);
		}
	}
}