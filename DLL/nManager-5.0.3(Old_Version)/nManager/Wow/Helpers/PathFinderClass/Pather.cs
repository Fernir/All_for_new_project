namespace nManager.Wow.Helpers.PathFinderClass
{
    using DetourLayer;
    using nManager.Helpful;
    using nManager.Wow.Class;
    using nManager.Wow.Helpers;
    using nManager.Wow.ObjectManager;
    using RecastLayer;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class Pather
    {
        private Dictionary<Tuple<int, int>, int> _loadedTiles;
        private nManager.Helpful.Timer _loadTileCheck;
        private readonly NavMesh _mesh;
        private readonly string _meshPath;
        private readonly NavMeshQuery _query;
        private static readonly List<string> blackListMaptitle = new List<string>();
        private const int Division = 2;
        public readonly bool IsDungeon;

        public Pather(string continent) : this(continent, new ConnectionHandlerDelegate(Pather.DefaultConnectionHandler))
        {
        }

        public Pather(string continent, ConnectionHandlerDelegate connectionHandler)
        {
            try
            {
                DetourStatus status;
                this.ConnectionHandler = connectionHandler;
                this.Continent = continent.Substring(continent.LastIndexOf('\\') + 1);
                string startupPath = Application.StartupPath;
                this._meshPath = startupPath + @"\Meshes";
                if (!Directory.Exists(this._meshPath))
                {
                    Logging.WriteNavigator(string.Concat(new object[] { (DetourStatus) (-2147483648), " No mesh for ", continent, " (Path: ", this._meshPath, ")" }));
                }
                this._mesh = new NavMesh();
                this._loadedTiles = new Dictionary<Tuple<int, int>, int>();
                if (this._loadTileCheck == null)
                {
                    this._loadTileCheck = new nManager.Helpful.Timer(60000.0);
                }
                if (WoWMap.FromMPQName(continent).Record.MapType == WoWMap.MapType.WDTOnlyType)
                {
                    string dungeonPath = this.GetDungeonPath();
                    if (!File.Exists(this._meshPath + @"\" + dungeonPath))
                    {
                        this.downloadTile(dungeonPath);
                    }
                    byte[] data = File.ReadAllBytes(this._meshPath + @"\" + dungeonPath);
                    status = this._mesh.Initialize(data);
                    this.AddMemoryPressure(data.Length);
                    this.IsDungeon = true;
                }
                else
                {
                    status = this._mesh.Initialize(0x249f0, 0x800, Utility.Origin, Utility.TileSize / 2f, Utility.TileSize / 2f);
                }
                if (status.HasFailed())
                {
                    Logging.WriteNavigator(status + " Failed to initialize the mesh");
                }
                this._query = new NavMeshQuery(new PatherCallback(this));
                this._query.Initialize(this._mesh, 0x10000);
                QueryFilter filter = new QueryFilter {
                    IncludeFlags = 0xffff,
                    ExcludeFlags = 0
                };
                this.Filter = filter;
                this.Filter.SetAreaCost(2, 4f);
                this.Filter.SetAreaCost(1, 1f);
                this.Filter.SetAreaCost(3, 1f);
                this.Filter.SetAreaCost(4, 20f);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pather(string continent, ConnectionHandlerDelegate connectionHandler): " + exception, true);
            }
        }

        private void AddMemoryPressure(int bytes)
        {
            try
            {
                GC.AddMemoryPressure((long) bytes);
                this.MemoryPressure += bytes;
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddMemoryPressure(int bytes): " + exception, true);
            }
        }

        private bool CheckDungeon()
        {
            try
            {
                if (this.IsDungeon)
                {
                    Logging.WriteError("Dungeon mesh doesn't support tiles", true);
                }
                return this.IsDungeon;
            }
            catch (Exception exception)
            {
                Logging.WriteError("CheckDungeon(): " + exception, true);
                return false;
            }
        }

        private void checkTilesAgeAndUnload()
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            foreach (KeyValuePair<Tuple<int, int>, int> pair in this._loadedTiles)
            {
                if (pair.Value < (Others.TimesSec - 900))
                {
                    this.RemoveTile(pair.Key.Item1, pair.Key.Item2);
                    Logging.WriteNavigator("Unloading old tile (" + this.GetTileName(pair.Key.Item1, pair.Key.Item2, true) + ")");
                    list.Add(pair.Key);
                }
            }
            foreach (Tuple<int, int> tuple in list)
            {
                this._loadedTiles.Remove(tuple);
            }
        }

        private static bool DefaultConnectionHandler(ConnectionData data)
        {
            try
            {
                return false;
            }
            catch (Exception exception)
            {
                Logging.WriteError("DefaultConnectionHandler(ConnectionData data): " + exception, true);
                return false;
            }
        }

        private static void DisableConnection(MeshTile tile, int index)
        {
            try
            {
                Poly polygon = tile.GetPolygon((ushort) (index + tile.Header.OffMeshBase));
                if (polygon != null)
                {
                    polygon.Disable();
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DisableConnection(MeshTile tile, int index): " + exception, true);
            }
        }

        public void Dispose()
        {
            try
            {
                this._query.Dispose();
            }
            catch (Exception)
            {
            }
            try
            {
                GC.RemoveMemoryPressure((long) this.MemoryPressure);
                this.MemoryPressure = 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pather > Dispose(): " + exception, true);
            }
        }

        private bool downloadTile(string fileName)
        {
            if (blackListMaptitle.Contains(fileName))
            {
                return true;
            }
            blackListMaptitle.Add(fileName);
            return this.forceDownloadTile(fileName);
        }

        public List<Point> FindLocalPath(Point startVec, Point endVec)
        {
            try
            {
                bool flag;
                return this.FindPathSimple(startVec, endVec, out flag, true);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindLocalpath(Point startVec, Point endVec): " + exception, true);
                return new List<Point>();
            }
        }

        public List<Point> FindPath(Point startVec, Point endVec)
        {
            try
            {
                bool flag;
                return this.FindPath(startVec, endVec, out flag);
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point startVec, Point endVec): " + exception, true);
                return new List<Point>();
            }
        }

        public List<Point> FindPath(Point startVec, Point endVec, out bool resultSuccess)
        {
            List<Point> list = this.FindPathSimple(startVec, endVec, out resultSuccess, false);
            if (list.Count < 2)
            {
                resultSuccess = false;
                return new List<Point>();
            }
            if ((endVec - startVec).Magnitude >= 3000f)
            {
                float magnitude = (endVec - list[list.Count - 1]).Magnitude;
                float num2 = 5f;
                while (num2 < magnitude)
                {
                    int num3 = (int) (list.Count * 0.8f);
                    List<Point> list2 = this.FindPathSimple(list[num3], endVec, out resultSuccess, false);
                    num2 = (endVec - list2[list2.Count - 1]).Magnitude;
                    if (num2 < magnitude)
                    {
                        for (int i = list.Count - 1; i > num3; i--)
                        {
                            list.RemoveAt(i);
                        }
                        foreach (Point point in list2)
                        {
                            list.Add(point);
                        }
                        magnitude = num2;
                        num2 = 5f;
                    }
                }
            }
            return list;
        }

        public List<Point> FindPathSimple(Point startVec, Point endVec, out bool resultSuccess, bool ShortPath = false)
        {
            try
            {
                ulong[] numArray4;
                DetourStatus status;
                float[] numArray5;
                StraightPathFlag[] flagArray;
                ulong[] numArray6;
                resultSuccess = true;
                float[] extents = new Point(4.5f, 200f, 4.5f, "None").ToFloatArray();
                float[] center = startVec.ToRecast().ToFloatArray();
                float[] numArray3 = endVec.ToRecast().ToFloatArray();
                if (!this.IsDungeon)
                {
                    this.LoadAround(startVec);
                    this.LoadAround(endVec);
                }
                ulong startRef = this._query.FindNearestPolygon(center, extents, this.Filter);
                if (startRef == 0L)
                {
                    Logging.WriteNavigator(string.Concat(new object[] { (DetourStatus) (-2147483648), " No polyref found for start (", startVec, ")" }));
                }
                ulong endRef = this._query.FindNearestPolygon(numArray3, extents, this.Filter);
                if (endRef == 0L)
                {
                    Logging.WriteNavigator(string.Concat(new object[] { (DetourStatus) (-2147483648), " No polyref found for end (", endVec, ")" }));
                }
                if ((startRef == 0L) || (endRef == 0L))
                {
                    return new List<Point>();
                }
                if (ShortPath)
                {
                    status = this._query.FindLocalPath(startRef, endRef, center, numArray3, this.Filter, out numArray4);
                }
                else
                {
                    status = this._query.FindPath(startRef, endRef, center, numArray3, this.Filter, out numArray4);
                }
                if (status.HasFailed() || (numArray4 == null))
                {
                    Logging.WriteNavigator(string.Concat(new object[] { status, " FindPath failed, start: ", startRef, " end: ", endRef }));
                    return new List<Point>();
                }
                if (status.HasFlag(DetourStatus.PartialResult))
                {
                    Logging.WriteNavigator("Warning, partial result: " + status);
                    resultSuccess = false;
                }
                status = this._query.FindStraightPath(center, numArray3, numArray4, out numArray5, out flagArray, out numArray6);
                if ((status.HasFailed() || (numArray5 == null)) || ((flagArray == null) || (numArray6 == null)))
                {
                    Logging.WriteNavigator(status + "FindStraightPath failed, refs in corridor: " + numArray4.Length);
                }
                if (numArray5 != null)
                {
                    List<Point> list = new List<Point>(numArray5.Length / 3);
                    for (int i = 0; i < (numArray5.Length / 3); i++)
                    {
                        list.Add(new Point(numArray5[i * 3], numArray5[(i * 3) + 1], numArray5[(i * 3) + 2], "None").ToWoW());
                    }
                    return list;
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("FindPath(Point startVec, Point endVec, out bool resultSuccess): " + exception, true);
                resultSuccess = false;
            }
            return new List<Point>();
        }

        private bool forceDownloadTile(string fileName)
        {
            try
            {
                string str = "http://meshes.thenoobbot.com/" + Utility.GetDetourSupportedVersion() + "/";
                string[] strArray = fileName.Split(new char[] { '\\' });
                Directory.CreateDirectory(this._meshPath + @"\" + strArray[0] + @"\");
                if (!Others.ExistFile(this._meshPath + @"\" + fileName))
                {
                    Logging.Write("Downloading \"" + fileName + "\"...");
                    if (Others.DownloadFile(str + fileName.Replace(@"\", "/") + ".gz", this._meshPath + @"\" + fileName + ".gz"))
                    {
                        if (!GZip.Decompress(this._meshPath + @"\" + fileName + ".gz"))
                        {
                            return false;
                        }
                        if (Others.ExistFile(this._meshPath + @"\" + fileName + ".gz"))
                        {
                            File.Delete(this._meshPath + @"\" + fileName + ".gz");
                        }
                        if (Others.ExistFile(this._meshPath + @"\" + fileName))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("forceDownloadTile(string fileName): " + exception, true);
                return false;
            }
        }

        public Point GetClosestPointOnTile(Point position, out bool success)
        {
            float num;
            float num2;
            float[] numArray3;
            float[] extents = new Point(20f, 2000f, 20f, "None").ToFloatArray();
            float[] center = position.ToRecast().ToFloatArray();
            this.GetTileByLocation(position, out num, out num2);
            int x = (int) System.Math.Floor((double) num);
            int y = (int) System.Math.Floor((double) num2);
            this.LoadTile(x, y);
            ulong polyRef = this._query.FindNearestPolygon(center, extents, this.Filter);
            if (polyRef == 0L)
            {
                success = false;
                return new Point();
            }
            if (this._query.closestPointOnPolyBoundary(polyRef, center, out numArray3).HasFailed())
            {
                success = false;
                return new Point();
            }
            success = true;
            return new Point(numArray3.ToWoW());
        }

        private string GetDungeonPath()
        {
            try
            {
                return (@"Dungeons\" + this.Continent + ".dmesh");
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDungeonPath(): " + exception, true);
                return "";
            }
        }

        public void GetTileByLocation(Point loc, out float x, out float y)
        {
            try
            {
                float num;
                float num2;
                this.CheckDungeon();
                GetTileByLocation(loc.ToRecast().ToFloatArray(), out num, out num2);
                x = num;
                y = num2;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetTileByLocation(Point loc, out int x, out int y): " + exception, true);
                x = 0f;
                y = 0f;
            }
        }

        public static void GetTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Utility.Origin[0]) / (Utility.TileSize / 2f);
            y = (loc[2] - Utility.Origin[2]) / (Utility.TileSize / 2f);
        }

        public string GetTileName(int x, int y, bool onlyName = false)
        {
            try
            {
                string continent = this.Continent;
                int num = x / 2;
                int num2 = y / 2;
                if (this.Continent == "Draenor")
                {
                    if (((num == 0x17) && (num2 == 0x15)) && (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"))
                    {
                        switch (Garrison.GetGarrisonLevel())
                        {
                            case 1:
                                continent = "FWHordeGarrisonLevel1";
                                break;

                            case 2:
                                continent = "FWHordeGarrisonLeve2new";
                                break;

                            case 3:
                                continent = "FWHordeGarrisonLevel2";
                                break;
                        }
                    }
                    else if (((num == 0x1f) && (num2 == 0x1c)) && (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() != "horde"))
                    {
                        switch (Garrison.GetGarrisonLevel())
                        {
                            case 1:
                                continent = "SMVAllianceGarrisonLevel1";
                                break;

                            case 2:
                                continent = "SMVAllianceGarrisonLevel2new";
                                break;

                            case 3:
                                continent = "SMVAllianceGarrisonLevel2";
                                break;
                        }
                    }
                }
                float num3 = (x * (Utility.TileSize / 2f)) - (num * Utility.TileSize);
                float num4 = (y * (Utility.TileSize / 2f)) - (num2 * Utility.TileSize);
                int num5 = (int) System.Math.Round((double) (num3 / (Utility.TileSize / 2f)));
                int num6 = (int) System.Math.Round((double) (num4 / (Utility.TileSize / 2f)));
                return string.Concat(new object[] { onlyName ? "" : (continent + @"\"), continent, "_", num, "_", num2, "_", num5, num6, ".tile" });
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetTileName(int x, int y, bool onlyName): " + exception, true);
                return "";
            }
        }

        public string GetTilePath(int x, int y)
        {
            try
            {
                return (this._meshPath + @"\" + this.GetTileName(x, y, false));
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetTilePath(int x, int y): " + exception, true);
                return "";
            }
        }

        public static void GetWoWTileByLocation(float[] loc, out float x, out float y)
        {
            x = (loc[0] - Utility.Origin[0]) / Utility.TileSize;
            y = (loc[2] - Utility.Origin[2]) / Utility.TileSize;
        }

        public float GetZ(Point position, bool strict = false)
        {
            float num;
            float num2;
            float[] extents = strict ? new Point(0.5f, 2000f, 0.5f, "None").ToFloatArray() : new Point(1.5f, 2000f, 1.5f, "None").ToFloatArray();
            float[] center = position.ToRecast().ToFloatArray();
            this.GetTileByLocation(position, out num, out num2);
            int x = (int) System.Math.Floor((double) num);
            int y = (int) System.Math.Floor((double) num2);
            this.LoadTile(x, y);
            ulong polyRef = this._query.FindNearestPolygon(center, extents, this.Filter);
            if (polyRef == 0L)
            {
                Logging.WriteDebug(string.Concat(new object[] { "There is no polygon in this location (Tile ", x, ",", y, "), coord: X:", position.X, ", Y:", position.Y }));
                return 0f;
            }
            float polyHeight = this._query.GetPolyHeight(polyRef, center);
            if ((polyHeight == 0f) && !strict)
            {
                float[] numArray3;
                polyHeight = this._query.closestPointOnPolyBoundary(polyRef, center, out numArray3).HasFailed() ? 0f : numArray3[1];
            }
            return polyHeight;
        }

        private static void HandleLog(string text)
        {
            try
            {
                Logging.WriteNavigator(text);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HandleLog(string text): " + exception, true);
            }
        }

        private void HandlePathfinderUpdate(float[] best)
        {
            try
            {
                if (!this.IsDungeon)
                {
                    float[] numArray = best.ToWoW();
                    this.LoadAround(new Point(numArray[0], numArray[1], numArray[2], "None"));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("HandlePathfinderUpdate(float[] best): " + exception, true);
            }
        }

        public void LoadAllTiles()
        {
            try
            {
                for (int i = 0; i < 0x80; i++)
                {
                    for (int j = 0; j < 0x80; j++)
                    {
                        this.downloadTile(this.GetTileName(j, i, false));
                        if (File.Exists(this.GetTilePath(j, i)))
                        {
                            this.LoadTile(j, i);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadAllTiles(): " + exception, true);
            }
        }

        public void LoadAround(Point loc)
        {
            try
            {
                if (!this.CheckDungeon())
                {
                    float num;
                    float num2;
                    this.GetTileByLocation(loc, out num, out num2);
                    int num3 = (int) System.Math.Floor((double) num);
                    int num4 = (int) System.Math.Floor((double) num2);
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            this.LoadTile(num3 + i, num4 + j);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadAround(Point loc): " + exception, true);
            }
        }

        private void LoadDynamic(int x, int y)
        {
            try
            {
                if (!this._mesh.HasTileAt(x, y) && this.LoadTile(x, y))
                {
                    Logging.WriteNavigator(string.Concat(new object[] { "Load dynamically: ", x, " ", y }));
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadDynamic(int x, int y): " + exception, true);
            }
        }

        public bool LoadTile(byte[] data)
        {
            try
            {
                MeshTile tile;
                if (this.CheckDungeon())
                {
                    return false;
                }
                DetourStatus status = this._mesh.AddTile(data, out tile);
                if (status.IsWrongVersion())
                {
                    Logging.WriteNavigator("This mesh tile is outdated.");
                    return false;
                }
                if (status.HasFailed())
                {
                    Logging.WriteNavigator("This mesh tile is corrupted.");
                    return false;
                }
                this.AddMemoryPressure(data.Length);
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadTile(byte[] data): " + exception, true);
                return false;
            }
        }

        public bool LoadTile(int x, int y)
        {
            try
            {
                if (this.CheckDungeon())
                {
                    return false;
                }
                if (this._loadTileCheck.IsReady)
                {
                    this._loadTileCheck.Reset();
                    this.checkTilesAgeAndUnload();
                }
                Tuple<int, int> key = new Tuple<int, int>(x, y);
                if (this._mesh.HasTileAt(x, y))
                {
                    this._loadedTiles[key] = Others.TimesSec;
                    return true;
                }
                string tilePath = this.GetTilePath(x, y);
                string fileName = this.GetTileName(x, y, false);
                if (!this.downloadTile(fileName))
                {
                    return false;
                }
                if (!File.Exists(tilePath))
                {
                    return false;
                }
                byte[] data = File.ReadAllBytes(tilePath);
                Logging.WriteNavigator(this.GetTileName(x, y, true) + " loaded.");
                if (!this.LoadTile(data))
                {
                    Others.DeleteFile(this._meshPath + @"\" + fileName);
                    if (!this.forceDownloadTile(fileName))
                    {
                        return false;
                    }
                    data = File.ReadAllBytes(tilePath);
                    if (!this.LoadTile(data))
                    {
                        Logging.WriteError("Problem with Meshes tile " + fileName + " , cannot load it.", true);
                        return false;
                    }
                }
                this._loadedTiles.Add(key, Others.TimesSec);
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("LoadTile(int x, int y): " + exception, true);
                return false;
            }
        }

        public bool RemoveTile(int x, int y)
        {
            try
            {
                return this._mesh.RemoveTileAt(x, y).HasSucceeded();
            }
            catch (Exception exception)
            {
                Logging.WriteError("RemoveTile(int x, int y): " + exception, true);
                return false;
            }
        }

        public bool RemoveTile(int x, int y, out byte[] tileData)
        {
            try
            {
                return this._mesh.RemoveTileAt(x, y, out tileData).HasSucceeded();
            }
            catch (Exception exception)
            {
                Logging.WriteError("RemoveTile(int x, int y, out byte[] tileData): " + exception, true);
                tileData = new byte[0];
                return false;
            }
        }

        public int ReportDanger(IEnumerable<Danger> dangers)
        {
            var selector = null;
            try
            {
                float[] extents = new float[] { 2.5f, 2.5f, 2.5f };
                if (selector == null)
                {
                    selector = <>h__TransparentIdentifier1 => this.Query.MarkAreaInCircle(<>h__TransparentIdentifier1.polyRef, <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.loc, <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.danger.Radius, this.Filter, PolyArea.Danger);
                }
                return (from danger in dangers
                    let loc = danger.Location.ToRecast().ToFloatArray()
                    let polyRef = this.Query.FindNearestPolygon(loc, extents, this.Filter)
                    where polyRef != 0L
                    select <>h__TransparentIdentifier1).Select(selector).Sum();
            }
            catch (Exception exception)
            {
                Logging.WriteError("ReportDanger(IEnumerable<Danger> dangers): " + exception, true);
                return 0;
            }
        }

        public ConnectionHandlerDelegate ConnectionHandler { get; set; }

        public string Continent { get; private set; }

        public QueryFilter Filter { get; private set; }

        public int MemoryPressure { get; private set; }

        public NavMesh Mesh
        {
            get
            {
                return this._mesh;
            }
        }

        public NavMeshQuery Query
        {
            get
            {
                return this._query;
            }
        }

        public delegate bool ConnectionHandlerDelegate(ConnectionData data);

        private class PatherCallback : NavMeshQueryCallback
        {
            private readonly Pather _parent;

            public PatherCallback(Pather parent)
            {
                try
                {
                    this._parent = parent;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PatherCallback(Pather parent): " + exception, true);
                }
            }

            public void Log(string text)
            {
                try
                {
                    Pather.HandleLog(text);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Log(string text): " + exception, true);
                }
            }

            public void PathfinderUpdate(float[] best)
            {
                try
                {
                    this._parent.HandlePathfinderUpdate(best);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PathfinderUpdate(float[] best): " + exception, true);
                }
            }
        }
    }
}

