using GreyMagic;
using System;
using System.Runtime.InteropServices;

namespace HighVoltz.HBRelog.WoW.Lua
{
	public class LuaNode
	{
		private LuaNode.LuaNodeStruct _luaNode;

		private readonly ExternalProcessReader _memory;

		private LuaTKey _key;

		private LuaTValue _value;

		public readonly IntPtr Address;

		public const uint Size = 40;

		public bool IsValid
		{
			get
			{
				return this.Address != IntPtr.Zero;
			}
		}

		public LuaTKey Key
		{
			get
			{
                return _key ?? (_key = new LuaTKey(_memory, _luaNode.Key)); 
			}
		}

		public LuaNode Next
		{
			get
			{
				return this.Key.Next;
			}
		}

		public LuaTValue Value
		{
			get
			{
				return _value ?? (_value = new LuaTValue(_memory, _luaNode.Value));
			}
		}

		public LuaNode(ExternalProcessReader memory, IntPtr address)
		{
			this.Address = address;
			this._memory = memory;
			this._luaNode = memory.Read<LuaNode.LuaNodeStruct>(address, false);
		}

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct LuaNodeStruct
		{
			public LuaTValueStruct Value;

			public LuaTKeyStruct Key;
		}
	}
}