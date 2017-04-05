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
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public static class FarmingTask
    {
        private static UInt128 _doakaFoa;
        private static bool _imounoax;
        private static WoWGameObject _roaloGaepam;
        public static bool CountThisLoot;
        public static WoWUnit CurUnit;
        public static bool NodeOrUnit;

        [CompilerGenerated]
        private static float <Fly>b__0(WoWGameObject x)
        {
            return x.GetDistance;
        }

        [CompilerGenerated]
        private static bool <Fly>b__1(WoWGameObject node)
        {
            return node.IsValid;
        }

        [CompilerGenerated]
        private static float <Ground>b__4(WoWGameObject x)
        {
            return x.GetDistance;
        }

        private static void Idaeqolaul(IEnumerable<WoWGameObject> hinoaLae)
        {
            try
            {
                if (CS$<>9__CachedAnonymousMethodDelegate5 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate5 = new Func<WoWGameObject, float>(FarmingTask.<Ground>b__4);
                }
                hinoaLae = hinoaLae.OrderBy<WoWGameObject, float>(CS$<>9__CachedAnonymousMethodDelegate5);
                foreach (WoWGameObject obj2 in hinoaLae)
                {
                    WoWGameObject gObject = obj2;
                    if (((_roaloGaepam != null) && _roaloGaepam.IsValid) && !nManagerSetting.IsBlackListed(_roaloGaepam.Guid))
                    {
                        gObject = _roaloGaepam;
                    }
                    if (gObject.IsValid)
                    {
                        _roaloGaepam = obj2;
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(gObject.Position) > 5f)
                        {
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(gObject.Position) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount) || !nManagerSetting.CurrentSetting.UseGroundMount)
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                                {
                                    if (!MountTask.OnFlyMount())
                                    {
                                        MountTask.Mount(true, true);
                                    }
                                    else
                                    {
                                        MountTask.Takeoff();
                                    }
                                    Kacaquha(hinoaLae);
                                    return;
                                }
                                if (MountTask.GetMountCapacity() == MountCapacity.Swimm)
                                {
                                    if (!MountTask.OnAquaticMount())
                                    {
                                        MountTask.Mount(true, false);
                                    }
                                    Kacaquha(hinoaLae);
                                    return;
                                }
                            }
                            if (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(gObject.Position) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount) && nManagerSetting.CurrentSetting.UseGroundMount) && ((MountTask.GetMountCapacity() == MountCapacity.Ground) && !MountTask.OnGroundMount()))
                            {
                                MountTask.Mount(true, false);
                            }
                            if (MovementManager.FindTarget(gObject, 5.5f, true, nManagerSetting.CurrentSetting.GatheringSearchRadius * 4f, false) == 0)
                            {
                                nManagerSetting.AddBlackList(gObject.Guid, 0x4e20);
                                _roaloGaepam = null;
                                return;
                            }
                            if (_doakaFoa != gObject.Guid)
                            {
                                _doakaFoa = gObject.Guid;
                                Logging.Write(string.Concat(new object[] { "Farm ", gObject.Name, " > ", gObject.Position }));
                            }
                            if (gObject.GetDistance < 5.5f)
                            {
                                MovementManager.StopMove();
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
                        if ((nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.AllInteractMountId())) && (!obj2.IsHerb || (obj2.IsHerb && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.HerbsInteractMountId()))))
                        {
                            MountTask.DismountMount(true);
                        }
                        else
                        {
                            _imounoax = false;
                            CountThisLoot = true;
                            NodeOrUnit = true;
                            Interact.InteractWith(gObject.GetBaseAddress, false);
                            Thread.Sleep((int) (Usefuls.Latency + 300));
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Interact.InteractWith(obj2.GetBaseAddress, false);
                                Thread.Sleep((int) (Usefuls.Latency + 250));
                            }
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(150);
                            }
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.AllInteractMountId())) && (!obj2.IsHerb || (obj2.IsHerb && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.HerbsInteractMountId()))))
                            {
                                CountThisLoot = false;
                            }
                            else
                            {
                                Thread.Sleep((int) (100 + Usefuls.Latency));
                                if ((nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.AllInteractMountId())) && (!obj2.IsHerb || (obj2.IsHerb && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.HerbsInteractMountId()))))
                                {
                                    CountThisLoot = false;
                                }
                                else
                                {
                                    if (CountThisLoot && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                                    {
                                        nManagerSetting.AddBlackList(gObject.Guid, 0x4e20);
                                    }
                                    Thread.Sleep(0x3e8);
                                    if (!_imounoax)
                                    {
                                        Logging.Write("Farm failed #2");
                                        if (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.HerbsInteractMountId()) || nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.AllInteractMountId())))
                                        {
                                            MountTask.DismountMount(true);
                                        }
                                    }
                                }
                            }
                        }
                        return;
                    }
                    MovementManager.StopMove();
                    nManagerSetting.AddBlackList(gObject.Guid, 0x1d4c0);
                    Logging.Write("Current node not valid, blacklist.");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FarmingTask > Ground(IEnumerable<WoWGameObject> nodes): " + exception, true);
            }
        }

        private static void Kacaquha(IEnumerable<WoWGameObject> hinoaLae)
        {
            try
            {
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<WoWGameObject, float>(FarmingTask.<Fly>b__0);
                }
                hinoaLae = hinoaLae.OrderBy<WoWGameObject, float>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate3 = new Func<WoWGameObject, bool>(FarmingTask.<Fly>b__1);
                }
                foreach (WoWGameObject obj2 in hinoaLae.Where<WoWGameObject>(CS$<>9__CachedAnonymousMethodDelegate3))
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
                    while ((obj2.IsValid && nManager.Products.Products.IsStarted) && ((!nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe && !nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat) && !timer.IsReady))
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
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                            {
                                return;
                            }
                            if (!flag3)
                            {
                                flag2 = true;
                            }
                        }
                        if (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(obj2.Position) < 4f) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceZ(obj2.Position) >= 5f)) && !flag)
                        {
                            flag = true;
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                            {
                                return;
                            }
                            num = obj2.Position.Z + 1.5f;
                            MovementManager.MoveTo(obj2.Position.X, obj2.Position.Y, num, false);
                            if ((obj2.GetDistance <= 3f) || !TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, obj2.Position, CGWorldFrameHitFlags.HitTestAllButLiquid))
                            {
                                goto Label_06A3;
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
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.AllInteractMountId()) && (!obj2.IsHerb || (obj2.IsHerb && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.HerbsInteractMountId()))))
                            {
                                if ((!SpellManager.HasSpell(0x29686) || (Usefuls.ContinentId != 0x45c)) && (Usefuls.ContinentId != 0x5b8))
                                {
                                    MountTask.DismountMount(true);
                                }
                            }
                            else if (obj2.IsHerb && nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff(SpellManager.DruidMountId()))
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
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat)
                            {
                                MountTask.DismountMount(true);
                            }
                            else
                            {
                                _imounoax = false;
                                CountThisLoot = true;
                                NodeOrUnit = true;
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
                                if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat)
                                {
                                    MountTask.DismountMount(true);
                                    CountThisLoot = false;
                                }
                                else
                                {
                                    Thread.Sleep((int) (Usefuls.Latency + 100));
                                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat)
                                    {
                                        MountTask.DismountMount(true);
                                        CountThisLoot = false;
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
                            if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                            {
                                return;
                            }
                            MovementManager.MoveTo(obj2.Position.X, obj2.Position.Y, num, false);
                        }
                    Label_06A3:
                        if (Farming.PlayerNearest(obj2))
                        {
                            Logging.Write("Player near the node, farm canceled");
                            nManagerSetting.AddBlackList(obj2.Guid, 0x3a98);
                            return;
                        }
                    }
                    if (timer.IsReady)
                    {
                        nManagerSetting.AddBlackList(obj2.Guid, 0xea60);
                    }
                    MovementManager.StopMove();
                    if (!_imounoax)
                    {
                        Logging.Write("Farm failed #1");
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FarmingTask > Fly(IEnumerable<WoWGameObject> nodes): " + exception, true);
            }
        }

        public static void Pulse(IEnumerable<WoWGameObject> nodes)
        {
            try
            {
                if (Usefuls.IsFlying)
                {
                    Kacaquha(nodes);
                }
                else
                {
                    Idaeqolaul(nodes);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FarmingTask > Pulse(IEnumerable<WoWGameObject> nodes): " + exception, true);
            }
        }

        public static void TakeFarmingLoots()
        {
            if (CountThisLoot)
            {
                _imounoax = true;
                CountThisLoot = false;
                LootingTask.LootAndConfirmBoPForAllItems(nManagerSetting.CurrentSetting.AutoConfirmOnBoPItems);
                Thread.Sleep(200);
                if (!Others.IsFrameVisible("LootFrame"))
                {
                    Statistics.Farms++;
                    Logging.Write("Farm successful");
                }
                else
                {
                    Statistics.Farms++;
                    Logging.Write("Farm partially successful");
                }
                if ((NodeOrUnit && (_roaloGaepam != null)) && _roaloGaepam.IsValid)
                {
                    nManagerSetting.AddBlackList(_roaloGaepam.Guid, 0xea60);
                }
                if ((!NodeOrUnit && (CurUnit != null)) && CurUnit.IsValid)
                {
                    nManagerSetting.AddBlackList(CurUnit.Guid, 0xea60);
                }
                _roaloGaepam = null;
                CurUnit = null;
                if (nManagerSetting.CurrentSetting.MakeStackOfElementalsItems && nManager.Wow.ObjectManager.ObjectManager.Me.InCombat)
                {
                    Elemental.AutoMakeElemental();
                }
            }
        }
    }
}

