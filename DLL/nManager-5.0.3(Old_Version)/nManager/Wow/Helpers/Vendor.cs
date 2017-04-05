namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Vendor
    {
        public static void BuyItem(string name, int number)
        {
            try
            {
                Lua.LuaDoString(string.Concat(new object[] { "function buy(n,q) for i=1,100 do if n==GetMerchantItemInfo(i) then BuyMerchantItem(i,q) end end end name = \"", name, "\" buy (name,", number, ")" }), false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("BuyItem(string name, int number): " + exception, true);
            }
        }

        public static void RepairAllItems()
        {
            try
            {
                Lua.LuaDoString("RepairAllItems()", false, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("RepairAllItems(): " + exception, true);
            }
        }

        public static void SellItems(List<string> itemSell, List<string> itemNoSell, List<WoWItemQuality> itemQuality)
        {
            try
            {
                string str = itemSell.Aggregate<string, string>("", (current, s) => current + " or namei == \"" + s + "\" ");
                string str2 = itemQuality.Aggregate<WoWItemQuality, string>(" 1 == 2 ", (current, s) => string.Concat(new object[] { current, " or r == ", (uint) s, " " }));
                string str3 = "";
                string str4 = "";
                if (itemNoSell.Count > 0)
                {
                    str4 = " end ";
                    str3 = itemNoSell.Aggregate<string, string>(" if ", (current, s) => (current + " and namei ~= \"" + s + "\" ")).Replace("if  and", "if ") + " then ";
                }
                for (int i = 0; i <= 4; i++)
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(string.Concat(new object[] { randomString, " = GetContainerNumSlots(", i, ")" }), false, true);
                    uint num2 = Others.ToUInt32(Lua.GetLocalizedText(randomString));
                    for (int j = 0; j < (num2 + 1); j++)
                    {
                        if (!Others.IsFrameVisible("MerchantFrame"))
                        {
                            return;
                        }
                        string str6 = "";
                        str6 = str6 + "local l,r,_=0 ";
                        str6 = string.Concat(new object[] { str6, "local l=GetContainerItemLink(", i, ", ", j, ") " }) + "if l then namei,_,r=GetItemInfo(l) ";
                        str6 = (str6 + "if " + str2 + " " + str + " then ") + str3;
                        Lua.LuaDoString((string.Concat(new object[] { str6, " UseContainerItem(", i, ", ", j, ") " }) + str4) + " end " + "end ", false, true);
                        Thread.Sleep(150);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("SellItems(List<String> itemSell, List<string> itemNoSell, List<Enums.WoWItemQuality> itemQuality): " + exception, true);
            }
        }
    }
}

