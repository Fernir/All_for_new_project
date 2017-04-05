namespace nManager.Wow.Helpers
{
    using System;
    using System.Runtime.InteropServices;

    public class WoWItemClass
    {
        private static DB2<ItemClassDB2Record> _rItemClassDB2;
        private ItemClassDB2Record _rItemClassRec0;

        private WoWItemClass(string name)
        {
            if (_rItemClassDB2 == null)
            {
                _rItemClassDB2 = new DB2<ItemClassDB2Record>(0xc68a90);
            }
            for (int i = _rItemClassDB2.MinIndex; i <= _rItemClassDB2.MaxIndex; i++)
            {
                this._rItemClassRec0 = _rItemClassDB2.GetRow(i);
                if ((this._rItemClassRec0.ClassId == i) && (this.Name == name))
                {
                    return;
                }
            }
            this._rItemClassRec0 = new ItemClassDB2Record();
        }

        public static WoWItemClass FromName(string name)
        {
            return new WoWItemClass(name);
        }

        public string Name
        {
            get
            {
                uint rowOffset = _rItemClassDB2.GetRowOffset((int) this._rItemClassRec0.ClassId);
                return _rItemClassDB2.String((rowOffset + this._rItemClassRec0.ClassNameOffset) + ((uint) ((int) Marshal.OffsetOf(typeof(ItemClassDB2Record), "ClassNameOffset"))));
            }
        }

        public ItemClassDB2Record Record
        {
            get
            {
                return this._rItemClassRec0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemClassDB2Record
        {
            public uint ClassId;
            public uint unk2;
            public uint unk3;
            public uint ClassNameOffset;
        }
    }
}

