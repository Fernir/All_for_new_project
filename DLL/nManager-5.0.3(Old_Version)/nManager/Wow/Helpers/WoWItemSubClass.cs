namespace nManager.Wow.Helpers
{
    using System;
    using System.Runtime.InteropServices;

    public class WoWItemSubClass
    {
        private static DBC<ItemSubClassDbcRecord> _rItemSubClassDBC;
        private ItemSubClassDbcRecord _rItemSubClassRec0;

        private WoWItemSubClass(string name, uint iClass)
        {
            if (_rItemSubClassDBC == null)
            {
                _rItemSubClassDBC = new DBC<ItemSubClassDbcRecord>(0xc698fc);
            }
            for (int i = _rItemSubClassDBC.MinIndex; i <= _rItemSubClassDBC.MaxIndex; i++)
            {
                this._rItemSubClassRec0 = _rItemSubClassDBC.GetRow(i);
                if (((this._rItemSubClassRec0.Index == i) && (this._rItemSubClassRec0.ClassId == iClass)) && (this.LongName == name))
                {
                    return;
                }
            }
            this._rItemSubClassRec0 = new ItemSubClassDbcRecord();
        }

        public static WoWItemSubClass FromNameAndClass(string name, uint iClass)
        {
            return new WoWItemSubClass(name, iClass);
        }

        public string LongName
        {
            get
            {
                uint rowOffset = _rItemSubClassDBC.GetRowOffset((int) this._rItemSubClassRec0.Index);
                return _rItemSubClassDBC.String((rowOffset + this._rItemSubClassRec0.SubClassLongNameOffset) + ((uint) ((int) Marshal.OffsetOf(typeof(ItemSubClassDbcRecord), "SubClassLongNameOffset"))));
            }
        }

        public ItemSubClassDbcRecord Record
        {
            get
            {
                return this._rItemSubClassRec0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ItemSubClassDbcRecord
        {
            public uint Index;
            public uint ClassId;
            public uint SubClassId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public uint[] Unk;
            public uint SubClassNameOffset;
            public uint SubClassLongNameOffset;
        }
    }
}

