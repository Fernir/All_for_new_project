namespace nManager.Wow.Helpers
{
    using nManager;
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Enums;
    using nManager.Wow.Helpers.PathFinderClass;
    using nManager.Wow.MemoryClass;
    using nManager.Wow.ObjectManager;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;

    public class PathFinder
    {
        private static Gaili _uritucipioxoxAtuano;

        public static List<Point> FindLocalPath(Point to, out bool resultSuccess, bool addFromAndStart = true)
        {
            return FindPath(to, out resultSuccess, addFromAndStart, true);
        }

        public static List<Point> FindPath(Point to)
        {
            try
            {
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
                return FindPath(from, to, nManager.Wow.Helpers.Usefuls.ContinentNameMpq);
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
                return FindPath(nManager.Wow.ObjectManager.ObjectManager.Me.Position, to, nManager.Wow.Helpers.Usefuls.ContinentNameMpq, out resultSuccess, addFromAndStart, false, ShortPath);
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
            if (!from.IsValid || !to.IsValid)
            {
                Logging.WriteError("public static List<Point> FindPath(Point from, Point to, string continentNameMpq, out bool resultSuccess, bool addFromAndStart = true, bool loadAllTile = false, bool ShortPath = false): Attempting to create a path from/to invalid position. Callstack: " + Hook.CurrentCallStack, true);
                resultSuccess = false;
                return new List<Point>();
            }
            List<Point> list = Ovucuso(from, to, continentNameMpq, out resultSuccess, addFromAndStart, loadAllTile, ShortPath, true);
            if (!resultSuccess)
            {
                if ((list.Count <= 300) || (from.DistanceTo(to) <= 8000f))
                {
                    return list;
                }
                Point ogepoluke = list[list.Count - 2];
                List<Point> collection = Ovucuso(ogepoluke, to, continentNameMpq, out resultSuccess, addFromAndStart, loadAllTile, ShortPath, true);
                if (resultSuccess)
                {
                    list.RemoveAt(list.Count - 1);
                    list.RemoveAt(list.Count - 1);
                    list.RemoveAt(list.Count - 1);
                    list.AddRange(collection);
                    Logging.WriteDebug("PathFinder magic was done here... #1");
                    return list;
                }
            }
            return list;
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
            if (_uritucipioxoxAtuano == null)
            {
                _uritucipioxoxAtuano = new Gaili(nManager.Wow.Helpers.Usefuls.ContinentNameMpq);
            }
            if (_uritucipioxoxAtuano.get_Continent() != nManager.Wow.Helpers.Usefuls.ContinentNameMpq)
            {
                _uritucipioxoxAtuano.XudeseujekeogId();
                _uritucipioxoxAtuano = new Gaili(nManager.Wow.Helpers.Usefuls.ContinentNameMpq);
            }
            return _uritucipioxoxAtuano.XawauTeidiXuih(position, out success);
        }

        public static float GetZPosition(Point point, bool strict = false)
        {
            try
            {
                if (_uritucipioxoxAtuano == null)
                {
                    _uritucipioxoxAtuano = new Gaili(nManager.Wow.Helpers.Usefuls.ContinentNameMpq);
                }
                if (_uritucipioxoxAtuano.get_Continent() != nManager.Wow.Helpers.Usefuls.ContinentNameMpq)
                {
                    _uritucipioxoxAtuano.XudeseujekeogId();
                    _uritucipioxoxAtuano = new Gaili(nManager.Wow.Helpers.Usefuls.ContinentNameMpq);
                }
                return _uritucipioxoxAtuano.WipiaGitolief(point, strict);
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

        private static List<Point> Ovucuso(Point ogepoluke, Point siqoubouEdoaj, string togouguviFaotie, out bool guidajaxiu, bool coalukupe, bool emiumiulRuab, bool arasanietPeosoi, bool oroapid = false)
        {
            if (!oroapid)
            {
                Logging.WriteError("FindPath: Error, accessed from unknown location.", true);
            }
            if (ogepoluke.DistanceTo(siqoubouEdoaj) <= 1f)
            {
                guidajaxiu = true;
                return new List<Point> { ogepoluke, siqoubouEdoaj };
            }
            if (ogepoluke.Type.ToLower() == "swimming")
            {
                if (TraceLine.TraceLineGo(new Point(siqoubouEdoaj.X, siqoubouEdoaj.Y, siqoubouEdoaj.Z + 1000f, "None"), siqoubouEdoaj, CGWorldFrameHitFlags.HitTestLiquid))
                {
                    if (!TraceLine.TraceLineGo(ogepoluke, siqoubouEdoaj, CGWorldFrameHitFlags.HitTestAll))
                    {
                        Logging.WriteNavigator("Swimmming right to the destination");
                        guidajaxiu = true;
                        return new List<Point> { ogepoluke, siqoubouEdoaj };
                    }
                    Logging.WriteNavigator("Swimming to the destination using the PathFinder");
                }
                else
                {
                    ogepoluke.Z = GetZPosition(ogepoluke, false);
                    Logging.WriteNavigator("Using the PathFinder to destination out of water");
                }
            }
            if (ogepoluke.Type.ToLower() == "flying")
            {
                ogepoluke.Z = GetZPosition(ogepoluke, false);
                Logging.WriteNavigator("Using the PathFinder while flying");
            }
            List<Point> source = new List<Point>();
            guidajaxiu = true;
            try
            {
                bool flag;
                if (!UsePatherFind || (togouguviFaotie == "None"))
                {
                    source.Add(ogepoluke);
                    source.Add(siqoubouEdoaj);
                    return source;
                }
                if (_uritucipioxoxAtuano == null)
                {
                    _uritucipioxoxAtuano = new Gaili(togouguviFaotie);
                }
                if (_uritucipioxoxAtuano.get_Continent() != togouguviFaotie)
                {
                    _uritucipioxoxAtuano.XudeseujekeogId();
                    _uritucipioxoxAtuano = new Gaili(togouguviFaotie);
                }
                if (coalukupe)
                {
                    source.Add(ogepoluke);
                }
                if (emiumiulRuab)
                {
                    _uritucipioxoxAtuano.Isueqehu();
                }
                source = arasanietPeosoi ? _uritucipioxoxAtuano.Asuagunebiohe(ogepoluke, siqoubouEdoaj, out guidajaxiu, out flag, true) : _uritucipioxoxAtuano.Ovucuso(ogepoluke, siqoubouEdoaj, out guidajaxiu, out flag);
                if (coalukupe && guidajaxiu)
                {
                    source.Add(siqoubouEdoaj);
                }
                if (!guidajaxiu && flag)
                {
                    Logging.WriteDebug("Reloading PathFinder...");
                    _uritucipioxoxAtuano.XudeseujekeogId();
                    _uritucipioxoxAtuano = new Gaili(togouguviFaotie);
                    source = arasanietPeosoi ? _uritucipioxoxAtuano.Asuagunebiohe(ogepoluke, siqoubouEdoaj, out guidajaxiu, out flag, true) : _uritucipioxoxAtuano.Ovucuso(ogepoluke, siqoubouEdoaj, out guidajaxiu, out flag);
                    if (coalukupe && guidajaxiu)
                    {
                        source.Add(siqoubouEdoaj);
                    }
                }
                for (int i = 0; i <= (source.Count - 2); i++)
                {
                    if ((source[i].DistanceTo(source[i + 1]) < 0.5) || !source[i + 1].IsValid)
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
                Logging.WriteNavigator("Path Count: " + source.Count<Point>() + (guidajaxiu ? "" : " but incomplete"));
                return source;
            }
            catch (Exception exception)
            {
                Logging.WriteError("ToRecast(this Point v): PATH FIND ERROR: " + exception, true);
                Console.WriteLine("Path find ERROR.");
                guidajaxiu = false;
                source = new List<Point>();
                if (coalukupe)
                {
                    if ((ogepoluke != null) && (((ogepoluke.X != 0f) || (ogepoluke.Y != 0f)) || (ogepoluke.Z != 0f)))
                    {
                        source.Add(ogepoluke);
                    }
                    if ((siqoubouEdoaj != null) && (((siqoubouEdoaj.X != 0f) || (siqoubouEdoaj.Y != 0f)) || (siqoubouEdoaj.Z != 0f)))
                    {
                        source.Add(siqoubouEdoaj);
                    }
                }
                return source;
            }
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

