using System;
using System.Runtime.InteropServices;

namespace HighVoltz.HBRelog.WoW.Lua
{
    [StructLayout(LayoutKind.Explicit, Size = 8, Pack = 1)]
    public struct LuaValueStruct
	{
		[FieldOffset(0)]
		public readonly int Boolean;

		[FieldOffset(0)]
		public readonly double Number;

		[FieldOffset(0)]
		public IntPtr Pointer;
	}
}