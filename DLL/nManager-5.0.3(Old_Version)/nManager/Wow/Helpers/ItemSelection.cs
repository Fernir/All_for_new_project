namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using System;
    using System.Runtime.InteropServices;

    public class ItemSelection
    {
        public static bool CheckItemStats(string link)
        {
            string randomString = Others.GetRandomString(Others.Random(4, 10));
            string str5 = ((randomString + "='' ") + "stats=GetItemStats(\"" + link + "\"); ") + "for key,value in pairs(stats) do ";
            Lua.LuaDoString((str5 + randomString + "=" + randomString + " .. key .. '^' .. value .. '^' ") + "end;", false, true);
            string[] strArray = Lua.GetLocalizedText(randomString).Split(new char[] { '^' });
            int index = 0;
            bool flag = true;
            while (index < (strArray.Length - 1))
            {
                string interfaceName = strArray[index];
                WoWStatistic item = ConvertToStatistic(interfaceName);
                if ((item != WoWStatistic.None) && !EquipmentAndStats.InternalEquipementStats.Contains(item))
                {
                    flag = false;
                }
                index += 2;
            }
            return flag;
        }

        private static WoWStatistic ConvertToStatistic(string InterfaceName)
        {
            switch (InterfaceName)
            {
                case "ITEM_MOD_AGILITY_SHORT":
                    return WoWStatistic.AGILITY;

                case "ITEM_MOD_ATTACK_POWER_SHORT":
                    return WoWStatistic.ATTACK_POWER;

                case "ITEM_MOD_CRIT_RATING_SHORT":
                    return WoWStatistic.CRIT_RATING;

                case "ITEM_MOD_DODGE_RATING_SHORT":
                    return WoWStatistic.DODGE_RATING;

                case "ITEM_MOD_EXPERTISE_RATING_SHORT":
                    return WoWStatistic.EXPERTISE_RATING;

                case "ITEM_MOD_HASTE_RATING_SHORT":
                    return WoWStatistic.HASTE_RATING;

                case "ITEM_MOD_HIT_RATING_SHORT":
                    return WoWStatistic.HIT_RATING;

                case "ITEM_MOD_INTELLECT_SHORT":
                    return WoWStatistic.INTELLECT;

                case "ITEM_MOD_MASTERY_RATING_SHORT":
                    return WoWStatistic.MASTERY_RATING;

                case "ITEM_MOD_PARRY_RATING_SHORT":
                    return WoWStatistic.PARRY_RATING;

                case "not known":
                    return WoWStatistic.RESILIENCE_RATING;

                case "ITEM_MOD_SPELL_POWER_SHORT":
                    return WoWStatistic.SPELL_POWER;

                case "ITEM_MOD_SPIRIT_SHORT":
                    return WoWStatistic.SPIRIT;

                case "ITEM_MOD_STAMINA_SHORT":
                    return WoWStatistic.STAMINA;

                case "ITEM_MOD_STRENGTH_SHORT":
                    return WoWStatistic.STRENGTH;
            }
            return WoWStatistic.None;
        }

        public static void EquipItem(nManager.Wow.Class.ItemInfo item)
        {
            Logging.Write("Equiping item \"" + item.ItemName + "\"");
            Lua.LuaDoString("EquipItemByName(\"" + item.ItemLink + "\", " + item.xItemEquipSlot + ")", false, true);
        }

        public static int EvaluateItemStatsVsEquiped(string link, out nManager.Wow.Class.ItemInfo equipment)
        {
            int itemSellPrice = 0;
            string str = "";
            nManager.Wow.Class.ItemInfo info = new nManager.Wow.Class.ItemInfo(link);
            uint classId = nManager.Wow.Helpers.WoWItemClass.FromName(info.ItemType).Record.ClassId;
            uint subClassId = WoWItemSubClass.FromNameAndClass(info.ItemSubType, classId).Record.SubClassId;
            Logging.WriteDebug(string.Concat(new object[] { "Item \"", info.ItemName, "\" equip \"", info.ItemEquipLoc, "\" class ", (nManager.Wow.Enums.WoWItemClass) classId, " subclass ", subClassId, " has a value of ", info.ItemSellPrice }));
            if (((classId == 4) && ((((((WowItemSubClassArmor) subClassId) <= EquipmentAndStats.EquipableArmorItemType) || (info.ItemEquipLoc == "INVTYPE_CLOAK")) || ((info.ItemEquipLoc == "INVTYPE_NECK") || (info.ItemEquipLoc == "INVTYPE_FINGER"))) || ((info.ItemEquipLoc == "INVTYPE_TRINKET") || ((info.ItemEquipLoc == "INVTYPE_SHIELD") && EquipmentAndStats.HasShield)))) && CheckItemStats(link))
            {
                itemSellPrice = -1;
            }
            if (((classId == 2) && EquipmentAndStats.EquipableWeapons.Contains((WowItemSubClassWeapon) subClassId)) && CheckItemStats(link))
            {
                itemSellPrice = -1;
            }
            if (itemSellPrice != 0)
            {
                itemSellPrice = 0;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                switch (info.ItemEquipLoc)
                {
                    case "INVTYPE_HEAD":
                        str = "INVSLOT_HEAD";
                        break;

                    case "INVTYPE_SHOULDER":
                        str = "INVSLOT_SHOULDER";
                        break;

                    case "INVTYPE_CHEST":
                    case "INVTYPE_ROBE":
                        str = "INVSLOT_CHEST";
                        break;

                    case "INVTYPE_WAIST":
                        str = "INVSLOT_WAIST";
                        break;

                    case "INVTYPE_LEGS":
                        str = "INVSLOT_LEGS";
                        break;

                    case "INVTYPE_FEET":
                        str = "INVSLOT_FEET";
                        break;

                    case "INVTYPE_WRIST":
                        str = "INVSLOT_WRIST";
                        break;

                    case "INVTYPE_HAND":
                        str = "INVSLOT_HAND";
                        break;

                    case "INVTYPE_CLOAK":
                        str = "INVSLOT_BACK";
                        break;

                    case "INVTYPE_NECK":
                        str = "INVSLOT_NECK";
                        break;

                    case "INVTYPE_FINGER":
                        str = "INVSLOT_FINGER1";
                        break;

                    case "INVTYPE_WEAPON":
                    case "INVTYPE_WEAPONMAINHAND":
                    case "INVTYPE_2HWEAPON":
                    case "INVTYPE_RANGED":
                        str = "INVSLOT_MAINHAND";
                        break;

                    case "INVTYPE_WEAPONOFFHAND":
                    case "INVTYPE_HOLDABLE":
                    case "INVTYPE_SHIELD":
                        str = "INVSLOT_OFFHAND";
                        break;
                }
                Lua.LuaDoString(randomString + "= GetInventoryItemLink(\"player\", " + str + ")", false, true);
                nManager.Wow.Class.ItemInfo info2 = new nManager.Wow.Class.ItemInfo(Lua.GetLocalizedText(randomString));
                if ((info.ItemEquipLoc == "INVTYPE_FINGER") || (info.ItemEquipLoc == "INVTYPE_TRINKET"))
                {
                    if (info.ItemEquipLoc == "INVTYPE_FINGER")
                    {
                        Lua.LuaDoString(randomString + "= GetInventoryItemLink(\"player\", INVSLOT_FINGER2)", false, true);
                    }
                    else
                    {
                        Lua.LuaDoString(randomString + "= GetInventoryItemLink(\"player\", INVSLOT_TRINKET2)", false, true);
                    }
                    nManager.Wow.Class.ItemInfo info3 = new nManager.Wow.Class.ItemInfo(Lua.GetLocalizedText(randomString));
                    if ((info3.ItemRarity < 7) && ((((info.ItemRarity > info3.ItemRarity) && (info.ItemLevel > (info3.ItemLevel - (((5 * info3.ItemLevel) / 100) * (info.ItemRarity - info3.ItemRarity))))) || ((info.ItemRarity == info3.ItemRarity) && (info.ItemLevel > info3.ItemLevel))) || ((info.ItemRarity < info3.ItemRarity) && (info.ItemLevel > (info3.ItemLevel + (((5 * info3.ItemLevel) / 100) * (info3.ItemRarity - info.ItemRarity)))))))
                    {
                        itemSellPrice = -1;
                        str = (info.ItemEquipLoc == "INVTYPE_FINGER") ? "INVSLOT_FINGER2" : "INVSLOT_TRINKET2";
                    }
                }
                if ((info2.ItemRarity < 7) && ((((info.ItemRarity > info2.ItemRarity) && (info.ItemLevel > (info2.ItemLevel - (((5 * info2.ItemLevel) / 100) * (info.ItemRarity - info2.ItemRarity))))) || ((info.ItemRarity == info2.ItemRarity) && (info.ItemLevel > info2.ItemLevel))) || ((info.ItemRarity < info2.ItemRarity) && (info.ItemLevel > (info2.ItemLevel + (((5 * info2.ItemLevel) / 100) * (info2.ItemRarity - info.ItemRarity)))))))
                {
                    itemSellPrice = -1;
                }
            }
            if (itemSellPrice == 0)
            {
                itemSellPrice = info.ItemSellPrice;
            }
            equipment = info;
            equipment.xItemEquipSlot = str;
            return itemSellPrice;
        }
    }
}

