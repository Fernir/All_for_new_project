namespace nManager.Wow.ObjectManager
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.Patchables;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class WoWObject
    {
        public WoWObject(uint address)
        {
            this.BaseAddress = address;
        }

        internal T GetDescriptor<T>(Descriptors.ObjectFields field) where T: struct
        {
            try
            {
                return this.GetDescriptor<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWObject > GetDescriptor<T>(Descriptors.ObjectFields field): " + exception, true);
                return default(T);
            }
        }

        internal T GetDescriptor<T>(uint field) where T: struct
        {
            try
            {
                return this.GetDescriptor<T>(this.BaseAddress, field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWObject > GetDescriptor<T>(uint field): " + exception, true);
                return default(T);
            }
        }

        internal T GetDescriptor<T>(uint baseAddress, uint field) where T: struct
        {
            try
            {
                object obj2 = null;
                if (baseAddress > 0)
                {
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(baseAddress + Descriptors.StartDescriptors) + (field * 4);
                    if (typeof(T) == typeof(string))
                    {
                        string str = "";
                        for (byte[] buffer = Memory.WowMemory.Memory.ReadBytes(dwAddress, 1); buffer[0] != 0; buffer = Memory.WowMemory.Memory.ReadBytes(dwAddress, 1))
                        {
                            str = str + Convert.ToChar(buffer[0]);
                            dwAddress++;
                        }
                        obj2 = str;
                        return (T) obj2;
                    }
                    if (typeof(T) == typeof(ulong))
                    {
                        obj2 = Memory.WowMemory.Memory.ReadUInt64(dwAddress);
                        return (T) obj2;
                    }
                    if (typeof(T) == typeof(UInt128))
                    {
                        obj2 = Memory.WowMemory.Memory.ReadUInt128(dwAddress);
                        return (T) obj2;
                    }
                    switch (System.Type.GetTypeCode(typeof(T)))
                    {
                        case TypeCode.Boolean:
                            obj2 = Memory.WowMemory.Memory.ReadShort(dwAddress) >= 0;
                            break;

                        case TypeCode.Char:
                            obj2 = (char) ((ushort) Memory.WowMemory.Memory.ReadShort(dwAddress));
                            break;

                        case TypeCode.Byte:
                            obj2 = Memory.WowMemory.Memory.ReadByte(dwAddress);
                            break;

                        case TypeCode.Int16:
                            obj2 = Memory.WowMemory.Memory.ReadShort(dwAddress);
                            break;

                        case TypeCode.UInt16:
                            obj2 = Memory.WowMemory.Memory.ReadUShort(dwAddress);
                            break;

                        case TypeCode.Int32:
                            obj2 = Memory.WowMemory.Memory.ReadInt(dwAddress);
                            break;

                        case TypeCode.UInt32:
                            obj2 = Memory.WowMemory.Memory.ReadUInt(dwAddress);
                            break;

                        case TypeCode.Int64:
                            obj2 = Memory.WowMemory.Memory.ReadInt64(dwAddress);
                            break;

                        case TypeCode.UInt64:
                            obj2 = Memory.WowMemory.Memory.ReadUInt64(dwAddress);
                            break;

                        case TypeCode.Single:
                            obj2 = Memory.WowMemory.Memory.ReadFloat(dwAddress);
                            break;

                        case TypeCode.Double:
                            obj2 = Memory.WowMemory.Memory.ReadDouble(dwAddress);
                            break;
                    }
                    if (obj2 != null)
                    {
                        return (T) obj2;
                    }
                    Logging.WriteError("WoWObject > GetDescriptor<T>(uint baseAddress, uint field): Value not found", true);
                }
                return default(T);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWObject > GetDescriptor<T>(uint baseAddress, uint field): " + exception, true);
                return default(T);
            }
        }

        internal void UpdateBaseAddress(uint address)
        {
            try
            {
                this.BaseAddress = address;
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWObject > UpdateBaseAddress(uint address): " + exception, true);
            }
        }

        protected uint BaseAddress { get; private set; }

        public int Entry
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>(Descriptors.ObjectFields.EntryID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > Entry: " + exception, true);
                    return 0;
                }
            }
        }

        public uint GetBaseAddress
        {
            get
            {
                try
                {
                    if (!this.IsValid)
                    {
                        return 0;
                    }
                    return this.BaseAddress;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > GetBaseAddress: " + exception, true);
                }
                return 0;
            }
        }

        public virtual float GetDistance
        {
            get
            {
                try
                {
                    return 0f;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > GetDistance: " + exception, true);
                    return 0f;
                }
            }
        }

        public int GetDynamicFlags
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>((uint) 10);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > Type: " + exception, true);
                    return 0;
                }
            }
        }

        public UInt128 Guid
        {
            get
            {
                try
                {
                    if (this.BaseAddress != 0)
                    {
                        return this.GetDescriptor<UInt128>(Descriptors.ObjectFields.Guid);
                    }
                    return 0;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > Guid: " + exception, true);
                    return 0;
                }
            }
        }

        public bool IsValid
        {
            get
            {
                try
                {
                    return ((this.BaseAddress != 0) && nManager.Wow.ObjectManager.ObjectManager.ObjectDictionary.ContainsKey(this.Guid));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > IsValid: " + exception, true);
                    return false;
                }
            }
        }

        public virtual string Name
        {
            get
            {
                try
                {
                    return string.Empty;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > Name: " + exception, true);
                    return "";
                }
            }
        }

        public virtual Point Position
        {
            get
            {
                try
                {
                    return new Point(0f, 0f, 0f, "None");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > Position: " + exception, true);
                    return new Point();
                }
            }
        }

        public float Scale
        {
            get
            {
                try
                {
                    float descriptor = this.GetDescriptor<float>(Descriptors.ObjectFields.Scale);
                    return ((descriptor > 0f) ? descriptor : 1f);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > Scale: " + exception, true);
                    return 0f;
                }
            }
        }

        public WoWObjectType Type
        {
            get
            {
                try
                {
                    return (WoWObjectType) Memory.WowMemory.Memory.ReadInt(this.BaseAddress + 12);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWObject > Type: " + exception, true);
                    return WoWObjectType.Object;
                }
            }
        }

        public bool UnitNearest
        {
            get
            {
                List<WoWUnit> objectWoWUnit = nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWUnit();
                float num = 0f;
                foreach (WoWUnit unit in objectWoWUnit)
                {
                    if ((unit.IsAlive && (unit.Position.DistanceTo2D(this.Position) <= (unit.AggroDistance + 1f))) && (UnitRelation.GetReaction(nManager.Wow.ObjectManager.ObjectManager.Me, unit) == Reaction.Hostile))
                    {
                        if (unit.MaxHealth > (nManager.Wow.ObjectManager.ObjectManager.Me.MaxHealth / 2))
                        {
                            num++;
                        }
                        else if (unit.MaxHealth > (nManager.Wow.ObjectManager.ObjectManager.Me.MaxHealth / 10))
                        {
                            num += 0.5f;
                        }
                    }
                }
                bool flag = num > nManagerSetting.CurrentSetting.DontHarvestIfMoreThanXUnitInAggroRange;
                if (flag)
                {
                    nManagerSetting.AddBlackList(this.Guid, 0x3a98);
                    Logging.Write(num + " hostile Units Near " + this.Name);
                }
                return flag;
            }
        }
    }
}

