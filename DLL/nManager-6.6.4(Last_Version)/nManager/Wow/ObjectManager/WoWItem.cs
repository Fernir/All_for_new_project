namespace nManager.Wow.ObjectManager
{
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.Patchables;
    using System;

    public class WoWItem : WoWObject
    {
        private string _paukoheuruTougiove;
        private nManager.Wow.Class.ItemInfo _pouhauseig;

        public WoWItem(uint address) : base(address)
        {
        }

        public T GetDescriptor<T>(Descriptors.ItemFields field) where T: struct
        {
            try
            {
                return base.Tuecom<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWItem > GetDescriptor<T>(Descriptors.ItemFields field): " + exception, true);
                return default(T);
            }
        }

        public nManager.Wow.Class.ItemInfo GetItemInfo
        {
            get
            {
                try
                {
                    return (this._pouhauseig ?? (this._pouhauseig = new nManager.Wow.Class.ItemInfo(base.Entry)));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWItem > GetItemInfo: " + exception, true);
                    return new nManager.Wow.Class.ItemInfo(0);
                }
            }
        }

        public bool IsEquippableItem
        {
            get
            {
                try
                {
                    string localizedText;
                    lock (this)
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { randomString, " = IsEquippableItem(", base.Entry, ")" }), false, true);
                        localizedText = Lua.GetLocalizedText(randomString);
                    }
                    return (localizedText != "");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWItem > IsEquippableItem: " + exception, true);
                    return false;
                }
            }
        }

        public override string Name
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(this._paukoheuruTougiove))
                    {
                        return this._paukoheuruTougiove;
                    }
                    lock (this)
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(string.Concat(new object[] { randomString, ", _, _, _, _, _, _, _ = GetItemInfo(", base.Entry, ")" }), false, true);
                        this._paukoheuruTougiove = Lua.GetLocalizedText(randomString);
                        return this._paukoheuruTougiove;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWItem > Name: " + exception, true);
                }
                return "";
            }
        }

        public UInt128 Owner
        {
            get
            {
                return base.Tuecom<UInt128>(base.GetBaseAddress, 12);
            }
        }

        public string SpellName
        {
            get
            {
                try
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + ",_ = GetItemSpell(" + this.Name + ")", false, true);
                    string localizedText = Lua.GetLocalizedText(randomString);
                    if ((localizedText != string.Empty) && (localizedText != "nil"))
                    {
                        return localizedText;
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWItem > SpellName: " + exception, true);
                }
                return "";
            }
        }
    }
}

