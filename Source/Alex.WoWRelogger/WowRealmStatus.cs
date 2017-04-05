using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Alex.WoWRelogger
{
	[DataContract]
	internal class WowRealmStatus
	{
		private readonly object _lockObject = new object();

		private Task _updateTask;

		private const string WowStatusApiBaseUrl = "http://www.battle.net/api/wow/realm/status?realms=";

		private const string USWowStatusApiBaseUrl = "http://us.battle.net/api/wow/realm/status?realms=";

		private const string EuWowStatusApiBaseUrl = "http://eu.battle.net/api/wow/realm/status?realms=";

		private const string KoreaWowStatusApiBaseUrl = "http://kr.battle.net/api/wow/realm/status?realms=";

		private const string TaiwanWowStatusApiBaseUrl = "http://tw.battle.net/api/wow/realm/status?realms=";

		private const string ChinaWowStatusApiBaseUrl = "http://www.battlenet.com.cn/api/wow/realm/status?realms=";

		public WowRealmStatus.WowRealmStatusEntry this[string realm, WowSettings.WowRegion region]
		{
			get
			{
				WowRealmStatus.WowRealmStatusEntry wowRealmStatusEntry;
				lock (this._lockObject)
				{
					wowRealmStatusEntry = this.Realms.FirstOrDefault<WowRealmStatus.WowRealmStatusEntry>((WowRealmStatus.WowRealmStatusEntry r) => {
						if (!r.Name.Equals(realm, StringComparison.InvariantCultureIgnoreCase))
						{
							return false;
						}
						return r.Region == region;
					});
				}
				return wowRealmStatusEntry;
			}
		}

		[DataMember(Name="realms")]
		public List<WowRealmStatus.WowRealmStatusEntry> Realms
		{
			get;
			private set;
		}

		public WowRealmStatus()
		{
			this.Realms = new List<WowRealmStatus.WowRealmStatusEntry>();
		}

		private string BuildWowRealmStatusApiUrl(string regionalUrl, IEnumerable<CharacterProfile> profiles)
		{
			List<string> strs = new List<string>();
			foreach (CharacterProfile profile in profiles)
			{
				string serverName = profile.Settings.WowSettings.ServerName;
				if (strs.Contains(serverName))
				{
					continue;
				}
				strs.Add(serverName);
			}
			string str = "";
			for (int i = 0; i < strs.Count; i++)
			{
				str = string.Concat(str, (i != strs.Count - 1 ? string.Concat(strs[i], ",") : strs[i]));
			}
			return string.Concat(regionalUrl, str);
		}

		private Task<List<WowRealmStatus.WowRealmStatusEntry>> CreateRealmUpdateTask(WowSettings.WowRegion region, IEnumerable<CharacterProfile> profiles)
		{
			return Task.Factory.StartNew<List<WowRealmStatus.WowRealmStatusEntry>>(() => {
				List<WowRealmStatus.WowRealmStatusEntry> realms;
				try
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.BuildWowRealmStatusApiUrl(this.GetUrlForRegion(region), profiles));
					httpWebRequest.GetResponse();
					using (WebResponse response = httpWebRequest.GetResponse())
					{
						using (Stream responseStream = response.GetResponseStream())
						{
							WowRealmStatus wowRealmStatu = (WowRealmStatus)(new DataContractJsonSerializer(typeof(WowRealmStatus))).ReadObject(responseStream);
							foreach (WowRealmStatus.WowRealmStatusEntry realm in wowRealmStatu.Realms)
							{
								realm.Region = region;
							}
							realms = wowRealmStatu.Realms;
						}
					}
				}
				catch (Exception exception)
				{
					Log.Err(exception.ToString(), new object[0]);
					realms = null;
				}
				return realms;
			});
		}

		private string GetKey(string realm, WowSettings.WowRegion region)
		{
			return string.Format("{0}-{1}", realm.ToUpper(), region);
		}

		private string GetUrlForRegion(WowSettings.WowRegion region)
		{
			switch (region)
			{
				case WowSettings.WowRegion.US:
				{
					return "http://us.battle.net/api/wow/realm/status?realms=";
				}
				case WowSettings.WowRegion.EU:
				{
					return "http://eu.battle.net/api/wow/realm/status?realms=";
				}
				case WowSettings.WowRegion.Korea:
				{
					return "http://kr.battle.net/api/wow/realm/status?realms=";
				}
				case WowSettings.WowRegion.China:
				{
					return "http://www.battlenet.com.cn/api/wow/realm/status?realms=";
				}
				case WowSettings.WowRegion.Taiwan:
				{
					return "http://tw.battle.net/api/wow/realm/status?realms=";
				}
			}
			return "http://www.battle.net/api/wow/realm/status?realms=";
		}

		public bool RealmIsOnline(string realm, WowSettings.WowRegion region)
		{
			WowRealmStatus.WowRealmStatusEntry item = this[realm, region];
			if (item == null)
			{
				return false;
			}
			return item.IsOnline;
		}

		public void Update()
		{
			if (this._updateTask == null || this._updateTask.Status == TaskStatus.RanToCompletion)
			{
				this._updateTask = new Task(new Action(this.UpdateWowRealmStatus));
				this._updateTask.Start();
			}
		}

		private void UpdateWowRealmStatus()
		{
			List<Task<List<WowRealmStatus.WowRealmStatusEntry>>> list = (
				from k in HbRelogManager.Settings.CharacterProfiles
				group k by k.Settings.WowSettings.Region into group1
				select this.CreateRealmUpdateTask(group1.Key, group1)).ToList<Task<List<WowRealmStatus.WowRealmStatusEntry>>>();
			Task.WaitAll(list.ToArray());
			lock (this._lockObject)
			{
				this.Realms.Clear();
				foreach (Task<List<WowRealmStatus.WowRealmStatusEntry>> task in list)
				{
					if (task.Result == null)
					{
						continue;
					}
					this.Realms.AddRange(task.Result);
				}
			}
		}

		[DataContract]
		public class WowRealmStatusEntry
		{
			[DataMember(Name="battlegroup")]
			public string Battlegroup
			{
				get;
				private set;
			}

			[DataMember(Name="queue")]
			public bool InQueue
			{
				get;
				private set;
			}

			[DataMember(Name="status")]
			public bool IsOnline
			{
				get;
				private set;
			}

			[DataMember(Name="name")]
			public string Name
			{
				get;
				private set;
			}

			[DataMember(Name="population")]
			public string Population
			{
				get;
				private set;
			}

			public WowSettings.WowRegion Region
			{
				get;
				internal set;
			}

			[DataMember(Name="slug")]
			public string Slug
			{
				get;
				private set;
			}

			[DataMember(Name="type")]
			public string Type
			{
				get;
				private set;
			}

			public WowRealmStatusEntry()
			{
			}
		}
	}
}