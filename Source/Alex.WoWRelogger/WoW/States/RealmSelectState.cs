using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using System;
using System.Threading;

namespace Alex.WoWRelogger.WoW.States
{
	internal class RealmSelectState : State
	{
		private readonly WowManager _wowManager;

		private string _cancelText;

		private string GlueDialogButton1Text
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("test=\"\";if GlueDialogButton1 ~= nil and GlueDialogButton1:IsVisible() then test=GlueDialogButton1:GetText(); end", "test");
			}
		}

		private string GlueDialogText
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("test='';if GlueDialogText ~= nil and GlueDialogText:IsVisible() then test=GlueDialogText:GetText(); end", "test");
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

		private bool IsRealmListVisible
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("test='';if RealmList ~= nil and RealmList:IsVisible() then test='true'; end", "test") == "true";
			}
		}

		private bool IsRetrivingRealmListVisible
		{
			get
			{
				return this.GlueDialogText.Contains("Retrieving");
			}
		}

		public override bool NeedToRun
		{
			get
			{
				if (this._wowManager.GameProcess == null || this._wowManager.GameProcess.HasExitedSafe() || this._wowManager.StartupSequenceIsComplete || this._wowManager.InGame || this._wowManager.IsConnectiongOrLoading)
				{
					return false;
				}
				return this._wowManager.GlueScreen == GlueScreen.RealmList;
			}
		}

		public override int Priority
		{
			get
			{
				return 600;
			}
			set
			{
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

		public RealmSelectState(WowManager wowManager)
		{
			this._wowManager = wowManager;
		}

		public override void Run()
		{
			this._wowManager.Profile.Status = "Changing realm";
			if (this._wowManager.Throttled)
			{
				return;
			}
			if (this._wowManager.ServerHasQueue)
			{
				string queueStatus = this.QueueStatus;
				this._wowManager.Profile.Status = (string.IsNullOrEmpty(queueStatus) ? queueStatus : "Waiting in server queue");
				this._wowManager.Profile.Log("Waiting in server queue", new object[0]);
				return;
			}
			if (this.IsRetrivingRealmListVisible)
			{
				this._wowManager.LockToken.DoString("if GlueDialogButton1 ~= nil and GlueDialogButton1:IsVisible() then GlueDialogButton1:Click(); end");
				return;
			}
			if (!this.IsRealmListVisible)
			{
				return;
			}
			if (this._wowManager.IsConnectiongOrLoading || this.IsConnecting)
			{
				return;
			}
			this._wowManager.StartupSequenceIsComplete = false;
			this._wowManager.LockToken.DoString(string.Format("C_RealmList.ConnectToRealm(RealmList_GetInfoFromName('{0}'))", this._wowManager.Settings.ServerName));
			Thread.Sleep(5000);
		}
	}
}