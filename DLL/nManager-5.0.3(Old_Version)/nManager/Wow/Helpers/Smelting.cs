namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using System;
    using System.Runtime.InteropServices;

    public static class Smelting
    {
        public static void CloseSmeltingWindow()
        {
            try
            {
                Lua.LuaDoString("RunMacroText(\"/click TradeSkillFrameCloseButton\"); ", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > OpenSmeltingWindow(): " + exception, true);
            }
        }

        public static bool NeedRun(bool openWindow = true)
        {
            try
            {
                Spell spell = new Spell("Smelting");
                if (!spell.KnownSpell)
                {
                    return false;
                }
                string command = "";
                if (openWindow)
                {
                    command = command + "RunMacroText(\"/click TradeSkillFrameCloseButton\"); CastSpellByName(\"" + spell.NameInGame + "\");";
                }
                command = command + "needSmelting = \"\"; numTrade = GetNumTradeSkills(); firstTrade = GetFirstTradeSkill(); while numTrade>=firstTrade do   skillName, skillType, numAvailable, isExpanded, altVerb, numSkillUps = GetTradeSkillInfo(numTrade);        if numAvailable > 0 then          needSmelting = \"true\";          numTrade =  firstTrade;        end   numTrade = numTrade - 1; end ";
                if (openWindow)
                {
                    command = command + "RunMacroText(\"/click TradeSkillFrameCloseButton\"); ";
                }
                Lua.LuaDoString(command, false, true);
                if (Lua.GetLocalizedText("needSmelting") == "true")
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > NeedRun(): " + exception, true);
            }
            return false;
        }

        public static void OpenSmeltingWindow()
        {
            try
            {
                Spell spell = new Spell("Smelting");
                if (spell.KnownSpell)
                {
                    Lua.LuaDoString("RunMacroText(\"/click TradeSkillFrameCloseButton\"); CastSpellByName(\"" + spell.NameInGame + "\"); ", false, true);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > OpenSmeltingWindow(): " + exception, true);
            }
        }

        public static void Pulse()
        {
            try
            {
                Lua.LuaDoString("numTrade = GetNumTradeSkills(); firstTrade = GetFirstTradeSkill(); while numTrade>=firstTrade do   skillName, skillType, numAvailable, isExpanded, altVerb, numSkillUps = GetTradeSkillInfo(numTrade);   if numAvailable > 0 then     SelectTradeSkill(numTrade);     RunMacroText(\"/click TradeSkillCreateAllButton\");     return;   end   numTrade = numTrade - 1; end", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Smelting > Pulse(): " + exception, true);
            }
        }
    }
}

