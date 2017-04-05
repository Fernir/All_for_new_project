namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.States;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    public static class MovementManager
    {
        private static uint _cacheTargetAddress;
        private static nManager.Helpful.Timer _canRemount = new nManager.Helpful.Timer(0.0);
        private static bool _chasing = false;
        private static int _currentTargetedPoint;
        private static uint _currFightMeleeControl;
        private static Point _distmountAttempt = new Point();
        private static bool _farm;
        private static bool _first;
        private static Point _jumpOverAttempt = new Point();
        private static bool _lastMoveToResult = true;
        private static int _lastNbStuck;
        private static bool _loop;
        private static bool _loopMoveTo;
        private static nManager.Helpful.Timer _maxTimerForStuckDetection = new nManager.Helpful.Timer();
        private static bool _movement;
        private static List<Point> _points = new List<Point>();
        private static List<Point> _pointsOrigine = new List<Point>();
        private static Point _pointTo = new Point();
        private static Npc _trakedTarget = new Npc();
        private static UInt128 _trakedTargetGuid;
        private static nManager.Helpful.Timer _updatePathSpecialTimer = new nManager.Helpful.Timer();
        private static Thread _worker;
        private static Thread _workerMoveTo;
        private static readonly nManager.Helpful.Timer MeleeControlMovementTimer = new nManager.Helpful.Timer(3000.0);
        private static readonly nManager.Helpful.Timer MeleeControlTimer = new nManager.Helpful.Timer(5000.0);
        public static int StuckCount;

        private static void AquaticMovementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && (_points.Count > 0))
                {
                    MountTask.Mount(false);
                    if (_movement)
                    {
                        int num = firstIdPoint;
                        if (_points[num].DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 30f)
                        {
                            bool flag;
                            List<Point> list = PathFinder.FindPath(_points[num], out flag, true, false);
                            if (flag)
                            {
                                for (int i = 0; i <= (list.Count - 1); i++)
                                {
                                    list[i].Type = "Swimming";
                                }
                                _points = list;
                                _loop = false;
                                num = 0;
                            }
                        }
                        bool flag2 = false;
                        while ((_movement && !flag2) && (!Usefuls.IsLoadingOrConnecting && Usefuls.InGame))
                        {
                            if (!(_points[num].Type.ToLower() != "swimming"))
                            {
                                goto Label_00E1;
                            }
                            return;
                        Label_00C7:
                            MovementsAction.Ascend(true, false, false);
                            Thread.Sleep(200);
                            MovementsAction.Ascend(false, false, false);
                        Label_00E1:
                            if ((!Usefuls.IsSwimming && !Usefuls.IsFlying) && (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (MountTask.GetMountCapacity() == MountCapacity.Fly)))
                            {
                                goto Label_00C7;
                            }
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[num]) < 7f) && _movement)
                            {
                                num++;
                                if (num > (_points.Count - 1))
                                {
                                    num = _points.Count - 1;
                                    flag2 = true;
                                    if (_loop)
                                    {
                                        flag2 = false;
                                        _points = new List<Point>();
                                        _points.AddRange(_pointsOrigine);
                                        num = 0;
                                    }
                                }
                                _currentTargetedPoint = num;
                                MoveTo(_points[num], false);
                            }
                            MoveTo(_points[num], false);
                            Thread.Sleep(70);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MovementManager > AquaticMouvementManager(int firstIdPoint): " + exception, true);
                StopMove();
            }
        }

        private static void AvoidMelee(WoWUnit unit)
        {
            MeleeControlMovementTimer.Reset();
            MovementsAction.MoveBackward(true, false);
            while ((!CombatClass.AboveMinRange(unit) && CombatClass.InRange(unit)) && (nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe && !MeleeControlMovementTimer.IsReady))
            {
                Thread.Sleep(50);
            }
            MovementsAction.MoveBackward(false, false);
        }

        public static void Face(Point position, bool doMove = true)
        {
            try
            {
                Point point = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                float num = NegativeAngle((float) System.Math.Atan2((double) (position.Y - point.Y), (double) (position.X - point.X)));
                float num2 = num - nManager.Wow.ObjectManager.ObjectManager.Me.Rotation;
                if (num2 < 0f)
                {
                    num2 = -num2;
                }
                if (num2 > 0.78539816339744828)
                {
                    nManager.Wow.ObjectManager.ObjectManager.Me.Rotation = num;
                    if (doMove)
                    {
                        MicroMove();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(Point position): " + exception, true);
            }
        }

        public static void Face(WoWGameObject obj, bool doMove = true)
        {
            try
            {
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                float num = NegativeAngle((float) System.Math.Atan2((double) (obj.Position.Y - position.Y), (double) (obj.Position.X - position.X)));
                float num2 = num - nManager.Wow.ObjectManager.ObjectManager.Me.Rotation;
                if (num2 < 0f)
                {
                    num2 = -num2;
                }
                if ((num2 > 0.78539816339744828) || (obj.GOType == WoWGameObjectType.FishingHole))
                {
                    nManager.Wow.ObjectManager.ObjectManager.Me.Rotation = num;
                    if (doMove)
                    {
                        MicroMove();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(WoWGameObject obj): " + exception, true);
            }
        }

        public static void Face(WoWPlayer obj, bool doMove = true)
        {
            try
            {
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                float num = NegativeAngle((float) System.Math.Atan2((double) (obj.Position.Y - position.Y), (double) (obj.Position.X - position.X)));
                float num2 = num - nManager.Wow.ObjectManager.ObjectManager.Me.Rotation;
                if (num2 < 0f)
                {
                    num2 = -num2;
                }
                if (num2 > 0.78539816339744828)
                {
                    nManager.Wow.ObjectManager.ObjectManager.Me.Rotation = num;
                    if (doMove)
                    {
                        MicroMove();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(WoWPlayer obj): " + exception, true);
            }
        }

        public static void Face(WoWUnit obj, bool doMove = true)
        {
            try
            {
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                float num = NegativeAngle((float) System.Math.Atan2((double) (obj.Position.Y - position.Y), (double) (obj.Position.X - position.X)));
                float num2 = num - nManager.Wow.ObjectManager.ObjectManager.Me.Rotation;
                if (num2 < 0f)
                {
                    num2 = -num2;
                }
                if (num2 > 0.78539816339744828)
                {
                    nManager.Wow.ObjectManager.ObjectManager.Me.Rotation = num;
                    if (doMove)
                    {
                        MicroMove();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(WoWUnit obj): " + exception, true);
            }
        }

        public static void Face(float angle, bool doMove = true)
        {
            try
            {
                nManager.Wow.ObjectManager.ObjectManager.Me.Rotation = angle;
                if (doMove)
                {
                    MicroMove();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Face(float angle): " + exception, true);
            }
        }

        public static uint FindTarget(WoWPlayer Player, float SpecialRange = 0f, bool doMount = true)
        {
            if (_trakedTargetGuid != Player.Guid)
            {
                Npc npc = new Npc {
                    Entry = Player.Entry,
                    Position = Player.Position,
                    Name = Player.Name,
                    Guid = Player.Guid
                };
                _trakedTarget = npc;
                _trakedTargetGuid = Player.Guid;
                resetTimers();
            }
            return FindTarget(ref _trakedTarget, SpecialRange, doMount, false, 0f);
        }

        public static uint FindTarget(WoWUnit Unit, float SpecialRange = 0f, bool doMount = true)
        {
            if (_trakedTargetGuid != Unit.Guid)
            {
                StopMove();
                Npc npc = new Npc {
                    Entry = Unit.Entry,
                    Position = Unit.Position,
                    Name = Unit.Name
                };
                _trakedTarget = npc;
                _trakedTargetGuid = Unit.Guid;
                resetTimers();
            }
            return FindTarget(ref _trakedTarget, SpecialRange, doMount, false, 0f);
        }

        public static uint FindTarget(WoWGameObject Object, float SpecialRange = 0f, bool doMount = true, float maxDist = 0f)
        {
            if (_trakedTargetGuid != Object.Guid)
            {
                StopMove();
                Npc npc = new Npc {
                    Entry = Object.Entry,
                    Position = Object.Position,
                    Name = Object.Name
                };
                _trakedTarget = npc;
                _trakedTargetGuid = Object.Guid;
                resetTimers();
            }
            return FindTarget(ref _trakedTarget, SpecialRange, doMount, false, maxDist);
        }

        public static uint FindTarget(ref Npc Target, float SpecialRange = 0f, bool doMount = true, bool isDead = false, float maxDist = 0f)
        {
            bool flag;
            bool flag2;
            uint num;
            if ((doMount && !InMovement) && ((Target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 5f) && (Target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount)))
            {
                MountTask.Mount(true);
            }
            if (!InMovement && (Target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > ((SpecialRange > 0f) ? ((double) SpecialRange) : ((new Random().NextDouble() * 2.0) + 2.5))))
            {
                num = UpdateTarget(ref Target, out flag2, isDead);
                if ((num == 0) && (MountTask.GetMountCapacity() == MountCapacity.Fly))
                {
                    Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(Target.Position));
                    LongMove.LongMoveByNewThread(Target.Position);
                    return 0;
                }
                List<Point> listPoints = PathFinder.FindPath(Target.Position, out flag, true, false);
                if (!flag)
                {
                    return 0;
                }
                if ((maxDist > 0f) && (nManager.Helpful.Math.DistanceListPoint(listPoints) > maxDist))
                {
                    return 0;
                }
                if (Target.Guid == 0)
                {
                    Logging.Write(string.Concat(new object[] { "Looking for ", Target.Name, " (", Target.Entry, ")." }));
                }
                float num2 = nManager.Helpful.Math.DistanceListPoint(listPoints);
                _cacheTargetAddress = num;
                _updatePathSpecialTimer = new nManager.Helpful.Timer(2000.0);
                _maxTimerForStuckDetection = new nManager.Helpful.Timer((double) (((int) ((num2 / 3f) * 1000f)) + 0xfa0));
                Go(listPoints);
                return num;
            }
            if ((InMovement && Usefuls.InGame) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe))
            {
                if (Target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= ((SpecialRange > 0f) ? ((double) SpecialRange) : ((new Random().NextDouble() * 2.0) + 2.5)))
                {
                    StopMove();
                    return UpdateTarget(ref Target, out flag2, isDead);
                }
                num = UpdateTarget(ref Target, out flag2, isDead);
                if (LongMove.IsLongMove)
                {
                    if (num == 0)
                    {
                        return 0;
                    }
                    LongMove.StopLongMove();
                }
                else if ((num == 0) && (MountTask.GetMountCapacity() == MountCapacity.Fly))
                {
                    StopMove();
                    Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(Target.Position));
                    LongMove.LongMoveByNewThread(Target.Position);
                    return 0;
                }
                if ((num != 0) && (num != _cacheTargetAddress))
                {
                    _cacheTargetAddress = num;
                    flag2 = true;
                }
                if (flag2 && _updatePathSpecialTimer.IsReady)
                {
                    _updatePathSpecialTimer = new nManager.Helpful.Timer(2000.0);
                    List<Point> list2 = PathFinder.FindPath(Target.Position, out flag, true, false);
                    if (!flag)
                    {
                        list2.Add(Target.Position);
                    }
                    _maxTimerForStuckDetection = new nManager.Helpful.Timer((double) (((int) ((nManager.Helpful.Math.DistanceListPoint(list2) / 3f) * 1000f)) + 0xfa0));
                    Go(list2);
                    return num;
                }
                if ((_maxTimerForStuckDetection != null) && _maxTimerForStuckDetection.IsReady)
                {
                    WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Target.Entry), Target.Position, false);
                    WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(Target.Entry, false), Target.Position, false);
                    if (unit.IsValid)
                    {
                        nManagerSetting.AddBlackList(unit.Guid, -1);
                    }
                    else if (obj2.IsValid)
                    {
                        nManagerSetting.AddBlackList(obj2.Guid, -1);
                    }
                    StopMove();
                }
            }
            return UpdateTarget(ref Target, out flag2, isDead);
        }

        public static void FlyingToGroundProfilesConverter(List<Point> inputPoints, out List<Point> outputPoints, out bool conversionStatus)
        {
            try
            {
                Logging.Write("Starting the conversion process. Keep in mind that the best profiles are handmade profiles.");
                List<Point> source = new List<Point>();
                outputPoints = new List<Point>();
                uint num = 0;
                foreach (Point point in inputPoints)
                {
                    float z = (point.Type.ToLower() == "flying") ? PathFinder.GetZPosition(point, false) : point.Z;
                    if (z == 0f)
                    {
                        num++;
                        continue;
                    }
                    if (source.Any<Point>())
                    {
                        bool flag;
                        source = PathFinder.FindPath(source[source.Count<Point>() - 1], new Point(point.X, point.Y, z, "None"), Usefuls.ContinentNameMpq, out flag, true, false, false);
                        if (flag)
                        {
                            goto Label_00C7;
                        }
                        num++;
                        continue;
                    }
                    source.Add(new Point(point.X, point.Y, z, "None"));
                Label_00C7:
                    outputPoints.AddRange(source);
                }
                decimal d = (((ulong) num) / ((long) inputPoints.Count)) * ((ulong) 100L);
                if (d > 0M)
                {
                    d = System.Math.Round(d, 0);
                    if (d == 0M)
                    {
                        d = decimal.op_Increment(d);
                    }
                }
                else
                {
                    d = 0M;
                }
                if (d > 60M)
                {
                    outputPoints = inputPoints;
                    conversionStatus = false;
                    Logging.WriteDebug("The conversion has failed, " + d + "% of the points have not been converted.");
                }
                else
                {
                    conversionStatus = true;
                    Logging.Write("The conversion has succeeded.");
                    Logging.WriteDebug("Conversion stats:");
                    Logging.WriteDebug(string.Concat(new object[] { 100M - d, "% of the points have been succesfully converted into ", inputPoints.Count, " grounds points." }));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FlyingToGroundProfilesConverter(List<Point> inputPoints, out List<Point> outputPoints, out bool conversionStatus): " + exception, true);
                Logging.Write("The conversion has failed...");
                outputPoints = inputPoints;
                conversionStatus = false;
            }
        }

        private static void FlyMovementManager(int firstIdPoint)
        {
            try
            {
                if (_movement && (_points.Count > 0))
                {
                    if (MountTask.GetMountCapacity() < MountCapacity.Fly)
                    {
                        Logging.Write("This profile needs you to be able to fly.");
                    }
                    if (!Usefuls.IsFlying)
                    {
                        MountTask.MountingFlyingMount(false);
                    }
                    if (_movement)
                    {
                        int num = firstIdPoint;
                        if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[num]) > 200f) || (_points.Count == 1))
                        {
                            Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[num]));
                            LongMove.LongMoveByNewThread(_points[num]);
                            Thread.Sleep(100);
                            while (LongMove.IsLongMove && _movement)
                            {
                                Thread.Sleep(50);
                            }
                            LongMove.StopLongMove();
                        }
                        bool flag = false;
                        while ((_movement && !flag) && (!Usefuls.IsLoadingOrConnecting && Usefuls.InGame))
                        {
                            if ((_points[num].Type.ToLower() == "flying") && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                            {
                                goto Label_012E;
                            }
                            return;
                        Label_0114:
                            MovementsAction.Ascend(true, false, false);
                            Thread.Sleep(200);
                            MovementsAction.Ascend(false, false, false);
                        Label_012E:
                            if ((MountTask.OnFlyMount() && !Usefuls.IsFlying) && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                            {
                                goto Label_0114;
                            }
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[num]) < 15f) && _movement)
                            {
                                num++;
                                if (num > (_points.Count - 1))
                                {
                                    num = _points.Count - 1;
                                    flag = true;
                                    if (_loop)
                                    {
                                        flag = false;
                                        _points = new List<Point>();
                                        _points.AddRange(_pointsOrigine);
                                        num = 0;
                                    }
                                }
                                if (_loop)
                                {
                                    _currentTargetedPoint = num;
                                }
                                MoveTo(_points[num], false);
                            }
                            MoveTo(_points[num], false);
                            Thread.Sleep(70);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MovementManager > FlyMouvementManager(int firstIdPoint): " + exception, true);
                StopMove();
            }
        }

        private static void GetToMelee(WoWUnit unit)
        {
            MeleeControlMovementTimer.Reset();
            MovementsAction.MoveForward(true, false);
            while ((!CombatClass.InRange(unit) && CombatClass.AboveMinRange(unit)) && (nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe && !MeleeControlMovementTimer.IsReady))
            {
                Thread.Sleep(50);
            }
            MovementsAction.MoveForward(false, false);
        }

        public static void Go(List<Point> points)
        {
            try
            {
                if (nManager.Helpful.Math.DistanceListPoint(_points) != nManager.Helpful.Math.DistanceListPoint(points))
                {
                    _loop = false;
                    _movement = false;
                    lock (typeof(MovementManager))
                    {
                        if (_worker == null)
                        {
                            _currentTargetedPoint = 0;
                            LaunchThreadMovementManager();
                        }
                        _pointsOrigine = new List<Point>();
                        _pointsOrigine.AddRange(points);
                        _points = new List<Point>();
                        _points.AddRange(points);
                        _movement = true;
                        _first = true;
                        return;
                    }
                }
                if ((_loop || !_movement) || !_first)
                {
                    _first = true;
                    _loop = false;
                    _movement = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Go(List<Point> points): " + exception, true);
            }
        }

        public static void GoLoop(List<Point> points)
        {
            try
            {
                if (nManager.Helpful.Math.DistanceListPoint(_points) != nManager.Helpful.Math.DistanceListPoint(points))
                {
                    _loop = false;
                    _movement = false;
                    lock (typeof(MovementManager))
                    {
                        if (_worker == null)
                        {
                            LaunchThreadMovementManager();
                        }
                        _pointsOrigine = new List<Point>();
                        _pointsOrigine.AddRange(points);
                        _points = new List<Point>();
                        _points.AddRange(points);
                        _currentTargetedPoint = nManager.Helpful.Math.NearestPointOfListPoints(_points, nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                        _loop = true;
                        _movement = true;
                        return;
                    }
                }
                if (!_loop || !_movement)
                {
                    _loop = true;
                    _movement = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GoLoop(List<Point> points): " + exception, true);
            }
        }

        private static void GroundMovementManager(int firstIdPoint)
        {
            try
            {
                if ((_movement && (_points.Count > 0)) && (_points[firstIdPoint].Type.ToLower() != "swimming"))
                {
                    if ((_loop && (_points[firstIdPoint].DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) >= 200f)) && !string.IsNullOrWhiteSpace(nManagerSetting.CurrentSetting.FlyingMountName))
                    {
                        Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_points[firstIdPoint]));
                        LongMove.LongMoveByNewThread(_points[firstIdPoint]);
                        Thread.Sleep(100);
                        while (LongMove.IsLongMove && _movement)
                        {
                            Thread.Sleep(50);
                        }
                        LongMove.StopLongMove();
                        if (!_movement)
                        {
                            return;
                        }
                    }
                    if (_loop || (nManager.Helpful.Math.DistanceListPoint(_points) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount))
                    {
                        if (nManagerSetting.CurrentSetting.UseGroundMount && (MountTask.GetMountCapacity() >= MountCapacity.Ground))
                        {
                            MountTask.MountingGroundMount(false);
                        }
                        else
                        {
                            MountTask.Mount(false);
                        }
                        if (Usefuls.IsFlying)
                        {
                            List<Point> list = new List<Point>();
                            for (int i = 0; i < _points.Count; i++)
                            {
                                Point item = new Point(_points[i].X, _points[i].Y, _points[i].Z + 2f, "flying");
                                list.Add(item);
                            }
                            _points = list;
                        }
                    }
                    _lastNbStuck = StuckCount;
                    int num2 = firstIdPoint;
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(10);
                    }
                    MoveTo(_points[num2], false);
                    bool flag = false;
                    while ((_movement && !flag) && (!Usefuls.IsLoadingOrConnecting && Usefuls.InGame))
                    {
                        try
                        {
                            if (_points[num2].Type.ToLower() == "swimming")
                            {
                                return;
                            }
                            if ((((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[num2]) <= 3f) && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted) || (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_points[num2]) <= 3f)) && ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceZ(_points[num2]) <= 10.5f) && _movement))
                            {
                                num2++;
                                if (num2 > (_points.Count - 1))
                                {
                                    num2 = _points.Count - 1;
                                    flag = true;
                                    if (_loop)
                                    {
                                        flag = false;
                                        _points = new List<Point>();
                                        _points.AddRange(_pointsOrigine);
                                        num2 = 0;
                                    }
                                }
                            }
                            if (!_lastMoveToResult || (_lastNbStuck != StuckCount))
                            {
                                try
                                {
                                    _lastNbStuck = StuckCount;
                                    StopMoveTo(true, false);
                                    _points = PathFinder.FindPath(_pointsOrigine[_pointsOrigine.Count - 1]);
                                    num2 = 0;
                                }
                                catch (Exception exception)
                                {
                                    Logging.WriteError("ThreadMovementManager()#1: " + exception, true);
                                }
                                _lastMoveToResult = true;
                            }
                            if (_loop)
                            {
                                _currentTargetedPoint = num2;
                            }
                            MoveTo(_points[num2], false);
                            Thread.Sleep(50);
                            if (Others.Random(1, 0x1388) == 5)
                            {
                                MovementsAction.Jump();
                            }
                            continue;
                        }
                        catch (Exception exception2)
                        {
                            Logging.WriteError("ThreadMovementManager()#2: " + exception2, true);
                            num2 = nManager.Helpful.Math.NearestPointOfListPoints(_points, nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                            continue;
                        }
                    }
                    if (!_chasing)
                    {
                        if (!_loop)
                        {
                            StopMove();
                        }
                        if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                        {
                            StopMoveTo(true, false);
                        }
                    }
                }
            }
            catch (Exception exception3)
            {
                Logging.WriteError("MovementManager > GroundMouvementManager(int firstIdPoint): " + exception3, true);
                StopMove();
            }
        }

        public static void LaunchThreadMovementManager()
        {
            try
            {
                lock (typeof(MovementManager))
                {
                    if (_worker == null)
                    {
                        Thread thread = new Thread(new ThreadStart(MovementManager.ThreadMovementManager)) {
                            IsBackground = true,
                            Name = "MovementManager"
                        };
                        _worker = thread;
                        _worker.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LaunchThreadMovementManager(): " + exception, true);
            }
        }

        private static void LaunchThreadMoveTo()
        {
            try
            {
                if (_workerMoveTo != null)
                {
                    try
                    {
                        _workerMoveTo.Interrupt();
                        _workerMoveTo.Abort();
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("LaunchThreadMoveTo()#1: " + exception, true);
                        _workerMoveTo = null;
                    }
                }
                Thread thread = new Thread(new ThreadStart(MovementManager.ThreadMoveTo)) {
                    IsBackground = true,
                    Name = "MoveTo"
                };
                _workerMoveTo = thread;
                _workerMoveTo.Start();
            }
            catch (Exception exception2)
            {
                Logging.WriteError("LaunchThreadMoveTo()#2: " + exception2, true);
                _loopMoveTo = false;
            }
        }

        private static void MeleeControl(WoWUnit unit, bool resetCount)
        {
            if ((_currFightMeleeControl <= 5) && ((_currFightMeleeControl == 0) || MeleeControlTimer.IsReady))
            {
                MeleeControlTimer.Reset();
                if ((_currFightMeleeControl > 0) && resetCount)
                {
                    _currFightMeleeControl = 0;
                }
                if (!CombatClass.AboveMinRange(unit) && nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe)
                {
                    Logging.WriteFight("Under the minimal distance from the target, move closer.");
                    _currFightMeleeControl++;
                    GetToMelee(unit);
                }
                if (!CombatClass.InRange(unit) && nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe)
                {
                    Logging.WriteFight("Over the maximal distance from the target, move closer.");
                    _currFightMeleeControl++;
                    AvoidMelee(unit);
                }
            }
        }

        private static void MicroMove()
        {
            if (!InMovement && !InMoveTo)
            {
                if (Others.Random(0, 2) == 0)
                {
                    MovementsAction.MoveForward(true, false);
                    Thread.Sleep(50);
                    MovementsAction.MoveForward(false, false);
                }
                else
                {
                    MovementsAction.MoveBackward(true, false);
                    Thread.Sleep(50);
                    MovementsAction.MoveBackward(false, false);
                }
            }
        }

        public static void MoveTo(WoWUnit obj)
        {
            try
            {
                MoveTo(obj.Position, false);
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(WoWUnit obj): " + exception, true);
            }
        }

        public static void MoveTo(Point point, bool farm = false)
        {
            try
            {
                if (((point.X != 0f) && (point.Y != 0f)) && (point.Z != 0f))
                {
                    if (_workerMoveTo == null)
                    {
                        LaunchThreadMoveTo();
                    }
                    _farm = farm;
                    _pointTo = new Point(point.X, point.Y, point.Z, "None");
                    _loopMoveTo = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(Point point): " + exception, true);
            }
        }

        public static void MoveTo(WoWGameObject obj, bool farm = false)
        {
            try
            {
                MoveTo(obj.Position, farm);
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(WoWGameObject obj): " + exception, true);
            }
        }

        public static void MoveTo(float x, float y, float z, bool farm = false)
        {
            try
            {
                MoveTo(new Point(x, y, z, "None"), farm);
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveTo(float x, float y, float z): " + exception, true);
            }
        }

        private static bool MoveToLocation(Point position)
        {
            try
            {
                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(100);
                }
                if ((((!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && nManager.Wow.ObjectManager.ObjectManager.Me.IsAlive) && (_canRemount.IsReady && !Fight.InFight)) && ((!Looting.IsLooting && !nManager.Wow.ObjectManager.ObjectManager.Me.InCombat) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InTransport && (nManager.Products.Products.ProductName.ToLower() != "fisherbot")))) && ((MountTask.GetMountCapacity() > MountCapacity.Feet) && (position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.MinimumDistanceToUseMount)))
                {
                    if ((!Usefuls.IsSwimming && nManagerSetting.CurrentSetting.UseGroundMount) && (MountTask.GetMountCapacity() >= MountCapacity.Ground))
                    {
                        MountTask.MountingGroundMount(false);
                    }
                    else
                    {
                        MountTask.Mount(false);
                    }
                }
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(1000.0);
                nManager.Helpful.Timer timer2 = new nManager.Helpful.Timer(10000.0);
                double num = position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) - 1.0;
                if (num > 45.0)
                {
                    timer2 = new nManager.Helpful.Timer(1000.0 * (num / 3.0));
                }
                else if (num > 40.0)
                {
                    timer2 = new nManager.Helpful.Timer(15000.0);
                }
                else if (num > 35.0)
                {
                    timer2 = new nManager.Helpful.Timer(13000.0);
                }
                else if (num > 30.0)
                {
                    timer2 = new nManager.Helpful.Timer(11000.0);
                }
                Point other = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                bool flag = false;
                while (!timer2.IsReady && Usefuls.InGame)
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(position) <= 0.6)
                    {
                        return true;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(position) <= 3.0) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceZ(position) < 6f)) && _farm)
                    {
                        return true;
                    }
                    Point point2 = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(100);
                    }
                    Point point3 = new Point();
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InTransport)
                    {
                        WoWObject obj2 = new WoWObject(nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(nManager.Wow.ObjectManager.ObjectManager.Me.TransportGuid).GetBaseAddress);
                        if (obj2.Type == WoWObjectType.GameObject)
                        {
                            WoWGameObject o = new WoWGameObject(obj2.GetBaseAddress);
                            if (o.IsValid)
                            {
                                point3 = new Point(position.TransformInvert(o));
                            }
                        }
                    }
                    if (point3.IsValid)
                    {
                        if ((ClickToMove.GetClickToMovePosition().DistanceTo(point3) > 1f) || (ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.Move))
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(point3.X, point3.Y, point3.Z, 0, 4, 0.5f);
                        }
                    }
                    else if ((ClickToMove.GetClickToMovePosition().DistanceTo(position) > 1f) || (ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.Move))
                    {
                        ClickToMove.CGPlayer_C__ClickToMove(position.X, position.Y, position.Z, 0, 4, 0.5f);
                    }
                    if (!_loopMoveTo || (_pointTo.DistanceTo(position) > 0.5f))
                    {
                        break;
                    }
                    if (point2.DistanceTo(other) > 2f)
                    {
                        other = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                        timer.Reset();
                    }
                    if (!_loopMoveTo || (_pointTo.DistanceTo(position) > 0.5f))
                    {
                        break;
                    }
                    if ((point2.DistanceTo(other) < 2f) && timer.IsReady)
                    {
                        if (flag)
                        {
                            UnStuck();
                            break;
                        }
                        Logging.WriteNavigator("Think we are stuck");
                        Logging.WriteNavigator("Trying something funny, hang on");
                        flag = true;
                        UnStuck();
                        timer.Reset();
                    }
                    if (!_loopMoveTo || (_pointTo.DistanceTo(position) > 0.5f))
                    {
                        break;
                    }
                    Thread.Sleep(0x23);
                }
                if (_loopMoveTo && (_pointTo.DistanceTo(position) < 0.5f))
                {
                    Logging.WriteNavigator("Waypoint timed out");
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("MoveToLocation(Point position): " + exception, true);
                return false;
            }
        }

        private static float NegativeAngle(float angle)
        {
            try
            {
                if (angle < 0f)
                {
                    angle += 6.283185f;
                }
                return angle;
            }
            catch (Exception exception)
            {
                Logging.WriteError("NegativeAngle(float angle): " + exception, true);
            }
            return 0f;
        }

        private static void resetTimers()
        {
            _maxTimerForStuckDetection = null;
            _updatePathSpecialTimer = new nManager.Helpful.Timer(2000.0);
        }

        public static void StopMove()
        {
            try
            {
                _loop = false;
                _movement = false;
                lock (typeof(MovementManager))
                {
                    StopMoveTo(true, false);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopMove(): " + exception, true);
            }
        }

        public static void StopMoveTo(bool stabilizeZAxis = true, bool stabilizeSides = false)
        {
            try
            {
                _loopMoveTo = false;
                if (nManager.Wow.ObjectManager.ObjectManager.Me.GetMove)
                {
                    if (stabilizeZAxis)
                    {
                        MovementsAction.Ascend(false, false, false);
                        MovementsAction.Descend(false, false, false);
                    }
                    MovementsAction.MoveForward(true, true);
                    MovementsAction.MoveBackward(true, true);
                    if (stabilizeSides)
                    {
                        MovementsAction.StrafeLeft(true, true);
                        MovementsAction.StrafeRight(true, true);
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("MovementManager > StopMoveTo(): " + exception, true);
            }
        }

        private static void ThreadMovementManager()
        {
            try
            {
                while (true)
                {
                    lock (typeof(MovementManager))
                    {
                        while ((_loop || _first) && (!Usefuls.IsLoadingOrConnecting && Usefuls.InGame))
                        {
                            if (Statistics.OffsetStats == 0xb5)
                            {
                                _first = false;
                                if (_movement && (_points.Count > 0))
                                {
                                    int firstIdPoint = 0;
                                    if (_loop)
                                    {
                                        firstIdPoint = _currentTargetedPoint;
                                    }
                                    if (_points[firstIdPoint].DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 0.5f)
                                    {
                                        firstIdPoint++;
                                    }
                                    if (firstIdPoint >= (_points.Count - 1))
                                    {
                                        firstIdPoint = 0;
                                    }
                                    if (_loop)
                                    {
                                        _currentTargetedPoint = firstIdPoint;
                                    }
                                    if (_points[firstIdPoint].Type.ToLower() == "flying")
                                    {
                                        FlyMovementManager(firstIdPoint);
                                    }
                                    else if (_points[firstIdPoint].Type.ToLower() == "swimming")
                                    {
                                        AquaticMovementManager(firstIdPoint);
                                    }
                                    else
                                    {
                                        GroundMovementManager(firstIdPoint);
                                    }
                                    Statistics.OffsetStats = 0x53;
                                }
                            }
                            Thread.Sleep(80);
                        }
                        if (!_loop && !_first)
                        {
                            _movement = false;
                        }
                    }
                    Thread.Sleep(150);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("ThreadMovementManager()#3: " + exception, true);
            }
        }

        private static void ThreadMoveTo()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (_loopMoveTo)
                        {
                            _lastMoveToResult = MoveToLocation(_pointTo);
                            _loopMoveTo = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ThreadMoveTo()#1: " + exception, true);
                        _loopMoveTo = false;
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception exception2)
            {
                Logging.WriteError("ThreadMoveTo()#2: " + exception2, true);
            }
        }

        public static void UnStuck()
        {
            try
            {
                IsUnStuck = true;
                Logging.WriteDebug("UnStuck() started.");
                MovementsAction.Ascend(false, false, false);
                MovementsAction.Descend(false, false, false);
                Logging.WriteDebug("Jump / Down released.");
                if ((_jumpOverAttempt.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 3f) && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                {
                    Logging.WriteDebug("UnStuck - We are currently mounted.");
                    MovementsAction.Ascend(true, false, false);
                    Thread.Sleep(Others.Random(500, 0x3e8));
                    MovementsAction.Ascend(false, false, false);
                    Logging.WriteDebug("UnStuck - Jump attempt done.");
                    _jumpOverAttempt = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    if (Usefuls.IsFlying)
                    {
                        Logging.WriteDebug("UnStuck - We are currently Flying.");
                        UnStuckFly();
                        StuckCount++;
                        Logging.WriteDebug("UnStuck - StuckCount updated, new value: " + StuckCount + ".");
                    }
                    IsUnStuck = false;
                }
                else
                {
                    Statistics.Stucks++;
                    Logging.WriteNavigator("UnStuck - Non-flying UnStuck in progress.");
                    if (_distmountAttempt.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 3f)
                    {
                        float num = 1f * ((float) System.Math.Cos((double) nManager.Wow.ObjectManager.ObjectManager.Me.Rotation));
                        float num2 = 1f * ((float) System.Math.Sin((double) nManager.Wow.ObjectManager.ObjectManager.Me.Rotation));
                        Point to = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + num, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + num2, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 1f, "None");
                        _distmountAttempt = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 1f, "None");
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && !TraceLine.TraceLineGo(_distmountAttempt, to, CGWorldFrameHitFlags.HitTestAll))
                        {
                            Logging.WriteNavigator("UnStuck - Dismounting.");
                            Usefuls.DisMount();
                            _canRemount = new nManager.Helpful.Timer(8000.0);
                            _canRemount.Reset();
                            IsUnStuck = false;
                            StuckCount++;
                            return;
                        }
                        _jumpOverAttempt = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 1.5f, "None");
                        to = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + num, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + num2, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 1.5f, "None");
                        if (!TraceLine.TraceLineGo(_jumpOverAttempt, to, CGWorldFrameHitFlags.HitTestAll))
                        {
                            Logging.WriteNavigator("UnStuck - Jumping over.");
                            MovementsAction.Ascend(true, false, false);
                            MovementsAction.MoveForward(true, false);
                            Thread.Sleep(0x4b);
                            MovementsAction.Ascend(false, false, false);
                            MovementsAction.MoveForward(false, false);
                            Thread.Sleep(350);
                            MovementsAction.Ascend(true, false, false);
                            MovementsAction.MoveForward(true, false);
                            Thread.Sleep(0x4b);
                            MovementsAction.Ascend(false, false, false);
                            MovementsAction.MoveForward(false, false);
                            IsUnStuck = false;
                            StuckCount++;
                            return;
                        }
                    }
                    StopMove();
                    Point point2 = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                    Logging.WriteDebug("UnStuck - lastPost = " + point2);
                    for (int i = 0; i < 8; i++)
                    {
                        Logging.WriteDebug("UnStuck - UnStuck attempt " + i + " started.");
                        int num4 = Others.Random(1, 8);
                        float num5 = System.Math.Abs((float) (point2.X - nManager.Wow.ObjectManager.ObjectManager.Me.Position.X));
                        float num6 = System.Math.Abs((float) (point2.Y - nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y));
                        Logging.WriteDebug("UnStuck - disX = " + num5);
                        Logging.WriteDebug("UnStuck - disY = " + num6);
                        if ((num5 < 2.5f) || (num6 < 2.5f))
                        {
                            switch (num4)
                            {
                                case 1:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Backward done.");
                                    break;

                                case 2:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Backward Left done.");
                                    break;

                                case 3:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Left done.");
                                    break;

                                case 4:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Forward Left done.");
                                    break;

                                case 5:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Forward done.");
                                    break;

                                case 6:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Forward Right done.");
                                    break;

                                case 7:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Right done.");
                                    break;

                                case 8:
                                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                                    Logging.WriteDebug("UnStuck - Backward Right done.");
                                    break;
                            }
                            if (Others.Random(1, 3) == 2)
                            {
                                MovementsAction.Ascend(true, false, false);
                                Thread.Sleep(Others.Random(500, 0x3e8));
                                MovementsAction.Ascend(false, false, false);
                                Logging.WriteDebug("UnStuck - k = 2, Jump attempt done.");
                            }
                            Thread.Sleep(Others.Random(700, 0xbb8));
                            Logging.WriteDebug("UnStuck - UnStuck attempt " + i + " finished.");
                        }
                        else
                        {
                            Logging.WriteDebug("UnStuck - UnStuck attempt " + i + " finished by breaking.");
                            break;
                        }
                    }
                    IsUnStuck = false;
                    StuckCount++;
                    Logging.WriteDebug("UnStuck - StuckCount updated, new value: " + StuckCount + ".");
                    Logging.WriteDebug("UnStuck() done.");
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("UnStuck(): " + exception, true);
            }
        }

        public static void UnStuckFly()
        {
            try
            {
                Logging.WriteDebug("UnStuckFly() started.");
                Statistics.Stucks++;
                Logging.WriteDebug("Flying UnStuck - Statistics.Stucks updated, new value: " + Statistics.Stucks + ".");
                Logging.WriteNavigator("UnStuck character > flying mode.");
                if (ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.None)
                {
                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                    Logging.WriteDebug("Flying UnStuck - Reset position to our current location.");
                }
                MovementsAction.Ascend(true, false, false);
                Thread.Sleep(Others.Random(200, 500));
                MovementsAction.Ascend(false, false, false);
                Logging.WriteDebug("Flying UnStuck - Jump attempt done.");
                Point point = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                Logging.WriteDebug("Flying UnStuck - lastPost = " + point);
                int num = Others.Random(2, 3);
                for (int i = 0; i < num; i++)
                {
                    Logging.WriteDebug("Flying UnStuck - UnStuck attempt " + i + "started.");
                    int num3 = Others.Random(1, 8);
                    int num4 = Others.Random(-15, 15);
                    float num5 = System.Math.Abs((float) (point.X - nManager.Wow.ObjectManager.ObjectManager.Me.Position.X));
                    float num6 = System.Math.Abs((float) (point.Y - nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y));
                    Logging.WriteDebug("Flying UnStuck - disX = " + num5);
                    Logging.WriteDebug("Flying UnStuck - disY = " + num6);
                    if ((num5 < 5f) || (num6 < 5f))
                    {
                        if (num3 == 1)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Backward done.");
                        }
                        else if (num3 == 2)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Backward Left done.");
                        }
                        else if (num3 == 3)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Left done.");
                        }
                        else if (num3 == 4)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Forward Left done.");
                        }
                        else if (num3 == 5)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Forward done.");
                        }
                        else if (num3 == 6)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Forward Right done.");
                        }
                        else if (num3 == 7)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Right done.");
                        }
                        else if (num3 == 8)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Backward Right done.");
                        }
                        Thread.Sleep(100);
                        nManager.Helpful.Timer timer = new nManager.Helpful.Timer(3000.0);
                        while ((ClickToMove.GetClickToMoveTypePush() == ClickToMoveType.Move) && !timer.IsReady)
                        {
                            Thread.Sleep(50);
                        }
                        if (ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.None)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 4, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Reset position to our current location and elevate");
                            MovementsAction.Ascend(true, false, false);
                            Thread.Sleep(Others.Random(100, 300));
                            MovementsAction.Ascend(false, false, false);
                        }
                        if (i == 7)
                        {
                            switch (Others.Random(1, 4))
                            {
                                case 2:
                                    MovementsAction.Ascend(true, false, false);
                                    Thread.Sleep(Others.Random(500, 0x3e8));
                                    MovementsAction.Ascend(false, false, false);
                                    Logging.WriteDebug("Flying UnStuck - Jump attempt done.");
                                    break;

                                case 3:
                                    MovementsAction.Descend(true, false, false);
                                    Thread.Sleep(Others.Random(500, 0x3e8));
                                    MovementsAction.Descend(false, false, false);
                                    Logging.WriteDebug("Flying UnStuck - Down attempt done.");
                                    break;
                            }
                        }
                        Logging.WriteDebug("Flying UnStuck - UnStuck attempt " + i + " finished.");
                    }
                    else
                    {
                        Logging.WriteDebug("Flying UnStuck - UnStuck attempt " + i + " finished by direct breaking.");
                        break;
                    }
                }
                Logging.WriteDebug("UnStuckFly() done.");
            }
            catch (Exception exception)
            {
                Logging.WriteError("UnStuckFly(): " + exception, true);
            }
        }

        public static uint UpdateTarget(ref Npc Target, out bool asMoved, bool isDead = false)
        {
            WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(Target.Entry, isDead), Target.Position, false);
            asMoved = false;
            if (unit.IsValid)
            {
                asMoved = Target.Position.DistanceTo(unit.Position) > 3f;
                Target.Position = unit.Position;
                Target.Name = unit.Name;
                return unit.GetBaseAddress;
            }
            WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(Target.Entry), Target.Position, false);
            if (obj2.IsValid)
            {
                asMoved = Target.Position.DistanceTo(obj2.Position) > 3f;
                Target.Position = obj2.Position;
                Target.Name = obj2.Name;
                return obj2.GetBaseAddress;
            }
            WoWPlayer objectWoWPlayer = nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer(Target.Guid);
            if ((objectWoWPlayer != null) && objectWoWPlayer.IsValid)
            {
                asMoved = Target.Position != objectWoWPlayer.Position;
                Target.Position = objectWoWPlayer.Position;
                Target.Name = objectWoWPlayer.Name;
                return objectWoWPlayer.GetBaseAddress;
            }
            return 0;
        }

        public static bool Chasing
        {
            get
            {
                return _chasing;
            }
            set
            {
                _chasing = value;
            }
        }

        public static List<Point> CurrentPath
        {
            get
            {
                try
                {
                    return _points;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PointId: " + exception, true);
                }
                return null;
            }
        }

        public static bool InMovement
        {
            get
            {
                try
                {
                    return _movement;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("InMovement: " + exception, true);
                }
                return false;
            }
        }

        public static bool InMoveTo
        {
            get
            {
                try
                {
                    return _loopMoveTo;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("InMoveTo: " + exception, true);
                }
                return false;
            }
        }

        public static bool IsUnStuck
        {
            [CompilerGenerated]
            get
            {
                return <IsUnStuck>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                <IsUnStuck>k__BackingField = value;
            }
        }

        public static int PointId
        {
            get
            {
                try
                {
                    return _currentTargetedPoint;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PointId: " + exception, true);
                }
                return 0;
            }
            set
            {
                try
                {
                    _currentTargetedPoint = value;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PointId: " + exception, true);
                    _currentTargetedPoint = 0;
                }
            }
        }
    }
}

