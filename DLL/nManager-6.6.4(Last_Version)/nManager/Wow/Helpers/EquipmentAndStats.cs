namespace nManager.Wow.Helpers
{
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class EquipmentAndStats
    {
        internal static List<WowItemSubClassWeapon> _caekiquehIciuwiaOna = new List<WowItemSubClassWeapon>();
        internal static List<WoWStatistic> _ewilosiegulWoacuomeTione = new List<WoWStatistic>();
        internal static bool _honuopapiuweUraep = false;
        internal static WowItemSubClassArmor _jawohuoleqire = WowItemSubClassArmor.Cloth;

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
                            SetPlayerSpe(WoWSpecialization.RogueOutlaw);
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
            _caekiquehIciuwiaOna.Clear();
            _ewilosiegulWoacuomeTione.Clear();
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
                    _jawohuoleqire = WowItemSubClassArmor.Cloth;
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Staff);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Wand);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Dagger);
                    goto Label_05AF;

                case WoWSpecialization.PaladinHoly:
                case WoWSpecialization.PaladinProtection:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Plate;
                    }
                    else
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Mail;
                    }
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword);
                    _honuopapiuweUraep = true;
                    goto Label_05AF;

                case WoWSpecialization.PaladinRetribution:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Plate;
                        break;
                    }
                    _jawohuoleqire = WowItemSubClassArmor.Mail;
                    break;

                case WoWSpecialization.WarriorArms:
                case WoWSpecialization.WarriorFury:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Plate;
                    }
                    else
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Mail;
                    }
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Polearm);
                    goto Label_05AF;

                case WoWSpecialization.WarriorProtection:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Plate;
                    }
                    else
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Mail;
                    }
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Dagger);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Fist);
                    _honuopapiuweUraep = true;
                    goto Label_05AF;

                case WoWSpecialization.DruidBalance:
                case WoWSpecialization.DruidFeral:
                case WoWSpecialization.DruidGuardian:
                case WoWSpecialization.DruidRestoration:
                    _jawohuoleqire = WowItemSubClassArmor.Leather;
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Staff);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Polearm);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Dagger);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Fist);
                    goto Label_05AF;

                case WoWSpecialization.DeathknightBlood:
                case WoWSpecialization.DeathknightFrost:
                case WoWSpecialization.DeathknightUnholy:
                    _jawohuoleqire = WowItemSubClassArmor.Plate;
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Polearm);
                    goto Label_05AF;

                case WoWSpecialization.HunterBeastMastery:
                case WoWSpecialization.HunterMarksmanship:
                case WoWSpecialization.HunterSurvival:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Mail;
                    }
                    else
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Leather;
                    }
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Gun);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Bow);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Crossbow);
                    goto Label_05AF;

                case WoWSpecialization.PriestDiscipline:
                case WoWSpecialization.PriestHoly:
                case WoWSpecialization.PriestShadow:
                    _jawohuoleqire = WowItemSubClassArmor.Cloth;
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Staff);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Wand);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Dagger);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace);
                    goto Label_05AF;

                case WoWSpecialization.RogueAssassination:
                case WoWSpecialization.RogueOutlaw:
                case WoWSpecialization.RogueSubtlety:
                    _jawohuoleqire = WowItemSubClassArmor.Leather;
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Dagger);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Fist);
                    goto Label_05AF;

                case WoWSpecialization.ShamanElemental:
                case WoWSpecialization.ShamanEnhancement:
                case WoWSpecialization.ShamanRestoration:
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Level >= 40)
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Mail;
                    }
                    else
                    {
                        _jawohuoleqire = WowItemSubClassArmor.Leather;
                    }
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace2);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Polearm);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Staff);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Dagger);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Fist);
                    _honuopapiuweUraep = true;
                    goto Label_05AF;

                case WoWSpecialization.WarlockAffliction:
                case WoWSpecialization.WarlockDemonology:
                case WoWSpecialization.WarlockDestruction:
                    _jawohuoleqire = WowItemSubClassArmor.Cloth;
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Staff);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Wand);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Dagger);
                    goto Label_05AF;

                case WoWSpecialization.MonkBrewmaster:
                case WoWSpecialization.MonkWindwalker:
                case WoWSpecialization.MonkMistweaver:
                    _jawohuoleqire = WowItemSubClassArmor.Leather;
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Polearm);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Staff);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword);
                    _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace);
                    goto Label_05AF;

                default:
                    _jawohuoleqire = WowItemSubClassArmor.Cloth;
                    goto Label_05AF;
            }
            _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Axe2);
            _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Sword2);
            _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Mace2);
            _caekiquehIciuwiaOna.Add(WowItemSubClassWeapon.Polearm);
        Label_05AF:
            switch (spe)
            {
                case WoWSpecialization.MageArcane:
                case WoWSpecialization.MageFire:
                case WoWSpecialization.MageFrost:
                    _ewilosiegulWoacuomeTione.AddRange(list3);
                    return;

                case WoWSpecialization.PaladinHoly:
                    _ewilosiegulWoacuomeTione.AddRange(list2);
                    return;

                case WoWSpecialization.PaladinProtection:
                    _ewilosiegulWoacuomeTione.AddRange(list5);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.PARRY_RATING);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.DODGE_RATING);
                    return;

                case ((WoWSpecialization) 0x43):
                case ((WoWSpecialization) 0x44):
                case ((WoWSpecialization) 0x45):
                    break;

                case WoWSpecialization.PaladinRetribution:
                    _ewilosiegulWoacuomeTione.AddRange(list6);
                    return;

                case WoWSpecialization.WarriorArms:
                case WoWSpecialization.WarriorFury:
                    _ewilosiegulWoacuomeTione.AddRange(list6);
                    return;

                case WoWSpecialization.WarriorProtection:
                    _ewilosiegulWoacuomeTione.AddRange(list5);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.PARRY_RATING);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.DODGE_RATING);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.CRIT_RATING);
                    return;

                case WoWSpecialization.DruidBalance:
                    _ewilosiegulWoacuomeTione.AddRange(list3);
                    return;

                case WoWSpecialization.DruidFeral:
                    _ewilosiegulWoacuomeTione.AddRange(list4);
                    return;

                case WoWSpecialization.DruidGuardian:
                    _ewilosiegulWoacuomeTione.AddRange(list4);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.DODGE_RATING);
                    return;

                case WoWSpecialization.DruidRestoration:
                    _ewilosiegulWoacuomeTione.AddRange(list2);
                    return;

                case WoWSpecialization.DeathknightBlood:
                    _ewilosiegulWoacuomeTione.AddRange(list5);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.PARRY_RATING);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.DODGE_RATING);
                    return;

                case WoWSpecialization.DeathknightFrost:
                case WoWSpecialization.DeathknightUnholy:
                    _ewilosiegulWoacuomeTione.AddRange(list6);
                    return;

                case WoWSpecialization.HunterBeastMastery:
                case WoWSpecialization.HunterMarksmanship:
                case WoWSpecialization.HunterSurvival:
                    _ewilosiegulWoacuomeTione.AddRange(list4);
                    return;

                case WoWSpecialization.PriestDiscipline:
                case WoWSpecialization.PriestHoly:
                    _ewilosiegulWoacuomeTione.AddRange(list2);
                    return;

                case WoWSpecialization.PriestShadow:
                    _ewilosiegulWoacuomeTione.AddRange(list3);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.SPIRIT);
                    return;

                case WoWSpecialization.RogueAssassination:
                case WoWSpecialization.RogueOutlaw:
                case WoWSpecialization.RogueSubtlety:
                    _ewilosiegulWoacuomeTione.AddRange(list4);
                    return;

                case WoWSpecialization.ShamanElemental:
                    _ewilosiegulWoacuomeTione.AddRange(list3);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.SPIRIT);
                    return;

                case WoWSpecialization.ShamanEnhancement:
                    _ewilosiegulWoacuomeTione.AddRange(list4);
                    return;

                case WoWSpecialization.ShamanRestoration:
                    _ewilosiegulWoacuomeTione.AddRange(list2);
                    return;

                case WoWSpecialization.WarlockAffliction:
                case WoWSpecialization.WarlockDemonology:
                case WoWSpecialization.WarlockDestruction:
                    _ewilosiegulWoacuomeTione.AddRange(list3);
                    return;

                case WoWSpecialization.MonkBrewmaster:
                    _ewilosiegulWoacuomeTione.AddRange(list4);
                    _ewilosiegulWoacuomeTione.Add(WoWStatistic.DODGE_RATING);
                    return;

                case WoWSpecialization.MonkWindwalker:
                    _ewilosiegulWoacuomeTione.AddRange(list4);
                    return;

                case WoWSpecialization.MonkMistweaver:
                    _ewilosiegulWoacuomeTione.AddRange(list2);
                    break;

                default:
                    return;
            }
        }

        public static WowItemSubClassArmor EquipableArmorItemType
        {
            get
            {
                return _jawohuoleqire;
            }
        }

        public static List<WowItemSubClassWeapon> EquipableWeapons
        {
            get
            {
                return _caekiquehIciuwiaOna;
            }
        }

        public static List<WoWStatistic> EquipementStats
        {
            get
            {
                return _ewilosiegulWoacuomeTione;
            }
        }

        public static bool HasShield
        {
            get
            {
                return _honuopapiuweUraep;
            }
        }
    }
}

