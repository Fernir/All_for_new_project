namespace nManager.Wow.Helpers
{
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class EquipmentAndStats
    {
        internal static WowItemSubClassArmor InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
        internal static List<WowItemSubClassWeapon> InternalEquipableWeapons = new List<WowItemSubClassWeapon>();
        internal static List<WoWStatistic> InternalEquipementStats = new List<WoWStatistic>();
        internal static bool InternalHasShield = false;

        public static void LoadWoWSpecialization()
        {
            while (nManager.Wow.ObjectManager.ObjectManager.Me.WowClass == WoWClass.None)
            {
                Thread.Sleep(10);
            }
            if (!CombatClass.IsAliveCombatClass && !HealerClass.IsAliveHealerClass)
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.WowSpecialization(true) != WoWSpecialization.None)
                {
                    SetPlayerSpe(nManager.Wow.ObjectManager.ObjectManager.Me.WowSpecialization(false));
                }
                else
                {
                    switch (nManager.Wow.ObjectManager.ObjectManager.Me.WowClass)
                    {
                        case WoWClass.Warrior:
                            SetPlayerSpe(WoWSpecialization.WarriorArms);
                            return;

                        case WoWClass.Paladin:
                            SetPlayerSpe(WoWSpecialization.PaladinRetribution);
                            return;

                        case WoWClass.Hunter:
                            SetPlayerSpe(WoWSpecialization.HunterMarksmanship);
                            return;

                        case WoWClass.Rogue:
                            SetPlayerSpe(WoWSpecialization.RogueCombat);
                            return;

                        case WoWClass.Priest:
                            SetPlayerSpe(WoWSpecialization.PriestShadow);
                            return;

                        case WoWClass.DeathKnight:
                            SetPlayerSpe(WoWSpecialization.DeathknightUnholy);
                            return;

                        case WoWClass.Shaman:
                            SetPlayerSpe(WoWSpecialization.ShamanRestoration);
                            return;

                        case WoWClass.Mage:
                            SetPlayerSpe(WoWSpecialization.MageFrost);
                            return;

                        case WoWClass.Warlock:
                            SetPlayerSpe(WoWSpecialization.WarlockDemonology);
                            return;

                        case WoWClass.Monk:
                            SetPlayerSpe(WoWSpecialization.MonkBrewmaster);
                            return;

                        case WoWClass.Druid:
                            SetPlayerSpe(WoWSpecialization.DruidBalance);
                            return;
                    }
                }
            }
        }

        public static void SetPlayerSpe(WoWSpecialization spe)
        {
            InternalEquipableWeapons.Clear();
            InternalEquipementStats.Clear();
            List<WoWStatistic> collection = new List<WoWStatistic> { 5, 7, 0x24, 0x20, 0x2d, 0x31 };
            List<WoWStatistic> list2 = new List<WoWStatistic> { 6 };
            list2.AddRange(collection);
            List<WoWStatistic> list3 = new List<WoWStatistic> { 0x1f };
            list3.AddRange(collection);
            List<WoWStatistic> list4 = new List<WoWStatistic> { 3, 7, 0x24, 0x20, 0x26, 0x31, 0x1f, 0x25 };
            List<WoWStatistic> list5 = new List<WoWStatistic> { 4, 7, 0x24, 0x31, 0x1f, 0x25 };
            List<WoWStatistic> list6 = new List<WoWStatistic> { 0x20, 0x26 };
            list6.AddRange(list5);
            switch (spe)
            {
                case WoWSpecialization.MageArcane:
                case WoWSpecialization.MageFire:
                case WoWSpecialization.MageFrost:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Wand);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    goto Label_05AF;

                case WoWSpecialization.PaladinHoly:
                case WoWSpecialization.PaladinProtection:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    }
                    else
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    }
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalHasShield = true;
                    goto Label_05AF;

                case WoWSpecialization.PaladinRetribution:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                        break;
                    }
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    break;

                case WoWSpecialization.WarriorArms:
                case WoWSpecialization.WarriorFury:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    }
                    else
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    }
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    goto Label_05AF;

                case WoWSpecialization.WarriorProtection:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    }
                    else
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    }
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist);
                    InternalHasShield = true;
                    goto Label_05AF;

                case WoWSpecialization.DruidBalance:
                case WoWSpecialization.DruidFeral:
                case WoWSpecialization.DruidGuardian:
                case WoWSpecialization.DruidRestoration:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist);
                    goto Label_05AF;

                case WoWSpecialization.DeathknightBlood:
                case WoWSpecialization.DeathknightFrost:
                case WoWSpecialization.DeathknightUnholy:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Plate;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    goto Label_05AF;

                case WoWSpecialization.HunterBeastMastery:
                case WoWSpecialization.HunterMarksmanship:
                case WoWSpecialization.HunterSurvival:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    }
                    else
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    }
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Gun);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Bow);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Crossbow);
                    goto Label_05AF;

                case WoWSpecialization.PriestDiscipline:
                case WoWSpecialization.PriestHoly:
                case WoWSpecialization.PriestShadow:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Wand);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    goto Label_05AF;

                case WoWSpecialization.RogueAssassination:
                case WoWSpecialization.RogueCombat:
                case WoWSpecialization.RogueSubtlety:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist);
                    goto Label_05AF;

                case WoWSpecialization.ShamanElemental:
                case WoWSpecialization.ShamanEnhancement:
                case WoWSpecialization.ShamanRestoration:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Mail;
                    }
                    else
                    {
                        InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    }
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Fist);
                    InternalHasShield = true;
                    goto Label_05AF;

                case WoWSpecialization.WarlockAffliction:
                case WoWSpecialization.WarlockDemonology:
                case WoWSpecialization.WarlockDestruction:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Wand);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Dagger);
                    goto Label_05AF;

                case WoWSpecialization.MonkBrewmaster:
                case WoWSpecialization.MonkWindwalker:
                case WoWSpecialization.MonkMistweaver:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Leather;
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Staff);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword);
                    InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace);
                    goto Label_05AF;

                default:
                    InternalEquipableArmorItemType = WowItemSubClassArmor.Cloth;
                    goto Label_05AF;
            }
            InternalEquipableWeapons.Add(WowItemSubClassWeapon.Axe2);
            InternalEquipableWeapons.Add(WowItemSubClassWeapon.Sword2);
            InternalEquipableWeapons.Add(WowItemSubClassWeapon.Mace2);
            InternalEquipableWeapons.Add(WowItemSubClassWeapon.Polearm);
        Label_05AF:
            switch (spe)
            {
                case WoWSpecialization.MageArcane:
                case WoWSpecialization.MageFire:
                case WoWSpecialization.MageFrost:
                    InternalEquipementStats.AddRange(list3);
                    return;

                case WoWSpecialization.PaladinHoly:
                    InternalEquipementStats.AddRange(list2);
                    return;

                case WoWSpecialization.PaladinProtection:
                    InternalEquipementStats.AddRange(list5);
                    InternalEquipementStats.Add(WoWStatistic.PARRY_RATING);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    return;

                case ((WoWSpecialization) 0x43):
                case ((WoWSpecialization) 0x44):
                case ((WoWSpecialization) 0x45):
                    break;

                case WoWSpecialization.PaladinRetribution:
                    InternalEquipementStats.AddRange(list6);
                    return;

                case WoWSpecialization.WarriorArms:
                case WoWSpecialization.WarriorFury:
                    InternalEquipementStats.AddRange(list6);
                    return;

                case WoWSpecialization.WarriorProtection:
                    InternalEquipementStats.AddRange(list5);
                    InternalEquipementStats.Add(WoWStatistic.PARRY_RATING);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    InternalEquipementStats.Add(WoWStatistic.CRIT_RATING);
                    return;

                case WoWSpecialization.DruidBalance:
                    InternalEquipementStats.AddRange(list3);
                    return;

                case WoWSpecialization.DruidFeral:
                    InternalEquipementStats.AddRange(list4);
                    return;

                case WoWSpecialization.DruidGuardian:
                    InternalEquipementStats.AddRange(list4);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    return;

                case WoWSpecialization.DruidRestoration:
                    InternalEquipementStats.AddRange(list2);
                    return;

                case WoWSpecialization.DeathknightBlood:
                    InternalEquipementStats.AddRange(list5);
                    InternalEquipementStats.Add(WoWStatistic.PARRY_RATING);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    return;

                case WoWSpecialization.DeathknightFrost:
                case WoWSpecialization.DeathknightUnholy:
                    InternalEquipementStats.AddRange(list6);
                    return;

                case WoWSpecialization.HunterBeastMastery:
                case WoWSpecialization.HunterMarksmanship:
                case WoWSpecialization.HunterSurvival:
                    InternalEquipementStats.AddRange(list4);
                    return;

                case WoWSpecialization.PriestDiscipline:
                case WoWSpecialization.PriestHoly:
                    InternalEquipementStats.AddRange(list2);
                    return;

                case WoWSpecialization.PriestShadow:
                    InternalEquipementStats.AddRange(list3);
                    InternalEquipementStats.Add(WoWStatistic.SPIRIT);
                    return;

                case WoWSpecialization.RogueAssassination:
                case WoWSpecialization.RogueCombat:
                case WoWSpecialization.RogueSubtlety:
                    InternalEquipementStats.AddRange(list4);
                    return;

                case WoWSpecialization.ShamanElemental:
                    InternalEquipementStats.AddRange(list3);
                    InternalEquipementStats.Add(WoWStatistic.SPIRIT);
                    return;

                case WoWSpecialization.ShamanEnhancement:
                    InternalEquipementStats.AddRange(list4);
                    return;

                case WoWSpecialization.ShamanRestoration:
                    InternalEquipementStats.AddRange(list2);
                    return;

                case WoWSpecialization.WarlockAffliction:
                case WoWSpecialization.WarlockDemonology:
                case WoWSpecialization.WarlockDestruction:
                    InternalEquipementStats.AddRange(list3);
                    return;

                case WoWSpecialization.MonkBrewmaster:
                    InternalEquipementStats.AddRange(list4);
                    InternalEquipementStats.Add(WoWStatistic.DODGE_RATING);
                    return;

                case WoWSpecialization.MonkWindwalker:
                    InternalEquipementStats.AddRange(list4);
                    return;

                case WoWSpecialization.MonkMistweaver:
                    InternalEquipementStats.AddRange(list2);
                    break;

                default:
                    return;
            }
        }

        public static WowItemSubClassArmor EquipableArmorItemType
        {
            get
            {
                return InternalEquipableArmorItemType;
            }
        }

        public static List<WowItemSubClassWeapon> EquipableWeapons
        {
            get
            {
                return InternalEquipableWeapons;
            }
        }

        public static List<WoWStatistic> EquipementStats
        {
            get
            {
                return InternalEquipementStats;
            }
        }

        public static bool HasShield
        {
            get
            {
                return InternalHasShield;
            }
        }
    }
}

