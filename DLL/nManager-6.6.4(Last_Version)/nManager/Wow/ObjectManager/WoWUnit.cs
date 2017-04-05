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
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Windows.Forms;

    public class WoWUnit : WoWObject
    {
        private bool _asoemaowaEbuhaToi;
        private readonly List<Eheanuleucud> _cachedUnitIdInfo;
        private readonly nManager.Helpful.Timer _cleanTimer;
        private Point _etusojor;
        private static List<uint> _gadesiSuer = new List<uint>();
        private bool _loipo;
        public static List<uint> CombatMount = new List<uint> { 0x2817e, 0x287ab };
        private static readonly List<uint> FlagsIds = new List<uint>();

        public WoWUnit(uint address) : base(address)
        {
            this._cachedUnitIdInfo = new List<Eheanuleucud>();
            this._cleanTimer = new nManager.Helpful.Timer(600000.0);
            this._etusojor = new Point();
        }

        public bool AuraIsActiveAndExpireInLessThanMs(uint idBuff, uint expireInLessThanMs, bool fromMe = false)
        {
            int num = this.AuraTimeLeft(idBuff, fromMe);
            if (num <= 0)
            {
                return false;
            }
            return (num <= expireInLessThanMs);
        }

        public bool AuraIsInactiveOrExpireInLessThanMs(uint idBuff, uint expireInLessThanMs, bool fromMe = false)
        {
            int num = this.AuraTimeLeft(idBuff, fromMe);
            return ((num <= 0) || (num <= expireInLessThanMs));
        }

        public int AuraTimeLeft(uint idBuff, bool fromMe = false)
        {
            nManager.Wow.Class.Auras.UnitAura aura = fromMe ? this.UnitAura(idBuff, nManager.Wow.ObjectManager.ObjectManager.Me.Guid) : this.UnitAura(idBuff);
            if (aura.IsValid)
            {
                return aura.AuraTimeLeftInMs;
            }
            return 0;
        }

        public int BuffStack(uint idBuff)
        {
            try
            {
                List<uint> homekuatasiUpeloepa = new List<uint> {
                    idBuff
                };
                return Umameireovoap.WacauGameNoiv(base.get_BaseAddress(), homekuatasiUpeloepa);
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
                return Umameireovoap.WacauGameNoiv(base.get_BaseAddress(), idBuffs);
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
                return base.Tuecom<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > GetDescriptor<T>(Descriptors.UnitFields field): " + exception, true);
                return default(T);
            }
        }

        public uint GetMaxPowerByPowerType(PowerType powerType)
        {
            uint num = this.Ilaiwir(powerType);
            uint num2 = Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + Descriptors.StartDescriptors);
            return Memory.WowMemory.Memory.ReadUInt(num2 + (0x108 + (num * 4)));
        }

        public uint GetPlayerInSpellRange(float range = 5f, bool friendly = true)
        {
            return nManager.Wow.ObjectManager.ObjectManager.GetPlayerInSpellRange(range, friendly, this);
        }

        public uint GetPowerByPowerType(PowerType powerType)
        {
            uint num = this.Ilaiwir(powerType);
            uint num2 = Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + Descriptors.StartDescriptors);
            return Memory.WowMemory.Memory.ReadUInt(num2 + (0xe8 + (num * 4)));
        }

        public string GetUnitId()
        {
            if (this._cleanTimer.IsReady)
            {
                if (this._cachedUnitIdInfo.Count > 1)
                {
                    for (int i = this._cachedUnitIdInfo.Count; i >= 0; i--)
                    {
                        if (this._cachedUnitIdInfo[i]._haofeuvawe > (Environment.TickCount - 0xea60))
                        {
                            this._cachedUnitIdInfo.RemoveAt(i);
                        }
                    }
                }
                this._cleanTimer.Reset();
            }
            if (this._cachedUnitIdInfo.Count > 1)
            {
                for (int j = this._cachedUnitIdInfo.Count; j >= 0; j--)
                {
                    Eheanuleucud eheanuleucud = this._cachedUnitIdInfo[j];
                    if ((eheanuleucud._desalea == base.Guid) && (eheanuleucud._haofeuvawe < (Environment.TickCount - 0xea60)))
                    {
                        return eheanuleucud._piutiteawa;
                    }
                    if (eheanuleucud._desalea == base.Guid)
                    {
                        this._cachedUnitIdInfo.RemoveAt(j);
                        break;
                    }
                }
            }
            string str = this.QoveruePi();
            Eheanuleucud item = new Eheanuleucud {
                _desalea = base.Guid,
                _haofeuvawe = Environment.TickCount,
                _piutiteawa = str
            };
            this._cachedUnitIdInfo.Add(item);
            return str;
        }

        public uint GetUnitInSpellRange(float range = 5f)
        {
            return nManager.Wow.ObjectManager.ObjectManager.GetUnitInSpellRange(range, this);
        }

        public bool HaveBuff(List<uint> idBuffs)
        {
            try
            {
                return Umameireovoap.Erefaude(base.get_BaseAddress(), idBuffs);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > HaveBuff(List<UInt32> idBuffs): " + exception, true);
                return false;
            }
        }

        public bool HaveBuff(uint idBuffs)
        {
            try
            {
                return Umameireovoap.Erefaude(base.get_BaseAddress(), idBuffs);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWUnit > HaveBuff(UInt32 idBuffs): " + exception, true);
                return false;
            }
        }

        private uint Ilaiwir(PowerType gekanoagUdia)
        {
            uint wowClass = (uint) nManager.Wow.ObjectManager.ObjectManager.Me.WowClass;
            uint num2 = (uint) ((((PowerType) wowClass) + gekanoagUdia) + ((PowerType) (0x12 * wowClass)));
            return Memory.WowMemory.Memory.ReadUInt((Memory.WowProcess.WowModule + 0xec4454) + (num2 * 4));
        }

        public bool IsInRange(float range)
        {
            Vector3 vector = this.Position - nManager.Wow.ObjectManager.ObjectManager.Me.Position;
            float num = (vector.X < 0f) ? -vector.X : vector.X;
            float num2 = (vector.Y < 0f) ? -vector.Y : vector.Y;
            float num3 = (vector.Z < 0f) ? -vector.Z : vector.Z;
            if (((num > range) || (num2 > range)) || (num3 > range))
            {
                return false;
            }
            if (this.GetDistance > range)
            {
                return false;
            }
            return true;
        }

        private uint Ojabak(uint umaked)
        {
            try
            {
                if (umaked > 3)
                {
                    return 0;
                }
                return Memory.WowMemory.Memory.ReadUInt((this.MeufefiuComekujab() + 60) + (4 * umaked));
            }
            catch (Exception exception)
            {
                Logging.WriteError(string.Concat(new object[] { "WoWUnit > QuestItem(", umaked, "): ", exception }), true);
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
            List<nManager.Wow.Class.Auras.UnitAura> auras = this.UnitAuras.Auras;
            for (int i = 0; i < auras.Count; i++)
            {
                nManager.Wow.Class.Auras.UnitAura aura = auras[i];
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

        private uint _inoufiate
        {
            get
            {
                return Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + 0xc68);
            }
        }

        private string _piutiteawa
        {
            get
            {
                if (this.IsValid)
                {
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        return "player";
                    }
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Pet.Guid)
                    {
                        return "pet";
                    }
                    if ((base.Type == WoWObjectType.Player) && nManager.Wow.Helpers.Party.IsInGroup())
                    {
                        if (nManager.Wow.Helpers.Party.GetPartyPlayersGUID().Contains(base.Guid))
                        {
                            string commandline = Others.GetRandomString(Others.Random(5, 10));
                            uint partyNumberPlayers = nManager.Wow.Helpers.Party.GetPartyNumberPlayers();
                            if (nManager.Wow.Helpers.Party.GetPartyNumberPlayers() <= 5)
                            {
                                for (int j = 1; j <= partyNumberPlayers; j++)
                                {
                                    Lua.LuaDoString(string.Concat(new object[] { commandline, " = UnitName(\"party", j, "\");" }), false, true);
                                    string localizedText = Lua.GetLocalizedText(commandline);
                                    if (localizedText == this.Name)
                                    {
                                        return ("party" + j);
                                    }
                                    if (localizedText == "nil")
                                    {
                                        break;
                                    }
                                }
                            }
                            try
                            {
                                Memory.WowMemory.GameFrameLock();
                                for (int k = 1; k <= partyNumberPlayers; k++)
                                {
                                    Lua.LuaDoString(string.Concat(new object[] { commandline, " = UnitName(\"raid", k, "\");" }), false, true);
                                    string str3 = Lua.GetLocalizedText(commandline);
                                    if (str3 == this.Name)
                                    {
                                        return ("raid" + k);
                                    }
                                    if (str3 == "nil")
                                    {
                                        break;
                                    }
                                }
                            }
                            finally
                            {
                                Memory.WowMemory.GameFrameUnLock();
                            }
                        }
                    }
                    else if (base.Type == WoWObjectType.Player)
                    {
                        return "none";
                    }
                    if (!this.IsBoss)
                    {
                        if (!(base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Target))
                        {
                            return "none";
                        }
                        return "target";
                    }
                    string randomString = Others.GetRandomString(Others.Random(5, 10));
                    for (int i = 1; i <= 5; i++)
                    {
                        Lua.LuaDoString(string.Concat(new object[] { randomString, " = UnitName(\"boss", i, "\");" }), false, true);
                        string str5 = Lua.GetLocalizedText(randomString);
                        if (str5 == this.Name)
                        {
                            return ("boss" + i);
                        }
                        if (str5 == "nil")
                        {
                            break;
                        }
                    }
                }
                return "none";
            }
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
                    if (this.IsEvading)
                    {
                        return false;
                    }
                    return (((this.GetDescriptor<uint>(Descriptors.UnitFields.Flags) & 0x2010182) == 0) && ((UnitRelation.GetReaction(this.Faction) < nManager.Wow.Enums.Reaction.Neutral) || ((UnitRelation.GetReaction(this.Faction) == nManager.Wow.Enums.Reaction.Neutral) && ((this.GetDescriptor<uint>(Descriptors.UnitFields.NpcFlags) == 0) || ((base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Target) && this.CanAttackTargetLUA)))));
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
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.PetInCombat);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > AutoAttack: " + exception, true);
                    return false;
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
                        Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xe02ea0);
                        Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xe02ea4);
                        Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0xe02ea8);
                        return ((Memory.WowMemory.Memory.ReadByte(base.GetBaseAddress + 0xfac) & 8) == 0);
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
                    return ((this.IsValid && this.IsNpcQuestGiver) && ((((base.QuestGiverStatus == QuestGiverStatus.TurnIn) || (base.QuestGiverStatus == QuestGiverStatus.TurnInInvisible)) || (base.QuestGiverStatus == QuestGiverStatus.TurnInRepeatable)) || (base.QuestGiverStatus == QuestGiverStatus.LowLevelTurnInRepeatable)));
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
                    return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0x107c);
                }
                if (this.CurrentSpellIdChannel > 0)
                {
                    return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0x108c);
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
                    return Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0x1078);
                }
                if (this.CurrentSpellIdChannel > 0)
                {
                    return Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + 0x1088);
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
                    return this.GetPowerByPowerType(PowerType.Chi);
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
                return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0x104c);
            }
        }

        public int CurrentSpellIdChannel
        {
            get
            {
                return Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0x1080);
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
                    return (TypeFlag) Memory.WowMemory.Memory.ReadInt(this.MeufefiuComekujab() + 0x24);
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

        public UInt128 Focus
        {
            get
            {
                try
                {
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        return Memory.WowMemory.Memory.ReadUInt128(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xcff3f4) + 0x80);
                    }
                    Logging.Write("You can only read the focus of Me.Focus.");
                    return 0;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > : Focus get" + exception, true);
                    return 0;
                }
            }
            set
            {
                try
                {
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        Memory.WowMemory.Memory.WriteInt128(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xcff3f4) + 0x80, value);
                        Lua.LuaDoString("UnitFrame_Update(FocusFrame);", false, true);
                    }
                    else
                    {
                        Logging.WriteError("You can only set the focus of Me.Focus.", true);
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Focus set: " + exception, true);
                }
            }
        }

        public uint Fury
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Fury);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Fury: " + exception, true);
                    return 0;
                }
            }
        }

        public uint FuryPercentage
        {
            get
            {
                try
                {
                    return ((this.Fury * 100) / this.MaxFury);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > FuryPercentage: " + exception, true);
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
                    Point point = new Point {
                        X = this.Position.X,
                        Y = this.Position.Y,
                        Z = this.Position.Z
                    };
                    this._etusojor = point;
                    Thread.Sleep(50);
                    if (this._etusojor != this.Position)
                    {
                        flag = true;
                    }
                    if (!flag)
                    {
                        Thread.Sleep(30);
                        if (this._etusojor != this.Position)
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

        public bool HasQuests
        {
            get
            {
                try
                {
                    return ((this.IsValid && this.IsNpcQuestGiver) && ((((base.QuestGiverStatus == QuestGiverStatus.Available) || (base.QuestGiverStatus == QuestGiverStatus.AvailableRepeatable)) || (base.QuestGiverStatus == QuestGiverStatus.LowLevelAvailable)) || (base.QuestGiverStatus == QuestGiverStatus.LowLevelAvailableRepeatable)));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > HasQuests: " + exception, true);
                    return false;
                }
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
                    if ((num2 > 100f) || (num2 < 0f))
                    {
                        return ((num2 > 100f) ? 100f : 0f);
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
                    if (((base.get_BaseAddress() == nManager.Wow.ObjectManager.ObjectManager.Me.get_BaseAddress()) && this.InTransport) && Usefuls.PlayerUsingVehicle)
                    {
                        return false;
                    }
                    return (this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.InCombat) || this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.PetInCombat));
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
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.PvPFlagged);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > InPVP: " + exception, true);
                    return false;
                }
            }
        }

        public float Insanity
        {
            get
            {
                try
                {
                    float powerByPowerType = this.GetPowerByPowerType(PowerType.Insanity);
                    if (powerByPowerType > 0f)
                    {
                        return (powerByPowerType / 100f);
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Insanity: " + exception, true);
                }
                return 0f;
            }
        }

        public float InsanityPercentage
        {
            get
            {
                try
                {
                    return ((this.Insanity * 100f) / this.MaxInsanity);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > InsanityPercentage: " + exception, true);
                    return 0f;
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

        public bool IsAboveGround
        {
            get
            {
                if (!this.IsFlying && !this.MovementStatus.HasFlag(MovementFlags.Levitating))
                {
                    return this.MovementStatus.HasFlag(MovementFlags.Hover);
                }
                return true;
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
                    uint num = Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + 0x11c) + 0x180);
                    if ((this.MeufefiuComekujab() <= 0) || (num > 0))
                    {
                        num2 = 0;
                    }
                    else
                    {
                        num2 = (Memory.WowMemory.Memory.ReadInt((uint) (this.MeufefiuComekujab() + 0x60)) >> 2) & 1;
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
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.Confused);
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
                        if (_gadesiSuer.Count <= 0)
                        {
                            _gadesiSuer = SpellManager.SpellListManager.SpellIdByName("Ghost");
                        }
                        return (this.HaveBuff(_gadesiSuer) || ((this.Health <= 1) || this.UnitDynamicFlags.HasFlag(nManager.Wow.Enums.UnitDynamicFlags.Dead)));
                    }
                    return ((this.Health <= 0) || this.UnitDynamicFlags.HasFlag(nManager.Wow.Enums.UnitDynamicFlags.Dead));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsDead: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsElite
        {
            get
            {
                if ((this.UnitClassification != nManager.Wow.Enums.UnitClassification.Elite) && (this.UnitClassification != nManager.Wow.Enums.UnitClassification.RareElite))
                {
                    return (this.UnitClassification == nManager.Wow.Enums.UnitClassification.WorldBoss);
                }
                return true;
            }
        }

        public bool IsEvading
        {
            get
            {
                if (this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.Rename) && !this.IsTotem)
                {
                    nManagerSetting.AddBlackList(base.Guid, 0x2710);
                    return true;
                }
                return false;
            }
        }

        public bool IsFalling
        {
            get
            {
                return this.MovementStatus.HasFlag(MovementFlags.Falling);
            }
        }

        public bool IsFlying
        {
            get
            {
                return this.MovementStatus.HasFlag(MovementFlags.Flying);
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

        public bool IsInvisible
        {
            get
            {
                try
                {
                    return this.UnitDynamicFlags.HasFlag(nManager.Wow.Enums.UnitDynamicFlags.Invisible);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Invisible: " + exception, true);
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
                    return this.UnitDynamicFlags.HasFlag(nManager.Wow.Enums.UnitDynamicFlags.Lootable);
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
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(CombatMount))
                    {
                        return false;
                    }
                    return ((((this.GetDescriptor<int>(Descriptors.UnitFields.MountDisplayID) > 0) || this.HaveBuff(SpellManager.DruidMountId())) || Usefuls.IsFlying) || this.OnTaxi);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.Auctioneer);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.Taxi);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.Innkeeper);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.MailInfo);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.QuestGiver);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.CanRepair);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.SpiritHealer);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.SpiritHealer);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.CanTrain);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.Vendor);
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
                    return this.UnitNPCFlags.HasFlag(nManager.Wow.Enums.UnitNPCFlags.SellsFood);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsNpcVendorFood: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsRare
        {
            get
            {
                if (this.UnitClassification != nManager.Wow.Enums.UnitClassification.Rare)
                {
                    return (this.UnitClassification == nManager.Wow.Enums.UnitClassification.RareElite);
                }
                return true;
            }
        }

        public bool IsSilenced
        {
            get
            {
                try
                {
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.Silenced);
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
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.Skinnable);
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
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.Stunned);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > IsStunned: " + exception, true);
                    return false;
                }
            }
        }

        public bool IsSwimming
        {
            get
            {
                return this.MovementStatus.HasFlag(MovementFlags.None | MovementFlags.Swimming);
            }
        }

        public bool IsTapped
        {
            get
            {
                try
                {
                    return this.UnitDynamicFlags.HasFlag(nManager.Wow.Enums.UnitDynamicFlags.Tapped);
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
                    return this.UnitDynamicFlags.HasFlag(nManager.Wow.Enums.UnitDynamicFlags.SpecialInfo);
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

        public bool IsTotem
        {
            get
            {
                if ((nManager.Wow.ObjectManager.ObjectManager.TotemEntryList == null) || (nManager.Wow.ObjectManager.ObjectManager.TotemEntryList.Count <= 0))
                {
                    nManager.Wow.ObjectManager.ObjectManager.TotemEntryList = new List<int>();
                    Logging.Write("Loading TotemEntryList...");
                    string[] strArray = Others.ReadFileAllLines(Application.StartupPath + @"\Data\TotemEntryList.txt");
                    for (int i = 0; i <= (strArray.Length - 1); i++)
                    {
                        int item = Others.ToInt32(strArray[i]);
                        if ((item > 0) && !nManager.Wow.ObjectManager.ObjectManager.TotemEntryList.Contains(item))
                        {
                            nManager.Wow.ObjectManager.ObjectManager.TotemEntryList.Add(item);
                        }
                    }
                    if (nManager.Wow.ObjectManager.ObjectManager.TotemEntryList.Count > 0)
                    {
                        Logging.Write("Loaded " + nManager.Wow.ObjectManager.ObjectManager.TotemEntryList.Count + " entries to Totem Entry List.");
                    }
                }
                if (!this._loipo)
                {
                    this._loipo = true;
                    this._asoemaowaEbuhaToi = (nManager.Wow.ObjectManager.ObjectManager.TotemEntryList != null) && nManager.Wow.ObjectManager.ObjectManager.TotemEntryList.Contains(base.Entry);
                }
                return this._asoemaowaEbuhaToi;
            }
        }

        public bool IsTracked
        {
            get
            {
                try
                {
                    return this.UnitDynamicFlags.HasFlag(nManager.Wow.Enums.UnitDynamicFlags.TrackUnit);
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
                    uint num = base.Tuecom<uint>(Descriptors.ObjectFields.DynamicFlags);
                    if (value)
                    {
                        num2 = num | 2;
                    }
                    else
                    {
                        num2 = num & -3L;
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
                if (this.UnitClassification == nManager.Wow.Enums.UnitClassification.WorldBoss)
                {
                    return false;
                }
                uint level = this.Level;
                uint num2 = nManager.Wow.ObjectManager.ObjectManager.Me.Level;
                int num3 = (int) (num2 - level);
                if (num3 <= 0)
                {
                    return false;
                }
                if ((num3 <= 2) && this.IsElite)
                {
                    return false;
                }
                if ((num3 > 3) && (this.UnitClassification == nManager.Wow.Enums.UnitClassification.Trivial))
                {
                    return true;
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
                    return ((base.get_BaseAddress() != 0) && nManager.Wow.ObjectManager.ObjectManager.ObjectDictionary.ContainsKey(base.Guid));
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

        public uint LunarPower
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.LunarPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > LunarPower: " + exception, true);
                    return 0;
                }
            }
        }

        public uint LunarPowerPercentage
        {
            get
            {
                try
                {
                    return ((this.LunarPower * 100) / this.MaxLunarPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > LunarPowerPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint Maelstrom
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Maelstrom);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Maelstrom: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaelstromPercentage
        {
            get
            {
                try
                {
                    return ((this.Maelstrom * 100) / this.MaxMaelstrom);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > MaelstromPercentage: " + exception, true);
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

        public uint MaxChi
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Chi);
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

        public uint MaxFury
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Fury);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxFury: " + exception, true);
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

        public float MaxInsanity
        {
            get
            {
                try
                {
                    float maxPowerByPowerType = this.GetMaxPowerByPowerType(PowerType.Insanity);
                    if (maxPowerByPowerType > 0f)
                    {
                        return (maxPowerByPowerType / 100f);
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxInsanity: " + exception, true);
                }
                return 0f;
            }
        }

        public uint MaxLunarPower
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.LunarPower);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxLunarPower: " + exception, true);
                    return 0;
                }
            }
        }

        public uint MaxMaelstrom
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Maelstrom);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > MaxMaelstrom: " + exception, true);
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

        public uint MaxPain
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Pain);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > MaxPain: " + exception, true);
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
                    return Memory.WowMemory.Memory.ReadInt(this.MeufefiuComekujab() + 0x6c);
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

        public MovementFlags MovementStatus
        {
            get
            {
                try
                {
                    return (MovementFlags) Memory.WowMemory.Memory.ReadInt(Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + 0x124) + 0x40);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetMovementStatus: " + exception, true);
                    return MovementFlags.None;
                }
            }
        }

        public override string Name
        {
            get
            {
                try
                {
                    if (base.get_BaseAddress() == nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress)
                    {
                        return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowProcess.WowModule + 0x1020550);
                    }
                    if (base.Type == WoWObjectType.Player)
                    {
                        return Usefuls.GetPlayerName(base.Guid);
                    }
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(this.MeufefiuComekujab() + 0x80));
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
                    Logging.WriteError("WoWUnit > NotAttackable is depreciated. Please use WoWUnit > Attackable instead.", true);
                    return !this.Attackable;
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
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.NotSelectable);
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
                return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.OnTaxi);
            }
        }

        public float Orientation
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xae8);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Orientation: " + exception, true);
                }
                return 0f;
            }
        }

        public uint Pain
        {
            get
            {
                try
                {
                    return this.GetPowerByPowerType(PowerType.Pain);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > Pain: " + exception, true);
                    return 0;
                }
            }
        }

        public uint PainPercentage
        {
            get
            {
                try
                {
                    return ((this.Pain * 100) / this.MaxPain);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > PainPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public bool PlayerControlled
        {
            get
            {
                try
                {
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.PVPAttackable);
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
                    if (base.get_BaseAddress() == 0)
                    {
                        return new Point(0f, 0f, 0f, "None");
                    }
                    Point point = new Point(Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xad8), Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xadc), Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xae0), "None");
                    if (this.IsSwimming)
                    {
                        point.Type = "Swimming";
                    }
                    if (this.IsAboveGround)
                    {
                        point.Type = "Flying";
                    }
                    if (this.InTransport && (Usefuls.ContinentId != 0x1e240))
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
                    return point;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > Position: " + exception, true);
                    return new Point(0f, 0f, 0f, "None");
                }
            }
        }

        public Point PositionAbsolute
        {
            get
            {
                Point point = new Point(Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xad8), Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xadc), Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xae0), "None");
                if (this.IsSwimming)
                {
                    point.Type = "Swimming";
                }
                if (this.IsAboveGround)
                {
                    point.Type = "Flying";
                }
                return point;
            }
        }

        public uint PowerTypeFocus
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

        public uint PowerTypeFocusPercentage
        {
            get
            {
                try
                {
                    return ((this.PowerTypeFocus * 100) / this.PowerTypeMaxFocus);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > PowerTypeFocusPercentage: " + exception, true);
                    return 0;
                }
            }
        }

        public uint PowerTypeMaxFocus
        {
            get
            {
                try
                {
                    return this.GetMaxPowerByPowerType(PowerType.Focus);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > PowerTypeMaxFocus: " + exception, true);
                    return 0;
                }
            }
        }

        public bool PVP
        {
            get
            {
                try
                {
                    return this.UnitFlags.HasFlag(nManager.Wow.Enums.UnitFlags.None | nManager.Wow.Enums.UnitFlags.PVPAttackable);
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
                return this.Ojabak(0);
            }
        }

        public uint QuestItem2
        {
            get
            {
                return this.Ojabak(1);
            }
        }

        public uint QuestItem3
        {
            get
            {
                return this.Ojabak(2);
            }
        }

        public uint QuestItem4
        {
            get
            {
                return this.Ojabak(3);
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
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(this.MeufefiuComekujab()));
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
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        return Memory.WowMemory.Memory.ReadUInt128(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xcff3f4) + 40);
                    }
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
                    if (base.Guid == nManager.Wow.ObjectManager.ObjectManager.Me.Guid)
                    {
                        Memory.WowMemory.Memory.WriteInt128(Memory.WowMemory.Memory.ReadUInt(Memory.WowProcess.WowModule + 0xcff3f4) + 40, value);
                        Lua.LuaDoString("UnitFrame_Update(TargetFrame);", false, true);
                    }
                    else
                    {
                        Logging.WriteError("You can only set the target of Me.Focus.", true);
                    }
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
                    return Memory.WowMemory.Memory.ReadUInt128(base.GetBaseAddress + 0xac8);
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
                return Umameireovoap.WacauGameNoiv(base.get_BaseAddress());
            }
        }

        public nManager.Wow.Enums.UnitClassification UnitClassification
        {
            get
            {
                return (nManager.Wow.Enums.UnitClassification) Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + 0xc68) + 0x2c);
            }
        }

        public nManager.Wow.Enums.UnitDynamicFlags UnitDynamicFlags
        {
            get
            {
                return base.Tuecom<nManager.Wow.Enums.UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags);
            }
        }

        public nManager.Wow.Enums.UnitFlags UnitFlags
        {
            get
            {
                return this.GetDescriptor<nManager.Wow.Enums.UnitFlags>(Descriptors.UnitFields.Flags);
            }
        }

        public UnitFlightMasterStatus UnitFlightMasteStatus
        {
            get
            {
                return (UnitFlightMasterStatus) Memory.WowMemory.Memory.ReadInt(base.get_BaseAddress() + 240);
            }
        }

        public nManager.Wow.Enums.UnitNPCFlags UnitNPCFlags
        {
            get
            {
                return this.GetDescriptor<nManager.Wow.Enums.UnitNPCFlags>(Descriptors.UnitFields.NpcFlags);
            }
        }

        public WoWClass WowClass
        {
            get
            {
                try
                {
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + Descriptors.StartDescriptors) + 0xd8;
                    byte num3 = Memory.WowMemory.Memory.ReadBytes(dwAddress, 4)[1];
                    if (num3 == 0)
                    {
                        num3 = Memory.WowMemory.Memory.ReadBytes(dwAddress - 4, 4)[1];
                    }
                    return (WoWClass) num3;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWUnit > WowClass: " + exception, true);
                    return WoWClass.None;
                }
            }
        }

        private class Eheanuleucud
        {
            public UInt128 _desalea;
            public int _haofeuvawe;
            public string _piutiteawa;
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

