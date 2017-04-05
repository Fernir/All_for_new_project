namespace nManager.Wow.Helpers
{
    using System;
    using System.Runtime.InteropServices;

    public class WoWLock
    {
        private static DBC<LockDbcRecord> _lockDBC;
        private readonly LockDbcRecord _lockDBCRecord0;

        private WoWLock(uint id)
        {
            if (_lockDBC == null)
            {
                _lockDBC = new DBC<LockDbcRecord>(0xc6aa0c);
            }
            this._lockDBCRecord0 = _lockDBC.GetRow((int) id);
        }

        public static WoWLock FromId(uint id)
        {
            return new WoWLock(id);
        }

        public LockDbcRecord Record
        {
            get
            {
                return this._lockDBCRecord0;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LockDbcRecord
        {
            public uint Id;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public uint[] KeyType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public uint[] LockType;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public uint[] Skill;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8)]
            public uint[] Action;
        }
    }
}

