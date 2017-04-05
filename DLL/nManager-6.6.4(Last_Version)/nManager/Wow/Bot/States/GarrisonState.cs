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
        private static List<int> _adamehawa;
        private static bool _afuocIsimig = nManagerSetting.CurrentSetting.ActivateHerbsHarvesting;
        private const int _aloeluelin = 0x1d071;
        private const int _asiegunNoleubitu = 0x1d077;
        private static bool _awualufuAquit = nManagerSetting.CurrentSetting.ActivateVeinsHarvesting;
        private static Stack<Revudoagel> _bonaxagaJe = null;
        private static int _copoahiuhuCitup;
        private static Npc _daetap = new Npc();
        private const int _eruca = 0x1afe0;
        private static Revudoagel _felaetoa = Revudoagel._rovuaqeadao;
        private const uint _fuexoeji = 0x2afbd;
        private static float _heutaqailopu = nManagerSetting.CurrentSetting.GatheringSearchRadius;
        private static int _ikeuq;
        private static Point _ixeiv;
        private static int _javoinaAsabir;
        private static bool _lesuceafou;
        private static Point _noifadoqavAge;
        private static bool _owaoboeAqaru = false;
        private static Point _pourixeihok;
        private const uint _udiaqecaosu = 0x2afb1;
        private static int _uruisilotiud;
        private static bool _uwecoalisoxuEsaqeulia;
        private static readonly Farming FarmingState = new Farming();

        public GarrisonState()
        {
            nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
            nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
            _owaoboeAqaru = true;
        }

        private static void IqifujeapobepGagoej()
        {
            new Thread(new ThreadStart(nManager.Products.Products.ProductStopFromFSM)).Start();
        }

        public static void RestoreSettings()
        {
            if (_owaoboeAqaru)
            {
                nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = _awualufuAquit;
                nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = _afuocIsimig;
                nManagerSetting.CurrentSetting.GatheringSearchRadius = _heutaqailopu;
                _owaoboeAqaru = false;
            }
        }

        public override void Run()
        {
            if (!MovementManager.InMovement)
            {
                bool flag;
                if (_bonaxagaJe == null)
                {
                    _bonaxagaJe = new Stack<Revudoagel>();
                    _bonaxagaJe.Push(Revudoagel._afuobieIno);
                    if (Garrison.GetGarrisonLevel() > 1)
                    {
                        _bonaxagaJe.Push(Revudoagel._ajobausuic);
                        _bonaxagaJe.Push(Revudoagel._odipuiqiofe);
                        _bonaxagaJe.Push(Revudoagel._pegegieriovusu);
                        _bonaxagaJe.Push(Revudoagel._iweaduevitUleipiwiu);
                    }
                    _bonaxagaJe.Push(Revudoagel._caosiji);
                    _adamehawa = new List<int> { 0x3a09a, 0x39d74, 0x39e87, 0x3a09c, 0x3a098, 0x3a09b };
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction == "Alliance")
                    {
                        _uruisilotiud = 0x14e0a;
                        _ikeuq = 0x3996d;
                        _copoahiuhuCitup = 0x12fa2;
                        _javoinaAsabir = 0x3996e;
                        Point point = new Point {
                            X = 1833.85f,
                            Y = 154.7408f,
                            Z = 76.66339f
                        };
                        _pourixeihok = point;
                        Point point2 = new Point {
                            X = 1886.021f,
                            Y = 83.23455f,
                            Z = 84.31888f
                        };
                        _ixeiv = point2;
                        Point point3 = new Point {
                            X = 1914.361f,
                            Y = 290.3863f,
                            Z = 88.96407f
                        };
                        _noifadoqavAge = point3;
                    }
                    else
                    {
                        _uruisilotiud = 0x14f17;
                        _ikeuq = 0x3a686;
                        _copoahiuhuCitup = 0x13f18;
                        _javoinaAsabir = 0x3a685;
                        Point point4 = new Point {
                            X = 5413.795f,
                            Y = 4548.928f,
                            Z = 139.1232f
                        };
                        _pourixeihok = point4;
                        Point point5 = new Point {
                            X = 5465.796f,
                            Y = 4430.045f,
                            Z = 145.4595f
                        };
                        _ixeiv = point5;
                        Point point6 = new Point {
                            X = 5592.229f,
                            Y = 4569.476f,
                            Z = 136.1069f
                        };
                        _noifadoqavAge = point6;
                    }
                    _uwecoalisoxuEsaqeulia = false;
                    _lesuceafou = false;
                }
                bool flag2 = false;
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                Revudoagel revudoagel = _bonaxagaJe.Peek();
                if (revudoagel != _felaetoa)
                {
                    _felaetoa = revudoagel;
                    flag2 = true;
                }
                switch (revudoagel)
                {
                    case Revudoagel._caosiji:
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
                                    LongMove.LongMoveGo(_noifadoqavAge);
                                    return;
                                }
                                if (_noifadoqavAge.DistanceTo(position) < 100f)
                                {
                                    List<Point> points = PathFinder.FindPath(_noifadoqavAge, out flag, true, false);
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
                                _bonaxagaJe.Clear();
                                break;
                            }
                        }
                        _bonaxagaJe.Pop();
                        break;

                    case Revudoagel._iweaduevitUleipiwiu:
                        if (flag2)
                        {
                            Logging.Write("Task: gather plants in garrison garden");
                        }
                        if (_pourixeihok.DistanceTo(position) > 15f)
                        {
                            MovementManager.Go(PathFinder.FindPath(_pourixeihok, out flag, true, false));
                            return;
                        }
                        nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = true;
                        nManagerSetting.CurrentSetting.GatheringSearchRadius = 30f;
                        if (!FarmingState.NeedToRun)
                        {
                            Logging.Write("Finished to farm garrison garden");
                            nManagerSetting.CurrentSetting.ActivateHerbsHarvesting = false;
                            _bonaxagaJe.Pop();
                        }
                        break;

                    case Revudoagel._pegegieriovusu:
                    {
                        if (flag2)
                        {
                            Logging.Write("Task: collect garden cache and send work order");
                        }
                        if (!_uwecoalisoxuEsaqeulia)
                        {
                            WoWGameObject nearestWoWGameObject = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(_ikeuq), false);
                            if (nearestWoWGameObject.GetBaseAddress != 0)
                            {
                                if (nearestWoWGameObject.Position.DistanceTo(position) > 5f)
                                {
                                    Npc npc2 = new Npc {
                                        Entry = nearestWoWGameObject.Entry,
                                        Position = nearestWoWGameObject.Position
                                    };
                                    _daetap = npc2;
                                    MovementManager.FindTarget(ref _daetap, 5f, true, false, 0f, false);
                                    return;
                                }
                                Thread.Sleep((int) (Usefuls.Latency + 250));
                                Interact.InteractWith(nearestWoWGameObject.GetBaseAddress, true);
                                _uwecoalisoxuEsaqeulia = true;
                                Thread.Sleep((int) (Usefuls.Latency + 0x6d6));
                            }
                        }
                        WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(_uruisilotiud, false), false, false, false);
                        if (unit.GetBaseAddress != 0)
                        {
                            if (unit.Position.DistanceTo(position) > 5f)
                            {
                                Npc npc3 = new Npc {
                                    Entry = unit.Entry,
                                    Position = unit.Position
                                };
                                _daetap = npc3;
                                MovementManager.FindTarget(ref _daetap, 5f, true, false, 0f, false);
                                return;
                            }
                            Interact.InteractWith(unit.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                            Interact.InteractWith(unit.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 500));
                            Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()", false, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                        }
                        _bonaxagaJe.Pop();
                        break;
                    }
                    case Revudoagel._odipuiqiofe:
                        if (flag2)
                        {
                            Logging.Write("Task: gather ores and carts in garrison mine");
                        }
                        if (_ixeiv.DistanceTo(position) > 15f)
                        {
                            MovementManager.Go(PathFinder.FindPath(_ixeiv, out flag, true, false));
                            return;
                        }
                        nManagerSetting.CurrentSetting.GatheringSearchRadius = 120f;
                        nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = true;
                        if (!FarmingState.NeedToRun)
                        {
                            Logging.Write("Finished to farm garrison mine");
                            nManagerSetting.CurrentSetting.ActivateVeinsHarvesting = false;
                            _bonaxagaJe.Pop();
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

                    case Revudoagel._ajobausuic:
                    {
                        if (flag2)
                        {
                            Logging.Write("Task: collect mine cache and send work order");
                        }
                        if (!_lesuceafou)
                        {
                            WoWGameObject obj4 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(_javoinaAsabir), false);
                            if (obj4.GetBaseAddress != 0)
                            {
                                if (obj4.Position.DistanceTo(position) > 5f)
                                {
                                    Npc npc4 = new Npc {
                                        Entry = obj4.Entry,
                                        Position = obj4.Position
                                    };
                                    _daetap = npc4;
                                    MovementManager.FindTarget(ref _daetap, 5f, true, false, 0f, false);
                                    return;
                                }
                                Thread.Sleep((int) (Usefuls.Latency + 250));
                                Interact.InteractWith(obj4.GetBaseAddress, true);
                                _lesuceafou = true;
                                Thread.Sleep((int) (Usefuls.Latency + 0x6d6));
                            }
                        }
                        WoWUnit unit2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(_copoahiuhuCitup, false), false, false, false);
                        if (unit2.GetBaseAddress != 0)
                        {
                            if (unit2.Position.DistanceTo(position) > 5f)
                            {
                                Npc npc5 = new Npc {
                                    Entry = unit2.Entry,
                                    Position = unit2.Position
                                };
                                _daetap = npc5;
                                MovementManager.FindTarget(ref _daetap, 5f, true, false, 0f, false);
                                return;
                            }
                            Interact.InteractWith(unit2.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                            Interact.InteractWith(unit2.GetBaseAddress, true);
                            Thread.Sleep((int) (Usefuls.Latency + 500));
                            Lua.LuaDoString("GarrisonCapacitiveDisplayFrame.CreateAllWorkOrdersButton:Click()", false, true);
                            Thread.Sleep((int) (Usefuls.Latency + 0x3e8));
                        }
                        _bonaxagaJe.Pop();
                        break;
                    }
                    case Revudoagel._afuobieIno:
                    {
                        if (flag2)
                        {
                            Logging.Write("Task: gather garrison cache");
                        }
                        if (_noifadoqavAge.DistanceTo(position) > 75f)
                        {
                            MovementManager.Go(PathFinder.FindPath(_noifadoqavAge));
                            return;
                        }
                        WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectById(_adamehawa), false);
                        if (obj2.GetBaseAddress != 0)
                        {
                            if (obj2.Position.DistanceTo(position) > 5f)
                            {
                                Npc npc = new Npc {
                                    Entry = obj2.Entry,
                                    Position = obj2.Position
                                };
                                _daetap = npc;
                                MovementManager.FindTarget(ref _daetap, 5f, true, false, 0f, false);
                                return;
                            }
                            Interact.InteractWith(obj2.GetBaseAddress, true);
                        }
                        _bonaxagaJe.Pop();
                        break;
                    }
                }
                if (_bonaxagaJe.Count == 0)
                {
                    Logging.Write("Garrison Farming completed");
                    IqifujeapobepGagoej();
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
                if (((!Usefuls.InGame || Usefuls.IsLoading) || (nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe || !nManager.Wow.ObjectManager.ObjectManager.Me.IsValid)) || (nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat || !nManager.Products.Products.IsStarted))
                {
                    return false;
                }
                if ((_bonaxagaJe != null) && (_bonaxagaJe.Count == 0))
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

        private enum Revudoagel
        {
            _rovuaqeadao,
            _caosiji,
            _iweaduevitUleipiwiu,
            _pegegieriovusu,
            _odipuiqiofe,
            _ajobausuic,
            _afuobieIno
        }
    }
}

