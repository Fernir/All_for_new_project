namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Threading;
    using System.Windows.Forms;

    public static class Talent
    {
        public static void DoTalents()
        {
            try
            {
                if ((VeatiuheviBeq() != 0) && Others.ExistFile(string.Concat(new object[] { Application.StartupPath, @"\CombatClasses\Talents\", nManager.Wow.ObjectManager.ObjectManager.Me.WowSpecialization(false), ".talents.txt" })))
                {
                    Lua.RunMacroText("/click PlayerTalentFrameCloseButton");
                    Thread.Sleep(400);
                    Lua.RunMacroText("/click TalentMicroButton");
                    foreach (string str2 in Others.ReadFile(string.Concat(new object[] { Application.StartupPath, @"\CombatClasses\Talents\", nManager.Wow.ObjectManager.ObjectManager.Me.WowSpecialization(false), ".talents.txt" }), false).Split(new char[] { '|' }))
                    {
                        Lua.LuaDoString("PlayerTalentFrame_SelectTalent(" + str2 + ")", false, true);
                    }
                    Lua.RunMacroText("/click PlayerTalentFrameTalentsLearnButton");
                    Thread.Sleep(400);
                    Lua.RunMacroText("/click PlayerTalentFrameCloseButton");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DoTalents(): " + exception, true);
            }
        }

        private static int VeatiuheviBeq()
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            return Others.ToInt32(Lua.LuaDoString(randomString + " = GetNumUnspentTalents()", randomString, false));
        }
    }
}

