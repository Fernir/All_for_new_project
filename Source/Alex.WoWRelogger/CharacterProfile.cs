using Alex.WoWRelogger.Settings;
using Alex.WoWRelogger.Utility;
using Alex.WoWRelogger.WoW;
using HighVoltz.HBRelog.Source.DB;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;

namespace Alex.WoWRelogger
{
	internal class CharacterProfile : INotifyPropertyChanged
	{
		public Random Rand;

		private string _status;

		private string _tooltip;

		private string _taskTooltip;

		private string _botInfoTooltip;

		private string _lastLog;

		public HighVoltz.HBRelog.Source.DB.Account Account
		{
			get;
			set;
		}

		public string BotInfoTooltip
		{
			get
			{
				return this._botInfoTooltip;
			}
			set
			{
				if (value != this._botInfoTooltip)
				{
					this._botInfoTooltip = value;
					this.UpdateTooltip();
				}
			}
		}

		public int Id
		{
			get;
			set;
		}

		public bool IsPaused
		{
			get;
			private set;
		}

		public bool IsRunning
		{
			get;
			private set;
		}

		public bool NeedToStop
		{
			get;
			set;
		}

		public ProfileSettings Settings
		{
			get;
			set;
		}

		public bool StartupSequenceIsComplete
		{
			get;
			set;
		}

		public string Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
				this.NotifyPropertyChanged("Status");
			}
		}

		public string TaskTooltip
		{
			get
			{
				return this._taskTooltip;
			}
			set
			{
				if (value != this._taskTooltip)
				{
					this._taskTooltip = value;
					this.UpdateTooltip();
				}
			}
		}

		public string Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					this.NotifyPropertyChanged("Tooltip");
				}
			}
		}

		public Alex.WoWRelogger.WoW.WowManager WowManager
		{
			get; private set;
        }

		public CharacterProfile()
		{
		    bool flag = false;
			this.IsRunning = false;
			this.StartupSequenceIsComplete = flag;
			this.Settings = new ProfileSettings();
			Guid guid = Guid.NewGuid();
			this.Rand = new Random(guid.GetHashCode());
			this.WowManager = new Alex.WoWRelogger.WoW.WowManager(this);
			this.Id = this.Rand.Next();
		}

		public void Err(string format, params object[] args)
		{
			Utility.Log.Write((HbRelogManager.Settings.UseDarkStyle ? Colors.LightBlue : Colors.DarkSlateBlue), string.Concat(this.Settings.ProfileName, ": "), Colors.Red, format, args);
		}

		public void Log(string format, params object[] args)
		{
			string str = string.Format(format, args);
			if (str == this._lastLog)
			{
				return;
			}
			this._lastLog = str;
			if (HbRelogManager.Settings.UseDarkStyle)
			{
                Utility.Log.Write(Colors.LightBlue, string.Concat(this.Settings.ProfileName, ": "), Colors.LightGreen, "{0}", new object[] { str });
				return;
			}
            Utility.Log.Write(Colors.DarkSlateBlue, string.Concat(this.Settings.ProfileName, ": "), Colors.DarkGreen, "{0}", new object[] { str });
		}

		private void NotifyPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		public void Pause()
		{
			this.Status = "Paused";
			this.IsPaused = true;
		}

		public void Pulse()
		{
			if (this.IsRunning && !this.IsPaused)
			{
				this.WowManager.Pulse();
			}
		}

		public CharacterProfile ShadowCopy()
		{
			CharacterProfile random = (CharacterProfile)this.MemberwiseClone();
			random.Settings = this.Settings.ShadowCopy();
			Guid guid = Guid.NewGuid();
			random.Rand = new Random(guid.GetHashCode());
			random.Id = random.Rand.Next();
			return random;
		}

		public void Start()
		{
			if (!this.IsRunning || this.IsPaused)
			{
				this.Status = "Running";
				this.WowManager.SetSettings(this.Settings.WowSettings);
				this.WowManager.Start();
				this.IsRunning = true;
				this.IsPaused = false;
			}
		}

		public void Stop()
		{
			this.Status = "Stopped";
			this.Status = "Stopped";
			this.StartupSequenceIsComplete = false;
			this.WowManager.Stop();
			this.IsRunning = false;
			this.IsPaused = false;
		}

		private void UpdateTooltip()
		{
			object obj;
			if (!string.IsNullOrEmpty(this.TaskTooltip))
			{
				obj = string.Concat(this.TaskTooltip, "\n");
			}
			else
			{
				obj = null;
			}
			this.Tooltip = string.Format("{0}{1}", obj, this.BotInfoTooltip);
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}