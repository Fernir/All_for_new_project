namespace nManager.Wow.Bot.States
{
    using nManager;
    using nManager.FiniteStateMachine;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class GarrisonState : nManager.FiniteStateMachine.State
    {
        private static int _cacheGarden;
        private static bool _cacheGardenGathered;
        private static List<int> _cacheGarrison;
        private static Point _cacheGarrisonPoint;
        private static int _cacheMine;
        private static bool _cacheMineGathered;
        private static Point _garden;
        private static Point _mineEntrance;
        private static int _npcGarden;
        private static int _npcMine;
        private static bool _oldActivateHerbsHarvesting = nManagerSetting.CurrentSetting.ActivateHerbsHarvesting;
        private static bool _oldActivateVeinsHarvesting = nManagerSetting.CurrentSetting.ActivateVeinsHarvesting;
        private static float _oldGatheringSearchRadius = nManagerSetting.CurrentSetting.GatheringSearchRadius;
        private static bool _settingsUnclean = false;
        private static Npc _targetNpc = new Npc();
        private static readonly Farming FarmingState = new Farming();
        private const int GarrisonHearthstone = 0x1afe0;
        private const int MinerCoffee = 0x1d071;
        private const uint MinerCoffeeBuff = 0x2afb1;
        private const int PreservedMiningPick = 0x1d077;
        private const uint PreservedMiningPickBuff = 0x2afbd;
        private static Task previousTask = Task.None;
        private static Stack<Task> tList = null;

        public GarrisonState()
        {
            nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
            nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
            _settingsUnclean = true;
        }

        private static void CloseProduct()
        {
            new Thread(new ThreadStart(nManager.Products.Products.ProductStopFromFSM)).Start();
        }

        public static void RestoreSettings()
        {
            if (_settingsUnclean)
            {
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = _oldActivateVeinsHarvesting;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = _oldActivateHerbsHarvesting;
                nManagerSetting.CurrentSetting.GatheringSearchRadius = _oldGatheringSearchRadius;
                _settingsUnclean = false;
            }
        }

        public override void Run()
        {
            if (!MovementManager.InMovement)
            {
                bool flag;
                if (tList == null)
                {
                    tList = new Stack<Task>();
                    tList.Push(Task.CheckGarrisonRessourceCache);
                    if (Garrison.GetGarrisonLevel() > 1)
                    {
                        tList.Push(Task.MineWorkOrder);
                        tList.Push(Task.GatherMinerals);
                        tList.Push(Task.GardenWorkOrder);
                        tList.Push(Task.GatherHerbs);
                    }
                    tList.Push(Task.GoToGarrison);
                    _cacheGarrison = new List<int> { 0x3a09a, 0x39d74, 0x39e87, 0x3a09c, 0x3a098, 0x3a09b };
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction == "Alliance")
                    {
                        _npcGarden = 0x14e0a;
                        _cacheGarden = 0x3996d;
                        _npcMine = 0x12fa2;
                        _cacheMine = 0x3996e;
                        Point point = new Point {
                            X = 1833.85f,
                            Y = 154.7408f,
                            Z = 76.66339f
                        };
                        _garden = point;
                        Point point2 = new Point {
                            X = 1886.021f,
                            Y = 83.23455f,
                            Z = 84.31888f
                        };
                        _mineEntrance = point2;
                        Point point3 = new Point {
                            X = 1914.361f,
                            Y = 290.3863f,
                            Z = 88.96407f
                        };
                        _cacheGarrisonPoint = point3;
                    }
                    else
                    {
                        _npcGarden = 0x14f17;
                        _cacheGarden = 0x3a686;
                        _npcMine = 0x13f18;
                        _cacheMine = 0x3a685;
                        Point point4 = new Point {
                            X = 5413.795f,
                            Y = 4548.928f,
                            Z = 139.1232f
                        };
                        _garden = point4;
                        Point point5 = new Point {
                            X = 5465.796f,
                            Y = 4430.045f,
                            Z = 145.4595f
                        };
                        _mineEntrance = point5;
                        Point point6 = new Point {
                            X = 5592.229f,
                            Y = 4569.476f,
                            Z = 136.1069f
                        };
                        _cacheGarrisonPoint = point6;
                    }
                    _cacheGardenGathered = false;
                    _cacheMineGathered = false;
                }
                bool flag2 = false;
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                Task task = tList.Peek();
                if (task != previousTask)
                {
                    previousTask = task;
                    flag2 = true;
                }
                switch (task)
                {
                    case Task.GoToGarrison:
                        if (flag2)
                        {
                            Logging.Write("Task: go to garrison");
                        }
                        if (!Garrison.GarrisonMapIdList.Contains(Usefuls.RealContinentId))
                        {
                            if (Usefuls.ContinentId == Usefuls.ContinentIdByContinentName("Draenor"))
                            {
                                if (MountTask.GetMountCapacity() == MountCapacity.Fly)
                                {
                                    LongMove.LongMoveGo(_cacheGarrisonPoint);
                                    return;
                                }
                                if (_cacheGarrisonPoint.DistanceTo(position) < 100f)
                                {
                                    List<Point> points = PathFinder.FindPath(_cacheGarrisonPoint, out flag, true, false);
                                    if (flag)
                                    {
                                        MovementManager.Go(points);
                                        return;
                                    }
                                }
                            }
                            if ((ItemsManager.GetItemCount(0x1afe0) > 0) && !ItemsManager.IsItemOnCooldown(0x1afe0))
                            {
                                Logging.Write("Using garrison Hearthstone");
                                ItemsManager.UseItem(0x1afe0);
                            }
                            else
                            {
                                Logging.Write("Run aborted, you are not in Draenor or too far away and don't known how to fly and don't have a Garrison Hearthstone or it's on Cooldown.");
                                tList.Clear();
                                break;
                            }
                        }
                        tList.Pop();
                        break;

                    case Task.GatherHerbs:
                        if (flag2)
                        {
                            Logging.Write("Task: gather plants in garrison garden");
                        }
                        if (_garden.DistanceTo(position) > 15f)
                        {
                            MovementManager.Go(PathFinder.FindPath(_garden, out flag, true, false));
                            return;
                        }
                        nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = true;
                        nManagerSetting.CurrentSetting.GatheringSearchRadius = 30f;
                        if (!FarmingState.NeedToRun)
                        {
                            Logging.Write("Finished to farm garrison garden");
                            nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                            tList.Pop();
                        }
                        break;

                    case Task.GardenWorkOrder:
                    {
                        if (flag2)
                        {
                            Logging.Write("Task: collect garden cache and send work order");
                        }
                        if (!_cacheGardenGathered)
                        {
                            WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(_cacheGarden), false);
                            if (nearestWoWGameObject.GetBaseAddress != 0)
                            {
                                if (nearestWoWGameObject.Position.DistanceTo(position) > 5f)
                                {
                                    Npc npc2 = new Npc {
                                        Entry = nearestWoWGameObject.Entry,
                                        Position = nearestWoWGameObject.Position
                                    };
                                    _targetNpc = npc2;
                                    MovementManager.FindTarget(ref _targetNpc, 5f, true, false, 0f);
                                    return;
                                }
                                Thread.Sleep((int) (Usefuls.Latency + 250));
                                Interact.InteractWith(nearestWoWGameObject.GetBaseAddress, true);
                                _cacheGardenGathered = true;
                                Thread.Sleep((int) (Usefuls.Latency + 0x6d6));
                            }
                        }
                        WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(_npcGarden, false), false, false, false);
                        if (unit.GetBaseAddress != 0)
                        {
                            if (unit.Position.DistanceTo(position) > 5f)
                            {
                                Npc npc3 = new Npc {
                                    Entry = unit.Entry,
                                    Position = unit.Position
                                };
                                _targetNpc = npc3;
                                MovementManager.FindTarget(ref _targetNpc, 5f, true, false, 0f);
                                return;
                            }
                            Interact.InteractWith(unit.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                            Interact.InteractWith(unit.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 500));
                            Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()", false, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                        }
                        tList.Pop();
                        break;
                    }
                    case Task.GatherMinerals:
                        if (flag2)
                        {
                            Logging.Write("Task: gather ores and carts in garrison mine");
                        }
                        if (_mineEntrance.DistanceTo(position) > 15f)
                        {
                            MovementManager.Go(PathFinder.FindPath(_mineEntrance, out flag, true, false));
                            return;
                        }
                        nManagerSetting.CurrentSetting.GatheringSearchRadius = 120f;
                        nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = true;
                        if (!FarmingState.NeedToRun)
                        {
                            Logging.Write("Finished to farm garrison mine");
                            nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
                            tList.Pop();
                            break;
                        }
                        Logging.Write("Take coffee and Mining Pick buffs");
                        if (((ItemsManager.GetItemCount(0x1d077) > 0) && !ItemsManager.IsItemOnCooldown(0x1d077)) && (ItemsManager.IsItemUsable(0x1d077) && !nManager.Wow.ObjectManager.ObjectManager.Me.HaveBuff((uint) 0x2afbd)))
                        {
                            ItemsManager.UseItem(0x1d077);
                            Thread.Sleep((int) (150 + Usefuls.Latency));
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(150);
                            }
                            Thread.Sleep(0x3e8);
                        }
                        if (((ItemsManager.GetItemCount(0x1d071) > 0) && !ItemsManager.IsItemOnCooldown(0x1d071)) && (ItemsManager.IsItemUsable(0x1d071) && (nManager.Wow.ObjectManager.ObjectManager.Me.BuffStack((uint) 0x2afb1) < 2)))
                        {
                            ItemsManager.UseItem(0x1d071);
                            Thread.Sleep((int) (150 + Usefuls.Latency));
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(150);
                            }
                        }
                        break;

                    case Task.MineWorkOrder:
                    {
                        if (flag2)
                        {
                            Logging.Write("Task: collect mine cache and send work order");
                        }
                        if (!_cacheMineGathered)
                        {
                            WoWGameObject obj4 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(_cacheMine), false);
                            if (obj4.GetBaseAddress != 0)
                            {
                                if (obj4.Position.DistanceTo(position) > 5f)
                                {
                                    Npc npc4 = new Npc {
                                        Entry = obj4.Entry,
                                        Position = obj4.Position
                                    };
                                    _targetNpc = npc4;
                                    MovementManager.FindTarget(ref _targetNpc, 5f, true, false, 0f);
                                    return;
                                }
                                Thread.Sleep((int) (Usefuls.Latency + 250));
                                Interact.InteractWith(obj4.GetBaseAddress, true);
                                _cacheMineGathered = true;
                                Thread.Sleep((int) (Usefuls.Latency + 0x6d6));
                            }
                        }
                        WoWUnit unit2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(_npcMine, false), false, false, false);
                        if (unit2.GetBaseAddress != 0)
                        {
                            if (unit2.Position.DistanceTo(position) > 5f)
                            {
                                Npc npc5 = new Npc {
                                    Entry = unit2.Entry,
                                    Position = unit2.Position
                                };
                                _targetNpc = npc5;
                                MovementManager.FindTarget(ref _targetNpc, 5f, true, false, 0f);
                                return;
                            }
                            Interact.InteractWith(unit2.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                            Interact.InteractWith(unit2.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 500));
                            Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()", false, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                        }
                        tList.Pop();
                        break;
                    }
                    case Task.CheckGarrisonRessourceCache:
                    {
                        if (flag2)
                        {
                            Logging.Write("Task: gather garrison cache");
                        }
                        if (_cacheGarrisonPoint.DistanceTo(position) > 75f)
                        {
                            MovementManager.Go(PathFinder.FindPath(_cacheGarrisonPoint));
                            return;
                        }
                        WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(_cacheGarrison), false);
                        if (obj2.GetBaseAddress != 0)
                        {
                            if (obj2.Position.DistanceTo(position) > 5f)
                            {
                                Npc npc = new Npc {
                                    Entry = obj2.Entry,
                                    Position = obj2.Position
                                };
                                _targetNpc = npc;
                                MovementManager.FindTarget(ref _targetNpc, 5f, true, false, 0f);
                                return;
                            }
                            Interact.InteractWith(obj2.GetBaseAddress, true);
                        }
                        tList.Pop();
                        break;
                    }
                }
                if (tList.Count == 0)
                {
                    Logging.Write("Garrison Farming completed");
                    CloseProduct();
                }
            }
        }

        public override List<nManager.FiniteStateMachine.State> BeforeStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override string DisplayName
        {
            get
            {
                return "GarrisonState";
            }
        }

        public override bool NeedToRun
        {
            get
            {
                if ((((!Usefuls.InGame || Usefuls.IsLoadingOrConnecting) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && (!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (!nManagerSetting.CurrentSetting.IgnoreFightIfMounted && !Usefuls.IsFlying)))) || !nManager.Products.Products.IsStarted)
                {
                    return false;
                }
                if ((tList != null) && (tList.Count == 0))
                {
                    return false;
                }
                return true;
            }
        }

        public override List<nManager.FiniteStateMachine.State> NextStates
        {
            get
            {
                return new List<nManager.FiniteStateMachine.State>();
            }
        }

        public override int Priority { get; set; }

        private enum Task
        {
            None,
            GoToGarrison,
            GatherHerbs,
            GardenWorkOrder,
            GatherMinerals,
            MineWorkOrder,
            CheckGarrisonRessourceCache
        }
    }
}

