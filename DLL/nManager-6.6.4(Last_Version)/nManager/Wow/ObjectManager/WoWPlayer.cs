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
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class WoWPlayer : WoWUnit
    {
        private static List<uint> _gadesiSuer = new List<uint>();

        public WoWPlayer(uint address) : base(address)
        {
        }

        private byte Afalidu(uint nogiuracoicaoAfae)
        {
            uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.get_BaseAddress() + Descriptors.StartDescriptors) + 0xd4;
            return Memory.WowMemory.Memory.ReadBytes(dwAddress, 4)[nogiuracoicaoAfae];
        }

        public T GetDescriptor<T>(Descriptors.PlayerFields field) where T: struct
        {
            try
            {
                return base.Tuecom<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWPlayer > GetDescriptor<T>(Descriptors.PlayerFields field): " + exception, true);
            }
            return default(T);
        }

        public void StopCast()
        {
            Lua.RunMacroText("/stopcasting");
        }

        public WoWSpecialization WowSpecialization(bool doOutput = false)
        {
            try
            {
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("if GetSpecialization() ~= nil and GetSpecializationInfo(GetSpecialization()) ~= nil then id,name,description,icon,role,primary = GetSpecializationInfo(GetSpecialization()) " + randomString + " = id .. \"^\" .. name .. \"^\" .. role .. \"^\" .. primary else " + randomString + " = 0 end", false, true);
                string[] source = Lua.GetLocalizedText(randomString).Split(new char[] { '^' });
                if (source.Count<string>() != 4)
                {
                    if (doOutput)
                    {
                        Logging.WriteDebug("WoW Specialization not found");
                    }
                    return WoWSpecialization.None;
                }
                if (doOutput)
                {
                    Logging.WriteDebug(string.Concat(new object[] { "WoW Specialization found: ", this.WowClass, " ", source[1], ", role: ", source[2] }));
                }
                return (WoWSpecialization) Others.ToInt32(source[0]);
            }
            catch (Exception exception)
            {
                Logging.WriteError("WoWPlayer > WoWSpecialization: " + exception, true);
            }
            return WoWSpecialization.None;
        }

        public int Experience
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>(Descriptors.PlayerFields.XP);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > Experience: " + exception, true);
                }
                return 0;
            }
        }

        public int ExperienceMax
        {
            get
            {
                try
                {
                    return this.GetDescriptor<int>(Descriptors.PlayerFields.NextLevelXP);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > ExperienceMax: " + exception, true);
                }
                return 0;
            }
        }

        public bool ForceIsCasting { get; set; }

        public int GetDurability
        {
            get
            {
                try
                {
                    int num = 0;
                    int num2 = 0;
                    int num3 = 0;
                    foreach (WoWObject obj2 in from o in nManager.Wow.ObjectManager.ObjectManager.ObjectList.ToArray()
                        where o.Type == WoWObjectType.Item
                        select o)
                    {
                        try
                        {
                            UInt128 num4 = base.Tuecom<UInt128>(obj2.GetBaseAddress, 12);
                            if (EquippedItems.IsEquippedItemByGuid(obj2.Guid) && (num4 == base.Guid))
                            {
                                int num5 = base.Tuecom<int>(obj2.GetBaseAddress, 0x4e);
                                if (num5 != 0)
                                {
                                    int num6 = base.Tuecom<int>(obj2.GetBaseAddress, 0x4d);
                                    if (num6 == 0)
                                    {
                                        num3++;
                                    }
                                    if (((num6 == 0) && (obj2 is WoWItem)) && ((obj2 as WoWItem).GetItemInfo.ItemType == "Weapon"))
                                    {
                                        num3++;
                                    }
                                    num += num6;
                                    num2 += num5;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Logging.WriteError("WoWPlayer > GetDurability#1: " + exception, true);
                        }
                    }
                    int num7 = (num2 == 0) ? 100 : ((num * 100) / num2);
                    if ((num7 > 0x23) && (num3 >= 2))
                    {
                        return 0x23;
                    }
                    return num7;
                }
                catch (Exception exception2)
                {
                    Logging.WriteError("WoWPlayer > GetDurability#2: " + exception2, true);
                }
                Logging.WriteError("Durability % finding had crashed, please report this issue, we are outputting 100% instead, it wont repair.", true);
                return 100;
            }
        }

        public bool InCombat
        {
            get
            {
                return (this.InCombatBlizzard && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0));
            }
        }

        public bool InCombatBlizzard
        {
            get
            {
                try
                {
                    bool flag = false;
                    bool flag2 = false;
                    try
                    {
                        if (base.GetBaseAddress == nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress)
                        {
                            if (this.IsDeadMe)
                            {
                                flag2 = true;
                            }
                            if (((nManager.Wow.ObjectManager.ObjectManager.Pet.GetBaseAddress > 0) && nManager.Wow.ObjectManager.ObjectManager.Pet.InCombat) && !nManager.Wow.ObjectManager.ObjectManager.Pet.IsDead)
                            {
                                flag = true;
                            }
                            return ((!flag2 && base.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.InCombat)) || flag);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("WoWPlayer > InCombat#1: " + exception, true);
                    }
                    return ((base.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.InCombat) || flag) && !flag2);
                }
                catch (Exception exception2)
                {
                    Logging.WriteError("WoWPlayer > InCombat#2: " + exception2, true);
                }
                return false;
            }
        }

        public bool InInevitableCombat
        {
            get
            {
                return (((nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying))) && (!CustomProfile.GetSetIgnoreFight && !Quest.GetSetIgnoreFight)) && !Quest.GetSetIgnoreAllFight);
            }
        }

        public bool IsCast
        {
            get
            {
                return this.IsCasting;
            }
        }

        public bool IsCasting
        {
            get
            {
                try
                {
                    return (((nManager.Wow.ObjectManager.ObjectManager.Me.Guid == base.Guid) && this.ForceIsCasting) || ((base.CurrentSpellIdCast > 0) || (base.CurrentSpellIdChannel > 0)));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > IsCasting: " + exception, true);
                }
                return false;
            }
        }

        public bool IsDeadMe
        {
            get
            {
                try
                {
                    if (!base.IsValid)
                    {
                        return true;
                    }
                    if (_gadesiSuer.Count <= 0)
                    {
                        _gadesiSuer = SpellManager.SpellListManager.SpellIdByName("Ghost");
                    }
                    return (base.HaveBuff(_gadesiSuer) || (((base.Health <= 1) || base.Tuecom<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead)) || ((this.PositionCorpse.X != 0f) && !(this.PositionCorpse.Y == 0f))));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > IsDeadMe: " + exception, true);
                }
                return false;
            }
        }

        public bool IsMainHandTemporaryEnchanted
        {
            get
            {
                try
                {
                    string randomString = Others.GetRandomString(Others.Random(4, 10));
                    Lua.LuaDoString(randomString + " = tostring(GetWeaponEnchantInfo())", false, true);
                    return (Lua.GetLocalizedText(randomString) == "true");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > IsMainHandTemporaryEnchanted: " + exception, true);
                    return false;
                }
            }
        }

        public TrackCreatureFlags MeCreatureTrack
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.PlayerFields.TrackCreatureMask);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > MeCreatureTrack: " + exception, true);
                }
                return 0;
            }
            set
            {
                try
                {
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + Descriptors.StartDescriptors) + 0x240c;
                    Memory.WowMemory.Memory.WriteUInt(dwAddress, (uint) value);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > MeCreatureTrack: " + exception, true);
                }
            }
        }

        public TrackObjectFlags MeObjectTrack
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.PlayerFields.TrackResourceMask);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > MeObjectTrack get: " + exception, true);
                }
                return TrackObjectFlags.None;
            }
            set
            {
                try
                {
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + Descriptors.StartDescriptors) + 0x2410;
                    Memory.WowMemory.Memory.WriteUInt(dwAddress, (uint) value);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > MeObjectTrack set: " + exception, true);
                }
            }
        }

        public string PlayerFaction
        {
            get
            {
                try
                {
                    switch (this.PlayerRace)
                    {
                        case "Human":
                        case "Dwarf":
                        case "Gnome":
                        case "Night Elf":
                        case "Draenei":
                        case "Worgen":
                        case "PandarenAlliance":
                            return "Alliance";

                        case "Orc":
                        case "Undead":
                        case "Tauren":
                        case "Troll":
                        case "Blood Elf":
                        case "Goblin":
                        case "PandarenHorde":
                            return "Horde";

                        case "PandarenNeutral":
                            return "Neutral";
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > PlayerFaction: " + exception, true);
                }
                return "Neutral";
            }
        }

        public string PlayerRace
        {
            get
            {
                try
                {
                    long faction = base.Faction;
                    if (faction.Equals((long) 1L))
                    {
                        return "Human";
                    }
                    if (faction.Equals((long) 0x64aL))
                    {
                        return "Blood Elf";
                    }
                    if (faction.Equals((long) 3L))
                    {
                        return "Dwarf";
                    }
                    if (faction.Equals((long) 0x73L))
                    {
                        return "Gnome";
                    }
                    if (faction.Equals((long) 4L))
                    {
                        return "Night Elf";
                    }
                    if (faction.Equals((long) 2L))
                    {
                        return "Orc";
                    }
                    if (faction.Equals((long) 6L))
                    {
                        return "Tauren";
                    }
                    if (faction.Equals((long) 0x74L))
                    {
                        return "Troll";
                    }
                    if (faction.Equals((long) 5L))
                    {
                        return "Undead";
                    }
                    if (faction.Equals((long) 0x65dL))
                    {
                        return "Draenei";
                    }
                    if (faction.Equals((long) 0x89cL))
                    {
                        return "Goblin";
                    }
                    if (faction.Equals((long) 0x89bL))
                    {
                        return "Worgen";
                    }
                    if (faction.Equals((long) 0x95bL))
                    {
                        return "PandarenNeutral";
                    }
                    if (faction.Equals((long) 0x962L))
                    {
                        return "PandarenHorde";
                    }
                    if (faction.Equals((long) 0x961L))
                    {
                        return "PandarenAlliance";
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > PlayerRace: " + exception, true);
                }
                return "Unknown";
            }
        }

        public Point PositionCorpse
        {
            get
            {
                try
                {
                    return new Point(Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xf3eddc), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xf3ede0), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xf3ede4), "None");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > PositionCorpse: " + exception, true);
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
                    return Memory.WowMemory.Memory.ReadFloat(base.get_BaseAddress() + 0xae8);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > Rotation get: " + exception, true);
                }
                return 0f;
            }
            set
            {
                try
                {
                    Memory.WowMemory.Memory.WriteFloat(base.get_BaseAddress() + 0xae8, value);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > : Rotation set" + exception, true);
                }
            }
        }

        public WoWClass WowClass
        {
            get
            {
                return (WoWClass) this.Afalidu(1);
            }
        }

        public WoWGender WowGender
        {
            get
            {
                return (WoWGender) this.Afalidu(3);
            }
        }

        public WoWRace WowRace
        {
            get
            {
                return (WoWRace) this.Afalidu(0);
            }
        }
    }
}

