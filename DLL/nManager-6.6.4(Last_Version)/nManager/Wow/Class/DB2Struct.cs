namespace nManager.Wow.Class
{
    using System;
    using System.Runtime.InteropServices;

    public class DB2Struct
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct WoWClientDB2
        {
            public IntPtr VTable;
            public int NumRows;
            public int StartArrayIndex;
            public int NumRows2;
            public int MaxIndex;
            public int MinIndex;
            public uint Unk7;
            public IntPtr Data;
            public IntPtr FirstRow;
            public IntPtr Rows;
            public IntPtr Unk11;
            public uint Unk12;
            public IntPtr Unk13;
            public uint RowEntrySize;
        }
    }
}

