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
        private static Point _pointLongMove = new Point();
        private static bool _used;
        private static bool _usedLoop;
        private const float altitude = 150f;
        private static nManager.Helpful.Timer RegenPath = new nManager.Helpful.Timer(0.0);

        public static void LongMoveByNewThread(Point point)
        {
            try
            {
                _pointLongMove = point;
                Thread thread2 = new Thread(new ThreadStart(LongMove.LongMoveGo)) {
                    IsBackground = true,
                    Name = "LongMove"
                };
                thread2.Start();
                Thread.Sleep(100);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveByNewThread(Point point): " + exception, true);
            }
        }

        private static void LongMoveGo()
        {
            try
            {
                LongMoveGo(_pointLongMove);
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveGo(): " + exception, true);
            }
        }

        public static void LongMoveGo(Point point)
        {
            try
            {
                int times = Others.Times;
                if (_used)
                {
                    _used = false;
                    while (_usedLoop)
                    {
                        Thread.Sleep(5);
                    }
                }
                _used = true;
                _usedLoop = true;
                MountTask.Mount(false);
                Point position = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                nManager.Helpful.Timer timer = new nManager.Helpful.Timer(2500.0);
                while ((nManager.Products.Products.IsStarted && (nManager.Wow.ObjectManager.ObjectManager.Me.IsMounted || (MountTask.GetMountCapacity() == MountCapacity.Feet))) && (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f) && _used) && _usedLoop))
                {
                    if (MountTask.GetMountCapacity() == MountCapacity.Feet)
                    {
                        if (RegenPath.IsReady && (nManager.Wow.ObjectManager.ObjectManager.Me.Position.DistanceTo(point) > 3.5f))
                        {
                            RegenPath = new nManager.Helpful.Timer(60000.0);
                            MovementManager.Go(PathFinder.FindPath(point));
                            RegenPath.Reset();
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
                                Point from = nManager.Wow.ObjectManager.ObjectManager.Me.Position;
                                from.Z -= 2f;
                                Point to = new Point(point.X, point.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f, "None");
                                if (point.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 130f)
                                {
                                    to = nManager.Helpful.Math.GetPosition2DOfLineByDistance(nManager.Wow.ObjectManager.ObjectManager.Me.Position, point, 130f);
                                    to.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                                }
                                if (TraceLine.TraceLineGo(from, to, CGWorldFrameHitFlags.HitTestAll) || (((nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 10f) < point.Z) && (point.DistanceTo2D(nManager.Wow.ObjectManager.ObjectManager.Me.Position) < 100f)))
                                {
                                    MovementsAction.Descend(false, false, false);
                                    MovementsAction.Ascend(true, false, false);
                                    timer = new nManager.Helpful.Timer(1000.0);
                                    to = new Point(point.X, point.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f, "None");
                                    if (point.DistanceTo(nManager.Wow.ObjectManager.ObjectManager.Me.Position) > 130f)
                                    {
                                        to = nManager.Helpful.Math.GetPosition2DOfLineByDistance(nManager.Wow.ObjectManager.ObjectManager.Me.Position, point, 60f);
                                        to.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 2.5f;
                                    }
                                    if (TraceLine.TraceLineGo(from, to, CGWorldFrameHitFlags.HitTestAll))
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
                                        to = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 150f, "None");
                                        Point a = new Point(nManager.Wow.ObjectManager.ObjectManager.Me.Position.X, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Y, nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z + 5f, "None");
                                        Point point6 = nManager.Helpful.Math.GetPosition2DOfLineByDistance(a, point, 80f);
                                        point6.Z = nManager.Wow.ObjectManager.ObjectManager.Me.Position.Z - 150f;
                                        if (!TraceLine.TraceLineGo(a, to, CGWorldFrameHitFlags.HitTestAll))
                                        {
                                            if (!TraceLine.TraceLineGo(a, point6, CGWorldFrameHitFlags.HitTestAll))
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
                            MountTask.Mount(false);
                            MovementsAction.Ascend(true, false, false);
                            Thread.Sleep(0x514);
                            MovementsAction.Ascend(false, false, false);
                        }
                    }
                    Thread.Sleep(150);
                }
                MovementsAction.Descend(false, false, false);
                MovementsAction.Ascend(false, false, false);
                _used = false;
                _usedLoop = false;
                RegenPath.ForceReady();
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > LongMoveGo(Point point): " + exception, true);
                _used = false;
                _usedLoop = false;
                RegenPath.ForceReady();
            }
        }

        public static void StopLongMove()
        {
            try
            {
                MovementsAction.Ascend(false, false, false);
                MovementsAction.Descend(false, false, false);
                _used = false;
                _usedLoop = false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LongMove > StopLongMove(): " + exception, true);
                _used = false;
                _usedLoop = false;
            }
        }

        public static bool IsLongMove
        {
            get
            {
                if (!_used)
                {
                    return _usedLoop;
                }
                return true;
            }
        }
    }
}

