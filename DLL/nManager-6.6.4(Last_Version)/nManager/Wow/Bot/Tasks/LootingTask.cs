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
                            goto Label_07B3;
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
                        FarmingTask.CurUnit = unit;
                        if (!CombatClass.InMeleeRange(unit) && ((!nManagerSetting.CurrentSetting.UseLootARange || (LootARangeId == 0)) || ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(unit.Position) > 40f) || !ItemsManager.IsItemUsable(LootARangeId))))
                        {
                            bool flag2;
                            List<Point> points = PathFinder.FindPath(unit.Position, out flag2, true, false);
                            if (points.Count <= 0)
                            {
                                points.Add(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                points.Add(unit.Position);
                            }
                            if (!flag2 && (((points.Count == 2) && (unit.GetDistance > 6f)) || (points.Count != 2)))
                            {
                                Logging.Write("No path to " + unit.Name + ", blacklisting.");
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
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
                                FarmingTask.CountThisLoot = true;
                                FarmingTask.NodeOrUnit = false;
                                ItemsManager.UseToy(LootARangeId);
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
                                FarmingTask.CountThisLoot = true;
                                FarmingTask.NodeOrUnit = false;
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
                                    goto Label_04B6;
                                }
                                WoWUnit unit = unit;
                                foreach (WoWUnit unit2 in from u in woWUnits
                                    where u != unit
                                    where u.Position.DistanceTo2D(unit.Position) <= 25f
                                    select u)
                                {
                                    nManagerSetting.AddBlackList(unit2.Guid, 0x1db + Usefuls.Latency);
                                }
                                nManagerSetting.AddBlackList(unit.Guid, 0x1db + Usefuls.Latency);
                            }
                            return;
                        }
                    Label_04B6:
                        if ((flag || !unit.IsLootable) && !unit.IsSkinnable)
                        {
                            continue;
                        }
                        if (!nManagerSetting.CurrentSetting.ActivateBeastSkinning || (nManager.Wow.ObjectManager.ObjectManager.GetNumberAttackPlayer() != 0))
                        {
                            goto Label_07B3;
                        }
                        if (flag || !unit.IsLootable)
                        {
                            if (unit.ExtraLootType.HasFlag(TypeFlag.HERB_LOOT))
                            {
                                if (Skill.GetValue(SkillLine.Herbalism) > 0)
                                {
                                    goto Label_05FE;
                                }
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                                continue;
                            }
                            if ((unit.ExtraLootType.HasFlag(TypeFlag.MINING_LOOT) || (unit.Entry == 0x199bf)) || (unit.Entry == 0x199ad))
                            {
                                if (Skill.GetValue(SkillLine.Mining) > 0)
                                {
                                    goto Label_05FE;
                                }
                                nManagerSetting.AddBlackList(unit.Guid, 0x493e0);
                                continue;
                            }
                            if (unit.ExtraLootType.HasFlag(TypeFlag.ENGENEERING_LOOT))
                            {
                                if (Skill.GetValue(SkillLine.Engineering) > 0)
                                {
                                    goto Label_05FE;
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
                    Label_05FE:
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
                    Label_07B3:
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

