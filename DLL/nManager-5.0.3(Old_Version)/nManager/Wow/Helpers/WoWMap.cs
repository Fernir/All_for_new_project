namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class WoWMap
    {
        private static List<uint> _blacklistedMaps = new List<uint>(new uint[] { 930, 0x3e3, 0x4a3 });
        private static DBC<MapDbcRecord> _rMapDBC;
        private MapDbcRecord _rMapDBCRecord0;

        private WoWMap(int reqId)
        {
            init();
            for (int i = _rMapDBC.MinIndex; i <= _rMapDBC.MaxIndex; i++)
            {
                this._rMapDBCRecord0 = _rMapDBC.GetRow(i);
                if ((this._rMapDBCRecord0.Id == i) && (i == reqId))
                {
                    return;
                }
            }
            this._rMapDBCRecord0 = new MapDbcRecord();
        }

        private WoWMap(string name, bool mpq = false)
        {
            init();
            for (int i = _rMapDBC.MinIndex; i <= _rMapDBC.MaxIndex; i++)
            {
                this._rMapDBCRecord0 = _rMapDBC.GetRow(i);
                if (this._rMapDBCRecord0.Id == i)
                {
                    string str = mpq ? this.MapMPQName : this.MapName;
                    if (str == name)
                    {
                        return;
                    }
                }
            }
            this._rMapDBCRecord0 = new MapDbcRecord();
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

        private static void init()
        {
            if (_rMapDBC == null)
            {
                _rMapDBC = new DBC<MapDbcRecord>(0xc74bb8);
            }
        }

        public static List<MapDbcRecord> WoWMaps(InstanceType iType, MapType mType)
        {
            init();
            List<MapDbcRecord> list = new List<MapDbcRecord>();
            for (int i = _rMapDBC.MinIndex; i <= _rMapDBC.MaxIndex; i++)
            {
                MapDbcRecord row = _rMapDBC.GetRow(i);
                if (((!row.IsBlacklistedMap() && (row.InstanceType == iType)) && ((row.MapType == mType) && !row.IsTestMap())) && !row.IsGarrisonMap())
                {
                    list.Add(row);
                }
            }
            return list;
        }

        public bool IsGarrisonMap
        {
            get
            {
                return this._rMapDBCRecord0.IsGarrisonMap();
            }
        }

        public bool IsTestMap
        {
            get
            {
                return this._rMapDBCRecord0.IsTestMap();
            }
        }

        public string MapMPQName
        {
            get
            {
                return this._rMapDBCRecord0.MapMPQName();
            }
        }

        public string MapName
        {
            get
            {
                return this._rMapDBCRecord0.MapName();
            }
        }

        public MapDbcRecord Record
        {
            get
            {
                return this._rMapDBCRecord0;
            }
        }

        public enum InstanceType : uint
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
            public nManager.Wow.Helpers.WoWMap.InstanceType InstanceType;
            public uint Flags;
            public uint unk4;
            public nManager.Wow.Helpers.WoWMap.MapType MapType;
            private uint MapNameOffset;
            public uint AreaTableId;
            private uint MapDescriptionHordeOffset;
            private uint MapDescriptionAllianceOffset;
            public uint LoadingScreenId;
            public float MinimapIconScale;
            public int GhostEntranceMapId;
            public float GhostEntranceX;
            public float GhostEntranceY;
            public int TimeOfDayOverride;
            public uint ExpansionId;
            public uint RaidOffset;
            public uint MaxPlayers;
            public int ParentMapId;
            public int CosmeticParentMapID;
            public uint TimeOffset;
            public string MapMPQName()
            {
                return WoWMap._rMapDBC.String((WoWMap._rMapDBC.GetRowOffset((int) this.Id) + this.MPQDirectoryNameOffset) + ((uint) ((int) Marshal.OffsetOf(typeof(WoWMap.MapDbcRecord), "MPQDirectoryNameOffset"))));
            }

            public string MapName()
            {
                return WoWMap._rMapDBC.String((WoWMap._rMapDBC.GetRowOffset((int) this.Id) + this.MapNameOffset) + ((uint) ((int) Marshal.OffsetOf(typeof(WoWMap.MapDbcRecord), "MapNameOffset"))));
            }

            public bool IsTestMap()
            {
                if ((this.Flags & 2) == 0)
                {
                    return ((this.Flags & 0x80) != 0);
                }
                return true;
            }

            public bool IsGarrisonMap()
            {
                return ((this.Flags & 0x4000000) != 0);
            }

            public bool IsBlacklistedMap()
            {
                return WoWMap._blacklistedMaps.Contains(this.Id);
            }
        }

        private enum MapFlags : uint
        {
            MAP_FLAG_DYNAMIC_DIFFICULTY = 0x100,
            MAP_FLAG_GARRISON = 0x4000000,
            MAP_FLAG_NOT_EXISTING = 0x80,
            MAP_FLAG_TEST_MAP = 2
        }

        public enum MapType : uint
        {
            ADTType = 1,
            TransportType = 3,
            WDTOnlyType = 2,
            WMOType = 4
        }
    }
}

