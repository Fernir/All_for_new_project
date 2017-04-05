using Alex.WoWRelogger;
using Alex.WoWRelogger.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Alex.WoWRelogger.Settings
{
	internal class GlobalSettings : INotifyPropertyChanged
	{
		private bool _autoAcceptTosEula;

		private Timer _autoSaveTimer;

		private bool _allowTrialAccounts;

		private bool _autoUpdateHB;

		private bool _checkHbResponsiveness;

		private bool _checkWowResponsiveness;

		private bool _checkRealmStatus;

		private string _gameWindowTitle;

		private int _hBDelay;

		private DateTime _lastSaveTimeStamp;

		private int _loginDelay;

		private bool _minimizeHbOnStart;

		private bool _useDarkStyle;

		private bool _setGameWindowTitle;

		private int _wowDelay;

		public readonly static string DefaultSettingsPath;

		private readonly static byte[] Key;

		private readonly static byte[] Iv;

		public bool AllowTrials
		{
			get
			{
				return this._allowTrialAccounts;
			}
			set
			{
				this._allowTrialAccounts = value;
				this.NotifyPropertyChanged("AllowTrials");
			}
		}

		public bool AutoAcceptTosEula
		{
			get
			{
				return this._autoAcceptTosEula;
			}
			set
			{
				this._autoAcceptTosEula = value;
				this.NotifyPropertyChanged("AutoAcceptTosEula");
			}
		}

		public bool AutoCreateAccounts
		{
			get;
			set;
		}

		public bool AutoUpdateHB
		{
			get
			{
				return this._autoUpdateHB;
			}
			set
			{
				this._autoUpdateHB = value;
				this.NotifyPropertyChanged("AutoUpdateHB");
			}
		}

		public ObservableCollection<CharacterProfile> CharacterProfiles
		{
			get;
			set;
		}

		public bool CheckHbResponsiveness
		{
			get
			{
				return this._checkHbResponsiveness;
			}
			set
			{
				this._checkHbResponsiveness = value;
				this.NotifyPropertyChanged("CheckHbResponsiveness");
			}
		}

		public bool CheckRealmStatus
		{
			get
			{
				return this._checkRealmStatus;
			}
			set
			{
				this._checkRealmStatus = value;
				this.NotifyPropertyChanged("CheckRealmStatus");
			}
		}

		public bool CheckWowResponsiveness
		{
			get
			{
				return this._checkWowResponsiveness;
			}
			set
			{
				this._checkWowResponsiveness = value;
				this.NotifyPropertyChanged("CheckWowResponsiveness");
			}
		}

		public uint FocusedWidgetOffset
		{
			get;
			set;
		}

		public uint GameStateOffset
		{
			get;
			set;
		}

		public string GameWindowTitle
		{
			get
			{
				return this._gameWindowTitle;
			}
			set
			{
				this._gameWindowTitle = value;
				this.NotifyPropertyChanged("GameWindowTitle");
			}
		}

		public int HBDelay
		{
			get
			{
				return this._hBDelay;
			}
			set
			{
				this._hBDelay = value;
				this.NotifyPropertyChanged("HBDelay");
			}
		}

		public uint LoadingScreenEnableCountOffset
		{
			get;
			set;
		}

		public int LoginDelay
		{
			get
			{
				return this._loginDelay;
			}
			set
			{
				this._loginDelay = value;
				this.NotifyPropertyChanged("LoginDelay");
			}
		}

		public uint LuaStateOffset
		{
			get;
			set;
		}

		public bool MinimizeHbOnStart
		{
			get
			{
				return this._minimizeHbOnStart;
			}
			set
			{
				this._minimizeHbOnStart = value;
				this.NotifyPropertyChanged("MinimizeHbOnStart");
			}
		}

		public TimeSpan SaveCompleteTimeSpan
		{
			get
			{
				TimeSpan now = DateTime.Now - this._lastSaveTimeStamp;
				if (this._autoSaveTimer != null && now < TimeSpan.FromSeconds(7))
				{
					return TimeSpan.FromSeconds(7) - now;
				}
				if (now >= TimeSpan.FromSeconds(2))
				{
					return TimeSpan.FromSeconds(0);
				}
				return TimeSpan.FromSeconds(2) - now;
			}
		}

		public bool SetGameWindowTitle
		{
			get
			{
				return this._setGameWindowTitle;
			}
			set
			{
				this._setGameWindowTitle = value;
				this.NotifyPropertyChanged("SetGameWindowTitle");
			}
		}

		public string SettingsPath
		{
			get;
			private set;
		}

		public bool UseDarkStyle
		{
			get
			{
				return this._useDarkStyle;
			}
			set
			{
				this._useDarkStyle = value;
				this.NotifyPropertyChanged("UseDarkStyle");
			}
		}

		public string WmrPurse
		{
			get;
			set;
		}

		public int WowDelay
		{
			get
			{
				return this._wowDelay;
			}
			set
			{
				this._wowDelay = value;
				this.NotifyPropertyChanged("WowDelay");
			}
		}

		public string WowVersion
		{
			get;
			set;
		}

		static GlobalSettings()
		{
			GlobalSettings.DefaultSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Alex\\WoWRelogger\\Setting.xml");
			GlobalSettings.Key = new byte[] { 230, 123, 245, 78, 43, 229, 126, 109, 126, 10, 134, 61, 167, 2, 138, 142 };
			GlobalSettings.Iv = new byte[] { 113, 110, 177, 211, 193, 101, 36, 36, 52, 12, 51, 73, 61, 42, 239, 236 };
		}

		private GlobalSettings()
		{
			this.CharacterProfiles = new ObservableCollection<CharacterProfile>();
			this.HBDelay = 3;
			bool flag = true;
			this.UseDarkStyle = true;
			bool flag1 = flag;
			bool flag2 = flag1;
			this.CheckHbResponsiveness = flag1;
			this.AutoUpdateHB = flag2;
			this.SettingsPath = GlobalSettings.DefaultSettingsPath;
		}

		public GlobalSettings Export(string path)
		{
			GlobalSettings observableCollection = (GlobalSettings)this.MemberwiseClone();
			observableCollection.CharacterProfiles = new ObservableCollection<CharacterProfile>();
			foreach (CharacterProfile characterProfile in this.CharacterProfiles)
			{
				CharacterProfile characterProfile1 = characterProfile.ShadowCopy();
				observableCollection.CharacterProfiles.Add(characterProfile1);
			}
			observableCollection.SettingsPath = path;
			return observableCollection;
		}

		private static T GetElementValue<T>(XElement element, T defaultValue)
		{
			if (element == null)
			{
				return defaultValue;
			}
			if ((object)defaultValue is Enum)
			{
				return (T)Enum.Parse(typeof(T), element.Value);
			}
			return (T)Convert.ChangeType(element.Value, typeof(T));
		}

		private static string GetTempSettingsPath(string settingsPath)
		{
			return string.Concat(settingsPath, ".tmp");
		}

		public static GlobalSettings Import(string path)
		{
			return GlobalSettings.Load(path);
		}

		public static GlobalSettings Load(string path = null)
		{
			path = path ?? GlobalSettings.DefaultSettingsPath;
			GlobalSettings globalSetting = new GlobalSettings();
			try
			{
				bool flag = File.Exists(path);
				string tempSettingsPath = GlobalSettings.GetTempSettingsPath(path);
				bool flag1 = (flag ? false : File.Exists(tempSettingsPath));
				if (flag | flag1)
				{
					if (flag1)
					{
						File.Move(tempSettingsPath, path);
					}
					XElement xElement = XElement.Load(path);
					globalSetting.WowVersion = xElement.Element("WowVersion").Value;
					globalSetting.AllowTrials = GlobalSettings.GetElementValue<bool>(xElement.Element("AllowTrials"), false);
					globalSetting.AutoCreateAccounts = GlobalSettings.GetElementValue<bool>(xElement.Element("AutoCreateAccounts"), false);
					globalSetting.WmrPurse = GlobalSettings.GetElementValue<string>(xElement.Element("WmrPurse"), null);
					globalSetting.WowDelay = GlobalSettings.GetElementValue<int>(xElement.Element("WowDelay"), 0);
					globalSetting.HBDelay = GlobalSettings.GetElementValue<int>(xElement.Element("HBDelay"), 10);
					globalSetting.LoginDelay = GlobalSettings.GetElementValue<int>(xElement.Element("LoginDelay"), 3);
					globalSetting.UseDarkStyle = GlobalSettings.GetElementValue<bool>(xElement.Element("UseDarkStyle"), true);
					globalSetting.CheckRealmStatus = GlobalSettings.GetElementValue<bool>(xElement.Element("CheckRealmStatus"), false);
					globalSetting.CheckHbResponsiveness = GlobalSettings.GetElementValue<bool>(xElement.Element("CheckHbResponsiveness"), true);
					globalSetting.CheckWowResponsiveness = GlobalSettings.GetElementValue<bool>(xElement.Element("CheckWowResponsiveness"), true);
					globalSetting.AutoUpdateHB = GlobalSettings.GetElementValue<bool>(xElement.Element("AutoUpdateHB"), true);
					globalSetting.MinimizeHbOnStart = GlobalSettings.GetElementValue<bool>(xElement.Element("MinimizeHbOnStart"), false);
					globalSetting.AutoAcceptTosEula = GlobalSettings.GetElementValue<bool>(xElement.Element("AutoAcceptTosEula"), false);
					globalSetting.SetGameWindowTitle = GlobalSettings.GetElementValue<bool>(xElement.Element("SetGameWindowTitle"), true);
					globalSetting.GameWindowTitle = GlobalSettings.GetElementValue<string>(xElement.Element("GameWindowTitle"), "{name} - {pid}");
					globalSetting.LuaStateOffset = GlobalSettings.GetElementValue<uint>(xElement.Element("LuaStateOffset"), 0);
					foreach (XElement xElement1 in xElement.Element("CharacterProfiles").Elements("CharacterProfile"))
					{
						CharacterProfile characterProfile = new CharacterProfile();
						XElement xElement2 = xElement1.Element("Settings");
						characterProfile.Settings.ProfileName = GlobalSettings.GetElementValue<string>(xElement2.Element("ProfileName"), null);
						characterProfile.Settings.IsEnabled = GlobalSettings.GetElementValue<bool>(xElement2.Element("IsEnabled"), false);
						XElement xElement3 = xElement2.Element("WowSettings");
						if (xElement3 != null)
						{
							characterProfile.Settings.WowSettings.AccountName = GlobalSettings.GetElementValue<string>(xElement3.Element("AcountName"), null);
							characterProfile.Settings.WowSettings.CharacterName = GlobalSettings.GetElementValue<string>(xElement3.Element("CharacterName"), null);
							characterProfile.Settings.WowSettings.Faction = GlobalSettings.GetElementValue<WowSettings.WowFaction>(xElement3.Element("Faction"), WowSettings.WowFaction.Alliance);
							characterProfile.Settings.WowSettings.ServerName = GlobalSettings.GetElementValue<string>(xElement3.Element("ServerName"), null);
							characterProfile.Settings.WowSettings.Region = GlobalSettings.GetElementValue<WowSettings.WowRegion>(xElement3.Element("Region"), WowSettings.WowRegion.Auto);
							characterProfile.Settings.WowSettings.WowPath = GlobalSettings.GetElementValue<string>(xElement3.Element("WowPath"), null);
							characterProfile.Settings.WowSettings.WowWindowWidth = GlobalSettings.GetElementValue<int>(xElement3.Element("WowWindowWidth"), 0);
							characterProfile.Settings.WowSettings.WowWindowHeight = GlobalSettings.GetElementValue<int>(xElement3.Element("WowWindowHeight"), 0);
							characterProfile.Settings.WowSettings.WowWindowX = GlobalSettings.GetElementValue<int>(xElement3.Element("WowWindowX"), 0);
							characterProfile.Settings.WowSettings.WowWindowY = GlobalSettings.GetElementValue<int>(xElement3.Element("WowWindowY"), 0);
							characterProfile.Settings.WowSettings.Params = GlobalSettings.GetElementValue<string>(xElement3.Element("Params"), null);
						}
						globalSetting.CharacterProfiles.Add(characterProfile);
					}
				}
			}
			catch (Exception exception)
			{
				Alex.WoWRelogger.Utility.Log.Err(exception.ToString(), new object[0]);
			}
			return globalSetting;
		}

		private void NotifyPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
			if (HbRelogManager.Settings != null)
			{
				HbRelogManager.Settings.QueueSave();
			}
		}

		private static FileStream ObtainLock(string path, FileAccess access, FileShare share = 0, int maxWaitTimeMs = 500)
		{
			FileStream fileStream;
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (true)
			{
				try
				{
					fileStream = File.Open(path, FileMode.OpenOrCreate, access, share);
					break;
				}
				catch (Exception exception)
				{
					if (stopwatch.ElapsedMilliseconds >= (long)maxWaitTimeMs)
					{
						throw;
					}
				}
				Thread.Sleep(0);
			}
			return fileStream;
		}

		public void QueueSave()
		{
			if (!((DateTime.Now - this._lastSaveTimeStamp) >= TimeSpan.FromSeconds(5)) || this._autoSaveTimer != null)
			{
				if (this._autoSaveTimer != null)
				{
					this._autoSaveTimer.Dispose();
				}
				this._autoSaveTimer = new Timer((object state) => {
					this.Save();
					this._autoSaveTimer.Dispose();
					this._autoSaveTimer = null;
				}, null, 5000, -1);
			}
			else
			{
				this.Save();
			}
			this._lastSaveTimeStamp = DateTime.Now;
		}

		public void Save()
		{
			try
			{
				XElement xElement = new XElement("BotManager");
				xElement.Add(new XElement("AllowTrials", (object)this.AllowTrials));
				xElement.Add(new XElement("AutoCreateAccounts", (object)this.AutoCreateAccounts));
				xElement.Add(new XElement("WmrPurse", this.WmrPurse));
				xElement.Add(new XElement("WowDelay", (object)this.WowDelay));
				xElement.Add(new XElement("HBDelay", (object)this.HBDelay));
				xElement.Add(new XElement("LoginDelay", (object)this.LoginDelay));
				xElement.Add(new XElement("UseDarkStyle", (object)this.UseDarkStyle));
				xElement.Add(new XElement("CheckRealmStatus", (object)this.CheckRealmStatus));
				xElement.Add(new XElement("CheckHbResponsiveness", (object)this.CheckHbResponsiveness));
				xElement.Add(new XElement("CheckWowResponsiveness", (object)this.CheckWowResponsiveness));
				xElement.Add(new XElement("MinimizeHbOnStart", (object)this.MinimizeHbOnStart));
				xElement.Add(new XElement("AutoUpdateHB", (object)this.AutoUpdateHB));
				xElement.Add(new XElement("AutoAcceptTosEula", (object)this.AutoAcceptTosEula));
				xElement.Add(new XElement("SetGameWindowTitle", (object)this.SetGameWindowTitle));
				xElement.Add(new XElement("GameWindowTitle", this.GameWindowTitle));
				xElement.Add(new XElement("WowVersion", this.WowVersion));
				xElement.Add(new XElement("LuaStateOffset", (object)this.LuaStateOffset));
				XElement xElement1 = new XElement("CharacterProfiles");
				foreach (CharacterProfile characterProfile in this.CharacterProfiles)
				{
					XElement xElement2 = new XElement("CharacterProfile");
					XElement xElement3 = new XElement("Settings");
					xElement3.Add(new XElement("ProfileName", characterProfile.Settings.ProfileName));
					xElement3.Add(new XElement("IsEnabled", (object)characterProfile.Settings.IsEnabled));
					XElement xElement4 = new XElement("WowSettings");
					xElement4.Add(new XElement("CharacterName", characterProfile.Settings.WowSettings.CharacterName));
					xElement4.Add(new XElement("Faction", (object)characterProfile.Settings.WowSettings.Faction));
					xElement4.Add(new XElement("ServerName", characterProfile.Settings.WowSettings.ServerName));
					xElement4.Add(new XElement("Region", (object)characterProfile.Settings.WowSettings.Region));
					xElement4.Add(new XElement("WowPath", characterProfile.Settings.WowSettings.WowPath));
					xElement4.Add(new XElement("WowWindowWidth", (object)characterProfile.Settings.WowSettings.WowWindowWidth));
					xElement4.Add(new XElement("WowWindowHeight", (object)characterProfile.Settings.WowSettings.WowWindowHeight));
					xElement4.Add(new XElement("WowWindowX", (object)characterProfile.Settings.WowSettings.WowWindowX));
					xElement4.Add(new XElement("WowWindowY", (object)characterProfile.Settings.WowSettings.WowWindowY));
					xElement4.Add(new XElement("Params", characterProfile.Settings.WowSettings.Params));
					xElement3.Add(xElement4);
					xElement2.Add(xElement3);
					xElement1.Add(xElement2);
				}
				xElement.Add(xElement1);
				string tempSettingsPath = GlobalSettings.GetTempSettingsPath(this.SettingsPath);
				string directoryName = Path.GetDirectoryName(tempSettingsPath);
				if (directoryName != null && !Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				XmlWriterSettings xmlWriterSetting = new XmlWriterSettings()
				{
					OmitXmlDeclaration = true,
					Indent = true
				};
				using (FileStream fileStream = GlobalSettings.ObtainLock(tempSettingsPath, FileAccess.Write, FileShare.Delete, 500))
				{
					using (XmlWriter xmlWriter = XmlWriter.Create(fileStream, xmlWriterSetting))
					{
						xElement.Save(xmlWriter);
					}
					if (File.Exists(this.SettingsPath))
					{
						File.Delete(this.SettingsPath);
					}
					File.Move(tempSettingsPath, this.SettingsPath);
				}
			}
			catch (Exception exception)
			{
				Alex.WoWRelogger.Utility.Log.Err(exception.ToString(), new object[0]);
			}
		}

		public GlobalSettings ShadowCopy()
		{
			GlobalSettings observableCollection = (GlobalSettings)this.MemberwiseClone();
			observableCollection.CharacterProfiles = new ObservableCollection<CharacterProfile>();
			foreach (CharacterProfile characterProfile in this.CharacterProfiles)
			{
				observableCollection.CharacterProfiles.Add(characterProfile.ShadowCopy());
			}
			return observableCollection;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}