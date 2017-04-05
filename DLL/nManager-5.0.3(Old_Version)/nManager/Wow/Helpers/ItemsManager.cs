namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ItemsManager
    {
        public static Dictionary<string, int> ItemIdCache = new Dictionary<string, int>();
        public static Dictionary<int, string> ItemNameCache = new Dictionary<int, string>();
        public static Dictionary<int, string> ItemSpellCache = new Dictionary<int, string>();

        private static WoWItem BestItemLevel(IEnumerable<WoWItem> listItem)
        {
            try
            {
                WoWItem item = new WoWItem(0);
                int itemLevel = 0;
                foreach (WoWItem item2 in listItem)
                {
                    if (item2.GetItemInfo.ItemLevel > itemLevel)
                    {
                        itemLevel = item2.GetItemInfo.ItemLevel;
                        item = item2;
                    }
                }
                return item;
            }
            catch (Exception exception)
            {
                Logging.WriteError("BestItemLevel(List<WoWItem> listItem): " + exception, true);
            }
            return new WoWItem(0);
        }

        public static void EquipItemByName(string name)
        {
            try
            {
                Lua.LuaDoString("EquipItemByName(\"" + name + "\")", false, true);
                Logging.Write("Equip item " + name);
            }
            catch (Exception exception)
            {
                Logging.WriteError("EquipItemByName(string name): " + exception, true);
            }
        }

        public static int GetItemCount(int entry)
        {
            lock (typeof(ItemsManager))
            {
                try
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(string.Concat(new object[] { randomString, " = GetItemCount(", entry, ");" }), false, true);
                    return Others.ToInt32(Lua.GetLocalizedText(randomString));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetItemCountByIdLUA(uint itemId): " + exception, true);
                }
                return 0;
            }
        }

        public static int GetItemCount(string name)
        {
            return GetItemCount((Others.ToInt32(name) > 0) ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static int GetItemIdByName(string name)
        {
            if (name != "")
            {
                try
                {
                    if (ItemIdCache.ContainsKey(name))
                    {
                        return ItemIdCache[name];
                    }
                    lock (typeof(ItemsManager))
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString("local nameItem = \"" + name + "\" _,itemLink,_,_,_,_,_,_,_,_,_  = GetItemInfo(nameItem); if itemLink == nil then " + randomString + " = 0 else _,_," + randomString + " = string.find(itemLink, \".*|Hitem:(%d+):.*\") end", false, true);
                        int num = Others.ToInt32(Lua.GetLocalizedText(randomString));
                        if (num > 0)
                        {
                            ItemIdCache.Add(name, num);
                        }
                        return num;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetIdByName(string name): " + exception, true);
                }
            }
            return 0;
        }

        public static string GetItemNameById(int entry)
        {
            try
            {
                if (ItemNameCache.ContainsKey(entry))
                {
                    return ItemNameCache[entry];
                }
                lock (typeof(ItemsManager))
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(string.Concat(new object[] { randomString, ",_,_,_,_,_,_,_,_,_,_ = GetItemInfo(", entry, ")" }), false, true);
                    string localizedText = Lua.GetLocalizedText(randomString);
                    if (!string.IsNullOrWhiteSpace(localizedText))
                    {
                        ItemNameCache.Add(entry, localizedText);
                    }
                    return localizedText;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetNameById(int entry): " + exception, true);
            }
            return "";
        }

        public static string GetItemNameById(uint entry)
        {
            return GetItemNameById((int) entry);
        }

        public static string GetItemSpell(int entry)
        {
            try
            {
                if (ItemSpellCache.ContainsKey(entry))
                {
                    return ItemSpellCache[entry];
                }
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { randomString, ",_ = GetItemSpell(", entry, ")" }), false, true);
                string localizedText = Lua.GetLocalizedText(randomString);
                if ((localizedText != string.Empty) && (localizedText != "nil"))
                {
                    ItemSpellCache.Add(entry, localizedText);
                    return localizedText;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSpellByItemId(uint itemId): " + exception, true);
            }
            return "";
        }

        public static string GetItemSpell(string name)
        {
            return GetItemSpell((Others.ToInt32(name) > 0) ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        private static List<WoWItem> GetItemSubType(IEnumerable<WoWItem> listItem, WoWItemTradeGoodsClass subType)
        {
            try
            {
                List<WoWItem> list = new List<WoWItem>();
                foreach (WoWItem item in listItem)
                {
                    if (item.GetItemInfo.ItemSubType == subType.ToString())
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemSubType(List<WoWItem> listItem, Enums.WoWItemTradeGoodsClass subType): " + exception, true);
            }
            return new List<WoWItem>();
        }

        private static List<WoWItem> GetItemType(IEnumerable<WoWItem> listItem, nManager.Wow.Enums.WoWItemClass type)
        {
            try
            {
                List<WoWItem> list = new List<WoWItem>();
                foreach (WoWItem item in listItem)
                {
                    if (item.GetItemInfo.ItemType == type.ToString())
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetItemType(List<WoWItem> listItem, Enums.WoWItemClass type): " + exception, true);
            }
            return new List<WoWItem>();
        }

        public static bool IsHarmfulItem(int entry)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpell(entry)))
                {
                    return false;
                }
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { randomString, " = tostring(IsHarmfulItem(", entry, "))" }), false, true);
                if (Lua.GetLocalizedText(randomString) == "true")
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsHarmfulItem(string itemName): " + exception, true);
            }
            return false;
        }

        public static bool IsHarmfulItem(string name)
        {
            return IsHarmfulItem((Others.ToInt32(name) > 0) ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static bool IsHelpfulItem(int entry)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpell(entry)))
                {
                    return false;
                }
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { randomString, " = tostring(IsHelpfulItem(", entry, "))" }), false, true);
                if (Lua.GetLocalizedText(randomString) == "true")
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsHarmfulItem(string itemName): " + exception, true);
            }
            return false;
        }

        public static bool IsHelpfulItem(string name)
        {
            return IsHelpfulItem((Others.ToInt32(name) > 0) ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static bool IsItemOnCooldown(int entry)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { "startTime, duration, enable = GetItemCooldown(", entry, ") ", randomString, " = startTime .. \"^\" .. duration .. \"^\" .. time() .. \"^\" .. enable" }), false, true);
                string[] source = Lua.GetLocalizedText(randomString).Split(new char[] { '^' });
                if (source.Count<string>() == 4)
                {
                    float num = Others.ToSingle(source[0]);
                    float num2 = Others.ToSingle(source[1]);
                    float num3 = Others.ToSingle(source[2]);
                    uint num4 = Others.ToUInt32(source[3]);
                    if ((num <= 0f) && (num2 <= 0f))
                    {
                        return false;
                    }
                    if ((num4 == 0) || ((num4 == 1) && ((num + num2) < num3)))
                    {
                        return true;
                    }
                    if (num4 == 1)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsItemOnCooldown(int entry): " + exception, true);
            }
            return true;
        }

        public static bool IsItemOnCooldown(string name)
        {
            return IsItemOnCooldown((Others.ToInt32(name) > 0) ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static bool IsItemUsable(int entry)
        {
            try
            {
                if (string.IsNullOrEmpty(GetItemSpell(entry)))
                {
                    return false;
                }
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(string.Concat(new object[] { randomString, ",_ = tostring(IsUsableItem(", entry, "))" }), false, true);
                if (Lua.GetLocalizedText(randomString) == "true")
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("IsUsableItemById(uint itemId): " + exception, true);
            }
            return false;
        }

        public static bool IsItemUsable(string name)
        {
            return IsItemUsable((Others.ToInt32(name) > 0) ? Others.ToInt32(name) : GetItemIdByName(name));
        }

        public static void UseItem(int entry)
        {
            UseItem(GetItemNameById(entry));
        }

        public static void UseItem(string name)
        {
            try
            {
                Lua.RunMacroText("/use " + name);
                Logging.WriteFight("Use item " + name + " (SpellName: " + GetItemSpell(name) + ")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(string name): " + exception, true);
            }
        }

        public static void UseItem(int entry, Point point)
        {
            try
            {
                string itemNameById = GetItemNameById(entry);
                ClickOnTerrain.Item(entry, point);
                Logging.WriteFight("Use item " + itemNameById + " (SpellName: " + GetItemSpell(itemNameById) + ")");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UseItem(int entry, Class.Point point): " + exception, true);
            }
        }

        public static void UseItem(string name, Point point)
        {
            UseItem((Others.ToInt32(name) > 0) ? Others.ToInt32(name) : GetItemIdByName(name), point);
        }
    }
}

