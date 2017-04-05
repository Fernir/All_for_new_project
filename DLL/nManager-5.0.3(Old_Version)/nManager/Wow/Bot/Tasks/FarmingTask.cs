namespace nManager.Wow.Bot.Tasks
{
    using nManager;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.States;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class FarmingTask
    {
        private static bool _countThisLoot;
        private static WoWGameObject _curNode;
        private static bool _firstRun = true;
        private static UInt128 _lastnode;
        private static bool _wasLooted;

        private static void Fly(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                nodes = from x in nodes
                    orderby x.GetDistance
                    select x;
                foreach (WoWGameObject obj2 in from node in nodes
                    where node.IsValid
                    select node)
                {
                    float num;
                    Point point;
                    Point point2;
                    Logging.Write(string.Concat(new object[] { "Farm ", obj2.Name, " (", obj2.Entry, ") > ", obj2.Position.X, "; ", obj2.Position.Y, "; ", obj2.Position.Z }));
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z < obj2.Position.Z)
                    {
                        num = obj2.Position.Z + 5.5f;
                    }
                    else
                    {
                        num = obj2.Position.Z + 2.5f;
                    }
                    point = new Point(obj2.Position) {
                        Z = point.Z + 2.5f
                    };
                    if (TraceLine.TraceLineGo(new Point(point) { Z = point2.Z + 80f }, point, CGWorldFrameHitFlags.HitTestAllButLiquid) && TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, point, CGWorldFrameHitFlags.HitTestAllButLiquid))
                    {
                        Logging.Write("Node stuck");
                        nManagerSetting.AddBlackList(obj2.Guid, 0x1d4c0);
                        return;
                    }
                    MovementManager.StopMove();
                    MovementManager.MoveTo(obj2.Position.X, obj2.Position.Y, num, true);
                    nManager.Helpful.Timer timer = new nManager.Helpful.Timer((double) (((int) ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(obj2.Position) / 3f) * 1000f)) + 0x1388));
                    bool flag = false;
                    bool flag2 = false;
                    while ((((obj2.IsValid && nManager.Products.Products.IsStarted) && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat || nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)) && !timer.IsReady)
                    {
                        if (!flag2)
                        {
                            bool flag3 = TraceLine.TraceLineGo(point);
                            num = flag3 ? nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z : point.Z;
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z < point.Z)
                            {
                                Point to = nManager.Helpful.Math.GetPosition2DOfLineByDistance(nManager.Wow.ObjectManager.ObjectManager.Me.Position, obj2.Position, (obj2.Position.Z + 2.5f) - nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z);
                                if (TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to, CGWorldFrameHitFlags.HitTestAllButLiquid))
                                {
                                    to = nManager.Helpful.Math.GetPosition2DOfLineByDistance(nManager.Wow.ObjectManager.ObjectManager.Me.Position, obj2.Position, 1f);
                                }
                                MovementManager.MoveTo(to.X, to.Y, obj2.Position.Z + 5f, false);
                            }
                            else
                            {
                                MovementManager.MoveTo(obj2.Position.X, obj2.Position.Y, num, false);
                            }
                            if (!flag3)
                            {
                                flag2 = true;
                            }
                        }
                        if (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(obj2.Position) < 4f) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceZ(obj2.Position) >= 5f)) && !flag)
                        {
                            flag = true;
                            num = obj2.Position.Z + 1.5f;
                            MovementManager.MoveTo(obj2.Position.X, obj2.Position.Y, num, false);
                            if ((obj2.GetDistance <= 3f) || !TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, obj2.Position, CGWorldFrameHitFlags.HitTestAllButLiquid))
                            {
                                goto Label_069B;
                            }
                            Logging.Write("Node outside view");
                            nManagerSetting.AddBlackList(obj2.Guid, 0x1d4c0);
                            break;
                        }
                        if (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(obj2.Position) < 1.1f) || (!Usefuls.IsFlying && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(obj2.Position) < 3f))) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceZ(obj2.Position) < 6f))
                        {
                            Thread.Sleep(150);
                            MovementManager.StopMove();
                            if (Usefuls.IsFlying)
                            {
                                MountTask.Land(false);
                            }
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                            {
                                MovementManager.StopMove();
                            }
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                            {
                                Thread.Sleep(50);
                            }
                            if (!obj2.IsHerb || (obj2.IsHerb && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId())))
                            {
                                if ((!SpellManager.HasSpell(0x29686) || (Usefuls.ContinentId != 0x45c)) && (Usefuls.ContinentId != 0x5b8))
                                {
                                    Usefuls.DisMount();
                                }
                            }
                            else if (obj2.IsHerb)
                            {
                                Logging.WriteDebug("Druid IsFlying ? " + Usefuls.IsFlying);
                                if (Usefuls.IsFlying)
                                {
                                    MountTask.Land(false);
                                    MovementManager.StopMove();
                                    if (Usefuls.IsFlying)
                                    {
                                        Logging.Write("You are still flying after two attemps of Landing.");
                                        Logging.Write("Make sure you have binded the action \"SitOrStand\" on a keyboard key and not any mouse button or special button.");
                                        Logging.Write("If you still have this message, please try a \"Reset Keybindings\" before posting on the forum.");
                                        Logging.Write("A work arround have been used, it may let you actually farm or not. Because it's random, please fix your keybinding issue.");
                                        MountTask.Land(true);
                                    }
                                }
                            }
                            Thread.Sleep((int) (Usefuls.Latency + 200));
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                            {
                                Usefuls.DisMount();
                            }
                            else
                            {
                                _wasLooted = false;
                                _countThisLoot = true;
                                Interact.InteractWith(obj2.GetBaseAddress, false);
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Interact.InteractWith(obj2.GetBaseAddress, false);
                                    Thread.Sleep((int) (Usefuls.Latency + 500));
                                }
                                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                                {
                                    Thread.Sleep(100);
                                }
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                                {
                                    Usefuls.DisMount();
                                    _countThisLoot = false;
                                }
                                else
                                {
                                    Thread.Sleep((int) (Usefuls.Latency + 100));
                                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))
                                    {
                                        Usefuls.DisMount();
                                        _countThisLoot = false;
                                    }
                                    else
                                    {
                                        nManagerSetting.AddBlackList(obj2.Guid, 0x4e20);
                                    }
                                }
                            }
                            return;
                        }
                        if (!nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                        {
                            Thread.Sleep(50);
                            MovementManager.MoveTo(obj2.Position.X, obj2.Position.Y, num, false);
                        }
                    Label_069B:
                        if (Farming.PlayerNearest(obj2))
                        {
                            Logging.Write("Player near the node, farm canceled");
                            nManagerSetting.AddBlackList(obj2.Guid, 0x3a98);
                            return;
                        }
                    }
                    if (timer.IsReady)
                    {
                        nManagerSetting.AddBlackList(obj2.Guid, -1);
                    }
                    MovementManager.StopMove();
                    if (!_wasLooted)
                    {
                        Logging.Write("Farm failed");
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FarmingTask > Fly(IEnumerable<WoWGameObject> nodes): " + exception, true);
            }
        }

        private static void Ground(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                nodes = from x in nodes
                    orderby x.GetDistance
                    select x;
                foreach (WoWGameObject obj2 in nodes)
                {
                    WoWGameObject obj3 = obj2;
                    if (((_curNode != null) && _curNode.IsValid) && !nManagerSetting.IsBlackListed(_curNode.Guid))
                    {
                        obj3 = _curNode;
                    }
                    if (obj3.IsValid)
                    {
                        _curNode = obj2;
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(obj3.Position) > 5f)
                        {
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(obj3.Position) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount) || !nManagerSetting.CurrentSetting.UseGroundMount)
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                                {
                                    if (!MountTask.OnFlyMount())
                                    {
                                        MountTask.Mount(true);
                                    }
                                    else
                                    {
                                        MountTask.Takeoff();
                                    }
                                    Fly(nodes);
                                    return;
                                }
                                if (MountTask.GetMountCapacity() == MountCapacity.Swimm)
                                {
                                    if (!MountTask.OnAquaticMount())
                                    {
                                        MountTask.Mount(true);
                                    }
                                    Fly(nodes);
                                    return;
                                }
                            }
                            if (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(obj3.Position) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount) && nManagerSetting.CurrentSetting.UseGroundMount) && ((MountTask.GetMountCapacity() == MountCapacity.Ground) && !MountTask.OnGroundMount()))
                            {
                                MountTask.Mount(true);
                            }
                            if (MovementManager.FindTarget(obj3, 5f, true, nManagerSetting.CurrentSetting.GatheringSearchRadius * 4f) == 0)
                            {
                                nManagerSetting.AddBlackList(obj3.Guid, 0x493e0);
                                _curNode = null;
                                return;
                            }
                            if (_lastnode != obj3.Guid)
                            {
                                _lastnode = obj3.Guid;
                                Logging.Write(string.Concat(new object[] { "Farm ", obj3.Name, " > ", obj3.Position }));
                            }
                            if (MovementManager.InMovement)
                            {
                                return;
                            }
                        }
                        MovementManager.StopMove();
                        while (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                        {
                            Thread.Sleep(250);
                        }
                        Thread.Sleep((int) (250 + Usefuls.Latency));
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                        {
                            if (!obj3.IsHerb || (obj3.IsHerb && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.MountDruidId())))
                            {
                                MountTask.DismountMount(true);
                            }
                        }
                        else
                        {
                            _wasLooted = false;
                            _countThisLoot = true;
                            Interact.InteractWith(obj3.GetBaseAddress, false);
                            Thread.Sleep((int) (Usefuls.Latency + 500));
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Interact.InteractWith(obj2.GetBaseAddress, false);
                                Thread.Sleep((int) (Usefuls.Latency + 500));
                            }
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(150);
                            }
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                            {
                                _countThisLoot = false;
                            }
                            else
                            {
                                Thread.Sleep((int) (100 + Usefuls.Latency));
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                                {
                                    _countThisLoot = false;
                                }
                                else
                                {
                                    nManagerSetting.AddBlackList(obj3.Guid, 0x4e20);
                                    if (!_wasLooted)
                                    {
                                        Logging.Write("Farm failed");
                                    }
                                }
                            }
                        }
                        return;
                    }
                    MovementManager.StopMove();
                    nManagerSetting.AddBlackList(obj3.Guid, -1);
                    Logging.Write("Farm failed");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FarmingTask > Ground(IEnumerable<WoWGameObject> nodes): " + exception, true);
            }
        }

        public static void Pulse(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                if (_firstRun)
                {
                    EventsListener.HookEvent(WoWEventsType.LOOT_READY, callback => TakeFarmingLoots(), false, true);
                    _firstRun = false;
                }
                if (Usefuls.IsFlying)
                {
                    Fly(nodes);
                }
                else
                {
                    Ground(nodes);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FarmingTask > Pulse(IEnumerable<WoWGameObject> nodes): " + exception, true);
            }
        }

        private static void TakeFarmingLoots()
        {
            if (_countThisLoot)
            {
                _countThisLoot = false;
                LootingTask.LootAndConfirmBoPForAllItems(nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems);
                Statistics.Farms++;
                Logging.Write("Farm successful");
                _wasLooted = true;
                _curNode = null;
                if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems && nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                {
                    Elemental.AutoMakeElemental();
                }
            }
        }
    }
}

