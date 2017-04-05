using Alex.WoWRelogger;
using Alex.WoWRelogger.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Alex.WoWRelogger.Settings
{
	public class WowSettings : INotifyPropertyChanged
	{
		private string _params;

		private string _characterName;

		private string _serverName;

		private string _wowPath;

		private int _wowWindowWidth;

		private int _wowWindowHeight;

		private int _wowWindowX;

		private int _wowWindowY;

		private WowSettings.WowRegion _region;

		private WowSettings.WowFaction _faction;

		private string _proxy;

		private Dictionary<string, string> _paramsDictionary
		{
			get;
			set;
		}

		public string AccountName
		{
			get;
			set;
		}

		public string CharacterName
		{
			get
			{
				return this._characterName;
			}
			set
			{
				this._characterName = value.FixString();
				this.NotifyPropertyChanged("CharacterName");
			}
		}

		public WowSettings.WowFaction Faction
		{
			get
			{
				return this._faction;
			}
			set
			{
				this._faction = value;
				this.NotifyPropertyChanged("Faction");
			}
		}

		public string Login
		{
			get;
			set;
		}

		public string Params
		{
			get
			{
				return this._params ?? "";
			}
			set
			{
				string str;
				this._params = value;
				int num = 0;
				int num1 = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				try
				{
					while (true)
					{
						if (value.Length <= num || value[num] != ' ')
						{
							num2 = value.IndexOf('=', num);
							if (num2 <= 0)
							{
								break;
							}
							num1 = num2 - 1;
							while (value.Length > num1 && value[num1] == ' ')
							{
								num1--;
							}
							string str1 = value.Substring(num, num1 - num + 1);
							num3 = num2 + 1;
							while (value.Length > num3 && value[num3] == ' ')
							{
								num3++;
							}
							if (value[num3] == '\"')
							{
								num3++;
								int num6 = value.IndexOf('\"', num3);
								if (num6 < num3)
								{
									break;
								}
								num4 = num6 - 1;
								str = value.Substring(num3, num4 - num3 + 1);
								num5 = num6 + 1;
							}
							else if (value.Substring(num3, 4) != "true")
							{
								if (value.Substring(num3, 5) != "false")
								{
									break;
								}
								str = "false";
								num5 = num3 + 5;
							}
							else
							{
								str = "true";
								num5 = num3 + 4;
							}
							this._paramsDictionary[str1] = str;
							while (value.Length > num5 && value[num5] == ' ')
							{
								num5++;
							}
							if (value.Length > num5 && value[num5] != ';')
							{
								break;
							}
							num = num5 + 1;
						}
						else
						{
							num++;
						}
					}
				}
				catch (Exception exception)
				{
				}
				this.NotifyPropertyChanged("Params");
			}
		}

		public string Password
		{
			get;
			set;
		}

		public string Proxy
		{
			get
			{
				return this._proxy;
			}
			set
			{
				this._proxy = value;
				this.NotifyPropertyChanged("Proxy");
			}
		}

		public WowSettings.WowRegion Region
		{
			get
			{
				return this._region;
			}
			set
			{
				this._region = value;
				this.NotifyPropertyChanged("Region");
			}
		}

		public string ServerName
		{
			get
			{
				return this._serverName;
			}
			set
			{
				this._serverName = value.FixString();
				this.NotifyPropertyChanged("ServerName");
			}
		}

		public string WowPath
		{
			get
			{
				return this._wowPath;
			}
			set
			{
				this._wowPath = value;
				this.NotifyPropertyChanged("WowPath");
			}
		}

		public int WowWindowHeight
		{
			get
			{
				return this._wowWindowHeight;
			}
			set
			{
				this._wowWindowHeight = value;
				this.NotifyPropertyChanged("WowWindowHeight");
			}
		}

		public int WowWindowWidth
		{
			get
			{
				return this._wowWindowWidth;
			}
			set
			{
				this._wowWindowWidth = value;
				this.NotifyPropertyChanged("WowWindowWidth");
			}
		}

		public int WowWindowX
		{
			get
			{
				return this._wowWindowX;
			}
			set
			{
				this._wowWindowX = value;
				this.NotifyPropertyChanged("WowWindowX");
			}
		}

		public int WowWindowY
		{
			get
			{
				return this._wowWindowY;
			}
			set
			{
				this._wowWindowY = value;
				this.NotifyPropertyChanged("WowWindowY");
			}
		}

		public WowSettings()
		{
			this.Login = "Email@battle.net";
			string str = "";
			string str1 = str;
			this.ServerName = str;
			this.Password = str1;
			this.WowPath = string.Empty;
			this.AccountName = "WoW1";
			this.Region = WowSettings.WowRegion.Auto;
			this._paramsDictionary = new Dictionary<string, string>()
			{
				{ "useUnlocker", "true" },
				{ "useProxy", "true" },
				{ "useInjector", "true" },
				{ "useAddon", "true" }
			};
		}

		public T GetParamValue<T>(string name)
		where T : IConvertible
		{
			bool flag;
			int num;
			double num1;
			T t = default(T);
			TypeCode typeCode = Convert.GetTypeCode(t);
			string empty = string.Empty;
			this._paramsDictionary.TryGetValue(name, out empty);
			if (empty != null)
			{
				if (typeCode == TypeCode.Boolean)
				{
					bool.TryParse(empty, out flag);
					return (T)Convert.ChangeType(flag, typeCode);
				}
				if (typeCode == TypeCode.Int32)
				{
					int.TryParse(empty, out num);
					return (T)Convert.ChangeType(num, typeCode);
				}
				if (typeCode == TypeCode.Double)
				{
					double.TryParse(empty, out num1);
					return (T)Convert.ChangeType(num1, typeCode);
				}
				if (typeCode == TypeCode.Empty)
				{
					if (string.IsNullOrEmpty(empty))
					{
						return t;
					}
					if (empty.Length >= 2 && (empty[0] == '\"' && empty[empty.Length - 1] == '\"' || empty[0] == '\'' && empty[empty.Length - 1] == '\''))
					{
						empty = empty.Substring(1, empty.Length - 2);
					}
					return (T)Convert.ChangeType(empty, TypeCode.String);
				}
			}
			return t;
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

		public static void PrintParams()
		{
			Log.Write("Params:", new object[0]);
			Log.Write("\tuseUnlocker", new object[0]);
			Log.Write("\tuseProxy", new object[0]);
			Log.Write("\taccountId", new object[0]);
			Log.Write("\taccountEmail", new object[0]);
			Log.Write("\tcustomLua", new object[0]);
			Log.Write("\tuseInjector", new object[0]);
			Log.Write("\tuseAddon", new object[0]);
		}

		public WowSettings ShadowCopy()
		{
			return (WowSettings)this.MemberwiseClone();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public enum WowFaction
		{
			Alliance,
			Horde
		}

		public enum WowRegion
		{
			Auto,
			US,
			EU,
			Korea,
			China,
			Taiwan
		}
	}
}