using GreyMagic;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HighVoltz.HBRelog.WoW.Lua
{
	public class LuaTable
	{
		private readonly LuaTable.LuaTableStuct _luaTable;

		private readonly ExternalProcessReader _memory;

		public readonly IntPtr Address;

		private bool _triedGetMetaTable;

		private LuaTable _metaTable;

		public byte Flags
		{
			get
			{
				return this._luaTable.Flags;
			}
		}

		public LuaTable MetaTable
		{
            get
            {
                if (!_triedGetMetaTable)
                {
                    _metaTable = (_luaTable.MetaTablePtr != IntPtr.Zero ? new LuaTable(_memory, _luaTable.MetaTablePtr) : null);
                    _triedGetMetaTable = true;
                }
                return _metaTable;
            }
		}

		public uint NodeCount
		{
			get
			{
				return this._luaTable.NodesCount;
			}
		}

		public IEnumerable<LuaNode> Nodes
		{
			get
			{
				if (this._luaTable.NodePtr == IntPtr.Zero)
				{
					yield break;
				}
				for (uint i = 0; i < this.NodeCount; i++)
				{
					LuaNode nodeAtIndex = this.GetNodeAtIndex(i);
					if (nodeAtIndex.IsValid)
					{
						yield return nodeAtIndex;
					}
				}
			}
		}

		public uint ValueCount
		{
			get
			{
				return this._luaTable.ValueCount;
			}
		}

		public LuaTable(ExternalProcessReader memory, IntPtr address)
		{
			this.Address = address;
			this._memory = memory;
			this._luaTable = this._memory.Read<LuaTable.LuaTableStuct>(address, false);
		}

		private LuaNode GetNodeAtIndex(uint idx)
		{
            return new LuaNode(_memory, _luaTable.NodePtr + (int)(LuaNode.Size * idx));
        }

		public LuaTValue GetValue(string key)
		{
			var num = H(key);
            LuaNode next = GetNodeAtIndex(num & (NodeCount - 1));
            while ((next.Key.Type != LuaType.String) || !string.Equals(key, next.Key.Value.String.Value))
            {
                next = next.Key.Next;
                if (next == null)
                {
                    return null;
                }
            }
            return next.Value;
		}

		private static uint H(string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(str);
			uint length = (uint)str.Length;
			uint num = (length >> 5) + 1;
			for (uint i = length; i >= num; i = i - num)
			{
            length ^= (length << 5) + (length >> 2) + bytes[i - 1];
			}
			return length;
		}

        #region Embedded Type: LuaTableStruct

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct LuaTableStuct
        {
            public readonly LuaCommonHeader Header;
            public readonly byte Flags; /* 1<<p means tagmethod(p) is not present */
            private readonly byte Log2Sizenode; /* log2 of size of `node' array */
            public readonly IntPtr MetaTablePtr;
            public readonly IntPtr ValuesPtr;
            public readonly IntPtr NodePtr;
            private readonly IntPtr lastFree; /* any free position is before this position */
            private readonly IntPtr gclist;
            public readonly uint ValueCount;

            public uint NodesCount
            {
                //todo  
                //in github get { return 1u << this.Log2Sizenode; }
                get { return 1u << (this.Log2Sizenode & 31); }
            }
        }

        #endregion
	}
}