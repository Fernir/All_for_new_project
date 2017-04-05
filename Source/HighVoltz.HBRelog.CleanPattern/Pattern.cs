using GreyMagic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace HighVoltz.HBRelog.CleanPattern
{
	public class Pattern
	{
		private const int CacheSize = 1280;

		public List<IModifier> Modifiers = new List<IModifier>();

		public byte[] Bytes
		{
			get;
			private set;
		}

		public bool[] Mask
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public Pattern()
		{
		}

		private bool DataCompare(byte[] data, uint dataOffset)
		{
			return !this.Mask.Where<bool>((bool t, int i) => {
				if (!t)
				{
					return false;
				}
                return (int)this.Bytes[i] != (int)data[(long)dataOffset + (long)i];
            }).Any<bool>();
            
        }

		public IntPtr Find(ExternalProcessReader bm)
		{
			IntPtr intPtr = this.FindStart(bm);
			intPtr = this.Modifiers.Aggregate<IModifier, IntPtr>(intPtr, (IntPtr current, IModifier mod) => mod.Apply(bm, current));
			return intPtr - (int)bm.Process.MainModule.BaseAddress;
		}

		private IntPtr FindStart(ExternalProcessReader bm)
		{
			ProcessModule mainModule = bm.Process.MainModule;
			IntPtr baseAddress = mainModule.BaseAddress;
			int moduleMemorySize = mainModule.ModuleMemorySize;
			int length = (int)this.Bytes.Length;
            uint num = 0U;
            while ((long)num < (long)(moduleMemorySize - length))
            {
                byte[] data = bm.ReadBytes(baseAddress + (int)num, 1280L > (long)moduleMemorySize - (long)num ? moduleMemorySize - (int)num : 1280, false);
                for (uint dataOffset = 0U; (long)dataOffset < (long)(data.Length - length); ++dataOffset)
                {
                    if (this.DataCompare(data, dataOffset))
                        return baseAddress + ((int)num + (int)dataOffset);
                }
                num += (uint)(1280 - length);
            }
            throw new InvalidDataException(string.Format("Pattern {0} not found", this.Name));
		}

		public static Pattern FromTextstyle(string name, string pattern, params IModifier[] modifiers)
		{
			Pattern list = new Pattern()
			{
				Name = name
			};
			if (modifiers != null)
			{
				list.Modifiers = modifiers.ToList<IModifier>();
			}
			string[] strArrays = pattern.Split(new char[] { ' ' });
			int num = 0;
			list.Bytes = new byte[(int)strArrays.Length];
			list.Mask = new bool[(int)strArrays.Length];
			string[] strArrays1 = strArrays;
			for (int i = 0; i < (int)strArrays1.Length; i++)
			{
				string str = strArrays1[i];
				if (str.Length > 2)
				{
					throw new InvalidDataException(string.Concat("Invalid token: ", str));
				}
				if (!str.Contains("?"))
				{
					byte num1 = byte.Parse(str, NumberStyles.HexNumber);
					list.Bytes[num] = num1;
					list.Mask[num] = true;
					num++;
				}
				else
				{
					int num2 = num;
					num = num2 + 1;
					list.Mask[num2] = false;
				}
			}
			return list;
		}
	}
}