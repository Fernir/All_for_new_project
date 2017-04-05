using GreyMagic;
using System;

namespace HighVoltz.HBRelog.CleanPattern
{
	public interface IModifier
	{
		IntPtr Apply(ExternalProcessReader bm, IntPtr address);
	}
}