namespace nManager.Wow.Bot.Tasks
{
    using nManager;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class LootingTask
    {
        public static int LootARangeId;

        public static void LootAndConfirmBoPForAllItems(bool forceBoP)
        {
            try
            {
                Thread.Sleep(Usefuls.Latency);
                Lua.LuaDoString("for slot = 1, GetNumLootItems() do     LootSlot(slot) " + (forceBoP ? " ConfirmLootSlot(slot) " : "") + "end", false, true);
                Thread.Sleep(Usefuls.Latency);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LootingTask > ConfirmOnBoPItems(): " + exception, true);
            }
        }

        public static void Pulse(IEnumerable<WoWUnit> woWUnits)
        {
            try
            {
                woWUnits = from x in woWUnits
                    orderby x.GetDistance
                    select x;
                foreach (WoWUnit unit in woWUnits)
                {
                    try
                    {
                        bool flag;
                        if (!nManager.Products.Products.IsStarted)
                        {
                            goto Label_075E;
                        }
                        if (!nManagerSetting.IsBlackListed(unit.Guid))
                        {
                            MovementManager.StopMove();
                            MovementManager.StopMove();
                            Thread.Sleep((int) (250 + Usefuls.Latency));
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(200);
                            }
                            if (unit.IsValid)
                            {
                                flag = false;
                                if (unit.IsLootable)
                                {
                                    Logging.Write("Loot " + unit.Name);
                                    goto Label_00E6;
                                }
                                if (unit.IsSkinnable && nManagerSetting.CurrentSetting.ActivateBeastSkinning)
                                {
                                    Logging.Write("Skin " + unit.Name);
                                    goto Label_00E6;
                                }
                            }
                        }
                        continue;
                    Label_00E6:
                        if (!CombatClass.InMeleeRange(unit) && ((!nManagerSetting.CurrentSetting.UseLootARange || (LootARangeId == 0)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) > 40f) || !ItemsManager.IsItemUsable(LootARangeId))))
                        {
                            List<Point> points = PathFinder.FindPath(unit.Position);
                            if (points.Count <= 0)
                            {
                                points.Add(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                points.Add(unit.Position);
                            }
                            MovementManager.Go(points);
                            nManager.Helpful.Timer timer = new nManager.Helpful.Timer((double) (((int) ((nManager.Helpful.Math.DistanceListPoint(points) / 3f) * 1000f)) + 0xbb8));
                            while ((((!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && unit.IsValid) && (nManager.Products.Products.IsStarted && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0))) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))) && !timer.IsReady)
                            {
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) <= 4f)
                                {
                                    MovementManager.StopMove();
                                    MovementManager.StopMove();
                                    MountTask.DismountMount(true);
                                    Thread.Sleep(250);
                                    while (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                                    {
                                        Thread.Sleep(50);
                                    }
                                    break;
                                }
                            }
                        }
                        if (unit.IsLootable)
                        {
                            if (((nManagerSetting.CurrentSetting.UseLootARange && !CombatClass.InMeleeRange(unit)) && ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) <= 40f) && (LootARangeId != 0))) && ItemsManager.IsItemUsable(LootARangeId))
                            {
                                while (ItemsManager.IsItemOnCooldown(LootARangeId))
                                {
                                    Thread.Sleep(250);
                                }
                                ItemsManager.UseItem(LootARangeId);
                                Thread.Sleep((int) (0x3e8 + Usefuls.Latency));
                                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                                    {
                                        return;
                                    }
                                    Thread.Sleep(150);
                                }
                            }
                            else
                            {
                                Interact.InteractWith(unit.GetBaseAddress, false);
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                                {
                                    return;
                                }
                                Thread.Sleep((int) (500 + Usefuls.Latency));
                            }
                            if (!nManagerSetting.CurrentSetting.ActivateBeastSkinning || (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() <= 0))
                            {
                                Statistics.Loots++;
                                if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    Elemental.AutoMakeElemental();
                                }
                                if (nManagerSetting.CurrentSetting.ActivateBeastSkinning)
                                {
                                    Thread.Sleep((int) (0x1db + Usefuls.Latency));
                                    flag = true;
                                    goto Label_0437;
                                }
                                WoWUnit unit = unit;
                                foreach (WoWUnit unit2 in from u in woWUnits
                                    where u != unit
                                    where u.Position.DistanceTo2D(unit.Position) <= 25f
                                    select u)
                                {
                                    nManagerSetting.AddBlackList(unit2.Guid, 0xa28);
                                }
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                            }
                            return;
                        }
                    Label_0437:
                        if (flag && !unit.IsSkinnable)
                        {
                            continue;
                        }
                        if (!nManagerSetting.CurrentSetting.ActivateBeastSkinning || (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() != 0))
                        {
                            goto Label_075E;
                        }
                        if (flag)
                        {
                            if (unit.ExtraLootType.HasFlag(TypeFlag.HERB_LOOT))
                            {
                                int num = Skill.GetValue(SkillLine.Herbalism);
                                if (num > 0)
                                {
                                    num += Skill.GetSkillBonus(SkillLine.Herbalism);
                                }
                                if (unit.GetSkillLevelRequired <= num)
                                {
                                    goto Label_05A9;
                                }
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                                continue;
                            }
                            if (unit.ExtraLootType.HasFlag(TypeFlag.MINING_LOOT))
                            {
                                int num2 = Skill.GetValue(SkillLine.Mining);
                                if (num2 > 0)
                                {
                                    num2 += Skill.GetSkillBonus(SkillLine.Mining);
                                }
                                if (unit.GetSkillLevelRequired <= num2)
                                {
                                    goto Label_05A9;
                                }
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                                continue;
                            }
                            if (unit.ExtraLootType.HasFlag(TypeFlag.ENGENEERING_LOOT))
                            {
                                int num3 = Skill.GetValue(SkillLine.Engineering);
                                if (num3 > 0)
                                {
                                    num3 += Skill.GetSkillBonus(SkillLine.Engineering);
                                }
                                if (unit.GetSkillLevelRequired <= num3)
                                {
                                    goto Label_05A9;
                                }
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                                continue;
                            }
                            if (Skill.GetValue(SkillLine.Skinning) <= 0)
                            {
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                                continue;
                            }
                        }
                    Label_05A9:
                        if (!CombatClass.InMeleeRange(unit))
                        {
                            List<Point> list2 = PathFinder.FindPath(unit.Position);
                            if (list2.Count <= 0)
                            {
                                list2.Add(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                list2.Add(unit.Position);
                            }
                            MovementManager.Go(list2);
                            nManager.Helpful.Timer timer2 = new nManager.Helpful.Timer((double) (((int) ((nManager.Helpful.Math.DistanceListPoint(list2) / 3f) * 1000f)) + 0xbb8));
                            while ((((!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && unit.IsValid) && (nManager.Products.Products.IsStarted && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() == 0))) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (nManagerSetting.CurrentSetting.IgnoreFightIfMounted || Usefuls.IsFlying)))) && !timer2.IsReady)
                            {
                                if (CombatClass.InMeleeRange(unit))
                                {
                                    MovementManager.StopMove();
                                    MovementManager.StopMove();
                                    MountTask.DismountMount(true);
                                    Thread.Sleep(250);
                                    while (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                                    {
                                        Thread.Sleep(50);
                                    }
                                    break;
                                }
                            }
                        }
                        Logging.Write("Skin " + unit.Name);
                        Interact.InteractWith(unit.GetBaseAddress, false);
                        Thread.Sleep((int) (200 + Usefuls.Latency));
                        while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                        {
                            Thread.Sleep(100);
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                        {
                            return;
                        }
                        Thread.Sleep((int) (400 + Usefuls.Latency));
                        if (nManagerSetting.CurrentSetting.ActivateBeastSkinning && (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() > 0))
                        {
                            return;
                        }
                        Statistics.Farms++;
                        nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                    Label_075E:
                        MovementManager.StopMove();
                        MovementManager.StopMove();
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LootingTask > Pulse(IEnumerable<WoWUnit> woWUnits): " + exception, true);
            }
        }
    }
}

