using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Alex.WoWRelogger.Utility
{
	internal sealed class KeyboardHook : IDisposable
	{
		private KeyboardHook.Window _window = new KeyboardHook.Window();

		private int _currentId;

		public KeyboardHook()
		{
			this._window.KeyPressed += new EventHandler<KeyPressedEventArgs>((object sender, KeyPressedEventArgs args) => {
				if (this.KeyPressed != null)
				{
					this.KeyPressed(this, args);
				}
			});
		}

		public void Dispose()
		{
			for (int i = this._currentId; i > 0; i--)
			{
				KeyboardHook.UnregisterHotKey(this._window.Handle, i);
			}
			this._window.Dispose();
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

		public void RegisterHotKey(ModifierKeys modifier, Keys key)
		{
			this._currentId = this._currentId + 1;
			KeyboardHook.UnregisterHotKey(this._window.Handle, this._currentId);
			if (!KeyboardHook.RegisterHotKey(this._window.Handle, this._currentId, (uint)modifier, (uint)key))
			{
				throw new InvalidOperationException("Couldn’t register the hot key.");
			}
		}

		[DllImport("user32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		public event EventHandler<KeyPressedEventArgs> KeyPressed;

		private class Window : NativeWindow, IDisposable
		{
			private static int WM_HOTKEY;

			static Window()
			{
				KeyboardHook.Window.WM_HOTKEY = 786;
			}

			public Window()
			{
				this.CreateHandle(new CreateParams());
			}

			public void Dispose()
			{
				this.DestroyHandle();
			}

			protected override void WndProc(ref Message m)
			{
				base.WndProc(ref m);
				if (m.Msg == KeyboardHook.Window.WM_HOTKEY)
				{
					Keys lParam = (Keys)((int)m.LParam >> 16 & 65535);
					ModifierKeys modifierKey = (ModifierKeys)((int)m.LParam & 65535);
					if (this.KeyPressed != null)
					{
						this.KeyPressed(this, new KeyPressedEventArgs(modifierKey, lParam));
					}
				}
			}

			public event EventHandler<KeyPressedEventArgs> KeyPressed;
		}
	}
}