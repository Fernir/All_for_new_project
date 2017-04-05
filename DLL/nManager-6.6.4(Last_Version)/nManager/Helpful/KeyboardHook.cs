namespace nManager.Helpful
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public sealed class KeyboardHook : IDisposable
    {
        private readonly Isiebe _window;
        private int _xecainoCuaqa;

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public KeyboardHook()
        {
            EventHandler<KeyPressedEventArgs> xeoxuIpecDapanik = null;
            this._window = new Isiebe();
            try
            {
                if (xeoxuIpecDapanik == null)
                {
                    xeoxuIpecDapanik = delegate (object sender, KeyPressedEventArgs args) {
                        if (this._exuewuleovuajeGonaesu != null)
                        {
                            this._exuewuleovuajeGonaesu(this, args);
                        }
                    };
                }
                this._window.Etoulul(xeoxuIpecDapanik);
            }
            catch (Exception exception)
            {
                Logging.WriteError("KeyboardHook(): " + exception, true);
            }
        }

        public void Dispose()
        {
            try
            {
                for (int i = this._xecainoCuaqa; i > 0; i--)
                {
                    UnregisterHotKey(this._window.Handle, i);
                }
                this._window.Dispose();
            }
            catch (Exception exception)
            {
                Logging.WriteError("KeyboardHook > Dispose(): " + exception, true);
            }
        }

        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            try
            {
                this._xecainoCuaqa++;
                RegisterHotKey(this._window.Handle, this._xecainoCuaqa, (uint) modifier, (uint) key);
            }
            catch (Exception exception)
            {
                Logging.WriteError("RegisterHotKey(ModifierKeys modifier, Keys key): " + exception, true);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private sealed class Isiebe : NativeWindow, IDisposable
        {
            private const int _eheikei = 0x312;

            public event EventHandler<KeyboardHook.KeyPressedEventArgs> _exuewuleovuajeGonaesu;

            public Isiebe()
            {
                try
                {
                    this.CreateHandle(new CreateParams());
                }
                catch (Exception exception)
                {
                    Logging.WriteError("KeyboardHook > Window > Window(): " + exception, true);
                }
            }

            public void Dispose()
            {
                try
                {
                    this.DestroyHandle();
                }
                catch (Exception exception)
                {
                    Logging.WriteError("KeyboardHook > Dispose(): " + exception, true);
                }
            }

            protected override void WndProc(ref Message m)
            {
                try
                {
                    base.WndProc(ref m);
                    if (m.Msg == 0x312)
                    {
                        Keys key = ((Keys) (((int) m.LParam) >> 0x10)) & Keys.KeyCode;
                        KeyboardHook.ModifierKeys modifier = ((KeyboardHook.ModifierKeys) ((int) m.LParam)) & ((KeyboardHook.ModifierKeys) 0xffff);
                        if (this._exuewuleovuajeGonaesu != null)
                        {
                            this._exuewuleovuajeGonaesu(this, new KeyboardHook.KeyPressedEventArgs(modifier, key));
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WndProc(ref Message m): " + exception, true);
                }
            }
        }

        public class KeyPressedEventArgs : EventArgs
        {
            private readonly Keys _key;
            private readonly KeyboardHook.ModifierKeys _modifier;

            internal KeyPressedEventArgs(KeyboardHook.ModifierKeys modifier, Keys key)
            {
                try
                {
                    this._modifier = modifier;
                    this._key = key;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("KeyPressedEventArgs(ModifierKeys modifier, Keys key): " + exception, true);
                }
            }

            public Keys Key
            {
                get
                {
                    try
                    {
                        return this._key;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("Keys Key: " + exception, true);
                    }
                    return Keys.None;
                }
            }

            public KeyboardHook.ModifierKeys Modifier
            {
                get
                {
                    try
                    {
                        return this._modifier;
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ModifierKeys Modifier: " + exception, true);
                    }
                    return KeyboardHook.ModifierKeys.Alt;
                }
            }
        }

        [Flags]
        public enum ModifierKeys : uint
        {
            Alt = 1,
            Control = 2,
            Shift = 4,
            Win = 8
        }
    }
}

