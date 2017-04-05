namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Products;
    using nManager.Wow.Bot.Tasks;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Threading;

    public class LongMove
    {
        private static bool _adaukuqiopoama;
        private static Thread _deunojaulovaiOtaoco;
        private const float _koubiti = 150f;
        private static nManager.Helpful.Timer _obuafiusabEc = new nManager.Helpful.Timer(0.0);
        private static bool _ruagiaqiesNuk;
        private static Point _suhoem = new Point();

        public static void LongMoveByNewThread(Point point)
        {
            try
            {
                if (((_deunojaulovaiOtaoco == null) || !_deunojaulovaiOtaoco.IsAlive) || (_suhoem.DistanceTo(point) >= 0.0001f))
                {
                    _suhoem = point;
                    Thread thread = new Thread(new ThreadStart(LongMove.VapoxuesioJe)) {
                        IsBackground = true,
                        Name = "LongMove"
                    };
                    _deunojaulovaiOtaoco = thread;
                    _deunojaulovaiOtaoco.Start();
                    Thread.Sleep(100);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveByNewThread(Point point): " + exception, true);
            }
        }

        public static void LongMoveGo(Point point)
        {
            try
            {
                int times = Others.Times;
                if (_adaukuqiopoama)
                {
                    _adaukuqiopoama = false;
                    while (_ruagiaqiesNuk)
                    {
                        Thread.Sleep(5);
                    }
                }
                _adaukuqiopoama = true;
                _ruagiaqiesNuk = true;
                MountTask.Mount(false, true);
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(2500.0);
                bool flag = false;
                while ((nManager.Products.Products.IsStarted && (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (MountTask.GetMountCapacity() == MountCapacity.Feet))) && (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f) && _adaukuqiopoama) && _ruagiaqiesNuk))
                {
                    bool flag2 = false;
                    if (Usefuls.IsFlying)
                    {
                        Point other = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                        if ((point.DistanceTo2D(other) <= 60f) || (flag && (point.DistanceTo2D(other) <= 110f)))
                        {
                            Point to = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.LerpByDistance(point, 3f));
                            bool flag3 = false;
                            Point point5 = new Point();
                            if (to.IsValid && TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to, CGWorldFrameHitFlags.HitTestAllButLiquid))
                            {
                                if (((point.DistanceTo2D(other) <= 60f) || (flag && (point.DistanceTo2D(other) <= 110f))) && (position.Z > point.Z))
                                {
                                    float degrees = 0f;
                                    bool flag4 = false;
                                    while (degrees < 360f)
                                    {
                                        float x = other.X + ((float) (50.0 * System.Math.Cos((double) nManager.Helpful.Math.DegreeToRadian(degrees))));
                                        float y = other.Y + ((float) (50.0 * System.Math.Sin((double) nManager.Helpful.Math.DegreeToRadian(degrees))));
                                        Point from = new Point(x, y, other.Z, "None");
                                        Point point7 = new Point(x, y, PathFinder.GetZPosition(x, y, false), "None");
                                        if (!TraceLine.TraceLineGo(from, new Point(from.LerpByDistance(point, 3f)), CGWorldFrameHitFlags.HitTestAllButLiquid))
                                        {
                                            point5 = from;
                                            MovementManager.MoveTo(point5, false);
                                            Thread.Sleep(0x9c4);
                                            flag = true;
                                            flag4 = true;
                                            break;
                                        }
                                        if (!TraceLine.TraceLineGo(from, point7, CGWorldFrameHitFlags.HitTestAllButLiquid))
                                        {
                                            bool flag5;
                                            PathFinder.FindPath(point7, point, Usefuls.ContinentNameMpq, out flag5, true, false, false);
                                            if (flag5)
                                            {
                                                point5 = from;
                                                MovementManager.MoveTo(point5, false);
                                                Thread.Sleep(0x9c4);
                                                flag = true;
                                                break;
                                            }
                                        }
                                        degrees += 20f;
                                        if (degrees >= 360f)
                                        {
                                            flag3 = true;
                                        }
                                    }
                                    if (flag4)
                                    {
                                        Thread.Sleep(0x3e8);
                                        continue;
                                    }
                                }
                                if (!flag3)
                                {
                                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(point5) > 5f)
                                    {
                                        Thread.Sleep(0x9c4);
                                    }
                                    if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(point5) > 5f)
                                    {
                                        continue;
                                    }
                                    MountTask.DismountMount(true);
                                    _adaukuqiopoama = false;
                                    _ruagiaqiesNuk = false;
                                    return;
                                }
                            }
                            else
                            {
                                MovementManager.MoveTo(to, false);
                                flag = false;
                            }
                        }
                    }
                    if ((MountTask.GetMountCapacity() <= MountCapacity.Ground) || flag2)
                    {
                        if (_obuafiusabEc.IsReady && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f))
                        {
                            _obuafiusabEc = new nManager.Helpful.Timer(60000.0);
                            MovementManager.Go(PathFinder.FindPath(point));
                            _obuafiusabEc.Reset();
                        }
                        else
                        {
                            Thread.Sleep(0x3e8);
                        }
                    }
                    else
                    {
                        if (!MovementManager.IsUnStuck)
                        {
                            if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo2D(point) > 15f)
                            {
                                Point point8 = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                                point8.Z -= 2f;
                                Point point9 = new Point(point.X, point.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f, "None");
                                if (point.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 130f)
                                {
                                    point9 = nManager.Helpful.Math.GetPosition2DOfLineByDistance(nManager.Wow.ObjectManager.ObjectManager.Me.Position, point, 130f);
                                    point9.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                                }
                                if (TraceLine.TraceLineGo(point8, point9, CGWorldFrameHitFlags.HitTestAll) || (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 10f) < point.Z) && (point.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 100f)))
                                {
                                    MovementsAction.Descend(false, false, false);
                                    MovementsAction.Ascend(true, false, false);
                                    timer = new nManager.Helpful.Timer(1000.0);
                                    point9 = new Point(point.X, point.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f, "None");
                                    if (point.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 130f)
                                    {
                                        point9 = nManager.Helpful.Math.GetPosition2DOfLineByDistance(nManager.Wow.ObjectManager.ObjectManager.Me.Position, point, 60f);
                                        point9.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                                    }
                                    if (TraceLine.TraceLineGo(point8, point9, CGWorldFrameHitFlags.HitTestAll))
                                    {
                                        MovementManager.StopMoveTo(false, false);
                                    }
                                    Thread.Sleep(800);
                                    if (position.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 1f)
                                    {
                                        MovementManager.UnStuckFly();
                                    }
                                    else
                                    {
                                        position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                                    }
                                }
                                else
                                {
                                    MovementsAction.Ascend(false, false, false);
                                    if (timer.IsReady)
                                    {
                                        point9 = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 150f, "None");
                                        Point a = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 5f, "None");
                                        Point point11 = nManager.Helpful.Math.GetPosition2DOfLineByDistance(a, point, 80f);
                                        point11.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 150f;
                                        if (!TraceLine.TraceLineGo(a, point9, CGWorldFrameHitFlags.HitTestAll))
                                        {
                                            if (!TraceLine.TraceLineGo(a, point11, CGWorldFrameHitFlags.HitTestAll))
                                            {
                                                MovementsAction.Descend(true, false, false);
                                            }
                                            else
                                            {
                                                timer = new nManager.Helpful.Timer(1000.0);
                                                MovementsAction.Descend(false, false, false);
                                            }
                                        }
                                        else
                                        {
                                            timer = new nManager.Helpful.Timer(1000.0);
                                            MovementsAction.Descend(false, false, false);
                                        }
                                    }
                                    MovementManager.MoveTo(point.X, point.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, false);
                                    if (Others.Times > (times + 0x5dc))
                                    {
                                        MovementManager.MoveTo(point.X, point.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z, false);
                                        times = Others.Times;
                                    }
                                }
                            }
                            else
                            {
                                MovementsAction.Descend(false, false, false);
                                MovementsAction.Ascend(false, false, false);
                                MovementManager.MoveTo(point, false);
                            }
                        }
                        if (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted && !Usefuls.IsFlying)
                        {
                            MovementsAction.Descend(false, false, false);
                            MovementsAction.Ascend(false, false, false);
                            MountTask.Mount(false, true);
                            MovementsAction.Ascend(true, false, false);
                            Thread.Sleep(0x514);
                            MovementsAction.Ascend(false, false, false);
                        }
                    }
                    Thread.Sleep(150);
                }
                MovementsAction.Descend(false, false, false);
                MovementsAction.Ascend(false, false, false);
                _adaukuqiopoama = false;
                _ruagiaqiesNuk = false;
                _obuafiusabEc.ForceReady();
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveGo(Point point): " + exception, true);
                _adaukuqiopoama = false;
                _ruagiaqiesNuk = false;
                _obuafiusabEc.ForceReady();
            }
        }

        public static void StopLongMove()
        {
            try
            {
                MovementsAction.Ascend(false, false, false);
                MovementsAction.Descend(false, false, false);
                _adaukuqiopoama = false;
                _ruagiaqiesNuk = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > StopLongMove(): " + exception, true);
                _adaukuqiopoama = false;
                _ruagiaqiesNuk = false;
            }
        }

        private static void VapoxuesioJe()
        {
            try
            {
                LongMoveGo(_suhoem);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveGo(): " + exception, true);
            }
        }

        public static bool IsLongMove
        {
            get
            {
                if (!_adaukuqiopoama)
                {
                    return _ruagiaqiesNuk;
                }
                return true;
            }
        }
    }
}

