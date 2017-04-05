using GreyMagic;
using System;

namespace HighVoltz.HBRelog.WoW.Lua
{
	public class LuaValue
	{
		private LuaValueStruct _luaValue;

		private readonly ExternalProcessReader _memory;

		private LuaTable _table;

		private LuaTString _string;

		public bool Boolean
		{
			get
			{
				return this._luaValue.Boolean != 0;
			}
		}

		public double Number
		{
			get
			{
				return this._luaValue.Number;
			}
		}

		public IntPtr Pointer
		{
			get
			{
				return this._luaValue.Pointer;
			}
		}

		public LuaTString String
		{
            get { return _string ?? (_string = new LuaTString(_memory, _luaValue.Pointer)); }
		}

		public LuaTable Table
		{
            get { return _table ?? (_table = new LuaTable(_memory, _luaValue.Pointer)); }
		}

		internal LuaValue(ExternalProcessReader memory, LuaValueStruct luaValue)
		{
			this._luaValue = luaValue;
			this._memory = memory;
		}
	}
}