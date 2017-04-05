using Alex.WoWRelogger;
using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using HighVoltz.HBRelog.Source;
using HighVoltz.HBRelog.Source.DB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace HighVoltz.HBRelog.Source.WoW
{
	internal class AidaProtocolHandler
	{
		private WowManager _wowManager;

		public AidaProtocolHandler(WowManager manager)
		{
			this._wowManager = manager;
		}

		public void Check()
		{
			string luaObject = this._wowManager.GetLuaObject("AIDA_EVENTS");
			if (luaObject != "")
			{
				int num = 0;
				while (true)
				{
					int num1 = luaObject.IndexOf('\"', num);
					if (num1 < 0)
					{
						break;
					}
					int num2 = num1 + 1;
					while (true)
					{
						num2 = luaObject.IndexOf('\"', num2);
						if (luaObject[num2 - 1] != '%')
						{
							break;
						}
						num2++;
					}
					string str = luaObject.Substring(num1 + 1, num2 - num1 - 1);
					this.ParseEvent(str);
					if (luaObject[num2 + 1] != ';' || luaObject.Length <= num2 + 2)
					{
						break;
					}
					num = num2 + 2;
				}
				this._wowManager.LockToken.DoString("if AIDA_Clear ~= nil then AIDA_Clear() end");
			}
		}

		private void ParseEvent(string e)
		{
			List<string> strs = new List<string>();
			int num = 0;
			int num1 = 0;
			while (true)
			{
				num1 = e.IndexOf(':', num1);
				if (num1 < 0)
				{
					break;
				}
				if (e[num1 - 1] == '%')
				{
					num1++;
				}
				else
				{
					string str = e.Substring(num, num1 - num);
					str = str.Replace("%\"", "\"").Replace("%;", ";").Replace("%:", ":");
					strs.Add(str);
					num = num1 + 1;
					num1 = num;
				}
			}
			string item = strs[0];
			if (item == "WHISPER")
			{
				Message message = new Message()
				{
					AccountName = this._wowManager.Settings.AccountName,
					Character = strs[1],
					Date = DateTime.Now,
					Email = this._wowManager.Settings.Login,
					Server = this._wowManager.Settings.ServerName,
					Text = strs[2]
				};
				AccountManager.AddMessage(message);
				Alex.WoWRelogger.Utility.Log.Write("{0}: {1}", new object[] { message.Character, message.Text });
			}
			if (item == "TRIAL_ACCOUNT" && !this._wowManager.Profile.Account.IsTrial)
			{
				AccountManager.Trial(this._wowManager.Profile.Account, false);
				Alex.WoWRelogger.Utility.Log.Write("Account {0} is trial !", new object[] { this._wowManager.Settings.Login });
				if (!HbRelogManager.Settings.AllowTrials)
				{
					this._wowManager.Profile.Stop();
					this._wowManager.Profile.Start();
				}
			}
			if (item == "MUTED_ACCOUNT")
			{
				AccountManager.Mute(this._wowManager.Profile.Account);
				Alex.WoWRelogger.Utility.Log.Write("Account {0} is muted !", new object[] { this._wowManager.Settings.Login });
				this._wowManager.Profile.Stop();
				this._wowManager.Profile.Start();
			}
			if (item == "AFK_MODE")
			{
				Helper.SendBackgroundKey(this._wowManager.GameProcess.MainWindowHandle, 'p', false);
				Thread.Sleep(100);
				Helper.SendBackgroundKey(this._wowManager.GameProcess.MainWindowHandle, 'p', false);
			}
		}
	}
}