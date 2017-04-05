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
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public class WoWPlayer : WoWUnit
    {
        private static List<uint> _ghostSpells = new List<uint>();

        public WoWPlayer(uint address) : base(address)
        {
        }

        private byte GetCharByte(uint index)
        {
            uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + Descriptors.StartDescriptors) + 0xe4;
            return Memory.WowMemory.Memory.ReadBytes(dwAddress, 4)[index];
        }

        public T GetDescriptor<T>(Descriptors.PlayerFields field) where T: struct
        {
            try
            {
                return base.GetDescriptor<T>((uint) field);
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
                if (base.Level < 10)
                {
                    if (doOutput)
                    {
                        Logging.WriteDebug("WoW Specialization: low level don't have specialization");
                    }
                    return WoWSpecialization.None;
                }
                string randomString = Others.GetRandomString(Others.Random(4, 10));
                Lua.LuaDoString("if GetSpecialization() ~= nil and GetSpecializationInfo(GetSpecialization()) ~= nil then id,name,description,icon,background,role = GetSpecializationInfo(GetSpecialization()) " + randomString + " = id .. \"^\" .. name .. \"^\" .. role else " + randomString + " = 0 end", false, true);
                string[] source = Lua.GetLocalizedText(randomString).Split(new char[] { '^' });
                if (source.Count<string>() != 3)
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

        public bool forceIsCast { get; set; }

        public int GetDurability
        {
            get
            {
                try
                {
                    int num = 0;
                    int num2 = 0;
                    foreach (WoWObject obj2 in from o in nManager.Wow.ObjectManager.ObjectManager.ObjectList.ToArray()
                        where o.Type == WoWObjectType.Item
                        select o)
                    {
                        try
                        {
                            UInt128 descriptor = base.GetDescriptor<UInt128>(obj2.GetBaseAddress, 12);
                            if (EquippedItems.IsEquippedItemByGuid(obj2.Guid) && (descriptor == base.Guid))
                            {
                                int num4 = base.GetDescriptor<int>(obj2.GetBaseAddress, 0x4e);
                                if (num4 != 0)
                                {
                                    int num5 = base.GetDescriptor<int>(obj2.GetBaseAddress, 0x4d);
                                    num += num5;
                                    num2 += num4;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            Logging.WriteError("WoWPlayer > GetDurability#1: " + exception, true);
                        }
                    }
                    return ((num2 == 0) ? 100 : ((num * 100) / num2));
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
                            return ((!flag2 && base.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Combat)) || flag);
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("WoWPlayer > InCombat#1: " + exception, true);
                    }
                    return ((base.GetDescriptor<UnitFlags>(Descriptors.UnitFields.Flags).HasFlag(UnitFlags.Combat) || flag) && !flag2);
                }
                catch (Exception exception2)
                {
                    Logging.WriteError("WoWPlayer > InCombat#2: " + exception2, true);
                }
                return false;
            }
        }

        public bool IsCast
        {
            get
            {
                try
                {
                    return (((Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0xf98) > 0) || (Memory.WowMemory.Memory.ReadInt(base.GetBaseAddress + 0xfb8) > 0)) || this.forceIsCast);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWPlayer > IsCast: " + exception, true);
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
                    if (_ghostSpells.Count <= 0)
                    {
                        _ghostSpells = SpellManager.SpellListManager.SpellIdByName("Ghost");
                    }
                    return (base.HaveBuff(_ghostSpells) || (((base.Health <= 1) || base.GetDescriptor<UnitDynamicFlags>(Descriptors.ObjectFields.DynamicFlags).HasFlag(UnitDynamicFlags.Dead)) || ((this.PositionCorpse.X != 0f) && !(this.PositionCorpse.Y == 0f))));
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
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + Descriptors.StartDescriptors) + 0x22f8;
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
                    uint dwAddress = Memory.WowMemory.Memory.ReadUInt(base.GetBaseAddress + Descriptors.StartDescriptors) + 0x22fc;
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
                    return new Point(Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xda594c), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xda5950), Memory.WowMemory.Memory.ReadFloat(Memory.WowProcess.WowModule + 0xda5954), "None");
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
                    return Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 0xad0);
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
                    Memory.WowMemory.Memory.WriteFloat(base.BaseAddress + 0xad0, value);
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
                return (WoWClass) this.GetCharByte(1);
            }
        }

        public WoWGender WowGender
        {
            get
            {
                return (WoWGender) this.GetCharByte(3);
            }
        }

        public WoWRace WowRace
        {
            get
            {
                return (WoWRace) this.GetCharByte(0);
            }
        }
    }
}

