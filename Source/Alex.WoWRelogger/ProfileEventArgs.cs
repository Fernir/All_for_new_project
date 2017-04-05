using System;
using System.Runtime.CompilerServices;

namespace Alex.WoWRelogger
{
	internal class ProfileEventArgs : EventArgs
	{
		public CharacterProfile Profile
		{
			get;
			set;
		}

		public ProfileEventArgs(CharacterProfile profile)
		{
			this.Profile = profile;
		}
	}
}