using Alex.WoWRelogger;
using Alex.WoWRelogger.FiniteStateMachine;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Alex.WoWRelogger.WoW.States
{
	internal class CharacterCreationState : State
	{
		private readonly WowManager _wowManager;

		private int _priority = 600;

		private string _cancelText;

		private string GlueDialogButton1Text
		{
			get
			{
				return this._wowManager.LockToken.DoStringWithReturn("test=\"\";if GlueDialogButton1 ~= nil and GlueDialogButton1:IsVisible() then test=GlueDialogButton1:GetText(); end", "test");
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

		public override bool NeedToRun
		{
			get
			{
				if (this._wowManager.GameProcess == null || this._wowManager.GameProcess.HasExitedSafe() || this._wowManager.InGame || this._wowManager.IsConnectiongOrLoading)
				{
					return false;
				}
				return this._wowManager.GlueScreen == GlueScreen.CharCreate;
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

		public CharacterCreationState(WowManager wowManager)
		{
			this._wowManager = wowManager;
		}

		private void ChangeRace()
		{
			this._wowManager.LockToken.DoString(string.Format("SetSelectedRace({0})", (this._wowManager.Settings.Faction == WowSettings.WowFaction.Alliance ? 1 : 9)));
		}

		private bool CheckGlueDialog()
		{
			if (this._wowManager.LockToken.DoStringWithReturn("test='false';if GlueDialog ~= nil and GlueDialog:IsVisible() then test='true'; end", "test") != "true")
			{
				return false;
			}
			Helper.SendBackgroundKey(this._wowManager.GameProcess.MainWindowHandle, '\r', false);
			return true;
		}

		private void CreateCharacter()
		{
			this.ChangeRace();
			string str = this.GenerateName(this._wowManager.Settings.CharacterName);
			this._wowManager.JustCreatedCharacter = str.FixString();
			this._wowManager.LockToken.DoString(string.Concat("CreateCharacter(\"", str, "\")"));
			Helper.SleepUntil(() => this._wowManager.LockToken.DoStringWithReturn("retScroll=''; if CharSelectRealmName and CharSelectRealmName:IsVisible() then retScroll='true' end", "retScroll") == "true", new TimeSpan(0, 0, 10));
		}

		private string GenerateName(string basename)
		{
			if (Helper.IsEnglish(basename))
			{
				char chr = "qwertyuiopasdfghjklzxcvbnm"[HbRelogManager.r.Next("qwertyuiopasdfghjklzxcvbnm".Length)];
				char chr1 = "qwertyuiopasdfghjklzxcvbnm"[HbRelogManager.r.Next("qwertyuiopasdfghjklzxcvbnm".Length)];
				return string.Concat(basename, chr.ToString(), chr1.ToString());
			}
			char chr2 = "йцукенгшщзхъфывапролджэячсмитьбю"[HbRelogManager.r.Next("йцукенгшщзхъфывапролджэячсмитьбю".Length)];
			char chr3 = "йцукенгшщзхъфывапролджэячсмитьбю"[HbRelogManager.r.Next("йцукенгшщзхъфывапролджэячсмитьбю".Length)];
			return string.Concat(basename, chr2.ToString(), chr3.ToString());
		}

		public override void Run()
		{
			if (this._wowManager.Throttled)
			{
				return;
			}
			this._wowManager.StartupSequenceIsComplete = false;
			this._wowManager.Profile.Log("CharacterCreationState is running", new object[0]);
			this._wowManager.Profile.Status = "Creating character";
			if (this._wowManager.IsConnectiongOrLoading || this.IsConnecting)
			{
				return;
			}
			if (this._wowManager.GlueScreen != GlueScreen.CharCreate)
			{
				return;
			}
			this.CreateCharacter();
			Thread.Sleep(2000);
		}
	}
}