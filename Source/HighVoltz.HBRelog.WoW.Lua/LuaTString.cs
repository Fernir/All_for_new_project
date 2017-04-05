using GreyMagic;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace HighVoltz.HBRelog.WoW.Lua
{
	public class LuaTString
	{
		private LuaTString.LuaTStringHeader _luaTString;

		private readonly ExternalProcessReader _memory;

		public readonly IntPtr Address;

		private string _string;

		private const int Size = 20;

		public uint Hash
		{
			get
			{
				return this._luaTString.Hash;
			}
		}

		public string Value
		{
            get
            {
                return (_string ?? (_string = _memory.ReadString(Address + Size, Encoding.UTF8, _luaTString.Length)));
            }
		}

		public LuaTString(ExternalProcessReader memory, IntPtr address)
		{
			this.Address = address;
			this._memory = memory;
			this._luaTString = memory.Read<LuaTString.LuaTStringHeader>(address, false);
		}

		public override string ToString()
		{
			return this.Value;
		}

        [StructLayout(LayoutKind.Sequential, Size = 20, Pack = 1)]
        private struct LuaTStringHeader
		{
			public readonly LuaCommonHeader Header;

			private readonly byte reserved1;

			private readonly byte reserved2;

			public readonly uint Hash;

			public readonly int Length;
		}
	}
}