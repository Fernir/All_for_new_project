using System;
using System.Runtime.InteropServices;

namespace HighVoltz.HBRelog.WoW.Lua
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LuaTKeyStruct
	{
		public readonly LuaValueStruct Value;

		public readonly LuaType Type;

		private readonly uint _unkC;

		public readonly IntPtr NextNodePtr;

		private readonly uint unk;
	}
}