namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Helpful.Win32;
    using nManager.Wow;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public static class Keybindings
    {
        private static List<KeybindingsStruct> _keybindingsList = new List<KeybindingsStruct>();

        public static void DownKeybindings(nManager.Wow.Enums.Keybindings action)
        {
            try
            {
                string keyByAction = GetKeyByAction(action, true);
                if (keyByAction != "")
                {
                    Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, keyByAction);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DownKeybindings(Enums.Keybindings action): " + exception, true);
            }
        }

        public static string GetAFreeKey(bool easyonly = false)
        {
            try
            {
                foreach (UnreservedVK dvk in Enum.GetValues(typeof(UnreservedVK)))
                {
                    if (!(dvk.ToString() == Usefuls.AfkKeyPress))
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { randomString, " = GetBindingAction(\"", dvk, "\", true)" }), false, false);
                        if (string.IsNullOrEmpty(Lua.GetLocalizedText(randomString)))
                        {
                            return dvk.ToString();
                        }
                    }
                }
                if (!easyonly)
                {
                    foreach (UnreservedVK dvk2 in Enum.GetValues(typeof(UnreservedVK)))
                    {
                        string commandline = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { commandline, " = GetBindingAction(\"CTRL-", dvk2, "\", true)" }), false, true);
                        if (string.IsNullOrEmpty(Lua.GetLocalizedText(commandline)))
                        {
                            return ("CTRL-" + dvk2);
                        }
                    }
                    foreach (UnreservedVK dvk3 in Enum.GetValues(typeof(UnreservedVK)))
                    {
                        string str4 = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { str4, " = GetBindingAction(\"SHIFT-", dvk3, "\", true)" }), false, true);
                        if (string.IsNullOrEmpty(Lua.GetLocalizedText(str4)))
                        {
                            return ("SHIFT-" + dvk3);
                        }
                    }
                    foreach (UnreservedVK dvk4 in Enum.GetValues(typeof(UnreservedVK)))
                    {
                        string str5 = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { str5, " = GetBindingAction(\"CTRL-SHIFT-", dvk4, "\", true)" }), false, true);
                        if (string.IsNullOrEmpty(Lua.GetLocalizedText(str5)))
                        {
                            return ("CTRL-SHIFT-" + dvk4);
                        }
                    }
                }
                return "";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetAFreeKey(): " + exception, true);
                return "";
            }
        }

        public static string GetKeyByAction(nManager.Wow.Enums.Keybindings action, bool autoAssignKeyIfNull = true)
        {
            try
            {
                foreach (KeybindingsStruct struct2 in _keybindingsList)
                {
                    if ((struct2.Key != "") && (struct2.Action == action))
                    {
                        return struct2.Key;
                    }
                }
                Lua.LuaDoString("key1, key2 = GetBindingKey(\"" + action + "\");", false, true);
                string localizedText = Lua.GetLocalizedText("key1");
                string str2 = Lua.GetLocalizedText("key2");
                if ((localizedText != "") && (localizedText != "BUTTON3"))
                {
                    KeybindingsStruct item = new KeybindingsStruct {
                        Action = action,
                        Key = localizedText
                    };
                    _keybindingsList.Add(item);
                    return localizedText;
                }
                if ((str2 != "") && (str2 != "BUTTON3"))
                {
                    KeybindingsStruct struct4 = new KeybindingsStruct {
                        Action = action,
                        Key = str2
                    };
                    _keybindingsList.Add(struct4);
                    return str2;
                }
                if (autoAssignKeyIfNull)
                {
                    string aFreeKey = GetAFreeKey(false);
                    if (!string.IsNullOrEmpty(aFreeKey))
                    {
                        if ((localizedText == "BUTTON3") || (str2 == "BUTTON3"))
                        {
                            Logging.WriteDebug(string.Concat(new object[] { action, " were bind to a mouse button, as TheNoobBot does not support mouse button, currently trying to bind it with key: ", aFreeKey, "." }));
                        }
                        else
                        {
                            Logging.WriteDebug(string.Concat(new object[] { action, " were not bind, currently trying to bind it with key: ", aFreeKey, "." }));
                        }
                        SetKeyByAction(action, aFreeKey);
                        KeybindingsStruct struct5 = new KeybindingsStruct {
                            Action = action,
                            Key = aFreeKey
                        };
                        _keybindingsList.Add(struct5);
                        return aFreeKey;
                    }
                    Logging.WriteDebug("No free keys found on 236 possible bindings, if you got that line, you mainly have a problem with your WoW keybindings.");
                    return "";
                }
                return "";
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetKeyByAction(Enums.Keybindings action, bool autoAssignKeyIfNull = true): " + exception, true);
                return "";
            }
        }

        public static void PressKeybindings(nManager.Wow.Enums.Keybindings action)
        {
            try
            {
                string keyByAction = GetKeyByAction(action, true);
                if (keyByAction != "")
                {
                    Keyboard.DownKey(Memory.WowProcess.MainWindowHandle, keyByAction);
                    Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, keyByAction);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("PressKeybindings(Enums.Keybindings action): " + exception, true);
            }
        }

        public static void ResetList()
        {
            try
            {
                lock (typeof(nManager.Wow.Helpers.Keybindings))
                {
                    _keybindingsList = new List<KeybindingsStruct>();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ResetList(): " + exception, true);
            }
        }

        public static void SetKeyByAction(nManager.Wow.Enums.Keybindings action, string key)
        {
            try
            {
                Lua.LuaDoString("SetBinding(GetBindingKey(\"" + action + "\"));", false, true);
                Lua.LuaDoString(string.Concat(new object[] { "SetBinding(\"", key.ToUpper(), "\", \"", action, "\");" }), false, true);
                Lua.LuaDoString("SaveBindings(2)", false, true);
                ResetList();
            }
            catch (Exception exception)
            {
                Logging.WriteError("SetKeyByAction(Enums.Keybindings action, string key): " + exception, true);
            }
        }

        public static void UpKeybindings(nManager.Wow.Enums.Keybindings action)
        {
            try
            {
                string keyByAction = GetKeyByAction(action, true);
                if (keyByAction != "")
                {
                    Keyboard.UpKey(Memory.WowProcess.MainWindowHandle, keyByAction);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("UpKeybindings(Enums.Keybindings action): " + exception, true);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeybindingsStruct
        {
            public string Key;
            public nManager.Wow.Enums.Keybindings Action;
        }
    }
}

