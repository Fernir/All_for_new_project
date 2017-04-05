using System;
using System.Runtime.InteropServices;

namespace HighVoltz.HBRelog.WoW.Lua
{
    [StructLayout(LayoutKind.Sequential, Size = 16)]
    public struct LuaTValueStruct
	{
		public readonly LuaValueStruct Value;

		public readonly LuaType Type;

		private readonly uint _unkC;
	}
}