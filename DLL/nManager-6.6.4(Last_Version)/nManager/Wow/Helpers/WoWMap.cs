namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class WoWMap
    {
        private static AnielOcoihij _ajikuifoep;
        private static List<uint> _getoviatakaNeavien = new List<uint>(new uint[] { 930, 0x3e3, 0x4a3 });
        private static BinaryReader[] _giafaetociaDu;
        private static MapDbcRecord[] _qiwukopSiaqio;
        private MapDbcRecord _siwibaexesoef;

        private WoWMap(int reqId)
        {
            Awoup();
            MapDbcRecord record = new MapDbcRecord();
            bool flag = false;
            for (int i = 0; i < (_ajikuifoep.get_RecordsCount() - 1); i++)
            {
                record = _qiwukopSiaqio[i];
                if (record.Id == reqId)
                {
                    flag = true;
                    break;
                }
            }
            this._siwibaexesoef = flag ? record : new MapDbcRecord();
        }

        private WoWMap(string name, bool mpq = false)
        {
            Awoup();
            MapDbcRecord record = new MapDbcRecord();
            bool flag = false;
            for (int i = 0; i < (_ajikuifoep.get_RecordsCount() - 1); i++)
            {
                record = _qiwukopSiaqio[i];
                string str = mpq ? record.MapMPQName() : record.MapName();
                if (str == name)
                {
                    flag = true;
                    break;
                }
            }
            this._siwibaexesoef = flag ? record : new MapDbcRecord();
        }

        [CompilerGenerated]
        private static bool <init>b__0(Table t)
        {
            return (t.Name == "Map");
        }

        [CompilerGenerated]
        private static bool <init>b__1(Table t)
        {
            return (t.Name == Path.GetFileName("Map"));
        }

        private static void Awoup()
        {
            if (_ajikuifoep == null)
            {
                DBFilesClient client = DBFilesClient.Load(Application.StartupPath + @"\Data\DBFilesClient\dblayout.xml");
                if (CS$<>9__CachedAnonymousMethodDelegate2 == null)
                {
                    CS$<>9__CachedAnonymousMethodDelegate2 = new Func<Table, bool>(WoWMap.<init>b__0);
                }
                IEnumerable<Table> source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate2);
                if (!source.Any<Table>())
                {
                    if (CS$<>9__CachedAnonymousMethodDelegate3 == null)
                    {
                        CS$<>9__CachedAnonymousMethodDelegate3 = new Func<Table, bool>(WoWMap.<init>b__1);
                    }
                    source = client.Tables.Where<Table>(CS$<>9__CachedAnonymousMethodDelegate3);
                }
                if (source.Count<Table>() == 1)
                {
                    Table def = source.First<Table>();
                    _ajikuifoep = DBReaderFactory.GetReader(Application.StartupPath + @"\Data\DBFilesClient\Map.db2", def) as AnielOcoihij;
                    if (((_giafaetociaDu == null) || (_qiwukopSiaqio == null)) && (_ajikuifoep != null))
                    {
                        _giafaetociaDu = _ajikuifoep.get_Rows().ToArray<BinaryReader>();
                        _qiwukopSiaqio = new MapDbcRecord[_giafaetociaDu.Length];
                        for (int i = 0; i < (_giafaetociaDu.Length - 1); i++)
                        {
                            _qiwukopSiaqio[i] = AnielOcoihij.Jejaebiniqeuf<MapDbcRecord>(_giafaetociaDu[i]);
                        }
                    }
                    Logging.Write(string.Concat(new object[] { _ajikuifoep.get_FileName(), " loaded with ", _ajikuifoep.get_RecordsCount(), " entries." }));
                }
                else
                {
                    Logging.Write("DB2 Map not read-able.");
                }
            }
        }

        public static WoWMap FromId(int id)
        {
            return new WoWMap(id);
        }

        public static WoWMap FromMPQName(string name)
        {
            return new WoWMap(name, true);
        }

        public static WoWMap FromName(string name)
        {
            return new WoWMap(name, false);
        }

        public static List<MapDbcRecord> WoWMaps(InstanceType iType, MapType mType)
        {
            Awoup();
            MapDbcRecord item = new MapDbcRecord();
            List<MapDbcRecord> list = new List<MapDbcRecord>();
            for (int i = 0; i < (_ajikuifoep.get_RecordsCount() - 1); i++)
            {
                item = _qiwukopSiaqio[i];
                if (((!item.IsBlacklistedMap() && (item.InstanceType == iType)) && ((item.MapType == mType) && !item.IsTestMap())) && !item.IsGarrisonMap())
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public bool IsGarrisonMap
        {
            get
            {
                return this._siwibaexesoef.IsGarrisonMap();
            }
        }

        public bool IsTestMap
        {
            get
            {
                return this._siwibaexesoef.IsTestMap();
            }
        }

        public string MapMPQName
        {
            get
            {
                return this._siwibaexesoef.MapMPQName();
            }
        }

        public string MapName
        {
            get
            {
                return this._siwibaexesoef.MapName();
            }
        }

        public MapDbcRecord Record
        {
            get
            {
                return this._siwibaexesoef;
            }
        }

        public enum ExtensionId : byte
        {
            Cataclysm = 3,
            Legion = 6,
            MoP = 4,
            TBC = 1,
            Vanilla = 0,
            WoD = 5,
            WoTLK = 2
        }

        public enum InstanceType : byte
        {
            Arena = 4,
            Battleground = 3,
            None = 0,
            Party = 1,
            Raid = 2,
            Scenario = 5
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MapDbcRecord
        {
            public uint Id;
            private uint MPQDirectoryNameOffset;
            public int Flags;
            public int Flags2;
            private int field10;
            public int field14_0;
            public int field14_1;
            private int MapNameOffset;
            private int MapDescriptionHordeOffset;
            private int MapDescriptionAllianceOffset;
            public ushort field28;
            public ushort field2A;
            public ushort field2C;
            public ushort field2E;
            public ushort field30;
            public ushort field32;
            public ushort field34;
            public nManager.Wow.Helpers.WoWMap.InstanceType InstanceType;
            public nManager.Wow.Helpers.WoWMap.MapType MapType;
            public nManager.Wow.Helpers.WoWMap.ExtensionId ExtensionId;
            public byte field39;
            public byte field3A_0;
            public byte field3A_1;
            public string MapMPQName()
            {
                string str;
                if ((WoWMap._ajikuifoep.get_StringTable() != null) && WoWMap._ajikuifoep.get_StringTable().TryGetValue((int) this.MPQDirectoryNameOffset, out str))
                {
                    return str;
                }
                return "";
            }

            public string MapName()
            {
                string str;
                if ((WoWMap._ajikuifoep.get_StringTable() != null) && WoWMap._ajikuifoep.get_StringTable().TryGetValue(this.MapNameOffset, out str))
                {
                    return str;
                }
                return "";
            }

            public bool IsTestMap()
            {
                if ((this.Flags & 2L) == 0L)
                {
                    return ((this.Flags & 0x80L) != 0L);
                }
                return true;
            }

            public bool IsGarrisonMap()
            {
                return ((this.Flags & 0x4000000L) != 0L);
            }

            public bool IsBlacklistedMap()
            {
                return WoWMap._getoviatakaNeavien.Contains(this.Id);
            }
        }

        public enum MapType : byte
        {
            ADTType = 1,
            TransportType = 3,
            WDTOnlyType = 2,
            WMOType = 4
        }

        private enum UwuajupealuGutubu : uint
        {
            _adaowaotefIsaihohe = 0x100,
            _gumuqeutuiwIpoOsudiaba = 2,
            _moileupexo = 0x4000000,
            _ojujac = 0x80
        }
    }
}

