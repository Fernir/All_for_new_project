namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;

    public class Bag
    {
        public static List<WoWItem> GetBagItem()
        {
            try
            {
                string localizedText;
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                List<WoWItem> list = new List<WoWItem>();
                string command = "";
                command = ((((command + "l=0 ") + randomString + " = \"\" ") + "ItemLinkT = \"\" " + "for b=0,4 do ") + "for s=1,GetContainerNumSlots(b) do " + "l=GetContainerItemLink(b,s) ") + "if l then " + "ItemLinkT = GetContainerItemLink(b,s) ";
                command = ((command + randomString + " = " + randomString + " .. ItemLinkT .. \"^\" ") + "end ") + "end " + "end ";
                lock (typeof(Bag))
                {
                    Lua.LuaDoString(command, false, true);
                    localizedText = Lua.GetLocalizedText(randomString);
                }
                List<uint> list2 = new List<uint>();
                foreach (string str4 in localizedText.Split(new char[] { '^' }))
                {
                    if (str4 != "")
                    {
                        try
                        {
                            list2.Add(Others.ToUInt32(str4.Split(new char[] { ':' })[1]));
                        }
                        catch (Exception exception)
                        {
                            Logging.WriteError("GetBagItem()#1: " + exception, true);
                        }
                    }
                }
                if (list2.Count > 0)
                {
                    List<WoWItem> objectWoWItem = nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWItem();
                    List<int> list4 = new List<int>();
                    foreach (WoWItem item in objectWoWItem)
                    {
                        try
                        {
                            if (item.Type == WoWObjectType.Item)
                            {
                                uint descriptor = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<uint>(item.GetBaseAddress, 9);
                                UInt128 num2 = nManager.Wow.ObjectManager.ObjectManager.Me.GetDescriptor<UInt128>(item.GetBaseAddress, 12);
                                if ((list2.Contains(descriptor) && (num2 == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)) && !list4.Contains(item.Entry))
                                {
                                    list.Add(item);
                                    list4.Add(item.Entry);
                                }
                            }
                        }
                        catch (Exception exception2)
                        {
                            Logging.WriteError("GetBagItem()#2: " + exception2, true);
                        }
                    }
                }
                return list;
            }
            catch (Exception exception3)
            {
                Logging.WriteError("GetBagItem()#3: " + exception3, true);
            }
            return new List<WoWItem>();
        }

        public static bool ItemIsInBag(string name)
        {
            bool flag2;
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                string command = "local c,l,r,_=0 ";
                command = ((((((command + randomString + " = \"False\" ") + "for b=0,4 do " + "for s=1,40 do  ") + "local l=GetContainerItemLink(b,s) " + "if l then namei,_,r=GetItemInfo(l) ") + "if namei == " + name + " then ") + randomString + " = \"True\" ") + " end " + "end ") + "end " + "end ";
                lock (typeof(Bag))
                {
                    Lua.LuaDoString(command, false, true);
                    flag2 = Lua.GetLocalizedText(randomString) == "True";
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ItemIsInBag(string name): " + exception, true);
                flag2 = false;
            }
            return flag2;
        }

        public static int NumFreeSlots
        {
            get
            {
                return Usefuls.GetContainerNumFreeSlots;
            }
        }
    }
}

