using Alex.WoWRelogger;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Alex.WoWRelogger.Settings
{
	public class ProfileSettings : INotifyPropertyChanged
	{
		private string _profileName;

		private bool _isEnabled;

		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				this._isEnabled = value;
				this.NotifyPropertyChanged("IsEnabled");
			}
		}

		public string ProfileName
		{
			get
			{
				return this._profileName;
			}
			set
			{
				this._profileName = value;
				this.NotifyPropertyChanged("ProfileName");
			}
		}

		public Alex.WoWRelogger.Settings.WowSettings WowSettings
		{
			get;
			set;
		}

		public ProfileSettings()
		{
			this.WowSettings = new Alex.WoWRelogger.Settings.WowSettings();
			this.ProfileName = string.Empty;
			this.IsEnabled = true;
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

		public ProfileSettings ShadowCopy()
		{
			ProfileSettings profileSetting = (ProfileSettings)this.MemberwiseClone();
			profileSetting.WowSettings = this.WowSettings.ShadowCopy();
			return profileSetting;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}