namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers.PathFinderClass;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class PathFinder
    {
        private static Pather _pather;

        public static List<Point> FindLocalPath(Point to, out bool resultSuccess, bool addFromAndStart = true)
        {
            return FindPath(to, out resultSuccess, addFromAndStart, true);
        }

        public static List<Point> FindPath(Point to)
        {
            try
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.Type.ToLower() == "swimming")
                {
                    if (TraceLine.TraceLineGo(new Point(to.X, to.Y, to.Z + 1000f, "None"), to, CGWorldFrameHitFlags.HitTestLiquid))
                    {
                        if (!TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to, CGWorldFrameHitFlags.HitTestAll))
                        {
                            Logging.WriteNavigator("Swimmming right to the destination");
                            return new List<Point> { to };
                        }
                        Logging.WriteNavigator("Swimming to the destination using the PathFinder");
                    }
                    else
                    {
                        Logging.WriteNavigator("Using the PathFinder to destination out of water");
                    }
                }
                return FindPath(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point to): " + exception, true);
            }
            return new List<Point>();
        }

        public static List<Point> FindPath(Point from, Point to)
        {
            try
            {
                return FindPath(from, to, Usefuls.ContinentNameMpq);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point from, Point to): " + exception, true);
            }
            return new List<Point>();
        }

        public static List<Point> FindPath(Point from, Point to, string continentNameMpq)
        {
            try
            {
                bool flag;
                return FindPath(from, to, continentNameMpq, out flag, true, false, false);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point from, Point to, string continentNameMpq): " + exception, true);
            }
            return new List<Point>();
        }

        public static List<Point> FindPath(Point to, out bool resultSuccess, bool addFromAndStart = true, bool ShortPath = false)
        {
            try
            {
                if (nManager.Wow.ObjectManager.ObjectManager.Me.Position.Type.ToLower() == "swimming")
                {
                    if (TraceLine.TraceLineGo(new Point(to.X, to.Y, to.Z + 1000f, "None"), to, CGWorldFrameHitFlags.HitTestLiquid))
                    {
                        if (!TraceLine.TraceLineGo(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to, CGWorldFrameHitFlags.HitTestAll))
                        {
                            Logging.WriteNavigator("Swimmming right to the destination");
                            resultSuccess = true;
                            return new List<Point> { to };
                        }
                        Logging.WriteNavigator("Swimming to the destination using the PathFinder");
                    }
                    else
                    {
                        Logging.WriteNavigator("Using the PathFinder to destination out of water");
                    }
                }
                return FindPath(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to, Usefuls.ContinentNameMpq, out resultSuccess, addFromAndStart, false, ShortPath);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point to, out bool resultSuccess): " + exception, true);
            }
            resultSuccess = false;
            return new List<Point>();
        }

        public static List<Point> FindPath(Point from, Point to, string continentNameMpq, out bool resultSuccess, bool addFromAndStart = true, bool loadAllTile = false, bool ShortPath = false)
        {
            List<Point> source = new List<Point>();
            resultSuccess = true;
            try
            {
                if (!UsePatherFind || (continentNameMpq == "None"))
                {
                    source.Add(from);
                    source.Add(to);
                    return source;
                }
                if (_pather == null)
                {
                    _pather = new Pather(continentNameMpq);
                }
                if (_pather.Continent != continentNameMpq)
                {
                    _pather.Dispose();
                    _pather = new Pather(continentNameMpq);
                }
                if (addFromAndStart)
                {
                    source.Add(from);
                }
                if (loadAllTile)
                {
                    _pather.LoadAllTiles();
                }
                source = ShortPath ? _pather.FindPathSimple(from, to, out resultSuccess, true) : _pather.FindPath(from, to, out resultSuccess);
                if (addFromAndStart && resultSuccess)
                {
                    source.Add(to);
                }
                for (int i = 0; i <= (source.Count - 2); i++)
                {
                    if (((source[i].DistanceTo(source[i + 1]) < 0.5) || (source[i + 1].X == 0f)) || ((source[i + 1].Y == 0f) || (source[i + 1].Z == 0f)))
                    {
                        source.RemoveAt(i + 1);
                        i--;
                    }
                }
                for (int j = source.Count - 2; j > 0; j--)
                {
                    Point point = nManager.Helpful.Math.GetPositionOffsetBy3DDistance(source[j - 1], source[j], 1.9f);
                    source[j] = point;
                }
                Logging.WriteNavigator("Path Count: " + source.Count<Point>() + (resultSuccess ? "" : " but incomplete"));
                return source;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToRecast(this Point v): PATH FIND ERROR: " + exception, true);
                Console.WriteLine("Path find ERROR.");
                resultSuccess = false;
                source = new List<Point>();
                if (addFromAndStart)
                {
                    if ((from != null) && (((from.X != 0f) || (from.Y != 0f)) || (from.Z != 0f)))
                    {
                        source.Add(from);
                    }
                    if ((to != null) && (((to.X != 0f) || (to.Y != 0f)) || (to.Z != 0f)))
                    {
                        source.Add(to);
                    }
                }
                return source;
            }
        }

        public static List<Point> FindPathUnstuck(Point to)
        {
            try
            {
                bool flag;
                List<Point> list = FindPath(to, out flag, true, false);
                if (!flag && (list.Count <= 2))
                {
                    Thread.Sleep(100);
                    Logging.WriteNavigator(string.Concat(new object[] { "FindPathUnstuck : FindPath failed. From: ", nManager.Wow.ObjectManager.ObjectManager.Me.Position, " To: ", to }));
                    for (int i = 0; !flag && (i <= 2); i++)
                    {
                        MovementsAction.MoveForward(true, false);
                        Thread.Sleep(0x3e8);
                        MovementsAction.Ascend(true, false, false);
                        Thread.Sleep(100);
                        MovementsAction.Ascend(false, false, false);
                        list = FindPath(to, out flag, true, false);
                        Thread.Sleep(200);
                        MovementsAction.MoveForward(false, false);
                    }
                }
                return list;
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPathUnstuck(Point to): " + exception, true);
            }
            return new List<Point>();
        }

        public static Point GetClosestPointOnTile(Point position, out bool success)
        {
            if (_pather == null)
            {
                _pather = new Pather(Usefuls.ContinentNameMpq);
            }
            if (_pather.Continent != Usefuls.ContinentNameMpq)
            {
                _pather.Dispose();
                _pather = new Pather(Usefuls.ContinentNameMpq);
            }
            return _pather.GetClosestPointOnTile(position, out success);
        }

        public static float GetZPosition(Point point, bool strict = false)
        {
            try
            {
                if (_pather == null)
                {
                    _pather = new Pather(Usefuls.ContinentNameMpq);
                }
                if (_pather.Continent != Usefuls.ContinentNameMpq)
                {
                    _pather.Dispose();
                    _pather = new Pather(Usefuls.ContinentNameMpq);
                }
                return _pather.GetZ(point, strict);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetZPosition(Point point): " + exception, true);
            }
            return 0f;
        }

        public static float GetZPosition(float x, float y, bool strict = false)
        {
            return GetZPosition(new Point(x, y, 0f, "None"), strict);
        }

        public static float GetZPosition(float x, float y, float z, bool strict = false)
        {
            return GetZPosition(new Point(x, y, z, "None"), strict);
        }

        public static bool UsePatherFind
        {
            get
            {
                return nManagerSetting.CurrentSetting.ActivatePathFindingFeature;
            }
        }
    }
}

