using Alex.WoWRelogger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alex.WoWRelogger.WoW
{
	internal class ConfigWtf
	{
		private const string ErrorMsg = "Warning: Possible corrupt \\WTF\\Config.wtf file at line #:{0}./n/tReason: {1}";

		private string _path;

		private readonly WowManager _wowManager;

		private readonly Dictionary<string, string> _settings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

		private const StringComparison Comparer = StringComparison.InvariantCultureIgnoreCase;

		public bool Changed
		{
			get;
			private set;
		}

		public ConfigWtf(WowManager wowManager, string path)
		{
			this._wowManager = wowManager;
			this._path = path;
			this.Load();
		}

		public void DeleteSetting(string settingName)
		{
			if (!this._settings.ContainsKey(settingName))
			{
				return;
			}
			this._settings.Remove(settingName);
			this.Changed = true;
		}

		public void EnsureAccountList(string value)
		{
			string str = string.Concat("!", value);
			if (this._settings.ContainsKey("accountList"))
			{
				string[] strArrays = this._settings["accountList"].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
				if (strArrays.Any<string>((string n) => {
					if (string.Equals(value, n, StringComparison.InvariantCultureIgnoreCase))
					{
						return true;
					}
					return string.Equals(str, n, StringComparison.InvariantCultureIgnoreCase);
				}))
				{
					bool flag = false;
					for (int i = 0; i < (int)strArrays.Length; i++)
					{
						if (strArrays[i].StartsWith("!") && !string.Equals(strArrays[i], str, StringComparison.InvariantCultureIgnoreCase))
						{
							strArrays[i] = strArrays[i].Substring(1);
							flag = true;
						}
						if (string.Equals(strArrays[i], value, StringComparison.InvariantCultureIgnoreCase))
						{
							strArrays[i] = string.Concat("!", strArrays[i]);
							flag = true;
						}
					}
					if (flag)
					{
						this._settings["accountList"] = string.Concat(strArrays.Aggregate<string>((string a, string b) => string.Concat(a, "|", b)), "|");
						this.Changed = true;
					}
					return;
				}
			}
			this._settings["accountList"] = string.Concat(str, "|");
			this.Changed = true;
		}

		public void EnsureValue(string key, string value)
		{
			if (this._settings.ContainsKey(key) && string.Equals(this._settings[key], value, StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}
			this._settings[key] = value;
			this.Changed = true;
		}

		private void Load()
		{
			string[] strArrays = File.ReadAllLines(this._path);
			for (int i = 0; i < (int)strArrays.Length; i++)
			{
				string str = strArrays[i];
				int num = i + 1;
				if (str.StartsWith("SET "))
				{
					string str1 = str.Substring(4, str.IndexOf(' ', 4) - 4);
					int num1 = str.IndexOf('\"') + 1;
					string str2 = str.Substring(num1, str.LastIndexOf('\"') - num1);
					if (!this._settings.ContainsKey(str1))
					{
						this._settings.Add(str1, str2);
					}
					else
					{
						this._wowManager.Profile.Log("Warning: Possible corrupt \\WTF\\Config.wtf file at line #:{0}./n/tReason: {1}", new object[] { num, string.Format("{0} found multiple times", str1) });
					}
				}
				else
				{
					this._wowManager.Profile.Log("Warning: Possible corrupt \\WTF\\Config.wtf file at line #:{0}./n/tReason: {1}", new object[] { num, "Does not start with Set" });
				}
			}
		}

		public void Save()
		{
			string str = Path.Combine(Path.GetDirectoryName(this._path), "Config.wtf.bak");
			if (!File.Exists(str))
			{
				this._wowManager.Profile.Log("Creating backup copy of Config.wtf", new object[0]);
				File.Copy(this._path, str);
			}
			StringBuilder stringBuilder = new StringBuilder(200);
			foreach (KeyValuePair<string, string> _setting in this._settings)
			{
				stringBuilder.Append(string.Format("SET {0} \"{1}\"\n", _setting.Key, _setting.Value));
			}
			File.WriteAllText(this._path, stringBuilder.ToString());
		}
	}
}