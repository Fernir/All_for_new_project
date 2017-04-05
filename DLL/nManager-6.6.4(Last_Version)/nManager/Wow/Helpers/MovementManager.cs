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
        private static Npc _amimoqouvuev = new Npc();
        private static bool _esieci;
        private static uint _evilapoxoaw;
        private static bool _ewaucifiof;
        private static uint _fauhiowuloase;
        private static Point _giuvago = new Point();
        private static bool _goewume = true;
        private static bool _ikivuehiogeawaHaxuiweci;
        private static Point _isinouCajajiuxu = new Point();
        private static UInt128 _loukeseliJax;
        private static nManager.Helpful.Timer _maipokeribi = new nManager.Helpful.Timer();
        private static Thread _moaviPoaweam;
        private static int _ogoqawaVui;
        private static nManager.Helpful.Timer _oqexua = new nManager.Helpful.Timer();
        private static bool _oxojosomatuoxCawadaok;
        private static List<Point> _peucaebaIboeqei = new List<Point>();
        private static Point _qisoaguoxejXuja = new Point();
        private static List<Point> _qutoexu = new List<Point>();
        private static bool _tiage;
        private static bool _uwifejiulo;
        private static nManager.Helpful.Timer _xasexekaqQog = new nManager.Helpful.Timer(0.0);
        private static Thread _xiqeohiOwo;
        private static int _xukiheocuwo;
        private static readonly object LockStopMoveTo = new object();
        private static readonly nManager.Helpful.Timer MeleeControlMovementTimer = new nManager.Helpful.Timer(3000.0);
        private static readonly nManager.Helpful.Timer MeleeControlTimer = new nManager.Helpful.Timer(5000.0);
        public static nManager.Helpful.Timer MountCheckTimer = new nManager.Helpful.Timer(15000.0);
        public static int StuckCount;
        public static nManager.Helpful.Timer SwimmingMountRecentlyTimer = new nManager.Helpful.Timer(0.0);

        private static float BiaxeawokuwIdenainoi(float piawe)
        {
            try
            {
                if (piawe < 0f)
                {
                    piawe += 6.283185f;
                }
                return piawe;
            }
            catch (Exception exception)
            {
                Logging.WriteError("NegativeAngle(float angle): " + exception, true);
            }
            return 0f;
        }

        public static void ConstantFaceCTM(WoWObject obj, bool start = true)
        {
            if (obj.IsValid)
            {
                UInt128 guid = obj.Guid;
                ClickToMove.CGPlayer_C__ClickToMove(0f, 0f, 0f, guid, start ? 13 : 4, 0f);
            }
        }

        private static void Ecepiawiacehi()
        {
            try
            {
                if (_xiqeohiOwo != null)
                {
                    try
                    {
                        _xiqeohiOwo.Interrupt();
                        _xiqeohiOwo.Abort();
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("LaunchThreadMoveTo()#1: " + exception, true);
                        _xiqeohiOwo = null;
                    }
                }
                Thread thread = new Thread(new ThreadStart(MovementManager.WucuveuniefoAsu)) {
                    IsBackground = true,
                    Name = "MoveTo"
                };
                _xiqeohiOwo = thread;
                _xiqeohiOwo.Start();
            }
            catch (Exception exception2)
            {
                Logging.WriteError("LaunchThreadMoveTo()#2: " + exception2, true);
                _ewaucifiof = false;
            }
        }

        private static void EtihereovuenemUq()
        {
            _oqexua = null;
            _maipokeribi = new nManager.Helpful.Timer(2000.0);
        }

        private static void ExaubuivanauNi(WoWUnit voiquni)
        {
            MeleeControlMovementTimer.Reset();
            MovementsAction.MoveBackward(true, false);
            while ((!CombatClass.AboveMinRange(voiquni) && CombatClass.InRange(voiquni)) && (nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe && !MeleeControlMovementTimer.IsReady))
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
                float num = BiaxeawokuwIdenainoi((float) System.Math.Atan2((double) (position.Y - point.Y), (double) (position.X - point.X)));
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
                float num = BiaxeawokuwIdenainoi((float) System.Math.Atan2((double) (obj.Position.Y - position.Y), (double) (obj.Position.X - position.X)));
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
                float num = BiaxeawokuwIdenainoi((float) System.Math.Atan2((double) (obj.Position.Y - position.Y), (double) (obj.Position.X - position.X)));
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
                float num = BiaxeawokuwIdenainoi((float) System.Math.Atan2((double) (obj.Position.Y - position.Y), (double) (obj.Position.X - position.X)));
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

        public static void FaceCTM(WoWObject obj, bool start = true)
        {
            if (obj.IsValid)
            {
                UInt128 guid = obj.Guid;
                ClickToMove.CGPlayer_C__ClickToMove(0f, 0f, 0f, guid, 2, 0f);
            }
        }

        public static uint FindTarget(WoWGameObject gObject, float specialRange = 0f, bool doMount = true, float maxDist = 0f, bool ignoreBlacklist = false)
        {
            if (_loukeseliJax != gObject.Guid)
            {
                StopMove();
                Npc npc = new Npc {
                    Entry = gObject.Entry,
                    Position = gObject.Position,
                    Name = gObject.Name
                };
                _amimoqouvuev = npc;
                _loukeseliJax = gObject.Guid;
                EtihereovuenemUq();
            }
            return FindTarget(ref _amimoqouvuev, specialRange, doMount, false, maxDist, ignoreBlacklist);
        }

        public static uint FindTarget(ref Npc target, float specialRange = 0f, bool doMount = true, bool isDead = false, float maxDist = 0f, bool ignoreBlacklist = false)
        {
            bool flag;
            bool flag2;
            uint num = UpdateTarget(ref target, out flag2, isDead, ignoreBlacklist);
            if (((doMount && !InMovement) && ((num <= 0) && (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 5f))) && ((target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) - specialRange) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount))
            {
                MountTask.Mount(false, true);
            }
            if ((doMount && !InMovement) && ((num > 0) && (target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 60f)))
            {
                MountTask.Mount(false, true);
            }
            WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(target.Guid);
            if (!InMovement && ((target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > ((specialRange > 0f) ? ((double) specialRange) : ((new Random().NextDouble() * 2.0) + 2.5))) || (((target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 5f) && (objectByGuid is WoWUnit)) && TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, target.Position, CGWorldFrameHitFlags.HitTestBoundingModels | CGWorldFrameHitFlags.HitTestMovableObjects | CGWorldFrameHitFlags.HitTestWMO))))
            {
                List<Point> listPoints = PathFinder.FindPath(target.Position, out flag, true, false);
                if ((((flag && (nManager.Helpful.Math.DistanceListPoint(listPoints) > 200f)) || !flag) && ((num == 0) && (MountTask.GetMountCapacity() == MountCapacity.Fly))) || Usefuls.IsFlying)
                {
                    Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(target.Position));
                    LongMove.LongMoveByNewThread(target.Position);
                    return 0;
                }
                if (!flag && ((target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 300f) || (listPoints.Count < 2)))
                {
                    Logging.Write("No path found for " + target.Name + ", abort FindTarget.");
                    return 0;
                }
                if ((maxDist > 0f) && (nManager.Helpful.Math.DistanceListPoint(listPoints) > maxDist))
                {
                    return 0;
                }
                if (target.Guid == 0)
                {
                    Logging.Write(string.Concat(new object[] { "Looking for ", target.Name, " (", target.Entry, ")." }));
                }
                float num2 = nManager.Helpful.Math.DistanceListPoint(listPoints);
                _evilapoxoaw = num;
                _maipokeribi = new nManager.Helpful.Timer(2000.0);
                _oqexua = new nManager.Helpful.Timer((double) (((int) ((num2 / 3f) * 1000f)) + 0xfa0));
                Go(listPoints);
                return num;
            }
            if ((InMovement && Usefuls.InGame) && (!nManager.Wow.ObjectManager.ObjectManager.Me.InInevitableCombat && !nManager.Wow.ObjectManager.ObjectManager.Me.IsDeadMe))
            {
                objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(target.Guid);
                if ((target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= ((specialRange > 0f) ? ((double) specialRange) : ((new Random().NextDouble() * 2.0) + 2.5))) && (((target.Position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) <= 5f) || !(objectByGuid is WoWUnit)) || !TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, target.Position, CGWorldFrameHitFlags.HitTestBoundingModels | CGWorldFrameHitFlags.HitTestMovableObjects | CGWorldFrameHitFlags.HitTestWMO)))
                {
                    StopMove();
                    return UpdateTarget(ref target, out flag2, isDead, ignoreBlacklist);
                }
                num = UpdateTarget(ref target, out flag2, isDead, ignoreBlacklist);
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
                    Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(target.Position));
                    LongMove.LongMoveByNewThread(target.Position);
                    return 0;
                }
                if ((num != 0) && (num != _evilapoxoaw))
                {
                    _evilapoxoaw = num;
                    flag2 = true;
                }
                if (flag2 && _maipokeribi.IsReady)
                {
                    _maipokeribi = new nManager.Helpful.Timer(2000.0);
                    List<Point> list2 = PathFinder.FindPath(target.Position, out flag, true, false);
                    if (!flag)
                    {
                        list2.Add(target.Position);
                    }
                    _oqexua = new nManager.Helpful.Timer((double) (((int) ((nManager.Helpful.Math.DistanceListPoint(list2) / 3f) * 1000f)) + 0xfa0));
                    Go(list2);
                    return num;
                }
                if ((_oqexua != null) && _oqexua.IsReady)
                {
                    WoWGameObject obj3 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(target.Entry), target.Position, ignoreBlacklist);
                    WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(target.Entry, false), target.Position, false, ignoreBlacklist, true);
                    if (unit.IsValid)
                    {
                        nManagerSetting.AddBlackList(unit.Guid, 0xea60);
                    }
                    else if (obj3.IsValid)
                    {
                        nManagerSetting.AddBlackList(obj3.Guid, 0xea60);
                    }
                    StopMove();
                }
            }
            return UpdateTarget(ref target, out flag2, isDead, ignoreBlacklist);
        }

        public static uint FindTarget(WoWPlayer player, float specialRange = 0f, bool doMount = true, float maxDist = 0f, bool isDead = false, bool ignoreBlacklist = false)
        {
            if (_loukeseliJax != player.Guid)
            {
                Npc npc = new Npc {
                    Entry = player.Entry,
                    Position = player.Position,
                    Name = player.Name,
                    Guid = player.Guid
                };
                _amimoqouvuev = npc;
                _loukeseliJax = player.Guid;
                EtihereovuenemUq();
            }
            return FindTarget(ref _amimoqouvuev, specialRange, doMount, isDead, maxDist, ignoreBlacklist);
        }

        public static uint FindTarget(WoWUnit unit, float specialRange = 0f, bool doMount = true, float maxDist = 0f, bool isDead = false, bool ignoreBlacklist = false)
        {
            if (_loukeseliJax != unit.Guid)
            {
                StopMove();
                Npc npc = new Npc {
                    Entry = unit.Entry,
                    Position = unit.Position,
                    Name = unit.Name
                };
                _amimoqouvuev = npc;
                _loukeseliJax = unit.Guid;
                EtihereovuenemUq();
            }
            return FindTarget(ref _amimoqouvuev, specialRange, doMount, isDead, maxDist, ignoreBlacklist);
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
                    float num2 = (point.Type.ToLower() == "flying") ? PathFinder.GetZPosition(point, false) : point.Z;
                    if (System.Math.Abs(num2) < 0.0001)
                    {
                        num++;
                        continue;
                    }
                    if (source.Any<Point>())
                    {
                        bool flag;
                        source = PathFinder.FindPath(source[source.Count<Point>() - 1], new Point(point.X, point.Y, num2, "None"), Usefuls.ContinentNameMpq, out flag, true, false, false);
                        if (flag)
                        {
                            goto Label_00D1;
                        }
                        num++;
                        continue;
                    }
                    source.Add(new Point(point.X, point.Y, num2, "None"));
                Label_00D1:
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

        private static void GicajuosuRed(int jaracoiriaboaIkuen)
        {
            try
            {
                lock (_qutoexu)
                {
                    if (_qutoexu.Count > 0)
                    {
                        if ((jaracoiriaboaIkuen < 0) || (jaracoiriaboaIkuen > (_qutoexu.Count - 1)))
                        {
                            jaracoiriaboaIkuen = 0;
                        }
                        if ((_uwifejiulo && (_qutoexu.Count > 0)) && (_qutoexu[jaracoiriaboaIkuen].Type.ToLower() != "swimming"))
                        {
                            if ((_esieci && (_qutoexu[jaracoiriaboaIkuen].DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) >= 200f)) && !string.IsNullOrWhiteSpace(nManagerSetting.CurrentSetting.FlyingMountName))
                            {
                                Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_qutoexu[jaracoiriaboaIkuen]));
                                LongMove.LongMoveByNewThread(_qutoexu[jaracoiriaboaIkuen]);
                                Thread.Sleep(100);
                                while (LongMove.IsLongMove && _uwifejiulo)
                                {
                                    Thread.Sleep(50);
                                }
                                LongMove.StopLongMove();
                                if (!_uwifejiulo)
                                {
                                    return;
                                }
                            }
                            if (_esieci || (nManager.Helpful.Math.DistanceListPoint(_qutoexu) >= nManagerSetting.CurrentSetting.MinimumDistanceToUseMount))
                            {
                                MountTask.Mount(false, false);
                                if (Usefuls.IsFlying)
                                {
                                    List<Point> list = new List<Point>();
                                    for (int i = 0; i <= (_qutoexu.Count - 1); i++)
                                    {
                                        Point item = new Point(_qutoexu[i].X, _qutoexu[i].Y, _qutoexu[i].Z + 2f, "flying");
                                        list.Add(item);
                                    }
                                    _qutoexu = list;
                                    if (jaracoiriaboaIkuen > (_qutoexu.Count - 1))
                                    {
                                        jaracoiriaboaIkuen = _qutoexu.Count - 1;
                                    }
                                }
                            }
                            _ogoqawaVui = StuckCount;
                            int num2 = jaracoiriaboaIkuen;
                            while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                            {
                                Thread.Sleep(10);
                            }
                            MoveTo(_qutoexu[num2], false);
                            bool flag = false;
                            while ((_uwifejiulo && !flag) && (!Usefuls.IsLoading && Usefuls.InGame))
                            {
                                try
                                {
                                    if ((_qutoexu.Count <= 0) || ((_qutoexu.Count - 1) < num2))
                                    {
                                        flag = true;
                                        return;
                                    }
                                    if (_qutoexu[num2].Type.ToLower() == "swimming")
                                    {
                                        return;
                                    }
                                    if ((((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_qutoexu[num2]) <= 3f) && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted) || (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_qutoexu[num2]) <= 3f)) && ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceZ(_qutoexu[num2]) <= 10.5f) && _uwifejiulo))
                                    {
                                        num2++;
                                        if (num2 > (_qutoexu.Count - 1))
                                        {
                                            num2 = _qutoexu.Count - 1;
                                            flag = true;
                                            if (_esieci)
                                            {
                                                flag = false;
                                                _qutoexu = new List<Point>();
                                                _qutoexu.AddRange(_peucaebaIboeqei);
                                                num2 = 0;
                                            }
                                        }
                                    }
                                    if (!_goewume || (_ogoqawaVui != StuckCount))
                                    {
                                        try
                                        {
                                            _ogoqawaVui = StuckCount;
                                            StopMoveTo(true, false);
                                            _qutoexu = PathFinder.FindPath(_peucaebaIboeqei[_peucaebaIboeqei.Count - 1]);
                                            num2 = 0;
                                        }
                                        catch (Exception exception)
                                        {
                                            Logging.WriteError("ThreadMovementManager()#1: " + exception, true);
                                        }
                                        _goewume = true;
                                    }
                                    if ((_qutoexu.Count <= 0) || ((_qutoexu.Count - 1) < num2))
                                    {
                                        return;
                                    }
                                    if (_esieci)
                                    {
                                        _xukiheocuwo = num2;
                                    }
                                    MoveTo(_qutoexu[num2], false);
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
                                    num2 = nManager.Helpful.Math.NearestPointOfListPoints(_qutoexu, nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                                    continue;
                                }
                            }
                            if (!_tiage)
                            {
                                if (!_esieci)
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
                }
            }
            catch (Exception exception3)
            {
                Logging.WriteError("MovementManager > GroundMouvementManager(int firstIdPoint): " + exception3, true);
                StopMove();
            }
        }

        public static void Go(List<Point> points)
        {
            try
            {
                if (System.Math.Abs((float) (nManager.Helpful.Math.DistanceListPoint(_qutoexu) - nManager.Helpful.Math.DistanceListPoint(points))) > 0.0001)
                {
                    _esieci = false;
                    _uwifejiulo = false;
                    lock (typeof(MovementManager))
                    {
                        if (_moaviPoaweam == null)
                        {
                            _xukiheocuwo = 0;
                            LaunchThreadMovementManager();
                        }
                        _peucaebaIboeqei = new List<Point>();
                        _peucaebaIboeqei.AddRange(points);
                        _qutoexu = new List<Point>();
                        _qutoexu.AddRange(points);
                        _uwifejiulo = true;
                        _ikivuehiogeawaHaxuiweci = true;
                        return;
                    }
                }
                if ((_esieci || !_uwifejiulo) || !_ikivuehiogeawaHaxuiweci)
                {
                    _ikivuehiogeawaHaxuiweci = true;
                    _esieci = false;
                    _uwifejiulo = true;
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
                if (System.Math.Abs((float) (nManager.Helpful.Math.DistanceListPoint(_qutoexu) - nManager.Helpful.Math.DistanceListPoint(points))) > 0.0001)
                {
                    _esieci = false;
                    _uwifejiulo = false;
                    lock (typeof(MovementManager))
                    {
                        if (_moaviPoaweam == null)
                        {
                            LaunchThreadMovementManager();
                        }
                        _peucaebaIboeqei = new List<Point>();
                        _peucaebaIboeqei.AddRange(points);
                        _qutoexu = new List<Point>();
                        _qutoexu.AddRange(points);
                        _xukiheocuwo = nManager.Helpful.Math.NearestPointOfListPoints(_qutoexu, nManager.Wow.ObjectManager.ObjectManager.Me.Position);
                        _esieci = true;
                        _uwifejiulo = true;
                        return;
                    }
                }
                if (!_esieci || !_uwifejiulo)
                {
                    _esieci = true;
                    _uwifejiulo = true;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("GoLoop(List<Point> points): " + exception, true);
            }
        }

        private static void Ibaopiqeipo(int jaracoiriaboaIkuen)
        {
            try
            {
                if (_uwifejiulo && (_qutoexu.Count > 0))
                {
                    if (MountTask.GetMountCapacity() < MountCapacity.Fly)
                    {
                        Logging.Write("This profile needs you to be able to fly.");
                    }
                    if (!Usefuls.IsFlying)
                    {
                        MountTask.MountingFlyingMount(false);
                    }
                    if (_uwifejiulo)
                    {
                        int num = jaracoiriaboaIkuen;
                        if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(_qutoexu[num]) > 200f) || (_qutoexu.Count == 1))
                        {
                            Logging.WriteNavigator("Long Move distance: " + nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_qutoexu[num]));
                            LongMove.LongMoveByNewThread(_qutoexu[num]);
                            Thread.Sleep(100);
                            while (LongMove.IsLongMove && _uwifejiulo)
                            {
                                Thread.Sleep(50);
                            }
                            LongMove.StopLongMove();
                        }
                        bool flag = false;
                        while ((_uwifejiulo && !flag) && (!Usefuls.IsLoading && Usefuls.InGame))
                        {
                            if ((_qutoexu[num].Type.ToLower() == "flying") && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
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
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_qutoexu[num]) < 15f) && _uwifejiulo)
                            {
                                num++;
                                if (num > (_qutoexu.Count - 1))
                                {
                                    num = _qutoexu.Count - 1;
                                    flag = true;
                                    if (_esieci)
                                    {
                                        flag = false;
                                        _qutoexu = new List<Point>();
                                        _qutoexu.AddRange(_peucaebaIboeqei);
                                        num = 0;
                                    }
                                }
                                if (_esieci)
                                {
                                    _xukiheocuwo = num;
                                }
                                MoveTo(_qutoexu[num], false);
                            }
                            MoveTo(_qutoexu[num], false);
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

        public static void LaunchThreadMovementManager()
        {
            try
            {
                lock (typeof(MovementManager))
                {
                    if (_moaviPoaweam == null)
                    {
                        Thread thread = new Thread(new ThreadStart(MovementManager.Popaicou)) {
                            IsBackground = true,
                            Name = "MovementManager"
                        };
                        _moaviPoaweam = thread;
                        _moaviPoaweam.Start();
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LaunchThreadMovementManager(): " + exception, true);
            }
        }

        public static void MicroMove()
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
                if (point.IsValid)
                {
                    if (_xiqeohiOwo == null)
                    {
                        Ecepiawiacehi();
                    }
                    _oxojosomatuoxCawadaok = farm;
                    _qisoaguoxejXuja = new Point(point.X, point.Y, point.Z, "None");
                    _ewaucifiof = true;
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

        private static void Neniefeugose(int jaracoiriaboaIkuen)
        {
            try
            {
                if ((_uwifejiulo && (_qutoexu.Count > 0)) && (_qutoexu.Count > jaracoiriaboaIkuen))
                {
                    int num = jaracoiriaboaIkuen;
                    if (_qutoexu[num].DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.MinimumDistanceToUseMount)
                    {
                        MountTask.Mount(false, false);
                    }
                    if (_uwifejiulo)
                    {
                        if (_qutoexu[num].DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 30f)
                        {
                            bool flag;
                            List<Point> list = PathFinder.FindPath(_qutoexu[num], out flag, true, false);
                            if (flag)
                            {
                                for (int i = 0; i <= (list.Count - 1); i++)
                                {
                                    list[i].Type = "Swimming";
                                }
                                _qutoexu = list;
                                _esieci = false;
                                num = 0;
                            }
                        }
                        bool flag2 = false;
                        while ((_uwifejiulo && !flag2) && (!Usefuls.IsLoading && Usefuls.InGame))
                        {
                            if (!(_qutoexu[num].Type.ToLower() != "swimming"))
                            {
                                goto Label_011A;
                            }
                            return;
                        Label_0100:
                            MovementsAction.Ascend(true, false, false);
                            Thread.Sleep(200);
                            MovementsAction.Ascend(false, false, false);
                        Label_011A:
                            if ((!Usefuls.IsSwimming && !Usefuls.IsFlying) && (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && (MountTask.GetMountCapacity() == MountCapacity.Fly)))
                            {
                                goto Label_0100;
                            }
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(_qutoexu[num]) < 7f) && _uwifejiulo)
                            {
                                num++;
                                if (num > (_qutoexu.Count - 1))
                                {
                                    num = _qutoexu.Count - 1;
                                    flag2 = true;
                                    if (_esieci)
                                    {
                                        flag2 = false;
                                        _qutoexu = new List<Point>();
                                        _qutoexu.AddRange(_peucaebaIboeqei);
                                        num = 0;
                                    }
                                }
                                _xukiheocuwo = num;
                                MoveTo(_qutoexu[num], false);
                            }
                            MoveTo(_qutoexu[num], false);
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

        private static void Owiri(WoWUnit voiquni)
        {
            MeleeControlMovementTimer.Reset();
            MovementsAction.MoveForward(true, false);
            while ((!CombatClass.InRange(voiquni) && CombatClass.AboveMinRange(voiquni)) && (nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe && !MeleeControlMovementTimer.IsReady))
            {
                Thread.Sleep(50);
            }
            MovementsAction.MoveForward(false, false);
        }

        private static void Popaicou()
        {
            try
            {
                while (true)
                {
                    lock (typeof(MovementManager))
                    {
                        while ((_esieci || _ikivuehiogeawaHaxuiweci) && (!Usefuls.IsLoading && Usefuls.InGame))
                        {
                            if (Statistics.OffsetStats == 0xb5)
                            {
                                _ikivuehiogeawaHaxuiweci = false;
                                if (_uwifejiulo && (_qutoexu.Count > 0))
                                {
                                    int jaracoiriaboaIkuen = 0;
                                    if (_esieci)
                                    {
                                        jaracoiriaboaIkuen = _xukiheocuwo;
                                    }
                                    if (jaracoiriaboaIkuen > (_qutoexu.Count - 1))
                                    {
                                        jaracoiriaboaIkuen = _qutoexu.Count - 1;
                                    }
                                    if (_qutoexu[jaracoiriaboaIkuen].DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 0.8f)
                                    {
                                        jaracoiriaboaIkuen++;
                                    }
                                    if ((jaracoiriaboaIkuen < 0) || (jaracoiriaboaIkuen > (_qutoexu.Count - 1)))
                                    {
                                        jaracoiriaboaIkuen = 0;
                                    }
                                    if (_esieci)
                                    {
                                        _xukiheocuwo = jaracoiriaboaIkuen;
                                    }
                                    if (_qutoexu[jaracoiriaboaIkuen].Type.ToLower() == "flying")
                                    {
                                        Ibaopiqeipo(jaracoiriaboaIkuen);
                                    }
                                    else if (_qutoexu[jaracoiriaboaIkuen].Type.ToLower() == "swimming")
                                    {
                                        Neniefeugose(jaracoiriaboaIkuen);
                                    }
                                    else
                                    {
                                        GicajuosuRed(jaracoiriaboaIkuen);
                                    }
                                    Statistics.OffsetStats = 0x53;
                                }
                                if ((_qutoexu.Count <= 0) && !_ikivuehiogeawaHaxuiweci)
                                {
                                    _uwifejiulo = false;
                                    _esieci = false;
                                }
                            }
                            Thread.Sleep(80);
                        }
                        if (!_esieci && !_ikivuehiogeawaHaxuiweci)
                        {
                            _uwifejiulo = false;
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

        public static void StopMove()
        {
            try
            {
                _esieci = false;
                _uwifejiulo = false;
                StopMoveTo(true, false);
            }
            catch (Exception exception)
            {
                Logging.WriteError("StopMove(): " + exception, true);
            }
        }

        public static void StopMoveTo(bool stabilizeZAxis = true, bool stabilizeSides = false)
        {
            lock (LockStopMoveTo)
            {
                try
                {
                    _ewaucifiof = false;
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
        }

        private static bool UlamiexLirevudis(Point acekeAcepok)
        {
            try
            {
                while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    Thread.Sleep(100);
                }
                if (MountCheckTimer.IsReady && ((!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (Usefuls.IsSwimming && !MountTask.OnAquaticMount())) || ((!SwimmingMountRecentlyTimer.IsReady && !Usefuls.IsSwimming) && MountTask.OnAquaticMount())))
                {
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.IsAlive && _xasexekaqQog.IsReady) && (!Fight.InFight && !Looting.IsLooting)) && ((!nManager.Wow.ObjectManager.ObjectManager.Me.InCombat && !Usefuls.PlayerUsingVehicle) && ((nManager.Products.Products.ProductName.ToLower() != "fisherbot") && (acekeAcepok.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > nManagerSetting.CurrentSetting.MinimumDistanceToUseMount))))
                    {
                        MountCapacity mountCapacity = MountTask.GetMountCapacity();
                        if (mountCapacity > MountCapacity.Feet)
                        {
                            if ((Usefuls.IsSwimming && !MountTask.OnAquaticMount()) && (mountCapacity == MountCapacity.Swimm))
                            {
                                MountTask.MountingAquaticMount(false);
                            }
                            else if ((!nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (MountTask.OnAquaticMount() && !Usefuls.IsSwimming)) && !Usefuls.IsSwimming)
                            {
                                MountTask.Mount(false, false);
                            }
                        }
                    }
                    if (_xasexekaqQog.IsReady)
                    {
                        MountCheckTimer.Reset();
                    }
                }
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(1000.0);
                nManager.Helpful.Timer timer2 = new nManager.Helpful.Timer(10000.0);
                double num = acekeAcepok.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) - 1.0;
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
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                bool flag = false;
                while (!timer2.IsReady && Usefuls.InGame)
                {
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(acekeAcepok) <= 0.6)
                    {
                        return true;
                    }
                    if (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(acekeAcepok) <= 3.0) && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceZ(acekeAcepok) < 6f)) && _oxojosomatuoxCawadaok)
                    {
                        return true;
                    }
                    Point point2 = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                    while (nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                    {
                        Thread.Sleep(100);
                    }
                    Point point3 = new Point();
                    if (nManager.Wow.ObjectManager.ObjectManager.Me.InTransport && (Usefuls.ContinentId != 0x1e240))
                    {
                        WoWObject objectByGuid = nManager.Wow.ObjectManager.ObjectManager.GetObjectByGuid(nManager.Wow.ObjectManager.ObjectManager.Me.TransportGuid);
                        if (objectByGuid is WoWGameObject)
                        {
                            WoWGameObject o = objectByGuid as WoWGameObject;
                            if (o.IsValid)
                            {
                                point3 = new Point(acekeAcepok.TransformInvert(o));
                            }
                        }
                    }
                    if ((ClickToMove.GetClickToMovePosition().DistanceTo(point3.IsValid ? point3 : acekeAcepok) > 1f) || (ClickToMove.GetClickToMoveTypePush() != ClickToMoveType.Move))
                    {
                        ClickToMove.CGPlayer_C__ClickToMove(point3.IsValid ? point3.X : acekeAcepok.X, point3.IsValid ? point3.Y : acekeAcepok.Y, point3.IsValid ? point3.Z : acekeAcepok.Z, 0, 5, 0.5f);
                    }
                    if (!_ewaucifiof || (_qisoaguoxejXuja.DistanceTo(acekeAcepok) > 0.5f))
                    {
                        break;
                    }
                    if (point2.DistanceTo(position) > 2f)
                    {
                        position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                        timer.Reset();
                    }
                    if (!_ewaucifiof || (_qisoaguoxejXuja.DistanceTo(acekeAcepok) > 0.5f))
                    {
                        break;
                    }
                    if ((point2.DistanceTo(position) < 2f) && timer.IsReady)
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
                    if (!_ewaucifiof || (_qisoaguoxejXuja.DistanceTo(acekeAcepok) > 0.5f))
                    {
                        break;
                    }
                    Thread.Sleep(0x23);
                }
                if (_ewaucifiof && (_qisoaguoxejXuja.DistanceTo(acekeAcepok) < 0.5f))
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

        public static void UnStuck()
        {
            try
            {
                if (!nManager.Wow.ObjectManager.ObjectManager.Me.IsCast)
                {
                    IsUnStuck = true;
                    Logging.WriteDebug("UnStuck() started.");
                    MovementsAction.Ascend(false, false, false);
                    MovementsAction.Descend(false, false, false);
                    Logging.WriteDebug("Jump / Down released.");
                    if ((_isinouCajajiuxu.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 3f) && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted)
                    {
                        Logging.WriteDebug("UnStuck - We are currently mounted.");
                        MovementsAction.Ascend(true, false, false);
                        Thread.Sleep(Others.Random(500, 0x3e8));
                        MovementsAction.Ascend(false, false, false);
                        Logging.WriteDebug("UnStuck - Jump attempt done.");
                        _isinouCajajiuxu = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
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
                        if (_giuvago.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 3f)
                        {
                            float num = 1f * ((float) System.Math.Cos((double) nManager.Wow.ObjectManager.ObjectManager.Me.Rotation));
                            float num2 = 1f * ((float) System.Math.Sin((double) nManager.Wow.ObjectManager.ObjectManager.Me.Rotation));
                            Point to = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + num, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + num2, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 2.5f, "None");
                            _giuvago = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 2.5f, "None");
                            if ((nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && !Usefuls.IsFlying) && !TraceLine.TraceLineGo(_giuvago, to, CGWorldFrameHitFlags.HitTestAll))
                            {
                                Logging.WriteNavigator("UnStuck - Dismounting.");
                                MountTask.DismountMount(true);
                                _xasexekaqQog = new nManager.Helpful.Timer(8000.0);
                                _xasexekaqQog.Reset();
                                IsUnStuck = false;
                                StuckCount++;
                                return;
                            }
                            _isinouCajajiuxu = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 1.5f, "None");
                            to = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + num, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + num2, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 1.5f, "None");
                            if (!TraceLine.TraceLineGo(_isinouCajajiuxu, to, CGWorldFrameHitFlags.HitTestAll))
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
                            if (((i > 3) && nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted) && !Usefuls.IsFlying)
                            {
                                Logging.WriteNavigator("UnStuck - Dismounting.");
                                MountTask.DismountMount(true);
                                _xasexekaqQog = new nManager.Helpful.Timer(8000.0);
                                _xasexekaqQog.Reset();
                                IsUnStuck = false;
                                StuckCount++;
                                return;
                            }
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
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
                                        Logging.WriteDebug("UnStuck - Backward done.");
                                        break;

                                    case 2:
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
                                        Logging.WriteDebug("UnStuck - Backward Left done.");
                                        break;

                                    case 3:
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
                                        Logging.WriteDebug("UnStuck - Left done.");
                                        break;

                                    case 4:
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
                                        Logging.WriteDebug("UnStuck - Forward Left done.");
                                        break;

                                    case 5:
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
                                        Logging.WriteDebug("UnStuck - Forward done.");
                                        break;

                                    case 6:
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
                                        Logging.WriteDebug("UnStuck - Forward Right done.");
                                        break;

                                    case 7:
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 10f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
                                        Logging.WriteDebug("UnStuck - Right done.");
                                        break;

                                    case 8:
                                        ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
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
                    ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
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
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Backward done.");
                        }
                        else if (num3 == 2)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Backward Left done.");
                        }
                        else if (num3 == 3)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Left done.");
                        }
                        else if (num3 == 4)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X - 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Forward Left done.");
                        }
                        else if (num3 == 5)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Forward done.");
                        }
                        else if (num3 == 6)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y + 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Forward Right done.");
                        }
                        else if (num3 == 7)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 15f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
                            Logging.WriteDebug("Flying UnStuck - Right done.");
                        }
                        else if (num3 == 8)
                        {
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X + 17f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y - 14f, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + num4, 0, 5, 0.5f);
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
                            ClickToMove.CGPlayer_C__ClickToMove(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, 0, 5, 0.5f);
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

        public static uint UpdateTarget(ref Npc target, out bool asMoved, bool isDead = false, bool ignoreBlacklist = false)
        {
            WoWUnit unit = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWUnit(nManager.Wow.ObjectManager.ObjectManager.GetWoWUnitByEntry(target.Entry, isDead), target.Position, false, ignoreBlacklist, true);
            asMoved = false;
            if (unit.IsValid)
            {
                asMoved = target.Position.DistanceTo(unit.Position) > 3f;
                target.Position = unit.Position;
                target.Name = unit.Name;
                target.Guid = unit.Guid;
                return unit.GetBaseAddress;
            }
            WoWGameObject obj2 = nManager.Wow.ObjectManager.ObjectManager.GetNearestWoWGameObject(nManager.Wow.ObjectManager.ObjectManager.GetWoWGameObjectByEntry(target.Entry), target.Position, ignoreBlacklist);
            if (obj2.IsValid)
            {
                asMoved = target.Position.DistanceTo(obj2.Position) > 3f;
                target.Position = obj2.Position;
                target.Name = obj2.Name;
                target.Guid = obj2.Guid;
                return obj2.GetBaseAddress;
            }
            WoWPlayer objectWoWPlayer = nManager.Wow.ObjectManager.ObjectManager.GetObjectWoWPlayer(target.Guid);
            if ((objectWoWPlayer != null) && objectWoWPlayer.IsValid)
            {
                asMoved = target.Position != objectWoWPlayer.Position;
                target.Position = objectWoWPlayer.Position;
                target.Name = objectWoWPlayer.Name;
                target.Guid = objectWoWPlayer.Guid;
                return objectWoWPlayer.GetBaseAddress;
            }
            return 0;
        }

        private static void WucuveuniefoAsu()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (_ewaucifiof)
                        {
                            _goewume = UlamiexLirevudis(_qisoaguoxejXuja);
                            _ewaucifiof = false;
                        }
                    }
                    catch (Exception exception)
                    {
                        Logging.WriteError("ThreadMoveTo()#1: " + exception, true);
                        _ewaucifiof = false;
                    }
                    Thread.Sleep(50);
                }
            }
            catch (Exception exception2)
            {
                Logging.WriteError("ThreadMoveTo()#2: " + exception2, true);
            }
        }

        private static void XuokovEfop(WoWUnit voiquni, bool qofiqapaixeKoite)
        {
            if ((_fauhiowuloase <= 5) && ((_fauhiowuloase == 0) || MeleeControlTimer.IsReady))
            {
                MeleeControlTimer.Reset();
                if ((_fauhiowuloase > 0) && qofiqapaixeKoite)
                {
                    _fauhiowuloase = 0;
                }
                if (!CombatClass.AboveMinRange(voiquni) && nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe)
                {
                    Logging.WriteFight("Under the minimal distance from the target, move closer.");
                    _fauhiowuloase++;
                    Owiri(voiquni);
                }
                if (!CombatClass.InRange(voiquni) && nManager.Wow.ObjectManager.ObjectManager.Target.InCombatWithMe)
                {
                    Logging.WriteFight("Over the maximal distance from the target, move closer.");
                    _fauhiowuloase++;
                    ExaubuivanauNi(voiquni);
                }
            }
        }

        public static bool Chasing
        {
            get
            {
                return _tiage;
            }
            set
            {
                _tiage = value;
            }
        }

        public static List<Point> CurrentPath
        {
            get
            {
                try
                {
                    return _qutoexu;
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
                    return _uwifejiulo;
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
                    return _ewaucifiof;
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
                    return _xukiheocuwo;
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
                    _xukiheocuwo = value;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PointId: " + exception, true);
                    _xukiheocuwo = 0;
                }
            }
        }
    }
}

