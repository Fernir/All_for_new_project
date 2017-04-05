using GreyMagic;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace HighVoltz.HBRelog.CleanPattern
{
	public class LeaModifier : IModifier
	{
		public LeaType Type
		{
			get;
			private set;
		}

		public LeaModifier()
		{
			this.Type = LeaType.Dword;
		}

		public LeaModifier(LeaType type)
		{
			this.Type = type;
		}

		public IntPtr Apply(ExternalProcessReader bm, IntPtr address)
		{
			switch (this.Type)
			{
				case LeaType.Byte:
				{
					return (IntPtr)bm.Read<byte>(address, false);
				}
				case LeaType.Word:
				{
					return (IntPtr)bm.Read<ushort>(address, false);
				}
				case LeaType.Dword:
				{
					return (IntPtr)((ulong)bm.Read<uint>(address, false));
				}
			}
			throw new InvalidDataException("Unknown LeaType");
		}
	}
}