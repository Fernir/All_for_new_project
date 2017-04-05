namespace nManager.Wow.Class
{
    using nManager.Helpful;
    using nManager.Wow.Helpers;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ItemInfo
    {
        public ItemInfo(int entryId)
        {
            try
            {
                string localizedText;
                lock (this)
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(string.Concat(new object[] { "itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture, itemSellPrice = GetItemInfo(", entryId, ") ", randomString, " = itemName .. \"^\" .. itemLink .. \"^\" .. itemRarity .. \"^\" .. itemLevel .. \"^\" .. itemMinLevel .. \"^\" .. itemType .. \"^\" .. itemSubType .. \"^\" .. itemStackCount .. \"^\" .. itemEquipLoc .. \"^\" .. itemTexture .. \"^\" .. itemSellPrice" }), false, true);
                    localizedText = Lua.GetLocalizedText(randomString);
                }
                string[] strArray = localizedText.Split(new char[] { '^' });
                if ((from s in strArray select s).Count<string>() == 11)
                {
                    this.ItemName = strArray[0];
                    this.ItemLink = strArray[1];
                    this.ItemRarity = Others.ToInt32(strArray[2]);
                    this.ItemLevel = Others.ToInt32(strArray[3]);
                    this.ItemMinLevel = Others.ToInt32(strArray[4]);
                    this.ItemType = strArray[5];
                    this.ItemSubType = strArray[6];
                    this.ItemStackCount = Others.ToInt32(strArray[7]);
                    this.ItemEquipLoc = strArray[8];
                    this.ItemTexture = strArray[9];
                    this.ItemSellPrice = Others.ToInt32(strArray[10]);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ItemInfo(int entryId): " + exception, true);
            }
        }

        public ItemInfo(string nameItem)
        {
            try
            {
                string localizedText;
                lock (this)
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString("itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture, itemSellPrice = GetItemInfo(\"" + nameItem + "\")  " + randomString + " = itemName .. \"^\" .. itemLink .. \"^\" .. itemRarity .. \"^\" .. itemLevel .. \"^\" .. itemMinLevel .. \"^\" .. itemType .. \"^\" .. itemSubType .. \"^\" .. itemStackCount .. \"^\" .. itemEquipLoc .. \"^\" .. itemTexture .. \"^\" .. itemSellPrice", false, true);
                    localizedText = Lua.GetLocalizedText(randomString);
                }
                string[] source = localizedText.Split(new char[] { '^' });
                if (source.Count<string>() < 2)
                {
                    this.ItemName = "";
                    this.ItemLink = "";
                    this.ItemRarity = 0;
                    this.ItemLevel = 0;
                    this.ItemMinLevel = 0x5b;
                    this.ItemType = "";
                    this.ItemSubType = "";
                    this.ItemStackCount = 0;
                    this.ItemEquipLoc = "";
                    this.ItemTexture = "";
                    this.ItemSellPrice = 0;
                }
                else
                {
                    this.ItemName = source[0];
                    this.ItemLink = source[1];
                    this.ItemRarity = Others.ToInt32(source[2]);
                    this.ItemLevel = Others.ToInt32(source[3]);
                    this.ItemMinLevel = Others.ToInt32(source[4]);
                    this.ItemType = source[5];
                    this.ItemSubType = source[6];
                    this.ItemStackCount = Others.ToInt32(source[7]);
                    this.ItemEquipLoc = source[8];
                    this.ItemTexture = source[9];
                    this.ItemSellPrice = Others.ToInt32(source[10]);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ItemInfo(string nameItem): " + exception, true);
            }
        }

        public string ItemEquipLoc { get; private set; }

        public int ItemLevel { get; private set; }

        public string ItemLink { get; private set; }

        public int ItemMinLevel { get; private set; }

        public string ItemName { get; private set; }

        public int ItemRarity { get; private set; }

        public int ItemSellPrice { get; private set; }

        public int ItemStackCount { get; private set; }

        public string ItemSubType { get; private set; }

        public string ItemTexture { get; private set; }

        public string ItemType { get; private set; }

        public string xItemEquipSlot { get; set; }
    }
}

