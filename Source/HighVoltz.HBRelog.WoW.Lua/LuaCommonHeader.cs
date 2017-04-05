using System;
using System.Runtime.InteropServices;

namespace HighVoltz.HBRelog.WoW.Lua
{
    [StructLayout(LayoutKind.Sequential, Size = 10, Pack = 1)]
	public struct LuaCommonHeader
	{
		public readonly IntPtr GCObjectPtr;

		private readonly uint _unk8;

		private readonly byte type;

		public readonly byte Marked;

		public LuaType Type
		{
			get
			{
				return (LuaType)this.type;
			}
		}
	}
}