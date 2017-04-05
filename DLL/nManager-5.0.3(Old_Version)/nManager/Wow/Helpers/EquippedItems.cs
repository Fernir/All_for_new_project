namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using nManager.Wow.Patchables;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    public class EquippedItems
    {
        public static WoWItem GetEquippedItem(int invSlot)
        {
            UInt128 guid = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<UInt128>((Descriptors.PlayerFields) (0x408 + (invSlot * 4)));
            return (nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWItem().FirstOrDefault<WoWItem>(x => (x.Guid == guid)) ?? new WoWItem(0));
        }

        public static WoWItem GetEquippedItem(WoWInventorySlot inventorySlot, uint resultNb = 1)
        {
            if (Usefuls.InGame && !Usefuls.IsLoadingOrConnecting)
            {
                uint num = 1;
                try
                {
                    foreach (WoWItem item in GetEquippedItems())
                    {
                        WoWItem item2 = item;
                        nManager.Wow.Class.ItemInfo getItemInfo = item2.GetItemInfo;
                        try
                        {
                            WoWInventorySlot slot = (WoWInventorySlot) Enum.Parse(typeof(WoWInventorySlot), getItemInfo.ItemEquipLoc);
                            if (Enum.IsDefined(typeof(WoWInventorySlot), slot) | slot.ToString().Contains(","))
                            {
                                if ((slot == inventorySlot) && (num == resultNb))
                                {
                                    return item;
                                }
                                if (slot == inventorySlot)
                                {
                                    num++;
                                }
                            }
                        }
                        catch (ArgumentException)
                        {
                            Logging.WriteError(string.Format("'{0}' is not a member of the WoWInventorySlot enumeration.", getItemInfo.ItemEquipLoc), true);
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetEquippedItem(WoWInventorySlot inventorySlot): " + exception, true);
                }
            }
            return new WoWItem(0);
        }

        public static List<WoWItem> GetEquippedItems()
        {
            try
            {
                var predicate = null;
                List<WoWItem> list = new List<WoWItem>();
                List<uint> itemId = new List<uint> {
                    GetEquippedItem(0).Entry,
                    GetEquippedItem(1).Entry,
                    GetEquippedItem(2).Entry,
                    GetEquippedItem(3).Entry,
                    GetEquippedItem(4).Entry,
                    GetEquippedItem(5).Entry,
                    GetEquippedItem(6).Entry,
                    GetEquippedItem(7).Entry,
                    GetEquippedItem(8).Entry,
                    GetEquippedItem(9).Entry,
                    GetEquippedItem(10).Entry,
                    GetEquippedItem(11).Entry,
                    GetEquippedItem(12).Entry,
                    GetEquippedItem(13).Entry,
                    GetEquippedItem(14).Entry,
                    GetEquippedItem(15).Entry,
                    GetEquippedItem(0x10).Entry,
                    GetEquippedItem(0x12).Entry
                };
                if (itemId.Count > 0)
                {
                    List<WoWItem> objectWoWItem = nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWItem();
                    if (predicate == null)
                    {
                        predicate = <>h__TransparentIdentifier1 => itemId.Contains(<>h__TransparentIdentifier1.<>h__TransparentIdentifier0.itemIdTemp) && (<>h__TransparentIdentifier1.itemGuidOwner == nManager.Wow.ObjectManager.ObjectManager.Me.Guid);
                    }
                    list.AddRange(from <>h__TransparentIdentifier1 in (from o in objectWoWItem
                        let itemIdTemp = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<uint>(o.GetBaseAddress, 9)
                        select new { <>h__TransparentIdentifier0 = <>h__TransparentIdentifier0, itemGuidOwner = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<UInt128>(o.GetBaseAddress, 12) }).Where(predicate) select <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.o);
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetEquippedItems(): " + exception, true);
            }
            return new List<WoWItem>();
        }

        public static bool IsEquippedItemByGuid(UInt128 guid)
        {
            UInt128 descriptor = 0;
            bool flag = false;
            for (int i = 0; i < 0x13; i++)
            {
                descriptor = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<UInt128>((Descriptors.PlayerFields) (0x408 + (i * 4)));
                if (!(descriptor != guid))
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                WoWItem item = (descriptor == guid) ? new WoWItem(nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(descriptor).GetBaseAddress) : new WoWItem(0);
                return item.IsValid;
            }
            return false;
        }
    }
}

