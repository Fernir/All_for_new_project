namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using nManager.Wow.Patchables;
    using System;

    public class Skill
    {
        public static int GetMaxValue(SkillLine skill)
        {
            try
            {
                uint num = Uxutousa(skill);
                if (num > 0)
                {
                    uint num3 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 0x1d04;
                    return Memory.WowMemory.Memory.ReadShort(((num * 2) + num3) + 0x400);
                }
                return 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetMaxValue(Enums.SkillLine skill): " + exception, true);
            }
            return 0;
        }

        public static int GetSkillBonus(SkillLine skill)
        {
            int num = 0;
            switch (nManager.Wow.ObjectManager.ObjectManager.Me.WowRace)
            {
                case WoWRace.Tauren:
                    if (skill == SkillLine.Herbalism)
                    {
                        num += 15;
                    }
                    break;

                case WoWRace.Gnome:
                    if (skill == SkillLine.Engineering)
                    {
                        num += 15;
                    }
                    break;

                case WoWRace.Goblin:
                    if (skill == SkillLine.Alchemy)
                    {
                        num += 15;
                    }
                    break;

                case WoWRace.BloodElf:
                    if (skill == SkillLine.Enchanting)
                    {
                        num += 10;
                    }
                    break;

                case WoWRace.Draenei:
                    if (skill == SkillLine.Jewelcrafting)
                    {
                        num += 5;
                    }
                    break;

                case WoWRace.Worgen:
                    if (skill == SkillLine.Skinning)
                    {
                        num += 15;
                    }
                    break;
            }
            if ((skill == SkillLine.Fishing) && nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0xb27e))
            {
                num += 10;
            }
            SkillLine line = skill;
            if (line <= SkillLine.Mining)
            {
                switch (line)
                {
                    case SkillLine.Herbalism:
                        if ((ItemsManager.GetItemCount(0x9f44) <= 0) && (ItemsManager.GetItemCount(0x14e9f) <= 0))
                        {
                            return num;
                        }
                        return (num + 10);

                    case SkillLine.Mining:
                        if ((ItemsManager.GetItemCount(0x9f44) <= 0) && (ItemsManager.GetItemCount(0xb55) <= 0))
                        {
                            return num;
                        }
                        return (num + 10);
                }
                return num;
            }
            if ((line != SkillLine.Fishing) && (line == SkillLine.Skinning))
            {
                if ((ItemsManager.GetItemCount(0x9f44) <= 0) && (ItemsManager.GetItemCount(0x1b5d) <= 0))
                {
                    return num;
                }
                return (num + 10);
            }
            return num;
        }

        public static int GetValue(SkillLine skill)
        {
            try
            {
                uint num = Uxutousa(skill);
                if (num > 0)
                {
                    uint num3 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 0x1d04;
                    return Memory.WowMemory.Memory.ReadShort(((num * 2) + num3) + 0x200);
                }
                return 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetValue(Enums.SkillLine skill): " + exception, true);
            }
            return 0;
        }

        private static uint Uxutousa(SkillLine esatare)
        {
            try
            {
                uint num2 = Memory.WowMemory.Memory.ReadUInt(nManager.Wow.ObjectManager.ObjectManager.Me.GetBaseAddress + Descriptors.StartDescriptors) + 0x1d04;
                uint num3 = 0;
                uint num4 = 0;
                do
                {
                    if (((SkillLine) Memory.WowMemory.Memory.ReadShort(num2 + num3)) == esatare)
                    {
                        return num4;
                    }
                    num4++;
                    num3 += 2;
                }
                while (num3 <= 0x200);
                return num4;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetBaseSkill(Enums.SkillLine skill): " + exception, true);
            }
            return 0;
        }
    }
}

