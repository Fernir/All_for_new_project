namespace nManager.Wow.ObjectManager
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.Patchables;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class WoWUnit : WoWObject
    {
        private static List<uint> _ghostSpells = new List<uint>();
        private Point _lastPosMove;
        private static readonly List<uint> FlagsIds = new List<uint>();

        public WoWUnit(uint address) : base(address)
        {
            this._lastPosMove = new Point();
        }

        public bool AuraIsActiveAndExpireInLessThanMs(uint idBuff, uint expireInLessThanMs, bool fromMe = false)
        {
            return ((this.AuraTimeLeft(idBuff, fromMe) > 0) && (this.AuraTimeLeft(idBuff, fromMe) < expireInLessThanMs));
        }

        public int AuraTimeLeft(uint idBuff, bool fromMe = false)
        {
            nManager.Wow.Class.Auras.UnitAura aura = this.UnitAura(idBuff);
            if (!aura.IsActive || (fromMe && !(aura.AuraCreatorGUID == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)))
            {
                return 0;
            }
            return aura.AuraTimeLeftInMs;
        }

        public int BuffStack(uint idBuff)
        {
            try
            {
                List<uint> buffId = new List<uint> {
                    idBuff
                };
                return BuffManager.AuraStack(base.BaseAddress, buffId);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > BuffStack(UInt32 idBuffs): " + exception, true);
                return -1;
            }
        }

        public int BuffStack(List<uint> idBuffs)
        {
            try
            {
                return BuffManager.AuraStack(base.BaseAddress, idBuffs);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > BuffStack(List<UInt32> idBuffs): " + exception, true);
                return -1;
            }
        }

        public T GetDescriptor<T>(Descriptors.UnitFields field) where T: struct
        {
            try
            {
                return base.GetDescriptor<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > GetDescriptor<T>(Descriptors.UnitFields field): " + exception, true);
                return default(T);
            }
        }

        public uint GetMaxPowerByPowerType(PowerType powerType)
        {
            uint powerIndexByPowerType = this.GetPowerIndexByPowerType(powerType);
            uint num2 = Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + Descriptors.StartDescriptors);
            return Memory.WowMemory.Memory.ReadUInt(num2 + (0x110 + (powerIndexByPowerType * 4)));
        }

        public uint GetPowerByPowerType(PowerType powerType)
        {
            uint powerIndexByPowerType = this.GetPowerIndexByPowerType(powerType);
            uint num2 = Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + Descriptors.StartDescriptors);
            return Memory.WowMemory.Memory.ReadUInt(num2 + (0xf4 + (powerIndexByPowerType * 4)));
        }

        private uint GetPowerIndexByPowerType(PowerType powerType)
        {
            uint num2 = Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + Descriptors.StartDescriptors) + 0xe4;
            uint num3 = (uint) ((((PowerType) Memory.WowMemory.Memory.ReadByte(num2 + 1)) + powerType) + (0x10 * Memory.WowMemory.Memory.ReadByte(num2 + 1)));
            return Memory.WowMemory.Memory.ReadUInt((Memory.WowProcess.WowModule + 0xd2f1bc) + (num3 * 4));
        }

        public bool HaveBuff(uint idBuffs)
        {
            try
            {
                return BuffManager.HaveBuff(base.BaseAddress, idBuffs);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > HaveBuff(UInt32 idBuffs): " + exception, true);
                return false;
            }
        }

        public bool HaveBuff(List<uint> idBuffs)
        {
            try
            {
                return BuffManager.HaveBuff(base.BaseAddress, idBuffs);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > HaveBuff(List<UInt32> idBuffs): " + exception, true);
                return false;
            }
        }

        private uint QuestItem(uint offset)
        {
            try
            {
                if (offset > 3)
                {
                    return 0;
                }
                return Memory.WowMemory.Memory.ReadUInt((this.GetDbCacheRowPtr + 60) + (4 * offset));
            }
            catch (Exception exception)
            {
                Logging.WriteError(string.Concat(new object[] { "WoWUnit > QuestItem(", offset, "): ", exception }), true);
                return 0;
            }
        }

        public nManager.Wow.Class.Auras.UnitAura UnitAura(List<uint> idBuffs)
        {
            foreach (nManager.Wow.Class.Auras.UnitAura aura in this.UnitAuras.Auras)
            {
                if (idBuffs.Contains(aura.AuraSpellId))
                {
                    return aura;
                }
            }
            return new nManager.Wow.Class.Auras.UnitAura();
        }

        public nManager.Wow.Class.Auras.UnitAura UnitAura(uint idBuff)
        {
            List<uint> idBuffs = new List<uint> {
                idBuff
            };
            return this.UnitAura(idBuffs);
        }

        public nManager.Wow.Class.Auras.UnitAura UnitAura(List<uint> idBuffs, UInt128 creatorGUID)
        {
            foreach (nManager.Wow.Class.Auras.UnitAura aura in this.UnitAuras.Auras)
            {
                if (idBuffs.Contains(aura.AuraSpellId) && (aura.AuraCreatorGUID == creatorGUID))
                {
                    return aura;
                }
            }
            return new nManager.Wow.Class.Auras.UnitAura();
        }

        public nManager.Wow.Class.Auras.UnitAura UnitAura(uint idBuff, UInt128 creatorGUID)
        {
            List<uint> idBuffs = new List<uint> {
                idBuff
            };
            return this.UnitAura(idBuffs, creatorGUID);
        }

        public float AggroDistance
        {
            get
            {
                try
                {
                    float num = ((this.Level <= 5) ? ((float) 12) : ((float) 20)) - ((nManager.Wow.ObjectManager.ObjectManager.Me.Level - this.Level) * 1.1f);
                    if (num < 5f)
                    {
                        num = 5f;
                    }
                    else if (num > 45f)
                    {
                        num = 45f;
                    }
                    return num;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > AggroDistance: " + exception, true);
                    return 20f;
                }
            }
        }

        public uint Alternate
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Alternate);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Alternate: " + exception, true);
                    return 0;
                }
            }
        }

        public uint AlternatePercentage
        {
            get
            {
                try
                {
                    return ((this.Alternate * 100) / this.MaxAlternate);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > AlternatePercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ArcaneCharges
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.ArcaneCharges);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > ArcaneCharges: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ArcaneChargesPercentage
        {
            get
            {
                try
                {
                    return ((this.ArcaneCharges * 100) / this.MaxArcaneCharges);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > ArcaneChargesPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public bool Attackable
        {
            get
            {
                try
                {
                    return (((this.GetDescriptor<uint>(Descriptors.UnitFields.Flags) & 0x10382) == 0) && ((UnitRelation.GetReaction(this.Faction) < nManager.Wow.Enums.Reaction.Neutral) || ((UnitRelation.GetReaction(this.Faction) == nManager.Wow.Enums.Reaction.Neutral) && ((this.GetDescriptor<uint>(Descriptors.UnitFields.NpcFlags) == 0) || ((base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Target) && this.CanAttackTargetLUA)))));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Attackable: " + exception, true);
                    return false;
                }
            }
        }

        public bool AutoAttack
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.PetInCombat);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > AutoAttack: " + exception, true);
                    return false;
                }
            }
        }

        public uint BurningEmbers
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.BurningEmbers);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > BurningEmbers: " + exception, true);
                    return 0;
                }
            }
        }

        public uint BurningEmbersPercentage
        {
            get
            {
                try
                {
                    return ((this.BurningEmbers * 100) / this.MaxBurningEmbers);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > BurningEmbersPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public bool CanAttackTargetLUA
        {
            get
            {
                try
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = tostring(UnitCanAttack(\"player\", \"target\"))", false, true);
                    return (Lua.GetLocalizedText(randomString) == "true");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > CanAttackLUA: " + exception, true);
                    return false;
                }
            }
        }

        public bool CanInterruptCurrentCast
        {
            get
            {
                try
                {
                    if (this.IsCast)
                    {
                        uint num = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xe02ea0);
                        uint num2 = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xe02ea4);
                        uint num3 = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xe02ea8);
                        return (((Memory.WowMemory.Memory.ReadByte(base.GetBaseAddress + 0xf2c) & 8) == 0) && (((num > 0) || (num2 > 0)) || (num3 > 0)));
                    }
                    return false;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > CanInterruptCurrentCast: " + exception, true);
                    return false;
                }
            }
        }

        public bool CanTurnIn
        {
            get
            {
                try
                {
                    nManager.Wow.Enums.UnitQuestGiverStatus status = (nManager.Wow.Enums.UnitQuestGiverStatus) Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0xf4);
                    return (((status.HasFlag(nManager.Wow.Enums.UnitQuestGiverStatus.TurnIn) || status.HasFlag(nManager.Wow.Enums.UnitQuestGiverStatus.TurnInInvisible)) || status.HasFlag(nManager.Wow.Enums.UnitQuestGiverStatus.TurnInRepeatable)) || status.HasFlag(nManager.Wow.Enums.UnitQuestGiverStatus.LowLevelTurnInRepeatable));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > CanTurnIn: " + exception, true);
                    return false;
                }
            }
        }

        public int CastEndsInMs
        {
            get
            {
                if ((this.IsCast && (this.CastingEndTime > 0)) && (this.CastingEndTime > Usefuls.GetWoWTime))
                {
                    return (this.CastingEndTime - Usefuls.GetWoWTime);
                }
                return 0;
            }
        }

        public int CastingEndTime
        {
            get
            {
                if (this.CurrentSpellIdCast > 0)
                {
                    return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0xfb4);
                }
                if (this.CurrentSpellIdChannel > 0)
                {
                    return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0xfc0);
                }
                return 0;
            }
        }

        public int CastingSpellId
        {
            get
            {
                if (this.CurrentSpellIdCast > 0)
                {
                    return this.CurrentSpellIdCast;
                }
                if (this.CurrentSpellIdChannel <= 0)
                {
                    return 0;
                }
                return this.CurrentSpellIdChannel;
            }
        }

        public uint CastingStartTime
        {
            get
            {
                if (this.CurrentSpellIdCast > 0)
                {
                    return Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xfb0);
                }
                if (this.CurrentSpellIdChannel > 0)
                {
                    return Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xfbc);
                }
                return 0;
            }
        }

        public uint Chi
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.LightForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Chi: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ChiPercentage
        {
            get
            {
                try
                {
                    return ((this.Chi * 100) / this.MaxChi);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > ChiPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ComboPoint
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.ComboPoint);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > ComboPoint: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ComboPointPercentage
        {
            get
            {
                try
                {
                    return ((this.ComboPoint * 100) / this.MaxComboPoint);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > ComboPointPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public UInt128 CreatedBy
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UInt128>(Descriptors.UnitFields.CreatedBy);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > CreatedBy: " + exception, true);
                    return 0;
                }
            }
        }

        public string CreatureRankTarget
        {
            get
            {
                string localizedText;
                lock (this)
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = UnitClassification(\"target\")", false, true);
                        localizedText = Lua.GetLocalizedText(randomString);
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("WoWUnit > CreatureRankTarget: " + exception, true);
                        localizedText = "";
                    }
                }
                return localizedText;
            }
        }

        public string CreatureTypeTarget
        {
            get
            {
                string localizedText;
                lock (this)
                {
                    try
                    {
                        string randomString = Others.GetRandomString(Others.Random(4, 10));
                        Lua.LuaDoString(randomString + " = UnitCreatureType(\"target\")", false, true);
                        localizedText = Lua.GetLocalizedText(randomString);
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("WoWUnit > CreatureTypeTarget: " + exception, true);
                        localizedText = "";
                    }
                }
                return localizedText;
            }
        }

        public int CurrentSpellIdCast
        {
            get
            {
                return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0xf98);
            }
        }

        public int CurrentSpellIdChannel
        {
            get
            {
                return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0xfb8);
            }
        }

        public uint DarkForce
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.DarkForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > DarkForce: " + exception, true);
                    return 0;
                }
            }
        }

        public uint DarkForcePercentage
        {
            get
            {
                try
                {
                    return ((this.DarkForce * 100) / this.MaxDarkForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > DarkForcePercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint DemonicFury
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.DemonicFury);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > DemonicFury: " + exception, true);
                    return 0;
                }
            }
        }

        public uint DemonicFuryPercentage
        {
            get
            {
                try
                {
                    return ((this.DemonicFury * 100) / this.MaxDemonicFury);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > DemonicFuryPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint DisplayId
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.UnitFields.DisplayID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > DisplayId: " + exception, true);
                    return 0;
                }
            }
        }

        public int Eclipse
        {
            get
            {
                try
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    return Others.ToInt32(Lua.LuaDoString(randomString + " = UnitPower('player',8)", randomString, false));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Eclipse: " + exception, true);
                    return 0;
                }
            }
        }

        public uint Energy
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Energy);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Energy: " + exception, true);
                    return 0;
                }
            }
        }

        public uint EnergyPercentage
        {
            get
            {
                try
                {
                    return ((this.Energy * 100) / this.MaxEnergy);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > EnergyPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public TypeFlag ExtraLootType
        {
            get
            {
                try
                {
                    return (TypeFlag) Memory.WowMemory.Memory.ReadInt(this.GetDbCacheRowPtr + 0x24);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > ExtraLootType: " + exception, true);
                    return TypeFlag.None;
                }
            }
        }

        public uint Faction
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.UnitFields.FactionTemplate);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Faction: " + exception, true);
                    return 0;
                }
            }
        }

        public WoWFactionTemplate FactionTemplate
        {
            get
            {
                try
                {
                    if (this.Faction == 0)
                    {
                        return null;
                    }
                    return WoWFactionTemplate.FromId(this.Faction);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > FactionTemplate: " + exception, true);
                    return null;
                }
            }
        }

        public uint Focus
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Focus);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Focus: " + exception, true);
                    return 0;
                }
            }
        }

        public uint FocusPercentage
        {
            get
            {
                try
                {
                    return ((this.Focus * 100) / this.MaxFocus);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > FocusPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public float GetBoundingRadius
        {
            get
            {
                try
                {
                    return this.GetDescriptor<float>(Descriptors.UnitFields.BoundingRadius);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > BoundingRadius: " + exception, true);
                    return 0f;
                }
            }
        }

        public float GetCombatReach
        {
            get
            {
                try
                {
                    return this.GetDescriptor<float>(Descriptors.UnitFields.CombatReach);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > GetCombatReach: " + exception, true);
                    return 0f;
                }
            }
        }

        public PartyEnums.PartyType GetCurrentPartyType
        {
            get
            {
                uint partyPointer = nManager.Wow.Helpers.Party.GetPartyPointer(PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE);
                uint num2 = nManager.Wow.Helpers.Party.GetPartyPointer(PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME);
                if (partyPointer > 0)
                {
                    return PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE;
                }
                if (num2 <= 0)
                {
                    return PartyEnums.PartyType.None;
                }
                return PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME;
            }
        }

        public PartyEnums.PartyType GetCurrentPartyTypeLUA
        {
            get
            {
                if (nManager.Wow.Helpers.Party.IsInGroupLUA(PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE))
                {
                    return PartyEnums.PartyType.LE_PARTY_CATEGORY_INSTANCE;
                }
                if (nManager.Wow.Helpers.Party.IsInGroupLUA(PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME))
                {
                    return PartyEnums.PartyType.LE_PARTY_CATEGORY_HOME;
                }
                return PartyEnums.PartyType.None;
            }
        }

        private uint GetDbCacheRowPtr
        {
            get
            {
                return Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0xc38);
            }
        }

        public override float GetDistance
        {
            get
            {
                try
                {
                    return this.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > GetDistance: " + exception, true);
                    return 0f;
                }
            }
        }

        public float GetDistance2D
        {
            get
            {
                try
                {
                    return this.Position.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > GetDistance2D: " + exception, true);
                    return 0f;
                }
            }
        }

        public bool GetMove
        {
            get
            {
                try
                {
                    bool flag = false;
                    this._lastPosMove = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    Thread.Sleep(50);
                    if (((System.Math.Round((double) this._lastPosMove.X, 1) != System.Math.Round((double) nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, 1)) || (System.Math.Round((double) this._lastPosMove.Z, 1) != System.Math.Round((double) nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 1))) || (System.Math.Round((double) this._lastPosMove.Y, 1) != System.Math.Round((double) nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, 1)))
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        Thread.Sleep(30);
                        if (((System.Math.Round((double) this._lastPosMove.X, 1) != System.Math.Round((double) nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, 1)) || (System.Math.Round((double) this._lastPosMove.Z, 1) != System.Math.Round((double) nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 1))) || (System.Math.Round((double) this._lastPosMove.Y, 1) != System.Math.Round((double) nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, 1)))
                        {
                            flag = true;
                        }
                    }
                    return flag;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > GetMove: " + exception, true);
                    return true;
                }
            }
        }

        public uint GetSkillLevelRequired
        {
            get
            {
                if (this.Level > 0x57)
                {
                    return (500 + ((this.Level - 0x57) * 20));
                }
                if (this.Level > 0x53)
                {
                    return (450 + ((this.Level - 0x53) * 20));
                }
                if (this.Level > 80)
                {
                    return (0x1b3 + ((this.Level - 80) * 5));
                }
                if (this.Level > 0x49)
                {
                    return (0x16d + ((this.Level - 0x49) * 10));
                }
                if (this.Level > 20)
                {
                    return (this.Level * 5);
                }
                if (this.Level < 10)
                {
                    return 1;
                }
                return ((this.Level * 10) - 100);
            }
        }

        public PartyRole GetUnitRole
        {
            get
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString(randomString + " = UnitGroupRolesAssigned(\"" + this.Name + "\")", false, true);
                switch (Lua.GetLocalizedText(randomString))
                {
                    case "TANK":
                        return PartyRole.Tank;

                    case "DAMAGER":
                        return PartyRole.DPS;

                    case "HEALER":
                        return PartyRole.Heal;
                }
                return PartyRole.None;
            }
        }

        public int Health
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>(Descriptors.UnitFields.Health);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Health: " + exception, true);
                    return 0;
                }
            }
        }

        public float HealthPercent
        {
            get
            {
                try
                {
                    long health = this.Health;
                    float num2 = ((float) (health * 100L)) / ((float) this.MaxHealth);
                    if ((num2 < 0f) || (num2 > 100f))
                    {
                        return 0f;
                    }
                    return num2;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > HealthPercent: " + exception, true);
                    return 0f;
                }
            }
        }

        public uint HolyPower
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.HolyPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > HolyPower: " + exception, true);
                    return 0;
                }
            }
        }

        public uint HolyPowerPercentage
        {
            get
            {
                try
                {
                    return ((this.HolyPower * 100) / this.MaxHolyPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > HolyPowerPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public bool InCombat
        {
            get
            {
                try
                {
                    if (((base.BaseAddress == nManager.Wow.ObjectManager.ObjectManager.Me.BaseAddress) && this.InTransport) && Usefuls.PlayerUsingVehicle)
                    {
                        return false;
                    }
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Combat);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > InCombat: " + exception, true);
                    return false;
                }
            }
        }

        public bool InCombatWithMe
        {
            get
            {
                try
                {
                    return ((this.InCombat && this.IsTargetingMe) && !this.IsDead);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > InCombatWithMe: " + exception, true);
                    return false;
                }
            }
        }

        public bool InPVP
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.PvPFlagged);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > InPVP: " + exception, true);
                    return false;
                }
            }
        }

        public bool InTransport
        {
            get
            {
                try
                {
                    return (this.TransportGuid > 0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > InTransport: " + exception, true);
                    return false;
                }
            }
        }

        public bool Invisible
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitDynamicFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitDynamicFlags.Invisible);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Invisible: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsAlive
        {
            get
            {
                try
                {
                    if (!this.IsValid)
                    {
                        return false;
                    }
                    return !this.IsDead;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsAlive: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsBoss
        {
            get
            {
                try
                {
                    int num2;
                    uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0x11c) + 0x180);
                    if ((this.GetDbCacheRowPtr <= 0) || (num > 0))
                    {
                        num2 = 0;
                    }
                    else
                    {
                        num2 = (Memory.WowMemory.Memory.ReadInt((uint) (this.GetDbCacheRowPtr + 0x5c)) >> 2) & 1;
                    }
                    return (num2 == 1);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsBoss: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsCast
        {
            get
            {
                try
                {
                    return (this.CastingSpellId > 0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsCast: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsConfused
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Confused);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsConfused: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsDead
        {
            get
            {
                try
                {
                    if (!this.IsValid)
                    {
                        return true;
                    }
                    if (this.IsNpcQuestGiver)
                    {
                        return false;
                    }
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        return nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe;
                    }
                    if (base.Type == WoWObjectType.Player)
                    {
                        if (_ghostSpells.Count <= 0)
                        {
                            _ghostSpells = SpellManager.SpellListManager.SpellIdByName("Ghost");
                        }
                        return (this.HaveBuff(_ghostSpells) || ((this.Health <= 1) || base.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead)));
                    }
                    return ((this.Health <= 0) || base.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsDead: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsHoldingWGFlag
        {
            get
            {
                try
                {
                    if (FlagsIds.Count <= 0)
                    {
                        FlagsIds.Add(0x5b25);
                        FlagsIds.Add(0x5b27);
                    }
                    return this.HaveBuff(FlagsIds);
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool IsHomePartyLeader
        {
            get
            {
                try
                {
                    return (nManager.Wow.Helpers.Party.GetPartyLeaderGUID() == base.Guid);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsHomePartyLeader: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsHostile
        {
            get
            {
                WoWUnit unit = this;
                if (unit.IsDead || !unit.IsValid)
                {
                    return false;
                }
                if (!(unit is WoWPlayer))
                {
                    return (unit.Reaction <= nManager.Wow.Enums.Reaction.Neutral);
                }
                WoWPlayer player = unit as WoWPlayer;
                if (!player.IsValid)
                {
                    return false;
                }
                if (player.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                {
                    return false;
                }
                if (player.PlayerFaction != nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction)
                {
                    return true;
                }
                string randomString = Others.GetRandomString(Others.Random(5, 10));
                return (Lua.LuaDoString(randomString + " = tostring(UnitIsEnemy(\"player\", \"target\"))", randomString, false) == "true");
            }
        }

        public bool IsInstancePartyLeader
        {
            get
            {
                try
                {
                    return (nManager.Wow.Helpers.Party.GetPartyLeaderGUID() == base.Guid);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsInstancePartyLeader: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsLootable
        {
            get
            {
                try
                {
                    return base.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Lootable);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsLootable: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsMounted
        {
            get
            {
                try
                {
                    return ((((this.GetDescriptor<int>(Descriptors.UnitFields.MountDisplayID) > 0) || this.HaveBuff(SpellManager.MountDruidId())) || this.InTransport) || Usefuls.IsFlying);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsMounted: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcAuctioneer
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Auctioneer);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcAuctioneer: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcFlightMaster
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Taxi);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcFlightMaster: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcInnkeeper
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Innkeeper);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcInnkeeper: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcMailbox
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.MailInfo);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcMailbox: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcQuestGiver
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.QuestGiver);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcQuestGiver: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcRepair
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.CanRepair);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcRepair: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcSpiritGuide
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.SpiritHealer);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcSpiritGuide: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcSpiritHealer
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.SpiritHealer);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcSpiritHealer: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcTrainer
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.CanTrain);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcTrainer: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcVendor
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.Vendor);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcVendor: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsNpcVendorFood
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitNPCFlags>(Descriptors.UnitFields.NpcFlags).HasFlag(UnitNPCFlags.SellsFood);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcVendorFood: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsSilenced
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.Silenced);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsSilenced: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsSkinnable
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.Skinnable);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsSkinnable: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsStunnable
        {
            get
            {
                try
                {
                    return (this.CreatureRankTarget != "worldboss");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsStunnable: " + exception, true);
                    return true;
                }
            }
        }

        public bool IsStunned
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.Stunned);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsStunned: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsTapped
        {
            get
            {
                try
                {
                    return base.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Tapped);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsTapped: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsTappedByMe
        {
            get
            {
                try
                {
                    return base.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.TappedByMe);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsTappedByMe: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsTargetingMe
        {
            get
            {
                try
                {
                    return (this.Target == nManager.Wow.ObjectManager.ObjectManager.Me.Guid);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsTargetingMe: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsTracked
        {
            get
            {
                try
                {
                    return base.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.TrackUnit);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsTracked get: " + exception, true);
                    return false;
                }
            }
            set
            {
                try
                {
                    long num2;
                    uint descriptor = base.GetDescriptor<uint>(Descriptors.ObjectFields.DynamicFlags);
                    if (value)
                    {
                        num2 = descriptor | 2;
                    }
                    else
                    {
                        num2 = descriptor & -3L;
                    }
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + Descriptors.StartDescriptors) + 40;
                    Memory.WowMemory.Memory.WriteInt64(dwAddress, num2);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsTracked set: " + exception, true);
                }
            }
        }

        public bool IsTrivial
        {
            get
            {
                if (this.IsBoss)
                {
                    return false;
                }
                uint level = this.Level;
                uint num2 = nManager.Wow.ObjectManager.ObjectManager.Me.Level;
                int num3 = (int) (num2 - level);
                if (num3 <= -3)
                {
                    return false;
                }
                if (num3 < 0)
                {
                    return ((nManager.Wow.ObjectManager.ObjectManager.Me.MaxHealth / 2) >= this.MaxHealth);
                }
                if (num3 < 5)
                {
                    return ((nManager.Wow.ObjectManager.ObjectManager.Me.MaxHealth * 1.5) >= this.MaxHealth);
                }
                if (num3 < 10)
                {
                    return ((nManager.Wow.ObjectManager.ObjectManager.Me.MaxHealth * 4) >= this.MaxHealth);
                }
                uint num4 = (uint) (((num2 / level) * 8) * nManager.Wow.ObjectManager.ObjectManager.Me.MaxHealth);
                return (num4 >= this.MaxHealth);
            }
        }

        public bool IsUnitBrawlerAndTappedByMe
        {
            get
            {
                if (!this.IsTappedByMe)
                {
                    return false;
                }
                if ((Usefuls.ContinentId != 0x413) && (Usefuls.ContinentId != 0x171))
                {
                    return false;
                }
                return true;
            }
        }

        public bool IsValid
        {
            get
            {
                try
                {
                    return ((base.BaseAddress != 0) && nManager.Wow.ObjectManager.ObjectManager.ObjectDictionary.ContainsKey(base.Guid));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsValid: " + exception, true);
                    return false;
                }
            }
        }

        public uint Level
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.UnitFields.Level);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Level: " + exception, true);
                    return 0;
                }
            }
        }

        public uint LightForce
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.LightForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > LightForce: " + exception, true);
                    return 0;
                }
            }
        }

        public uint LightForcePercentage
        {
            get
            {
                try
                {
                    return ((this.LightForce * 100) / this.MaxLightForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > LightForcePercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint Mana
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Mana);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Mana: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ManaPercentage
        {
            get
            {
                try
                {
                    if (this.MaxMana > 0)
                    {
                        return ((this.Mana * 100) / this.MaxMana);
                    }
                    return 100;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > ManaPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxAlternate
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Alternate);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxAlternate: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxArcaneCharges
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.ArcaneCharges);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxArcaneCharges: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxBurningEmbers
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.BurningEmbers);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxBurningEmbers: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxChi
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.LightForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxChi: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxComboPoint
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.ComboPoint);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxComboPoint: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxDarkForce
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.DarkForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxDarkForce: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxDemonicFury
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.DemonicFury);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxDemonicFury: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxEnergy
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Energy);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxEnergy: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxFocus
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Focus);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxFocus: " + exception, true);
                    return 0;
                }
            }
        }

        public int MaxHealth
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>(Descriptors.UnitFields.MaxHealth);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxHealth: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxHolyPower
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.HolyPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxHolyPower: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxLightForce
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.LightForce);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxLightForce: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxMana
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Mana);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxMana: " + exception, true);
                    return 0;
                }
            }
        }

        public float MaxRage
        {
            get
            {
                try
                {
                    return (((float) this.GetMaxPowerByPowerType(PowerType.Rage)) / 10f);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxRage: " + exception, true);
                    return 0f;
                }
            }
        }

        public uint MaxRunes
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Runes);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxRunes: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxRunicPower
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.RunicPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxRunicPower: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxShadowOrbs
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.ShadowOrbs);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxShadowOrbs: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxSoulShards
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.SoulShards);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxSoulShards: " + exception, true);
                    return 0;
                }
            }
        }

        public int ModelId
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadInt(this.GetDbCacheRowPtr + 0x6c);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > ModelId: " + exception, true);
                    return 0;
                }
            }
        }

        public int MountDisplayId
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>(Descriptors.UnitFields.MountDisplayID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MountDisplayId: " + exception, true);
                    return 0;
                }
            }
        }

        public override string Name
        {
            get
            {
                try
                {
                    if (base.BaseAddress == nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress)
                    {
                        return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + 0xe981d0);
                    }
                    if (base.Type == WoWObjectType.Player)
                    {
                        return Usefuls.GetPlayerName(base.Guid);
                    }
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(this.GetDbCacheRowPtr + 0x7c));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Name: " + exception, true);
                    return "";
                }
            }
        }

        public bool NotAttackable
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.NotAttackable);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > NotAttackable: " + exception, true);
                    return false;
                }
            }
        }

        public bool NotSelectable
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.NotSelectable);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > NotSelectable: " + exception, true);
                    return false;
                }
            }
        }

        public bool OnTaxi
        {
            get
            {
                return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.TaxiFlight);
            }
        }

        public float Orientation
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 0xad0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Orientation: " + exception, true);
                }
                return 0f;
            }
        }

        public bool PlayerControlled
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.PlayerControlled);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > PlayerControlled: " + exception, true);
                    return false;
                }
            }
        }

        public override Point Position
        {
            get
            {
                try
                {
                    if (base.BaseAddress == 0)
                    {
                        return new Point(0f, 0f, 0f, "None");
                    }
                    Point point = new Point(Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 0xac0), Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 0xac4), Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 0xac8), "None");
                    if (this.InTransport)
                    {
                        WoWObject obj2 = new WoWObject(nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(this.TransportGuid).GetBaseAddress);
                        if (obj2.Type == WoWObjectType.GameObject)
                        {
                            WoWGameObject obj3 = new WoWGameObject(obj2.GetBaseAddress);
                            if (obj3.IsValid)
                            {
                                Vector3 vector = point.Transform(obj3.WorldMatrix);
                                return new Point(vector.X, vector.Y, vector.Z, "None");
                            }
                        }
                        else if (obj2.Type == WoWObjectType.Unit)
                        {
                            WoWUnit unit = new WoWUnit(obj2.GetBaseAddress);
                            if (unit.IsValid && unit.IsAlive)
                            {
                                return unit.Position;
                            }
                        }
                        else if (obj2.Type == WoWObjectType.Player)
                        {
                            if (obj2.GetBaseAddress == nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress)
                            {
                                return nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                            }
                            WoWPlayer player = new WoWPlayer(obj2.GetBaseAddress);
                            if (player.IsValid && player.IsAlive)
                            {
                                return player.Position;
                            }
                        }
                    }
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        if (Usefuls.IsFlying)
                        {
                            point.Type = "Flying";
                        }
                        if (Usefuls.IsSwimming)
                        {
                            point.Type = "Swimming";
                        }
                    }
                    return point;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Position: " + exception, true);
                    return new Point(0f, 0f, 0f, "None");
                }
            }
        }

        public bool PVP
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.None | UnitFlags.PlayerControlled);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > PVP: " + exception, true);
                    return false;
                }
            }
        }

        public uint QuestItem1
        {
            get
            {
                return this.QuestItem(0);
            }
        }

        public uint QuestItem2
        {
            get
            {
                return this.QuestItem(1);
            }
        }

        public uint QuestItem3
        {
            get
            {
                return this.QuestItem(2);
            }
        }

        public uint QuestItem4
        {
            get
            {
                return this.QuestItem(3);
            }
        }

        public float Rage
        {
            get
            {
                try
                {
                    return (((float) this.GetPowerByPowerType(PowerType.Rage)) / 10f);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Rage: " + exception, true);
                    return 0f;
                }
            }
        }

        public uint RagePercentage
        {
            get
            {
                try
                {
                    return (uint) ((this.Rage * 100f) / this.MaxRage);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > RagePercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public nManager.Wow.Enums.Reaction Reaction
        {
            get
            {
                try
                {
                    return UnitRelation.GetReaction(nManager.Wow.ObjectManager.ObjectManager.Me.Faction, this.Faction);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Reaction: " + exception, true);
                    return nManager.Wow.Enums.Reaction.Neutral;
                }
            }
        }

        public uint Runes
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Runes);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Runes: " + exception, true);
                    return 0;
                }
            }
        }

        public uint RunesPercentage
        {
            get
            {
                try
                {
                    return ((this.Runes * 100) / this.MaxRunes);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > RunesPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint RunicPower
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.RunicPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > RunicPower: " + exception, true);
                    return 0;
                }
            }
        }

        public uint RunicPowerPercentage
        {
            get
            {
                try
                {
                    return ((this.RunicPower * 100) / this.MaxRunicPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > RunicPowerPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ShadowOrbs
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.ShadowOrbs);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > ShadowOrbs: " + exception, true);
                    return 0;
                }
            }
        }

        public uint ShadowOrbsPercentage
        {
            get
            {
                try
                {
                    return ((this.ShadowOrbs * 100) / this.MaxShadowOrbs);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > ShadowOrbsPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint SoulShards
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.SoulShards);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > SoulShards: " + exception, true);
                    return 0;
                }
            }
        }

        public uint SoulShardsPercentage
        {
            get
            {
                try
                {
                    return ((this.SoulShards * 100) / this.MaxSoulShards);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > SoulShardsPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public string SubName
        {
            get
            {
                try
                {
                    if (base.Type == WoWObjectType.Player)
                    {
                        return "";
                    }
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(this.GetDbCacheRowPtr));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > SubName: " + exception, true);
                    return "";
                }
            }
        }

        public UInt128 SummonedBy
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UInt128>(Descriptors.UnitFields.SummonedBy);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > SummonedBy: " + exception, true);
                    return 0;
                }
            }
        }

        public UInt128 Target
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UInt128>(Descriptors.UnitFields.Target);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > : Target get" + exception, true);
                    return 0;
                }
            }
            set
            {
                try
                {
                    Memory.WowMemory.Memory.WriteInt128(Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + Descriptors.StartDescriptors) + 160, value);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Target set: " + exception, true);
                }
            }
        }

        public UInt128 TransportGuid
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUInt128(base.GetBaseAddress + 0xab0);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > TransportGuid: " + exception, true);
                    return 0;
                }
            }
        }

        public nManager.Wow.Class.Auras.UnitAuras UnitAuras
        {
            get
            {
                return BuffManager.AuraStack(base.BaseAddress);
            }
        }

        public nManager.Wow.Enums.UnitQuestGiverStatus UnitQuestGiverStatus
        {
            get
            {
                return (nManager.Wow.Enums.UnitQuestGiverStatus) Memory.WowMemory.Memory.ReadInt(base.BaseAddress + 0xf4);
            }
        }

        public enum PartyRole
        {
            Tank,
            DPS,
            Heal,
            None
        }
    }
}

