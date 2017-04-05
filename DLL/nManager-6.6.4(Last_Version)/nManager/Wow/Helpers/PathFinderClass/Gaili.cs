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

    internal class Gaili
    {
        private static object _awuigeowoUwuen = new object();
        private nManager.Helpful.Timer _erahedafiGoqaiIh;
        private readonly List<string> _loadedTilesString;
        private readonly NavMesh _mesh;
        private readonly string _meshPath;
        private const int _ofuxuapih = 2;
        private Dictionary<Tuple<int, int>, int> _ovabiaw;
        private Dictionary<Tuple<int, int>, int> _qeacuUteruexo;
        private readonly NavMeshQuery _query;
        private static readonly List<string> blackListMaptitle = new List<string>();
        public readonly bool IsDungeon;

        public Gaili(string continent) : this(continent, new ConnectionHandlerDelegate(Gaili.TureugaogokiceErobeumio))
        {
        }

        public Gaili(string continent, ConnectionHandlerDelegate connectionHandler)
        {
            this._loadedTilesString = new List<string>();
            lock (_awuigeowoUwuen)
            {
                try
                {
                    DetourStatus status;
                    this.set_ConnectionHandler(connectionHandler);
                    this.set_Continent(continent.Substring(continent.LastIndexOf('\\') + 1));
                    string startupPath = Application.StartupPath;
                    this._meshPath = startupPath + @"\Meshes";
                    if (!Directory.Exists(this._meshPath))
                    {
                        Logging.WriteNavigator(string.Concat(new object[] { (DetourStatus) (-2147483648), " No mesh for ", continent, " (Path: ", this._meshPath, ")" }));
                    }
                    this._mesh = new NavMesh();
                    this._qeacuUteruexo = new Dictionary<Tuple<int, int>, int>();
                    this._ovabiaw = new Dictionary<Tuple<int, int>, int>();
                    if (this._erahedafiGoqaiIh == null)
                    {
                        this._erahedafiGoqaiIh = new nManager.Helpful.Timer(60000.0);
                    }
                    if ((WoWMap.FromMPQName(continent).Record.MapType == WoWMap.MapType.WDTOnlyType) || (continent == "AllianceGunship"))
                    {
                        string toeraKetaheo = this.Iciaqouho();
                        if (!File.Exists(this._meshPath + @"\" + toeraKetaheo))
                        {
                            this.UjemiaqeUqari(toeraKetaheo);
                        }
                        byte[] data = File.ReadAllBytes(this._meshPath + @"\" + toeraKetaheo);
                        status = this._mesh.Initialize(data);
                        this.Unaikihe(data.Length);
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
                    this._query = new NavMeshQuery(new Ebejihiqo(this));
                    this._query.Initialize(this._mesh, 0xffff0);
                    QueryFilter filter = new QueryFilter {
                        IncludeFlags = 0xffff,
                        ExcludeFlags = 0
                    };
                    this.set_Filter(filter);
                    this.get_Filter().SetAreaCost(2, 4f);
                    this.get_Filter().SetAreaCost(1, 1f);
                    this.get_Filter().SetAreaCost(3, 1f);
                    this.get_Filter().SetAreaCost(4, 20f);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("Pather(string continent, ConnectionHandlerDelegate connectionHandler): " + exception, true);
                }
            }
        }

        [CompilerGenerated]
        private static <>f__AnonymousType3<UraiceixaororSioSix, float[]> <ReportDanger>b__3(UraiceixaororSioSix danger)
        {
            return new { danger = danger, loc = danger.get_Location().ToRecast().ToFloatArray() };
        }

        [CompilerGenerated]
        private static bool <ReportDanger>b__5(<>f__AnonymousType4<<>f__AnonymousType3<UraiceixaororSioSix, float[]>, ulong> <>h__TransparentIdentifier1)
        {
            return (<>h__TransparentIdentifier1.polyRef != 0L);
        }

        [CompilerGenerated]
        private int <ReportDanger>b__6(<>f__AnonymousType4<<>f__AnonymousType3<UraiceixaororSioSix, float[]>, ulong> <>h__TransparentIdentifier1)
        {
            return this.AciegoHaug().MarkAreaInCircle(<>h__TransparentIdentifier1.polyRef, <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.loc, <>h__TransparentIdentifier1.<>h__TransparentIdentifier0.danger.get_Radius(), this.get_Filter(), PolyArea.Danger);
        }

        public int AcugunuotoHogupigo(IEnumerable<UraiceixaororSioSix> uduovaitixauUmopua)
        {
            var selector = null;
            int num;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    <>c__DisplayClassa classa;
                    float[] extents = new float[] { 2.5f, 2.5f, 2.5f };
                    if (CS$<>9__CachedAnonymousMethodDelegate7 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate7 = new Func<UraiceixaororSioSix, <>f__AnonymousType3<UraiceixaororSioSix, float[]>>(Gaili.<ReportDanger>b__3);
                    }
                    if (CS$<>9__CachedAnonymousMethodDelegate8 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate8 = new Func<<>f__AnonymousType4<<>f__AnonymousType3<UraiceixaororSioSix, float[]>, ulong>, bool>(Gaili.<ReportDanger>b__5);
                    }
                    if (selector == null)
                    {
                        selector = new Func<<>f__AnonymousType4<<>f__AnonymousType3<UraiceixaororSioSix, float[]>, ulong>, int>(this.<ReportDanger>b__6);
                    }
                    num = uduovaitixauUmopua.Select(CS$<>9__CachedAnonymousMethodDelegate7).Select(new Func<<>f__AnonymousType3<UraiceixaororSioSix, float[]>, <>f__AnonymousType4<<>f__AnonymousType3<UraiceixaororSioSix, float[]>, ulong>>(classa.<ReportDanger>b__4)).Where(CS$<>9__CachedAnonymousMethodDelegate8).Select(selector).Sum();
                }
                catch (Exception exception)
                {
                    Logging.WriteError("ReportDanger(IEnumerable<Danger> dangers): " + exception, true);
                    num = 0;
                }
            }
            return num;
        }

        public List<Point> Asuagunebiohe(Point cuevejeijoi, Point ahaipeacDetoiniew, out bool guidajaxiu, out bool viloemei, bool arasanietPeosoi = false)
        {
            lock (_awuigeowoUwuen)
            {
                try
                {
                    ulong[] numArray4;
                    DetourStatus status;
                    float[] numArray5;
                    StraightPathFlag[] flagArray;
                    ulong[] numArray6;
                    viloemei = false;
                    guidajaxiu = true;
                    float[] extents = new Point(4.5f, 200f, 4.5f, "None").ToFloatArray();
                    float[] center = cuevejeijoi.ToRecast().ToFloatArray();
                    float[] numArray3 = ahaipeacDetoiniew.ToRecast().ToFloatArray();
                    if (!this.IsDungeon)
                    {
                        this.Qeinaewidea(cuevejeijoi);
                        this.Qeinaewidea(ahaipeacDetoiniew);
                    }
                    ulong startRef = this._query.FindNearestPolygon(center, extents, this.get_Filter());
                    if (startRef == 0L)
                    {
                        viloemei = true;
                        Logging.WriteNavigator(string.Concat(new object[] { (DetourStatus) (-2147483648), " No polyref found for start (", cuevejeijoi, ")" }));
                    }
                    ulong endRef = this._query.FindNearestPolygon(numArray3, extents, this.get_Filter());
                    if (endRef == 0L)
                    {
                        viloemei = true;
                        Logging.WriteNavigator(string.Concat(new object[] { (DetourStatus) (-2147483648), " No polyref found for end (", ahaipeacDetoiniew, ")" }));
                    }
                    if ((startRef == 0L) || (endRef == 0L))
                    {
                        return new List<Point>();
                    }
                    if (arasanietPeosoi)
                    {
                        status = this._query.FindLocalPath(startRef, endRef, center, numArray3, this.get_Filter(), out numArray4);
                    }
                    else
                    {
                        status = this._query.FindPath(startRef, endRef, center, numArray3, this.get_Filter(), out numArray4);
                    }
                    if (status.HasFailed() || (numArray4 == null))
                    {
                        Logging.WriteNavigator(string.Concat(new object[] { status, " FindPath failed, start: ", startRef, " end: ", endRef }));
                        return new List<Point>();
                    }
                    if (status.HasFlag(DetourStatus.PartialResult))
                    {
                        Logging.WriteNavigator("Warning, partial result: " + status);
                        guidajaxiu = false;
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
                    guidajaxiu = false;
                    viloemei = false;
                }
                return new List<Point>();
            }
        }

        private bool DejixoOgea()
        {
            bool isDungeon;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    if (this.IsDungeon)
                    {
                        Logging.WriteError("Dungeon mesh doesn't support tiles", true);
                    }
                    isDungeon = this.IsDungeon;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("CheckDungeon(): " + exception, true);
                    isDungeon = false;
                }
            }
            return isDungeon;
        }

        private static void Eliowemoburius(MeshTile diviamukoadiLadeu, int nogiuracoicaoAfae)
        {
            lock (_awuigeowoUwuen)
            {
                try
                {
                    Poly polygon = diviamukoadiLadeu.GetPolygon((ushort) (nogiuracoicaoAfae + diviamukoadiLadeu.Header.OffMeshBase));
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
        }

        private void Enakau(float[] mopaesutEhi)
        {
            lock (_awuigeowoUwuen)
            {
                try
                {
                    if (!this.IsDungeon)
                    {
                        float[] numArray = mopaesutEhi.ToWoW();
                        this.Qeinaewidea(new Point(numArray[0], numArray[1], numArray[2], "None"));
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("HandlePathfinderUpdate(float[] best): " + exception, true);
                }
            }
        }

        private string Iciaqouho()
        {
            try
            {
                return (@"Dungeons\" + this.get_Continent() + ".dmesh");
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetDungeonPath(): " + exception, true);
                return "";
            }
        }

        public string IpuenepuhibuxeQe(int duiwiqoOtuem, int raowiofieh)
        {
            string str;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    str = this._meshPath + @"\" + this.IsivaikauqabaiGuixeroqe(duiwiqoOtuem, raowiofieh, false);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetTilePath(int x, int y): " + exception, true);
                    str = "";
                }
            }
            return str;
        }

        public string IsivaikauqabaiGuixeroqe(int duiwiqoOtuem, int raowiofieh, bool edegeu = false)
        {
            string str2;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    string str = this.get_Continent();
                    int num = duiwiqoOtuem / 2;
                    int num2 = raowiofieh / 2;
                    if (this.get_Continent() == "Draenor")
                    {
                        if (((num == 0x17) && (num2 == 0x15)) && (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() == "horde"))
                        {
                            switch (Garrison.GetGarrisonLevel())
                            {
                                case 1:
                                    str = "FWHordeGarrisonLevel1";
                                    break;

                                case 2:
                                    str = "FWHordeGarrisonLeve2new";
                                    break;

                                case 3:
                                    str = "FWHordeGarrisonLevel2";
                                    break;
                            }
                        }
                        else if (((num == 0x1f) && (num2 == 0x1c)) && (nManager.Wow.ObjectManager.ObjectManager.Me.PlayerFaction.ToLower() != "horde"))
                        {
                            switch (Garrison.GetGarrisonLevel())
                            {
                                case 1:
                                    str = "SMVAllianceGarrisonLevel1";
                                    break;

                                case 2:
                                    str = "SMVAllianceGarrisonLevel2new";
                                    break;

                                case 3:
                                    str = "SMVAllianceGarrisonLevel2";
                                    break;
                            }
                        }
                    }
                    float num3 = (duiwiqoOtuem * (Utility.TileSize / 2f)) - (num * Utility.TileSize);
                    float num4 = (raowiofieh * (Utility.TileSize / 2f)) - (num2 * Utility.TileSize);
                    int num5 = (int) System.Math.Round((double) (num3 / (Utility.TileSize / 2f)));
                    int num6 = (int) System.Math.Round((double) (num4 / (Utility.TileSize / 2f)));
                    str2 = string.Concat(new object[] { edegeu ? "" : (str + @"\"), str, "_", num, "_", num2, "_", num5, num6, ".tile" });
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetTileName(int x, int y, bool onlyName): " + exception, true);
                    str2 = "";
                }
            }
            return str2;
        }

        public void Isueqehu()
        {
            lock (_awuigeowoUwuen)
            {
                try
                {
                    for (int i = 0; i < 0x80; i++)
                    {
                        for (int j = 0; j < 0x80; j++)
                        {
                            this.UjemiaqeUqari(this.IsivaikauqabaiGuixeroqe(j, i, false));
                            if (File.Exists(this.IpuenepuhibuxeQe(j, i)))
                            {
                                this.OpoufaidemSerehanu(j, i);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadAllTiles(): " + exception, true);
                }
            }
        }

        private bool LogaunonoOlahiudac(string toeraKetaheo)
        {
            bool flag2;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    string str = "http://meshes.thenoobbot.com/" + Utility.GetDetourSupportedVersion() + "/";
                    string[] strArray = toeraKetaheo.Split(new char[] { '\\' });
                    Directory.CreateDirectory(this._meshPath + @"\" + strArray[0] + @"\");
                    if (!Others.ExistFile(this._meshPath + @"\" + toeraKetaheo))
                    {
                        Logging.Write("Downloading \"" + toeraKetaheo + "\"...");
                        if (Others.DownloadFile(str + toeraKetaheo.Replace(@"\", "/") + ".gz", this._meshPath + @"\" + toeraKetaheo + ".gz"))
                        {
                            if (!GZip.Decompress(this._meshPath + @"\" + toeraKetaheo + ".gz"))
                            {
                                return false;
                            }
                            if (Others.ExistFile(this._meshPath + @"\" + toeraKetaheo + ".gz"))
                            {
                                File.Delete(this._meshPath + @"\" + toeraKetaheo + ".gz");
                            }
                            if (Others.ExistFile(this._meshPath + @"\" + toeraKetaheo))
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    flag2 = true;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("forceDownloadTile(string fileName): " + exception, true);
                    flag2 = false;
                }
            }
            return flag2;
        }

        public bool MamieOmuimae(int duiwiqoOtuem, int raowiofieh)
        {
            bool flag2;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    flag2 = this._mesh.RemoveTileAt(duiwiqoOtuem, raowiofieh).HasSucceeded();
                }
                catch (Exception exception)
                {
                    Logging.WriteError("RemoveTile(int x, int y): " + exception, true);
                    flag2 = false;
                }
            }
            return flag2;
        }

        public bool MamieOmuimae(int duiwiqoOtuem, int raowiofieh, out byte[] cuisufimabufAxeomu)
        {
            bool flag2;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    flag2 = this._mesh.RemoveTileAt(duiwiqoOtuem, raowiofieh, out cuisufimabufAxeomu).HasSucceeded();
                }
                catch (Exception exception)
                {
                    Logging.WriteError("RemoveTile(int x, int y, out byte[] tileData): " + exception, true);
                    cuisufimabufAxeomu = new byte[0];
                    flag2 = false;
                }
            }
            return flag2;
        }

        private void Manoemu()
        {
            lock (_awuigeowoUwuen)
            {
                List<Tuple<int, int>> list = new List<Tuple<int, int>>();
                foreach (KeyValuePair<Tuple<int, int>, int> pair in this._ovabiaw)
                {
                    if (pair.Value < (Others.TimesSec - 900))
                    {
                        Logging.WriteNavigator("Allowing to retry failed tile (" + this.IsivaikauqabaiGuixeroqe(pair.Key.Item1, pair.Key.Item2, true) + ")");
                        list.Add(pair.Key);
                    }
                }
                foreach (Tuple<int, int> tuple in list)
                {
                    this._ovabiaw.Remove(tuple);
                }
            }
        }

        public bool OpoufaidemSerehanu(byte[] egefuteaweosouEmDati)
        {
            bool flag2;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    MeshTile tile;
                    if (this.DejixoOgea())
                    {
                        return false;
                    }
                    DetourStatus status = this._mesh.AddTile(egefuteaweosouEmDati, out tile);
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
                    this.Unaikihe(egefuteaweosouEmDati.Length);
                    flag2 = true;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadTile(byte[] data): " + exception, true);
                    flag2 = false;
                }
            }
            return flag2;
        }

        public bool OpoufaidemSerehanu(int duiwiqoOtuem, int raowiofieh)
        {
            bool flag2;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    if (this.DejixoOgea())
                    {
                        return false;
                    }
                    if (this._erahedafiGoqaiIh.IsReady)
                    {
                        this._erahedafiGoqaiIh.Reset();
                        this.VoabowieruGuneiqewi();
                        this.Manoemu();
                    }
                    Tuple<int, int> key = new Tuple<int, int>(duiwiqoOtuem, raowiofieh);
                    if (this._mesh.HasTileAt(duiwiqoOtuem, raowiofieh))
                    {
                        this._qeacuUteruexo[key] = Others.TimesSec;
                        return true;
                    }
                    if (this._ovabiaw.ContainsKey(key))
                    {
                        this._ovabiaw[key] = Others.TimesSec;
                        return false;
                    }
                    string path = this.IpuenepuhibuxeQe(duiwiqoOtuem, raowiofieh);
                    string toeraKetaheo = this.IsivaikauqabaiGuixeroqe(duiwiqoOtuem, raowiofieh, false);
                    if (!this.UjemiaqeUqari(toeraKetaheo))
                    {
                        return false;
                    }
                    if (!File.Exists(path))
                    {
                        return false;
                    }
                    byte[] egefuteaweosouEmDati = File.ReadAllBytes(path);
                    if (!this.OpoufaidemSerehanu(egefuteaweosouEmDati))
                    {
                        Others.DeleteFile(this._meshPath + @"\" + toeraKetaheo);
                        if (!this.LogaunonoOlahiudac(toeraKetaheo))
                        {
                            return false;
                        }
                        egefuteaweosouEmDati = File.ReadAllBytes(path);
                        if (!this.OpoufaidemSerehanu(egefuteaweosouEmDati))
                        {
                            Logging.WriteError("Problem with Meshes tile " + toeraKetaheo + " , cannot load it.", true);
                            if (!this._qeacuUteruexo.ContainsKey(key))
                            {
                                this._ovabiaw[key] = Others.TimesSec;
                            }
                            return false;
                        }
                    }
                    this._loadedTilesString.Add(this.IsivaikauqabaiGuixeroqe(duiwiqoOtuem, raowiofieh, true));
                    if (this._loadedTilesString.Count >= 10)
                    {
                        string str3 = "";
                        foreach (string str4 in this._loadedTilesString)
                        {
                            str3 = str4 + ", " + str3;
                        }
                        this._loadedTilesString.Clear();
                        Logging.WriteNavigator(str3 + " loaded.");
                    }
                    if (!this._qeacuUteruexo.ContainsKey(key))
                    {
                        this._qeacuUteruexo.Add(key, Others.TimesSec);
                    }
                    flag2 = true;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadTile(int x, int y): " + exception, true);
                    flag2 = false;
                }
            }
            return flag2;
        }

        private void Osiriocour(int duiwiqoOtuem, int raowiofieh)
        {
            lock (_awuigeowoUwuen)
            {
                try
                {
                    if (!this._mesh.HasTileAt(duiwiqoOtuem, raowiofieh) && this.OpoufaidemSerehanu(duiwiqoOtuem, raowiofieh))
                    {
                        Logging.WriteNavigator(string.Concat(new object[] { "Load dynamically: ", duiwiqoOtuem, " ", raowiofieh }));
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadDynamic(int x, int y): " + exception, true);
                }
            }
        }

        public List<Point> Ovucuso(Point cuevejeijoi, Point ahaipeacDetoiniew)
        {
            List<Point> list;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    bool flag;
                    bool flag2;
                    list = this.Ovucuso(cuevejeijoi, ahaipeacDetoiniew, out flag, out flag2);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("FindPath(Point startVec, Point endVec): " + exception, true);
                    list = new List<Point>();
                }
            }
            return list;
        }

        public List<Point> Ovucuso(Point cuevejeijoi, Point ahaipeacDetoiniew, out bool guidajaxiu, out bool ewoegopisot)
        {
            lock (_awuigeowoUwuen)
            {
                List<Point> list = this.Asuagunebiohe(cuevejeijoi, ahaipeacDetoiniew, out guidajaxiu, out ewoegopisot, false);
                if ((list == null) || (list.Count < 2))
                {
                    guidajaxiu = false;
                    return new List<Point>();
                }
                if ((ahaipeacDetoiniew - cuevejeijoi).Magnitude >= 3000f)
                {
                    float magnitude = (ahaipeacDetoiniew - list[list.Count - 1]).Magnitude;
                    float num2 = 5f;
                    while (num2 < magnitude)
                    {
                        int num3 = (int) (list.Count * 0.8f);
                        List<Point> list2 = this.Asuagunebiohe(list[num3], ahaipeacDetoiniew, out guidajaxiu, out ewoegopisot, false);
                        if (list2 == null)
                        {
                            guidajaxiu = false;
                            return new List<Point>();
                        }
                        num2 = (ahaipeacDetoiniew - list2[list2.Count - 1]).Magnitude;
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
        }

        public void Qeinaewidea(Point isiqifacUruhue)
        {
            lock (_awuigeowoUwuen)
            {
                try
                {
                    if (!this.DejixoOgea())
                    {
                        float num;
                        float num2;
                        this.Tovia(isiqifacUruhue, out num, out num2);
                        int num3 = (int) System.Math.Floor((double) num);
                        int num4 = (int) System.Math.Floor((double) num2);
                        for (int i = -1; i < 2; i++)
                        {
                            for (int j = -1; j < 2; j++)
                            {
                                this.OpoufaidemSerehanu(num3 + i, num4 + j);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    Logging.WriteError("LoadAround(Point loc): " + exception, true);
                }
            }
        }

        private static void RiarutevasavRutiaba(string lobuaRoaboepea)
        {
            try
            {
                Logging.WriteNavigator(lobuaRoaboepea);
            }
            catch (Exception exception)
            {
                Logging.WriteError("HandleLog(string text): " + exception, true);
            }
        }

        public void Tovia(Point isiqifacUruhue, out float duiwiqoOtuem, out float raowiofieh)
        {
            lock (_awuigeowoUwuen)
            {
                try
                {
                    float num;
                    float num2;
                    this.DejixoOgea();
                    Tovia(isiqifacUruhue.ToRecast().ToFloatArray(), out num, out num2);
                    duiwiqoOtuem = num;
                    raowiofieh = num2;
                }
                catch (Exception exception)
                {
                    Logging.WriteError("GetTileByLocation(Point loc, out int x, out int y): " + exception, true);
                    duiwiqoOtuem = 0f;
                    raowiofieh = 0f;
                }
            }
        }

        public static void Tovia(float[] isiqifacUruhue, out float duiwiqoOtuem, out float raowiofieh)
        {
            duiwiqoOtuem = (isiqifacUruhue[0] - Utility.Origin[0]) / (Utility.TileSize / 2f);
            raowiofieh = (isiqifacUruhue[2] - Utility.Origin[2]) / (Utility.TileSize / 2f);
        }

        private static bool TureugaogokiceErobeumio(RoafuLui egefuteaweosouEmDati)
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

        private bool UjemiaqeUqari(string toeraKetaheo)
        {
            lock (_awuigeowoUwuen)
            {
                if (blackListMaptitle.Contains(toeraKetaheo))
                {
                    return true;
                }
                blackListMaptitle.Add(toeraKetaheo);
                return this.LogaunonoOlahiudac(toeraKetaheo);
            }
        }

        private void Unaikihe(int oguveonuxasAboWoroges)
        {
            try
            {
                GC.AddMemoryPressure((long) oguveonuxasAboWoroges);
                this.set_MemoryPressure(this.get_MemoryPressure() + oguveonuxasAboWoroges);
            }
            catch (Exception exception)
            {
                Logging.WriteError("AddMemoryPressure(int bytes): " + exception, true);
            }
        }

        public List<Point> Uriajioju(Point cuevejeijoi, Point ahaipeacDetoiniew)
        {
            List<Point> list;
            lock (_awuigeowoUwuen)
            {
                try
                {
                    bool flag;
                    bool flag2;
                    list = this.Asuagunebiohe(cuevejeijoi, ahaipeacDetoiniew, out flag, out flag2, true);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("FindLocalpath(Point startVec, Point endVec): " + exception, true);
                    list = new List<Point>();
                }
            }
            return list;
        }

        private void VoabowieruGuneiqewi()
        {
            lock (_awuigeowoUwuen)
            {
                List<Tuple<int, int>> list = new List<Tuple<int, int>>();
                foreach (KeyValuePair<Tuple<int, int>, int> pair in this._qeacuUteruexo)
                {
                    if (pair.Value < (Others.TimesSec - 900))
                    {
                        this.MamieOmuimae(pair.Key.Item1, pair.Key.Item2);
                        Logging.WriteNavigator("Unloading old tile (" + this.IsivaikauqabaiGuixeroqe(pair.Key.Item1, pair.Key.Item2, true) + ")");
                        list.Add(pair.Key);
                    }
                }
                foreach (Tuple<int, int> tuple in list)
                {
                    this._qeacuUteruexo.Remove(tuple);
                }
            }
        }

        public float WipiaGitolief(Point acekeAcepok, bool qiuleoxOqRoafuniw = false)
        {
            lock (_awuigeowoUwuen)
            {
                float num;
                float num2;
                float[] extents = qiuleoxOqRoafuniw ? new Point(0.5f, 2000f, 0.5f, "None").ToFloatArray() : new Point(1.5f, 2000f, 1.5f, "None").ToFloatArray();
                float[] center = acekeAcepok.ToRecast().ToFloatArray();
                this.Tovia(acekeAcepok, out num, out num2);
                int duiwiqoOtuem = (int) System.Math.Floor((double) num);
                int raowiofieh = (int) System.Math.Floor((double) num2);
                this.OpoufaidemSerehanu(duiwiqoOtuem, raowiofieh);
                ulong polyRef = this._query.FindNearestPolygon(center, extents, this.get_Filter());
                if (polyRef == 0L)
                {
                    Logging.WriteDebug(string.Concat(new object[] { "There is no polygon in this location (Tile ", duiwiqoOtuem, ",", raowiofieh, "), coord: X:", acekeAcepok.X, ", Y:", acekeAcepok.Y }));
                    return 0f;
                }
                float polyHeight = this._query.GetPolyHeight(polyRef, center);
                if ((polyHeight == 0f) && !qiuleoxOqRoafuniw)
                {
                    float[] numArray3;
                    polyHeight = this._query.closestPointOnPolyBoundary(polyRef, center, out numArray3).HasFailed() ? 0f : numArray3[1];
                }
                return polyHeight;
            }
        }

        public Point XawauTeidiXuih(Point acekeAcepok, out bool xaetuehAs)
        {
            lock (_awuigeowoUwuen)
            {
                float num;
                float num2;
                float[] numArray3;
                float[] extents = new Point(20f, 2000f, 20f, "None").ToFloatArray();
                float[] center = acekeAcepok.ToRecast().ToFloatArray();
                this.Tovia(acekeAcepok, out num, out num2);
                int duiwiqoOtuem = (int) System.Math.Floor((double) num);
                int raowiofieh = (int) System.Math.Floor((double) num2);
                this.OpoufaidemSerehanu(duiwiqoOtuem, raowiofieh);
                ulong polyRef = this._query.FindNearestPolygon(center, extents, this.get_Filter());
                if (polyRef == 0L)
                {
                    xaetuehAs = false;
                    return new Point();
                }
                if (this._query.closestPointOnPolyBoundary(polyRef, center, out numArray3).HasFailed())
                {
                    xaetuehAs = false;
                    return new Point();
                }
                xaetuehAs = true;
                return new Point(numArray3.ToWoW());
            }
        }

        public void XudeseujekeogId()
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
                GC.RemoveMemoryPressure((long) this.get_MemoryPressure());
                this.set_MemoryPressure(0);
            }
            catch (Exception exception)
            {
                Logging.WriteError("Pather > Dispose(): " + exception, true);
            }
        }

        public static void Xurie(float[] isiqifacUruhue, out float duiwiqoOtuem, out float raowiofieh)
        {
            duiwiqoOtuem = (isiqifacUruhue[0] - Utility.Origin[0]) / Utility.TileSize;
            raowiofieh = (isiqifacUruhue[2] - Utility.Origin[2]) / Utility.TileSize;
        }

        public ConnectionHandlerDelegate _ebohio { get; set; }

        public int _evaohualealal { get; private set; }

        public NavMesh _goiwuh
        {
            get
            {
                return this._mesh;
            }
        }

        public NavMeshQuery _imaumoruemeHotei
        {
            get
            {
                return this._query;
            }
        }

        public QueryFilter _koraimem { get; private set; }

        public string _qagaodiqPeboa { get; private set; }

        public delegate bool ConnectionHandlerDelegate(RoafuLui data);

        private class Ebejihiqo : NavMeshQueryCallback
        {
            private readonly Gaili _parent;

            public Ebejihiqo(Gaili parent)
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
                    Gaili.RiarutevasavRutiaba(text);
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
                    this._parent.Enakau(best);
                }
                catch (Exception exception)
                {
                    Logging.WriteError("PathfinderUpdate(float[] best): " + exception, true);
                }
            }
        }
    }
}

