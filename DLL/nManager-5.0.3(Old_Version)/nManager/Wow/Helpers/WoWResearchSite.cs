namespace nManager.Wow.Helpers
{
    using System;
    using System.Runtime.InteropServices;

    public class WoWResearchSite
    {
        private static DB2<ResearchSiteDb2Record> _rSiteDB2;
        private ResearchSiteDb2Record _rSiteDB2Record0;

        private WoWResearchSite(int reqId)
        {
            init();
            for (int i = _rSiteDB2.MinIndex; i <= _rSiteDB2.MaxIndex; i++)
            {
                this._rSiteDB2Record0 = _rSiteDB2.GetRow(i);
                if ((this._rSiteDB2Record0.Id == i) && (i == reqId))
                {
                    return;
                }
            }
            this._rSiteDB2Record0 = new ResearchSiteDb2Record();
        }

        private WoWResearchSite(string name, bool SecondOne = false)
        {
            bool flag = false;
            init();
            for (int i = _rSiteDB2.MinIndex; i <= _rSiteDB2.MaxIndex; i++)
            {
                this._rSiteDB2Record0 = _rSiteDB2.GetRow(i);
                if (((this._rSiteDB2Record0.Id == i) && (this.Name == name)) && (this._rSiteDB2Record0.Map == Usefuls.ContinentId))
                {
                    if (!SecondOne || flag)
                    {
                        return;
                    }
                    flag = true;
                }
            }
            this._rSiteDB2Record0 = new ResearchSiteDb2Record();
        }

        public static WoWResearchSite FromId(int id)
        {
            return new WoWResearchSite(id);
        }

        public static WoWResearchSite FromName(string name, bool secondOne = false)
        {
            return new WoWResearchSite(name, secondOne);
        }

        private static void init()
        {
            if (_rSiteDB2 == null)
            {
                _rSiteDB2 = new DB2<ResearchSiteDb2Record>(0xc6e868);
            }
        }

        public int MaxIndex
        {
            get
            {
                init();
                return _rSiteDB2.MaxIndex;
            }
        }

        public int MinIndex
        {
            get
            {
                init();
                return _rSiteDB2.MinIndex;
            }
        }

        public string Name
        {
            get
            {
                uint rowOffset = _rSiteDB2.GetRowOffset((int) this._rSiteDB2Record0.Id);
                return _rSiteDB2.String((rowOffset + this._rSiteDB2Record0.NameOffset) + ((uint) ((int) Marshal.OffsetOf(typeof(ResearchSiteDb2Record), "NameOffset"))));
            }
        }

        public ResearchSiteDb2Record Record
        {
            get
            {
                return this._rSiteDB2Record0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ResearchSiteDb2Record
        {
            public uint Id;
            public uint Map;
            public uint QuestIdPoint;
            public uint NameOffset;
            public uint Unk;
        }
    }
}

