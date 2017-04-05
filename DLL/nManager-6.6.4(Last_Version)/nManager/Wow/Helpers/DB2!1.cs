namespace nManager.Wow.Helpers
{
    using nManager.Helpful;
    using nManager.Wow;
    using nManager.Wow.Class;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    public class DB2<T> where T: struct
    {
        private readonly DB2Struct.WoWClientDB2 m_header;
        private readonly Dictionary<int, uint> m_rowAddresses;
        private readonly Dictionary<int, T> m_rows;

        public DB2(uint offset)
        {
            try
            {
                this.m_header = (DB2Struct.WoWClientDB2) Memory.WowMemory.Memory.ReadObject(Memory.WowProcess.WowModule + offset, typeof(DB2Struct.WoWClientDB2));
                this.m_rows = new Dictionary<int, T>(this.m_header.NumRows);
                this.m_rowAddresses = new Dictionary<int, uint>(this.m_header.NumRows);
                for (int i = 0; i < this.m_header.NumRows; i++)
                {
                    uint dwAddress = (uint) (((int) this.m_header.FirstRow) + (i * Marshal.SizeOf(typeof(T))));
                    int key = Memory.WowMemory.Memory.ReadInt(dwAddress);
                    T local = (T) Memory.WowMemory.Memory.ReadObject(dwAddress, typeof(T));
                    this.m_rowAddresses.Add(key, dwAddress);
                    this.m_rows.Add(key, local);
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("DB2(uint offset): " + exception, true);
            }
        }

        public T GetRow(int index)
        {
            try
            {
                if (this.HasRow(index))
                {
                    return this.m_rows[index];
                }
                return default(T);
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetRow(int index): " + exception, true);
            }
            return default(T);
        }

        public uint GetRowOffset(int index)
        {
            try
            {
                if (this.HasRowOffset(index))
                {
                    return this.m_rowAddresses[index];
                }
                return 0;
            }
            catch (Exception exception)
            {
                Logging.WriteError("GetRowOffset(int index): " + exception, true);
            }
            return 0;
        }

        public bool HasRow(int index)
        {
            return this.m_rows.ContainsKey(index);
        }

        public bool HasRowOffset(int index)
        {
            return this.m_rowAddresses.ContainsKey(index);
        }

        public string String(uint address)
        {
            return Memory.WowMemory.Memory.ReadUTF8String(address);
        }

        public T this[int index]
        {
            get
            {
                return this.m_rows[index];
            }
        }

        public int MaxIndex
        {
            get
            {
                return this.m_header.MaxIndex;
            }
        }

        public int MinIndex
        {
            get
            {
                return this.m_header.MinIndex;
            }
        }

        public int NumRows
        {
            get
            {
                return this.m_header.NumRows;
            }
        }

        public Dictionary<int, T> Rows
        {
            get
            {
                return this.m_rows;
            }
        }
    }
}

