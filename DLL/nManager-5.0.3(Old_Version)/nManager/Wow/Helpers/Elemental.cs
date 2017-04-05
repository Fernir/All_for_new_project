namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public class Elemental
    {
        public static void AutoMakeElemental()
        {
            try
            {
                if (Others.ExistFile(Application.StartupPath + @"\Data\autoMakeElementalMacro.txt"))
                {
                    string command = Others.ReadFile(Application.StartupPath + @"\Data\autoMakeElementalMacro.txt", false).Replace(Environment.NewLine, " ");
                    Lua.LuaDoString(command, false, true);
                    Thread.Sleep(Usefuls.Latency);
                    Lua.LuaDoString(command, false, true);
                    Thread.Sleep(Usefuls.Latency);
                    Lua.LuaDoString(command, false, true);
                    Thread.Sleep(Usefuls.Latency);
                    Lua.LuaDoString(command, false, true);
                    Thread.Sleep(Usefuls.Latency);
                    Lua.LuaDoString(command, false, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("AutoMakeElemental(): " + exception, true);
            }
        }
    }
}

