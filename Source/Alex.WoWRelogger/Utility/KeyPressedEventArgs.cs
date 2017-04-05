using System;
using System.Windows.Forms;

namespace Alex.WoWRelogger.Utility
{
	public class KeyPressedEventArgs : EventArgs
	{
		private ModifierKeys _modifier;

		private Keys _key;

		public Keys Key
		{
			get
			{
				return this._key;
			}
		}

		public ModifierKeys Modifier
		{
			get
			{
				return this._modifier;
			}
		}

		internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
		{
			this._modifier = modifier;
			this._key = key;
		}
	}
}