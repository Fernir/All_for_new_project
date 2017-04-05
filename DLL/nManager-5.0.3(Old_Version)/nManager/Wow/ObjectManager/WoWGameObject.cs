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
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class WoWGameObject : WoWObject
    {
        public static Spell LoggingSpell;

        public WoWGameObject(uint address) : base(address)
        {
        }

        public uint Data(uint offset)
        {
            try
            {
                if (offset > 0x1f)
                {
                    return 0;
                }
                return Memory.WowMemory.Memory.ReadUInt((Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0x274) + 20) + (4 * offset));
            }
            catch (Exception exception)
            {
                Logging.WriteError(string.Concat(new object[] { "GameObject > Data(", offset, "): ", exception }), true);
                return 0;
            }
        }

        public T GetDescriptor<T>(Descriptors.GameObjectFields field) where T: struct
        {
            try
            {
                return base.GetDescriptor<T>((uint) field);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDescriptor<T>(Descriptors.GameObjectFields field): " + exception, true);
                return default(T);
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
                return Memory.WowMemory.Memory.ReadUInt((Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0x274) + 0x9c) + (4 * offset));
            }
            catch (Exception exception)
            {
                Logging.WriteError(string.Concat(new object[] { "GameObject > QuestItem(", offset, "): ", exception }), true);
                return 0;
            }
        }

        private SkillLine SkillByLockType(WoWGameObjectLockType lType)
        {
            switch (lType)
            {
                case WoWGameObjectLockType.LOCKTYPE_PICKLOCK:
                    return SkillLine.Lockpicking;

                case WoWGameObjectLockType.LOCKTYPE_HERBALISM:
                    return SkillLine.Herbalism;

                case WoWGameObjectLockType.LOCKTYPE_MINING:
                    return SkillLine.Mining;

                case WoWGameObjectLockType.LOCKTYPE_OPEN:
                case WoWGameObjectLockType.LOCKTYPE_TREASURE:
                    return SkillLine.Free;

                case WoWGameObjectLockType.LOCKTYPE_QUICK_OPEN:
                case WoWGameObjectLockType.LOCKTYPE_OPEN_TINKERING:
                case WoWGameObjectLockType.LOCKTYPE_OPEN_KNEELING:
                    return SkillLine.None;

                case WoWGameObjectLockType.LOCKTYPE_FISHING:
                    return SkillLine.Fishing;

                case WoWGameObjectLockType.LOCKTYPE_INSCRIPTION:
                    return SkillLine.Inscription;
            }
            return SkillLine.None;
        }

        public bool CanOpen
        {
            get
            {
                if (this.GOFlags.HasFlag(GameObjectFlags.GO_FLAG_IN_USE) || this.GOFlags.HasFlag(GameObjectFlags.GO_FLAG_INTERACT_COND))
                {
                    return false;
                }
                if (nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects.Count > 0)
                {
                    int entryid = 0;
                    if ((from entry in nManagerSetting.CurrentSetting.DontHarvestTheFollowingObjects
                        where int.TryParse(entry.Trim(), out entryid)
                        select entry).Any<string>(entry => this.Entry == entryid))
                    {
                        return false;
                    }
                }
                if (this.LockEntry != 0)
                {
                    WoWLock @lock = WoWLock.FromId(this.LockEntry);
                    if (@lock.Record.KeyType == null)
                    {
                        return false;
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        switch (((WoWGameObjectLockKeyType) @lock.Record.KeyType[i]))
                        {
                            case WoWGameObjectLockKeyType.LOCK_KEY_NONE:
                            {
                                continue;
                            }
                            case WoWGameObjectLockKeyType.LOCK_KEY_ITEM:
                            {
                                int num2 = (int) @lock.Record.LockType[i];
                                if (ItemsManager.GetItemCount(num2) >= 0)
                                {
                                    continue;
                                }
                                return false;
                            }
                            case WoWGameObjectLockKeyType.LOCK_KEY_SKILL:
                            {
                                if (@lock.Record.LockType[i] != 0x1c)
                                {
                                    break;
                                }
                                uint num3 = @lock.Record.Skill[i];
                                if (LoggingSpell == null)
                                {
                                    LoggingSpell = new Spell("Logging");
                                }
                                if (!LoggingSpell.KnownSpell)
                                {
                                    return false;
                                }
                                if (num3 == 3)
                                {
                                    return (LoggingSpell.Id == 0x2900b);
                                }
                                if ((num3 == 2) && (LoggingSpell.Id != 0x2900a))
                                {
                                    return (LoggingSpell.Id == 0x2900b);
                                }
                                return true;
                            }
                            default:
                            {
                                continue;
                            }
                        }
                        if (@lock.Record.LockType[i] == 13)
                        {
                            return (nManagerSetting.CurrentSetting.ActivateVeinsHarvesting && (base.Entry == 0x38c5d));
                        }
                        SkillLine skill = this.SkillByLockType((WoWGameObjectLockType) @lock.Record.LockType[i]);
                        switch (skill)
                        {
                            case SkillLine.None:
                                return false;

                            case SkillLine.Free:
                                break;

                            default:
                            {
                                if ((skill == SkillLine.Herbalism) && !nManagerSetting.CurrentSetting.ActivateHerbsHarvesting)
                                {
                                    return false;
                                }
                                if ((skill == SkillLine.Mining) && !nManagerSetting.CurrentSetting.ActivateVeinsHarvesting)
                                {
                                    return false;
                                }
                                uint num4 = @lock.Record.Skill[i];
                                if ((skill == SkillLine.Lockpicking) && !nManagerSetting.CurrentSetting.ActivateChestLooting)
                                {
                                    return false;
                                }
                                if (skill == SkillLine.Lockpicking)
                                {
                                    Spell spell = new Spell(0x70c);
                                    return (spell.KnownSpell && ((nManager.Wow.ObjectManager.ObjectManager.Me.Level * 5) >= num4));
                                }
                                int num5 = Skill.GetValue(skill);
                                if (num5 != 0)
                                {
                                    num5 += Skill.GetSkillBonus(skill);
                                }
                                this.IsHerb = skill == SkillLine.Herbalism;
                                if ((num5 > 0) && (num5 >= num4))
                                {
                                    return true;
                                }
                                if ((num5 > 0) && (num5 < num4))
                                {
                                    return false;
                                }
                                return Garrison.GarrisonMapIdList.Contains(Usefuls.RealContinentId);
                            }
                        }
                    }
                }
                if (!nManagerSetting.CurrentSetting.ActivateChestLooting)
                {
                    return false;
                }
                if (this.GOType == WoWGameObjectType.Goober)
                {
                    return false;
                }
                if (this.GOType == WoWGameObjectType.Chest)
                {
                    if ((this.Data8 != 0) && !Quest.GetLogQuestId().Contains((int) this.Data8))
                    {
                        return false;
                    }
                    if ((base.Entry == 0x33685) && ((nManager.Wow.ObjectManager.ObjectManager.Me.Level < 90) || Usefuls.IsCompletedAchievement(0x1998, true)))
                    {
                        return false;
                    }
                    if (base.Entry == 0x39ae0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public string CastBarCaption
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0x274) + 12));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > CastBarCaption: " + exception, true);
                }
                return "";
            }
        }

        public UInt128 CreatedBy
        {
            get
            {
                try
                {
                    return this.GetDescriptor<UInt128>(Descriptors.GameObjectFields.CreatedBy);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > CreatedBy: " + exception, true);
                }
                return 0;
            }
        }

        public uint Data0
        {
            get
            {
                return this.Data(0);
            }
        }

        public uint Data1
        {
            get
            {
                return this.Data(1);
            }
        }

        public uint Data10
        {
            get
            {
                return this.Data(10);
            }
        }

        public uint Data11
        {
            get
            {
                return this.Data(11);
            }
        }

        public uint Data12
        {
            get
            {
                return this.Data(12);
            }
        }

        public uint Data13
        {
            get
            {
                return this.Data(13);
            }
        }

        public uint Data14
        {
            get
            {
                return this.Data(14);
            }
        }

        public uint Data15
        {
            get
            {
                return this.Data(15);
            }
        }

        public uint Data16
        {
            get
            {
                return this.Data(0x10);
            }
        }

        public uint Data17
        {
            get
            {
                return this.Data(0x11);
            }
        }

        public uint Data18
        {
            get
            {
                return this.Data(0x12);
            }
        }

        public uint Data19
        {
            get
            {
                return this.Data(0x13);
            }
        }

        public uint Data2
        {
            get
            {
                return this.Data(2);
            }
        }

        public uint Data20
        {
            get
            {
                return this.Data(20);
            }
        }

        public uint Data21
        {
            get
            {
                return this.Data(0x15);
            }
        }

        public uint Data22
        {
            get
            {
                return this.Data(0x16);
            }
        }

        public uint Data23
        {
            get
            {
                return this.Data(0x17);
            }
        }

        public uint Data24
        {
            get
            {
                return this.Data(0x18);
            }
        }

        public uint Data25
        {
            get
            {
                return this.Data(0x19);
            }
        }

        public uint Data26
        {
            get
            {
                return this.Data(0x1a);
            }
        }

        public uint Data27
        {
            get
            {
                return this.Data(0x1b);
            }
        }

        public uint Data28
        {
            get
            {
                return this.Data(0x1c);
            }
        }

        public uint Data29
        {
            get
            {
                return this.Data(0x1d);
            }
        }

        public uint Data3
        {
            get
            {
                return this.Data(3);
            }
        }

        public uint Data30
        {
            get
            {
                return this.Data(30);
            }
        }

        public uint Data31
        {
            get
            {
                return this.Data(0x1f);
            }
        }

        public uint Data4
        {
            get
            {
                return this.Data(4);
            }
        }

        public uint Data5
        {
            get
            {
                return this.Data(5);
            }
        }

        public uint Data6
        {
            get
            {
                return this.Data(6);
            }
        }

        public uint Data7
        {
            get
            {
                return this.Data(7);
            }
        }

        public uint Data8
        {
            get
            {
                return this.Data(8);
            }
        }

        public uint Data9
        {
            get
            {
                return this.Data(9);
            }
        }

        public uint DisplayId
        {
            get
            {
                try
                {
                    return this.GetDescriptor<uint>(Descriptors.GameObjectFields.DisplayID);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > DisplayId: " + exception, true);
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
                    return this.GetDescriptor<uint>(Descriptors.GameObjectFields.FactionTemplate);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWGameObject > Faction: " + exception, true);
                    return 0;
                }
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
                    Logging.WriteError("GameObjectFields > GetDistance: " + exception, true);
                }
                return 0f;
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
                    Logging.WriteError("GameObjectFields > GetDistance2D: " + exception, true);
                }
                return 0f;
            }
        }

        public GameObjectFlags GOFlags
        {
            get
            {
                try
                {
                    return this.GetDescriptor<GameObjectFlags>(Descriptors.GameObjectFields.Flags);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > Flags: " + exception, true);
                }
                return (GameObjectFlags) 0;
            }
        }

        public WoWGameObjectType GOType
        {
            get
            {
                try
                {
                    return ((this.GetDescriptor<int>(0x18) >> 8) & 0xff);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > Type: " + exception, true);
                }
                return WoWGameObjectType.Door;
            }
        }

        public string IconName
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0x274) + 8));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > IconName: " + exception, true);
                }
                return "";
            }
        }

        public bool IsHerb { get; set; }

        public bool IsValid
        {
            get
            {
                try
                {
                    return ((this.Name != "") && base.IsValid);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("WoWGameObject > IsValid: " + exception, true);
                    return false;
                }
            }
        }

        public uint LockEntry
        {
            get
            {
                try
                {
                    switch (this.GOType)
                    {
                        case WoWGameObjectType.Door:
                        case WoWGameObjectType.Button:
                            return this.Data1;

                        case WoWGameObjectType.Questgiver:
                        case WoWGameObjectType.Chest:
                        case WoWGameObjectType.Trap:
                        case WoWGameObjectType.Goober:
                        case WoWGameObjectType.FlagStand:
                        case WoWGameObjectType.FlagDrop:
                            return this.Data0;
                    }
                    return 0;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > LockType: " + exception, true);
                }
                return 0;
            }
        }

        public override string Name
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadUTF8String(Memory.WowMemory.Memory.ReadUInt(Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0x274) + 180));
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > Name: " + exception, true);
                }
                return "";
            }
        }

        public float Orientation
        {
            get
            {
                try
                {
                    Quaternion rotations = this.Rotations;
                    float num = (float) System.Math.Atan2(0.0 + (((rotations.X * rotations.Y) + (rotations.Z * rotations.W)) * 2.0), 1.0 - (((rotations.Y * rotations.Y) + (rotations.Z * rotations.Z)) * 2.0));
                    return ((num < 0f) ? (num + 6.283185f) : num);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObject > Orientation: " + exception, true);
                    return 0f;
                }
            }
        }

        public override Point Position
        {
            get
            {
                try
                {
                    return new Point(Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 680), Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 0x2ac), Memory.WowMemory.Memory.ReadFloat(base.BaseAddress + 0x2b0), "None");
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObjectFields > Position: " + exception, true);
                }
                return new Point();
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

        public Quaternion Rotations
        {
            get
            {
                return new Quaternion(Memory.WowMemory.Memory.ReadInt64(base.BaseAddress + 0x150));
            }
        }

        public float Size
        {
            get
            {
                try
                {
                    return Memory.WowMemory.Memory.ReadFloat(Memory.WowMemory.Memory.ReadUInt(base.BaseAddress + 0x274) + 0x98);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GameObject > Size: " + exception, true);
                    return 0f;
                }
            }
        }

        public Matrix4 WorldMatrix
        {
            get
            {
                Matrix4.MatrixColumn column = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject(base.BaseAddress + 0x278, typeof(Matrix4.MatrixColumn));
                Matrix4.MatrixX x = new Matrix4.MatrixX(column.m1, column.m2, column.m3, column.m4);
                Matrix4.MatrixColumn column2 = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject((base.BaseAddress + 0x278) + 0x10, typeof(Matrix4.MatrixColumn));
                Matrix4.MatrixY y = new Matrix4.MatrixY(column2.m1, column2.m2, column2.m3, column2.m4);
                Matrix4.MatrixColumn column3 = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject((base.BaseAddress + 0x278) + 0x20, typeof(Matrix4.MatrixColumn));
                Matrix4.MatrixZ z = new Matrix4.MatrixZ(column3.m1, column3.m2, column3.m3, column3.m4);
                Matrix4.MatrixColumn column4 = (Matrix4.MatrixColumn) Memory.WowMemory.Memory.ReadObject((base.BaseAddress + 0x278) + 0x30, typeof(Matrix4.MatrixColumn));
                return new Matrix4(x, y, z, new Matrix4.MatrixW(column4.m1, column4.m2, column4.m3, column4.m4));
            }
        }

        public enum GameObjectFlags
        {
            GO_FLAG_IN_USE = 1,
            GO_FLAG_INTERACT_COND = 4,
            GO_FLAG_LOCKED = 2,
            GO_FLAG_NO_INTERACT = 0x10,
            GO_FLAG_NODESPAWN = 0x20,
            GO_FLAG_TRANSPORT = 8,
            GO_FLAG_TRIGGERED = 0x40
        }
    }
}

