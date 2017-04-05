using GreyMagic;
using System;

namespace HighVoltz.HBRelog.WoW.Lua
{
	public class LuaTValue
	{
		private LuaTValueStruct _luaTValue;

		private readonly ExternalProcessReader _memory;

		private LuaValue _value;

		private LuaTable _table;

		private LuaTString _string;

		public bool Boolean
		{
			get
			{
				return this._luaTValue.Value.Boolean != 0;
			}
		}

		public double Number
		{
			get
			{
				return this._luaTValue.Value.Number;
			}
		}

		public IntPtr Pointer
		{
			get
			{
				return this._luaTValue.Value.Pointer;
			}
		}

		public LuaTString String
		{
            get { return _string ?? (_string = new LuaTString(_memory, _luaTValue.Value.Pointer)); }
		}

		public LuaTable Table
		{
            get { return _table ?? (_table = new LuaTable(_memory, _luaTValue.Value.Pointer)); }
		}

		public LuaType Type
		{
			get
			{
				return this._luaTValue.Type;
			}
		}

		public LuaValue Value
		{
			get { return _value ?? (_value = new LuaValue(_memory, _luaTValue.Value)); }
		}

		public LuaTValue(ExternalProcessReader memory, LuaTValueStruct luaTValue)
		{
			this._luaTValue = luaTValue;
			this._memory = memory;
		}
	}
}