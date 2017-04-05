namespace nManager.Helpful
{
    using nManager.Helpful.Win32;
    using nManager.Wow;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public static class Keyboard
    {
        public static void DownKey(IntPtr mainWindowHandle, string key)
        {
            try
            {
                DownKey(mainWindowHandle, (Keys) VkKeyScan(key));
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownKey(IntPtr mainWindowHandle, string key): " + exception, true);
            }
        }

        public static void DownKey(IntPtr mainWindowHandle, Keys key)
        {
            try
            {
                Memory.WowMemory.GameFrameUnLock();
                Native.SendMessage(mainWindowHandle, 0x100, (int) key, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownKey(IntPtr mainWindowHandle, Keys key): " + exception, true);
            }
        }

        public static void PressKey(IntPtr mainWindowHandle, Keys key)
        {
            try
            {
                Memory.WowMemory.GameFrameUnLock();
                Native.SendMessage(mainWindowHandle, 0x100, (int) key, 0);
                Thread.Sleep(100);
                Native.SendMessage(mainWindowHandle, 0x101, (int) key, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("PressKey(IntPtr mainWindowHandle, Keys key): " + exception, true);
            }
        }

        public static void SendText(IntPtr mainWindowHandle, string text)
        {
            try
            {
                try
                {
                    Clipboard.SetText(text);
                }
                catch
                {
                    return;
                }
                Thread.Sleep(10);
                Memory.WowMemory.GameFrameUnLock();
                Native.SendMessage(mainWindowHandle, 0x100, 0xa2, 0);
                Native.SendMessage(mainWindowHandle, 0x100, 0x56, 0);
                Thread.Sleep(10);
                Native.SendMessage(mainWindowHandle, 0x101, 0xa2, 0);
                Native.SendMessage(mainWindowHandle, 0x101, 0x56, 0);
                Thread.Sleep(10);
            }
            catch (Exception exception)
            {
                Logging.WriteError("SendText(IntPtr mainWindowHandle, string text): " + exception, true);
            }
        }

        public static void UpKey(IntPtr mainWindowHandle, string key)
        {
            try
            {
                UpKey(mainWindowHandle, (Keys) VkKeyScan(key));
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpKey(IntPtr mainWindowHandle, string key): " + exception, true);
            }
        }

        public static void UpKey(IntPtr mainWindowHandle, Keys key)
        {
            try
            {
                Memory.WowMemory.GameFrameUnLock();
                Native.SendMessage(mainWindowHandle, 0x101, (int) key, 0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpKey(IntPtr mainWindowHandle, Keys key): " + exception, true);
            }
        }

        private static int VkKeyScan(string key)
        {
            try
            {
                if (key == "1")
                {
                    return 0x31;
                }
                if (key == "2")
                {
                    return 50;
                }
                if (key == "3")
                {
                    return 0x33;
                }
                if (key == "4")
                {
                    return 0x34;
                }
                if (key == "5")
                {
                    return 0x35;
                }
                if (key == "6")
                {
                    return 0x36;
                }
                if (key == "7")
                {
                    return 0x37;
                }
                if (key == "8")
                {
                    return 0x38;
                }
                if (key == "9")
                {
                    return 0x39;
                }
                if (key == "0")
                {
                    return 0x30;
                }
                if (key == ")")
                {
                    return 0xdb;
                }
                if (key == "-")
                {
                    return 0xbd;
                }
                if (key == "=")
                {
                    return 0xbb;
                }
                key = key.ToLower();
                char[] chArray = key.ToCharArray();
                if (key.Length > 1)
                {
                    if (key == "{CTRL}".ToLower())
                    {
                        return 0x11;
                    }
                    if (key == "{ALT}".ToLower())
                    {
                        return 0x40000;
                    }
                    if (key == "{SHIFT}".ToLower())
                    {
                        return 0x10000;
                    }
                    if (key == "{SPACE}".ToLower())
                    {
                        return 0x20;
                    }
                    if (key == "{UP}".ToLower())
                    {
                        return 0x26;
                    }
                    if (key == "{DOWN}".ToLower())
                    {
                        return 40;
                    }
                    if (key == "{LEFT}".ToLower())
                    {
                        return 0x25;
                    }
                    if (key == "{RIGHT}".ToLower())
                    {
                        return 0x27;
                    }
                    if (key == "f1".ToLower())
                    {
                        return 0x70;
                    }
                    if (key == "shift".ToLower())
                    {
                        return 0x10000;
                    }
                    if (key == "f2".ToLower())
                    {
                        return 0x71;
                    }
                    if (key == "space".ToLower())
                    {
                        return 0x20;
                    }
                    if (key == "ctrl".ToLower())
                    {
                        return 0x11;
                    }
                    if (key == "f3".ToLower())
                    {
                        return 0x72;
                    }
                    if (key == "f4".ToLower())
                    {
                        return 0x73;
                    }
                    if (key == "f5".ToLower())
                    {
                        return 0x74;
                    }
                    if (key == "f8".ToLower())
                    {
                        return 0x77;
                    }
                    if (key == "numpad1".ToLower())
                    {
                        return 0x61;
                    }
                    if (key == "f9".ToLower())
                    {
                        return 120;
                    }
                    if (key == "numpad4".ToLower())
                    {
                        return 100;
                    }
                    if (key == "numpad7".ToLower())
                    {
                        return 0x67;
                    }
                    if (key == "f7".ToLower())
                    {
                        return 0x76;
                    }
                    if (key == "alt".ToLower())
                    {
                        return 0x40000;
                    }
                    if (key == "numpadplus".ToLower())
                    {
                        return 0x6b;
                    }
                    if (key == "pagedown".ToLower())
                    {
                        return 0x22;
                    }
                    if (key == "numlock".ToLower())
                    {
                        return 0x90;
                    }
                    if (key == "up".ToLower())
                    {
                        return 0x26;
                    }
                    if (key == "numpad2".ToLower())
                    {
                        return 0x62;
                    }
                    if (key == "numpad5".ToLower())
                    {
                        return 0x65;
                    }
                    if (key == "numpad8".ToLower())
                    {
                        return 0x68;
                    }
                    if (key == "end".ToLower())
                    {
                        return 0x23;
                    }
                    if (key == "tab".ToLower())
                    {
                        return 9;
                    }
                    if (key == "down".ToLower())
                    {
                        return 40;
                    }
                    if (key == "numpaddivide".ToLower())
                    {
                        return 0x6f;
                    }
                    if (key == "backspace".ToLower())
                    {
                        return 8;
                    }
                    if (key == "delete".ToLower())
                    {
                        return 0x2e;
                    }
                    if (key == "numpad0".ToLower())
                    {
                        return 0x60;
                    }
                    if (key == "numpad3".ToLower())
                    {
                        return 0x63;
                    }
                    if (key == "enter".ToLower())
                    {
                        return 13;
                    }
                    if (key == "numpad6".ToLower())
                    {
                        return 0x66;
                    }
                    if (key == "numpad9".ToLower())
                    {
                        return 0x69;
                    }
                    if (key == "f6".ToLower())
                    {
                        return 0x75;
                    }
                    if (key == "pageup".ToLower())
                    {
                        return 0x21;
                    }
                    if (key == "home".ToLower())
                    {
                        return 0x24;
                    }
                    if (key == "escape".ToLower())
                    {
                        return 0x20;
                    }
                    if (key == "numpadminus".ToLower())
                    {
                        return 0x6d;
                    }
                    if (key == "left".ToLower())
                    {
                        return 0x25;
                    }
                    if (key == "f10".ToLower())
                    {
                        return 0x79;
                    }
                    if (key == "f11".ToLower())
                    {
                        return 0x7a;
                    }
                    if (key == "right".ToLower())
                    {
                        return 0x27;
                    }
                    if (key == "f12".ToLower())
                    {
                        return 0x7b;
                    }
                    if (key == "printscreen".ToLower())
                    {
                        return 0x2c;
                    }
                    if (key == "f14".ToLower())
                    {
                        return 0x7d;
                    }
                    if (key == "f15".ToLower())
                    {
                        return 0x7e;
                    }
                    if (key == "f16".ToLower())
                    {
                        return 0x7f;
                    }
                    if (key == "f17".ToLower())
                    {
                        return 0x80;
                    }
                    if (key == "f18".ToLower())
                    {
                        return 0x81;
                    }
                    if (key == "f19".ToLower())
                    {
                        return 130;
                    }
                    if (key == "numpadequals".ToLower())
                    {
                        return 0;
                    }
                    if (key == "-".ToLower())
                    {
                        return 0x6d;
                    }
                }
                return Native.VkKeyScan(chArray[0]);
            }
            catch (Exception exception)
            {
                Logging.WriteError("VkKeyScan(string key): " + exception, true);
            }
            return 0;
        }
    }
}

