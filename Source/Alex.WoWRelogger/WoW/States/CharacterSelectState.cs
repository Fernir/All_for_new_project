using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using HighVoltz.HBRelog.Source.DB;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Alex.WoWRelogger.WoW.States
{
	internal class CharacterSelectState : State
	{
		private int _priority = 500;

		private readonly Stopwatch _realmChangeSw = new Stopwatch();

		private readonly WowManager _wowManager;

		private const string HAS_MAIL = "This character has mail";

		private string CurrentRealmName
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("retServerName = CharSelectRealmName:GetText()", "retServerName");
			}
		}

		private string GlueDialogText
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("test='';if GlueDialogText ~= nil and GlueDialogText:IsVisible() then test=GlueDialogText:GetText(); end", "test");
			}
		}

		private bool HasMail
		{
			get
			{
				string glueDialogText = this.GlueDialogText;
				if (string.IsNullOrEmpty(glueDialogText))
				{
					return false;
				}
				return glueDialogText.Contains("This character has mail");
			}
		}

		private bool IsWorldServerDown
		{
			get
			{
				return this.GlueDialogText.Contains("World server is down");
			}
		}

		public override bool NeedToRun
		{
			get
			{
				if (this._wowManager.GameProcess == null || this._wowManager.GameProcess.HasExitedSafe() || this._wowManager.InGame || this._wowManager.IsConnectiongOrLoading)
				{
					return false;
				}
				return this._wowManager.GlueScreen == GlueScreen.CharSelect;
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

		private bool ShouldChangeRealm
		{
			get
			{
				string currentRealmName = this.CurrentRealmName;
				if (string.IsNullOrEmpty(currentRealmName))
				{
					return false;
				}
				this._wowManager.Profile.Log("Realmname: {0}", new object[] { currentRealmName });
				return !currentRealmName.Contains(this._wowManager.Settings.ServerName);
			}
		}

		public CharacterSelectState(WowManager wowManager)
		{
			this._wowManager = wowManager;
		}

		private void ChangeRealm()
		{
			this._wowManager.LockToken.DoString("CharSelectChangeRealmButton:Click()");
			this._wowManager.Profile.Log("Changing server.", new object[0]);
		}

		private void CheckGlueDialog()
		{
			this._wowManager.LockToken.DoString("if GlueDialog ~= nil and GlueDialog:IsVisible() then GlueDialog:Hide(); end");
		}

		private void ClickPlayTrial()
		{
			this._wowManager.LockToken.DoString("PromotionFrame_Hide()");
		}

		public void DeleteCharacters()
		{
			string str = "for i=GetNumCharacters(), 1, -1  do \r\n                    local aCharname = GetCharacterInfo(i)\r\n                    if string.sub(aCharname,1,string.len('{0}'))=='{0}' and aCharname ~= '{1}' then \r\n                        retDelete='true'\r\n                        DeleteCharacter(GetCharIDFromIndex(i))\r\n                        return;\r\n                    end\r\n                  end\r\n                  retDelete=''\r\n                ";
			this._wowManager.LockToken.DoString(string.Format(str, this._wowManager.Settings.CharacterName, this._wowManager.JustCreatedCharacter));
		}

		private void HandleCharacterSelect()
		{
			string str = (this._wowManager.Settings.GetParamValue<bool>("useAddon") ? "true" : "false");
			string str1 = " SetAddonVersionCheck(false); AddonList_DisableOutOfDate();\r\n               CanEnterWorld = 'false'\r\n               for i=1, GetNumCharacters() do \r\n                    if GetCharacterInfo(i)=='{0}' then \r\n\r\n                        CharacterSelect_SelectCharacter(i)\r\n                        if {1} then AddonList_EnableAll() else AddonList_DisableAll() end \r\n                        SaveAddOns()                   \r\n                        CanEnterWorld = 'true'\r\n                        return\r\n                    end\r\n               end\r\n               CharSelectCreateCharacterButton:Click() \r\n                ";
			this._wowManager.Profile.Log("JustCreatedCharacter = {0}", new object[] { this._wowManager.JustCreatedCharacter });
			if (this._wowManager.LockToken.DoStringWithReturn(string.Format(str1, this._wowManager.JustCreatedCharacter.FixString(), str), "CanEnterWorld") == "true")
			{
				Helper.SendBackgroundKey(this._wowManager.GameProcess.MainWindowHandle, '\r', false);
				this._wowManager.Profile.Status = "Loading";
			}
		}

		private void HandlePressCreateButton()
		{
			this._wowManager.LockToken.DoString("CharSelectCreateCharacterButton:Click()");
		}

		private bool IsExpired()
		{
			return this._wowManager.LockToken.DoStringWithReturn("test='false';if IsVeteranTrialAccount() then test='true'; end", "test") == "true";
		}

		private bool IsTrial()
		{
			return this._wowManager.LockToken.DoStringWithReturn("test='false';if PromotionFrame ~= nil and PromotionFrame:IsVisible() then test='true'; end", "test") == "true";
		}

		public override void Run()
		{
			Thread.Sleep(2000);
			this._wowManager.StartupSequenceIsComplete = false;
			this._wowManager.Profile.Log("Running state: CharacterSelectState", new object[0]);
			this._wowManager.Profile.Status = "Selecting character";
			if (!this._wowManager.Throttled)
			{
				bool flag = false;
				if (this.HasMail)
				{
					flag = true;
				}
				this._wowManager.Profile.Account.RegIp = "";
				AccountManager.UpdateAccount(this._wowManager.Profile.Account);
				this.CheckGlueDialog();
				if (this.IsExpired())
				{
					Alex.WoWRelogger.Utility.Log.Write("Account {0} is expired !", new object[] { this._wowManager.Settings.Login });
					AccountManager.Expired(this._wowManager.Profile.Account);
					if (!HbRelogManager.Settings.AllowTrials)
					{
						this._wowManager.Profile.Stop();
						this._wowManager.Profile.Start();
						return;
					}
				}
				if (this.IsTrial())
				{
					this.ClickPlayTrial();
					AccountManager.Trial(this._wowManager.Profile.Account, !HbRelogManager.Settings.AllowTrials);
					if (!HbRelogManager.Settings.AllowTrials)
					{
						this._wowManager.Profile.Stop();
						this._wowManager.Profile.Start();
						return;
					}
				}
				if (this.ShouldChangeRealm)
				{
					this.ChangeRealm();
					Helper.SleepUntil(() => this._wowManager.LockToken.DoStringWithReturn("retScroll=''; if RealmListScrollFrameScrollBarScrollUpButton and RealmListScrollFrameScrollBarScrollUpButton:IsVisible() then retScroll='true' end", "retScroll") == "true", new TimeSpan(0, 0, 10));
					return;
				}
				if (!flag)
				{
					this.DeleteCharacters();
					if (this._wowManager.GetLuaObject("retDelete") == "true")
					{
						return;
					}
				}
				if (this.HasMail)
				{
					this.CheckGlueDialog();
					Thread.Sleep(1000);
				}
				this._wowManager.NeedGlueScreenUpdate = true;
				if (this._wowManager.GlueScreen != GlueScreen.CharSelect)
				{
					return;
				}
				this.HandleCharacterSelect();
			}
		}
	}
}