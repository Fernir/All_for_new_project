namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class Login
    {
        private static bool _login;

        public static bool Pulse(SettingsLogin settings)
        {
            try
            {
                _login = true;
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(120000.0);
                while ((((nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress == 0) || (nManager.Wow.ObjectManager.ObjectManager.Me.Guid == 0L)) || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) && _login)
                {
                    if (timer.IsReady)
                    {
                        return false;
                    }
                    string command = "if (WoWAccountSelectDialog and WoWAccountSelectDialog:IsShown()) then for i = 0, GetNumGameAccounts() do if GetGameAccountInfo(i) == '" + settings.BNetName + "' then WoWAccountSelect_SelectAccount(i) end end elseif (AccountLoginUI and AccountLoginUI:IsVisible()) then local editbox = AccountLoginPasswordEdit; editbox:SetText('" + settings.Password + "'); DefaultServerLogin('" + settings.Login + "', editbox); AccountLoginUI:Hide() elseif (RealmList and RealmList:IsVisible()) then for i = 1, select('#',GetRealmCategories()) do for j = 1, GetNumRealms(i) do if GetRealmInfo(i, j) == '" + settings.Realm.Replace("'", @"\'") + "' then RealmList:Hide() ChangeRealm(i, j) end end end end ";
                    string str2 = "if (CharacterSelectUI and CharacterSelectUI:IsVisible()) then if GetServerName() ~= '" + settings.Realm.Replace("'", @"\'") + "' and (not RealmList or not RealmList:IsVisible()) then RequestRealmList(1) else for i = 0,GetNumCharacters() do if (GetCharacterInfo(i) == '" + settings.Character + "') then CharacterSelect_SelectCharacter(i) end end end end ";
                    Lua.LuaDoString(command, true, true);
                    Thread.Sleep(0x1388);
                    Lua.LuaDoString(str2, true, true);
                    Thread.Sleep(0x9c4);
                    Keyboard.PressKey(Memory.WowProcess.MainWindowHandle, Keys.Enter);
                    Thread.Sleep(0x1f40);
                    Application.DoEvents();
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Login > Pulse(SettingsLogin settings): " + exception, true);
                return false;
            }
        }

        public static void StopLogin()
        {
            _login = false;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SettingsLogin
        {
            public string Realm;
            public string Login;
            public string Character;
            public string Password;
            public string BNetName;
            public SettingsLogin(string realm, string login, string character, string password, string bNetName)
            {
                this = new nManager.Wow.Helpers.Login.SettingsLogin();
                try
                {
                    this.Realm = realm;
                    this.Login = login;
                    this.Character = character;
                    this.Password = password;
                    this.BNetName = bNetName;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("SettingsLogin(string realm, string login, string character, string password, string bNetName): " + exception, true);
                }
            }
        }
    }
}

