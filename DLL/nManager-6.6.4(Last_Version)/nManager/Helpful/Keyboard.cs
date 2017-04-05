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
                DownKey(mainWindowHandle, (Keys) LaomaEtigou(key));
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

        private static int LaomaEtigou(string xamifouxekoaToufal)
        {
            try
            {
                if (xamifouxekoaToufal == "1")
                {
                    return 0x31;
                }
                if (xamifouxekoaToufal == "2")
                {
                    return 50;
                }
                if (xamifouxekoaToufal == "3")
                {
                    return 0x33;
                }
                if (xamifouxekoaToufal == "4")
                {
                    return 0x34;
                }
                if (xamifouxekoaToufal == "5")
                {
                    return 0x35;
                }
                if (xamifouxekoaToufal == "6")
                {
                    return 0x36;
                }
                if (xamifouxekoaToufal == "7")
                {
                    return 0x37;
                }
                if (xamifouxekoaToufal == "8")
                {
                    return 0x38;
                }
                if (xamifouxekoaToufal == "9")
                {
                    return 0x39;
                }
                if (xamifouxekoaToufal == "0")
                {
                    return 0x30;
                }
                if (xamifouxekoaToufal == ")")
                {
                    return 0xdb;
                }
                if (xamifouxekoaToufal == "-")
                {
                    return 0xbd;
                }
                if (xamifouxekoaToufal == "=")
                {
                    return 0xbb;
                }
                xamifouxekoaToufal = xamifouxekoaToufal.ToLower();
                char[] chArray = xamifouxekoaToufal.ToCharArray();
                if (xamifouxekoaToufal.Length > 1)
                {
                    if (xamifouxekoaToufal == "{CTRL}".ToLower())
                    {
                        return 0x11;
                    }
                    if (xamifouxekoaToufal == "{ALT}".ToLower())
                    {
                        return 0x40000;
                    }
                    if (xamifouxekoaToufal == "{SHIFT}".ToLower())
                    {
                        return 0x10000;
                    }
                    if (xamifouxekoaToufal == "{SPACE}".ToLower())
                    {
                        return 0x20;
                    }
                    if (xamifouxekoaToufal == "{UP}".ToLower())
                    {
                        return 0x26;
                    }
                    if (xamifouxekoaToufal == "{DOWN}".ToLower())
                    {
                        return 40;
                    }
                    if (xamifouxekoaToufal == "{LEFT}".ToLower())
                    {
                        return 0x25;
                    }
                    if (xamifouxekoaToufal == "{RIGHT}".ToLower())
                    {
                        return 0x27;
                    }
                    if (xamifouxekoaToufal == "f1".ToLower())
                    {
                        return 0x70;
                    }
                    if (xamifouxekoaToufal == "shift".ToLower())
                    {
                        return 0x10000;
                    }
                    if (xamifouxekoaToufal == "f2".ToLower())
                    {
                        return 0x71;
                    }
                    if (xamifouxekoaToufal == "space".ToLower())
                    {
                        return 0x20;
                    }
                    if (xamifouxekoaToufal == "ctrl".ToLower())
                    {
                        return 0x11;
                    }
                    if (xamifouxekoaToufal == "f3".ToLower())
                    {
                        return 0x72;
                    }
                    if (xamifouxekoaToufal == "f4".ToLower())
                    {
                        return 0x73;
                    }
                    if (xamifouxekoaToufal == "f5".ToLower())
                    {
                        return 0x74;
                    }
                    if (xamifouxekoaToufal == "f8".ToLower())
                    {
                        return 0x77;
                    }
                    if (xamifouxekoaToufal == "numpad1".ToLower())
                    {
                        return 0x61;
                    }
                    if (xamifouxekoaToufal == "f9".ToLower())
                    {
                        return 120;
                    }
                    if (xamifouxekoaToufal == "numpad4".ToLower())
                    {
                        return 100;
                    }
                    if (xamifouxekoaToufal == "numpad7".ToLower())
                    {
                        return 0x67;
                    }
                    if (xamifouxekoaToufal == "f7".ToLower())
                    {
                        return 0x76;
                    }
                    if (xamifouxekoaToufal == "alt".ToLower())
                    {
                        return 0x40000;
                    }
                    if (xamifouxekoaToufal == "numpadplus".ToLower())
                    {
                        return 0x6b;
                    }
                    if (xamifouxekoaToufal == "pagedown".ToLower())
                    {
                        return 0x22;
                    }
                    if (xamifouxekoaToufal == "numlock".ToLower())
                    {
                        return 0x90;
                    }
                    if (xamifouxekoaToufal == "up".ToLower())
                    {
                        return 0x26;
                    }
                    if (xamifouxekoaToufal == "numpad2".ToLower())
                    {
                        return 0x62;
                    }
                    if (xamifouxekoaToufal == "numpad5".ToLower())
                    {
                        return 0x65;
                    }
                    if (xamifouxekoaToufal == "numpad8".ToLower())
                    {
                        return 0x68;
                    }
                    if (xamifouxekoaToufal == "end".ToLower())
                    {
                        return 0x23;
                    }
                    if (xamifouxekoaToufal == "tab".ToLower())
                    {
                        return 9;
                    }
                    if (xamifouxekoaToufal == "down".ToLower())
                    {
                        return 40;
                    }
                    if (xamifouxekoaToufal == "numpaddivide".ToLower())
                    {
                        return 0x6f;
                    }
                    if (xamifouxekoaToufal == "backspace".ToLower())
                    {
                        return 8;
                    }
                    if (xamifouxekoaToufal == "delete".ToLower())
                    {
                        return 0x2e;
                    }
                    if (xamifouxekoaToufal == "numpad0".ToLower())
                    {
                        return 0x60;
                    }
                    if (xamifouxekoaToufal == "numpad3".ToLower())
                    {
                        return 0x63;
                    }
                    if (xamifouxekoaToufal == "enter".ToLower())
                    {
                        return 13;
                    }
                    if (xamifouxekoaToufal == "numpad6".ToLower())
                    {
                        return 0x66;
                    }
                    if (xamifouxekoaToufal == "numpad9".ToLower())
                    {
                        return 0x69;
                    }
                    if (xamifouxekoaToufal == "f6".ToLower())
                    {
                        return 0x75;
                    }
                    if (xamifouxekoaToufal == "pageup".ToLower())
                    {
                        return 0x21;
                    }
                    if (xamifouxekoaToufal == "home".ToLower())
                    {
                        return 0x24;
                    }
                    if (xamifouxekoaToufal == "escape".ToLower())
                    {
                        return 0x20;
                    }
                    if (xamifouxekoaToufal == "numpadminus".ToLower())
                    {
                        return 0x6d;
                    }
                    if (xamifouxekoaToufal == "left".ToLower())
                    {
                        return 0x25;
                    }
                    if (xamifouxekoaToufal == "f10".ToLower())
                    {
                        return 0x79;
                    }
                    if (xamifouxekoaToufal == "f11".ToLower())
                    {
                        return 0x7a;
                    }
                    if (xamifouxekoaToufal == "right".ToLower())
                    {
                        return 0x27;
                    }
                    if (xamifouxekoaToufal == "f12".ToLower())
                    {
                        return 0x7b;
                    }
                    if (xamifouxekoaToufal == "printscreen".ToLower())
                    {
                        return 0x2c;
                    }
                    if (xamifouxekoaToufal == "f14".ToLower())
                    {
                        return 0x7d;
                    }
                    if (xamifouxekoaToufal == "f15".ToLower())
                    {
                        return 0x7e;
                    }
                    if (xamifouxekoaToufal == "f16".ToLower())
                    {
                        return 0x7f;
                    }
                    if (xamifouxekoaToufal == "f17".ToLower())
                    {
                        return 0x80;
                    }
                    if (xamifouxekoaToufal == "f18".ToLower())
                    {
                        return 0x81;
                    }
                    if (xamifouxekoaToufal == "f19".ToLower())
                    {
                        return 130;
                    }
                    if (xamifouxekoaToufal == "numpadequals".ToLower())
                    {
                        return 0;
                    }
                    if (xamifouxekoaToufal == "-".ToLower())
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
                UpKey(mainWindowHandle, (Keys) LaomaEtigou(key));
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
    }
}

