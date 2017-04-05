using GreyMagic;
using System;
using System.Runtime.CompilerServices;

namespace HighVoltz.HBRelog.CleanPattern
{
	public class AddModifier : IModifier
	{
		public uint Offset
		{
			get;
			private set;
		}

		public AddModifier(uint val)
		{
			this.Offset = val;
		}

		public IntPtr Apply(ExternalProcessReader bm, IntPtr addr)
		{
			return addr + (int)this.Offset;
		}
	}
}