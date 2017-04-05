using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Alex.WoWRelogger.Utility
{
	internal static class Extensions
	{
		public static string FixString(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			if (str.Length == 1)
			{
				return str.ToUpperInvariant();
			}
			char upperInvariant = char.ToUpperInvariant(str[0]);
			return string.Concat(upperInvariant.ToString(), str.Substring(1));
		}

		public static bool HasExitedSafe(this Process process)
		{
			bool hasExited;
			if (process == null)
			{
				return true;
			}
			try
			{
				hasExited = process.HasExited;
			}
			catch
			{
				hasExited = true;
			}
			return hasExited;
		}
	}
}