namespace nManager.Wow.ObjectManager
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Patchables;
    using System;

    public class WoWAreaTrigger : WoWObject
    {
        private uint _boeda;
        private nManager.Wow.Class.Spell _oqiuruemawouseIp;

        public WoWAreaTrigger(uint address) : base(address)
        {
        }

        public T GetDescriptor<T>(Descriptors.AreaTriggerFields field) where T: struct
        {
            try
            {
                return base.Tuecom<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.AreaTriggerFields field): " + exception, true);
                return default(T);
            }
        }

        public override string ToString()
        {
            return string.Format("AreaTrigger: {0}BaseAddress: {1}, {0}Caster: {2}, {0}OverrideScaleCurve: {3}, {0}ExtraScaleCurve: {4}, {0}Duration: {5}, {0}TimeToTarget: {6}, {0}{0}TimeToTargetScale: {7}, {0}TimeToTargetExtraScale: {8}, {0}SpellID: {9}, {0}SpellVisualID: {10}, {0}BoundsRadius2D: {11}, {0}Faction: {12}, {0}Name: {13}, {0}Entry: {14}", new object[] { Environment.NewLine, base.get_BaseAddress(), this.Caster, "", "", this.Duration, this.TimeToTarget, this.TimeToTargetScale, this.TimeToTargetExtraScale, this.SpellID, this.SpellXSpellVisualID, this.BoundsRadius2D, this.Faction, this.Name, base.Entry });
        }

        public float[] BoundsRadius2D
        {
            get
            {
                try
                {
                    return new float[] { this.GetDescriptor<float>(Descriptors.AreaTriggerFields.BoundsRadius2D), this.GetDescriptor<float>((Descriptors.AreaTriggerFields) 0x1b) };
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > Radius: " + exception, true);
                }
                return new float[2];
            }
        }

        public UInt128 Caster
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UInt128>(Descriptors.AreaTriggerFields.Caster);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > Caster: " + exception, true);
                }
                return 0;
            }
        }

        public uint DecalPropertiesID
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.AreaTriggerFields.DecalPropertiesID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > DecalPropertiesID: " + exception, true);
                }
                return 0;
            }
        }

        public uint Duration
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.AreaTriggerFields.Duration);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > Duration: " + exception, true);
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
                    Logging.WriteError("AreaTriggerFields > Faction: " + exception, true);
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
                    return ((this.SpellID > 0) && base.IsValid);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > IsValid: " + exception, true);
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
                    return this.Spell.NameInGame;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > Name: " + exception, true);
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
                    Logging.WriteError("AreaTriggerFields > Position: " + exception, true);
                }
                return new Point();
            }
        }

        public float Rotation
        {
            get
            {
                try
                {
                    Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0x148);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > Rotation: " + exception, true);
                }
                return 0f;
            }
        }

        public nManager.Wow.Class.Spell Spell
        {
            get
            {
                if (this._oqiuruemawouseIp != null)
                {
                    return this._oqiuruemawouseIp;
                }
                if (base.Entry > 0)
                {
                    this._oqiuruemawouseIp = new nManager.Wow.Class.Spell((uint) base.Entry);
                    return this._oqiuruemawouseIp;
                }
                return new nManager.Wow.Class.Spell(0);
            }
        }

        public uint SpellID
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + 0x88);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > SpellID: " + exception, true);
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
                    return this.GetDescriptor<uint>(Descriptors.AreaTriggerFields.SpellXSpellVisualID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > SpellVisualID: " + exception, true);
                }
                return 0;
            }
        }

        public uint TimeToTarget
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTarget);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTarget: " + exception, true);
                }
                return 0;
            }
        }

        public uint TimeToTargetExtraScale
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTargetExtraScale);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTargetExtraScale: " + exception, true);
                }
                return 0;
            }
        }

        public uint TimeToTargetScale
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.AreaTriggerFields.TimeToTargetScale);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("AreaTriggerFields > TimeToTargetScale: " + exception, true);
                }
                return 0;
            }
        }
    }
}

