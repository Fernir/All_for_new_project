namespace nManager.Wow.ObjectManager
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Patchables;
    using System;

    public class WoWDynamicObject : WoWObject
    {
        private uint _boeda;

        public WoWDynamicObject(uint address) : base(address)
        {
        }

        public T GetDescriptor<T>(Descriptors.DynamicObjectFields field) where T: struct
        {
            try
            {
                return base.Tuecom<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.DynamicObjectFields field): " + exception, true);
                return default(T);
            }
        }

        public override string ToString()
        {
            return string.Format("DynamicObject: {0}BaseAddress: {1}, {0}Caster: {2}, {0}TypeAndVisualID: {3}, {0}SpellID: {4}, {0}Radius: {5}, {0}CastTime: {6}, {0}", new object[] { Environment.NewLine, base.get_BaseAddress(), this.Caster, base.Type, this.SpellID, this.Radius, this.CastTime });
        }

        public UInt128 Caster
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UInt128>(Descriptors.DynamicObjectFields.Caster);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("DynamicObjectFields > Caster: " + exception, true);
                }
                return 0;
            }
        }

        public uint CastTime
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.DynamicObjectFields.CastTime);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > CastTime: " + exception, true);
                }
                return 0;
            }
        }

        public uint Faction
        {
            get
            {
                try
                {
                    if (this._boeda == 0)
                    {
                        WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(this.Caster);
                        if ((objectByGuid.Type == WoWObjectType.Unit) && (objectByGuid is WoWUnit))
                        {
                            this._boeda = (objectByGuid as WoWUnit).Faction;
                        }
                        if ((objectByGuid.Type == WoWObjectType.GameObject) && (objectByGuid is WoWGameObject))
                        {
                            this._boeda = (objectByGuid as WoWGameObject).Faction;
                        }
                    }
                    return this._boeda;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWGameObject > Faction: " + exception, true);
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
                    return (((this.SpellID > 0) && (this.Radius > 0f)) && base.IsValid);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWGameObject > IsValid: " + exception, true);
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
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + 620) + 180));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > Name: " + exception, true);
                }
                return "";
            }
        }

        public override Point Position
        {
            get
            {
                try
                {
                    return new Point(Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0x138), Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0x13c), Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 320), "None");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > Position: " + exception, true);
                }
                return new Point();
            }
        }

        public float Radius
        {
            get
            {
                try
                {
                    return this.GetDescriptor<float>(Descriptors.DynamicObjectFields.Radius);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > Radius: " + exception, true);
                }
                return 0f;
            }
        }

        public uint SpellID
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.DynamicObjectFields.SpellID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > SpellID: " + exception, true);
                }
                return 0;
            }
        }

        public uint SpellXSpellVisualID
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.DynamicObjectFields.SpellXSpellVisualID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > SpellXSpellVisualID: " + exception, true);
                }
                return 0;
            }
        }
    }
}

