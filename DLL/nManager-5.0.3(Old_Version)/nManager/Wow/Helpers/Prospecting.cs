namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using System;
    using System.Collections.Generic;

    public static class Prospecting
    {
        public static bool NeedRun(List<string> items)
        {
            try
            {
                Spell spell = new Spell("Prospecting");
                if (!spell.KnownSpell)
                {
                    return false;
                }
                string str = "";
                List<string> list = new List<string>();
                foreach (string str2 in items)
                {
                    if (!string.IsNullOrWhiteSpace(str2) && str2.Contains("'"))
                    {
                        list.Add(str2.Replace("'", "’"));
                    }
                    if (ItemsManager.GetItemCount(str2) >= 5)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            str = str + ", ";
                        }
                        str = str + "\"" + str2 + "\"";
                    }
                }
                foreach (string str3 in list)
                {
                    if (!items.Contains(str3))
                    {
                        items.Add(str3);
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    Logging.Write("Prospecting interrupted, no items founds from the list, check that the names are correctly typed and that you have at least 5 of them.");
                    return false;
                }
                Lua.LuaDoString("myTable = {" + str + "} needRun = \"false\" for key,value in pairs(myTable) do \titemsToProspect = value \t_, itemLink = GetItemInfo(itemsToProspect) \t_, _, Color, Ltype, Id, Enchant, Gem1, Gem2, Gem3, Gem4, Suffix, Unique, LinkLvl, Name = string.find(itemLink, \"|?c?f?f?(%x*)|?H?([^:]*):?(%d+):?(%d*):?(%d*):?(%d*):?(%d*):?(%d*):?(%-?%d*):?(%-?%d*):?(%d*)|?h?%[?([^%[%]]*)%]?|?h?|?r?\") \tfisrtI = -1 \tfisrtJ = -1 \tif tonumber(Id) > 0 then \t\tfor i=0,4 do \t\t\t for j=1,GetContainerNumSlots(i)do \t\t\t\tidT = GetContainerItemID(i,j) \t\t\t\t if tonumber(Id) == idT then \t\t\t\t\t_, itemCount, _, _, _ = GetContainerItemInfo(i,j); \t\t\t\t\tif tonumber(itemCount) >=5 then \t\t\t\t\t\tneedRun = \"true\"\t\t\t\t\t\treturn \t\t\t\t\tend \t\t\t\t end \t\t\t end \t\tend \tend end", false, true);
                return (Lua.GetLocalizedText("needRun") == "true");
            }
            catch (Exception exception)
            {
                Logging.WriteError("Prospecting > NeedRun(List<string> items): " + exception, true);
                return false;
            }
        }

        public static void Pulse(List<string> items)
        {
            try
            {
                Spell spell = new Spell("Prospecting");
                if (spell.KnownSpell)
                {
                    string str = "";
                    List<string> list = new List<string>();
                    foreach (string str2 in items)
                    {
                        if (!string.IsNullOrWhiteSpace(str2) && str2.Contains("'"))
                        {
                            list.Add(str2.Replace("'", "’"));
                        }
                        if (ItemsManager.GetItemCount(str2) >= 5)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                str = str + ", ";
                            }
                            str = str + "\"" + str2 + "\"";
                        }
                    }
                    foreach (string str3 in list)
                    {
                        if (!items.Contains(str3))
                        {
                            items.Add(str3);
                        }
                    }
                    if (string.IsNullOrEmpty(str))
                    {
                        Logging.Write("Prospecting interrupted, no items founds from the list, check that the names are correctly typed and that you have at least 5 of them.");
                    }
                    else
                    {
                        Lua.LuaDoString("myTable = {" + str + "} for key,value in pairs(myTable) do \titemsToProspect = value \t_, itemLink = GetItemInfo(itemsToProspect) \t_, _, Color, Ltype, Id, Enchant, Gem1, Gem2, Gem3, Gem4, Suffix, Unique, LinkLvl, Name = string.find(itemLink, \"|?c?f?f?(%x*)|?H?([^:]*):?(%d+):?(%d*):?(%d*):?(%d*):?(%d*):?(%d*):?(%-?%d*):?(%-?%d*):?(%d*)|?h?%[?([^%[%]]*)%]?|?h?|?r?\") \tfisrtI = -1 \tfisrtJ = -1 \tif tonumber(Id) > 0 then \t\tfor i=0,4 do \t\t\t for j=1,GetContainerNumSlots(i)do \t\t\t\tidT = GetContainerItemID(i,j) \t\t\t\t if tonumber(Id) == idT then \t\t\t\t\t_, itemCount, _, _, _ = GetContainerItemInfo(i,j); \t\t\t\t\tif tonumber(itemCount) >=5 then \t\t\t\t\t\tCastSpellByName(\"" + spell.NameInGame + "\"); \t\t\t\t\t\tUseContainerItem(i,j) \t\t\t\t\tend \t\t\t\t end \t\t\t end \t\tend \tend end", false, true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Prospecting > Pulse(List<string> items): " + exception, true);
            }
        }
    }
}

