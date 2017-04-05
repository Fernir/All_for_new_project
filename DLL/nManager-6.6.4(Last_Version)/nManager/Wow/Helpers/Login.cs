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
        private static bool _ecukepoutaudaoAg;

        public static bool Pulse(SettingsLogin settings)
        {
            try
            {
                _ecukepoutaudaoAg = true;
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(120000.0);
                while ((((nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress == 0) || (nManager.Wow.ObjectManager.ObjectManager.Me.Guid == 0L)) || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid) && _ecukepoutaudaoAg)
                {
                    if (timer.IsReady)
                    {
                        return false;
                    }
                    string command = "if (AccountLogin.UI and AccountLogin.UI:IsVisible()) then AccountLogin.UI.AccountEditBox:SetText('" + settings.Login + "'); AccountLogin.UI.PasswordEditBox:SetText('" + settings.Password + "'); AccountLogin_Login() end ";
                    string str2 = "if (AccountLogin.UI.WoWAccountSelectDialog and AccountLogin.UI.WoWAccountSelectDialog:IsShown()) then C_Login.SelectGameAccount('" + settings.BNetName + "') end ";
                    string str3 = "local realms = RealmList.selectedCategory and C_RealmList.GetRealmsInCategory(RealmList.selectedCategory) or {}; local scrollFrame = RealmListScrollFrame; local offset = HybridScrollFrame_GetOffset(scrollFrame) for i=1, #scrollFrame.buttons do local idx = i + offset; if ( idx <= #realms ) then local realmAddr = realms[idx]; local name, numChars, versionMismatch, isPvP, isRP, populationState, versionMajor, versionMinor, versionRev, versionBuild = C_RealmList.GetRealmInfo(realmAddr); if (name == '" + settings.Realm + "') then C_RealmList.ConnectToRealm(realmAddr); end end end";
                    string str4 = "if (CharacterSelectUI and CharacterSelectUI:IsVisible()) then if GetServerName() ~= '" + settings.Realm.Replace("'", @"\'") + "' and (not RealmList or not RealmList:IsVisible()) then CharacterSelect_ChangeRealm() else for i = 0,GetNumCharacters() do if (GetCharacterInfo(i) == '" + settings.Character + "') then CharacterSelect_SelectCharacter(i) end end end end ";
                    Lua.LuaDoString(command, true, true);
                    Logging.Write("Running: Battle.net account login for email " + (settings.Login.Substring(0, 4) + "########") + "...");
                    Application.DoEvents();
                    Thread.Sleep(0x1388);
                    Lua.LuaDoString(str2, true, true);
                    Logging.Write("Running: Battle.Net account login for account " + (settings.BNetName.Contains("WoW") ? settings.BNetName : (settings.BNetName.Substring(0, 4) + "########")) + "...");
                    Application.DoEvents();
                    Thread.Sleep(0x1388);
                    Lua.LuaDoString(str3, true, true);
                    Logging.Write("Running: Realm selection...");
                    Application.DoEvents();
                    Thread.Sleep(0x1388);
                    Lua.LuaDoString(str4, true, true);
                    Logging.Write("Running: Char selection...");
                    Application.DoEvents();
                    Thread.Sleep(0x1388);
                    Keyboard.PressKey(Memory.WowProcess.MainWindowHandle, Keys.Enter);
                    Application.DoEvents();
                    Logging.Write("Running: Loading the game...");
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
            _ecukepoutaudaoAg = false;
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

