using GreyMagic;
using System;

namespace HighVoltz.HBRelog.WoW.Lua
{
	public class LuaTKey
	{
		private LuaTKeyStruct _luaTKeyStruct;

		private readonly ExternalProcessReader _memory;

		private LuaValue _value;

		public LuaNode Next
		{
			get
            {
                try
                {
                    return _luaTKeyStruct.NextNodePtr != IntPtr.Zero ? new LuaNode(_memory, _luaTKeyStruct.NextNodePtr) : null;
                }
                catch (System.AccessViolationException)
                {
                    return null;
                }
            }
		}

		public LuaType Type
		{
			get
			{
				return this._luaTKeyStruct.Type;
			}
		}

		public LuaValue Value
		{
            get { return _value ?? (_value = new LuaValue(_memory, _luaTKeyStruct.Value)); }
		}

		public LuaTKey(ExternalProcessReader memory, LuaTKeyStruct luaTKeyStruct)
		{
			this._luaTKeyStruct = luaTKeyStruct;
			this._memory = memory;
		}
	}
}