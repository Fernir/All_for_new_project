namespace nManager.Wow.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class Ocitie
    {
        public static string Boutoehaidiad(this BinaryReader agawipiDoaj)
        {
            byte num;
            List<byte> list = new List<byte>();
            while ((num = agawipiDoaj.ReadByte()) != 0)
            {
                list.Add(num);
            }
            return Encoding.UTF8.GetString(list.ToArray());
        }

        public static long Diateucifawoca(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if (uwononiec == null)
            {
                return agawipiDoaj.ReadInt64();
            }
            byte[] buffer = agawipiDoaj.ReadBytes((0x20 - uwononiec._kapetUxu) >> 3);
            long num = 0L;
            for (int i = 0; i < buffer.Length; i++)
            {
                num |= buffer[i] << (i * 8);
            }
            return num;
        }

        public static short Diodeu(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if ((uwononiec != null) && (uwononiec._kapetUxu != 0x10))
            {
                throw new Exception("TypeCode.Int16 Unknown meta.Bits");
            }
            return agawipiDoaj.ReadInt16();
        }

        public static double EgokiwoWuanefa(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if ((uwononiec != null) && (uwononiec._kapetUxu != -32))
            {
                throw new Exception("TypeCode.Double Unknown meta.Bits");
            }
            return agawipiDoaj.ReadDouble();
        }

        public static int Eheah(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if (uwononiec == null)
            {
                return agawipiDoaj.ReadInt32();
            }
            byte[] buffer = agawipiDoaj.ReadBytes((0x20 - uwononiec._kapetUxu) >> 3);
            int num = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                num |= buffer[i] << (i * 8);
            }
            return num;
        }

        public static string Ereomiahor(this BinaryReader agawipiDoaj)
        {
            string str = string.Empty;
            uint num = agawipiDoaj.ReadUInt32();
            for (uint i = 0; i < num; i++)
            {
                str = str + ((char) agawipiDoaj.ReadByte());
            }
            return str;
        }

        public static byte Esiregoleroah(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if ((uwononiec != null) && (uwononiec._kapetUxu != 0x18))
            {
                throw new Exception("TypeCode.Byte Unknown meta.Bits");
            }
            return agawipiDoaj.ReadByte();
        }

        public static BinaryReader Faigoaluqoah(string toeraKetaheo)
        {
            return new BinaryReader(new FileStream(toeraKetaheo, FileMode.Open), Encoding.UTF8);
        }

        public static ulong Faubouteonopu(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if (uwononiec == null)
            {
                return agawipiDoaj.ReadUInt64();
            }
            byte[] buffer = agawipiDoaj.ReadBytes((0x20 - uwononiec._kapetUxu) >> 3);
            ulong num = 0L;
            for (int i = 0; i < buffer.Length; i++)
            {
                num |= buffer[i] << (i * 8);
            }
            return num;
        }

        public static ulong FiecuosuMe(this BinaryReader agawipiDoaj)
        {
            ulong num = 0L;
            byte num2 = agawipiDoaj.ReadByte();
            if (num2 != 0)
            {
                for (int i = 0; i < 9; i++)
                {
                    if ((num2 & (((int) 1) << i)) != 0)
                    {
                        num += agawipiDoaj.ReadByte() << (i * 8);
                    }
                }
            }
            return num;
        }

        public static Coords3 FienuquruogeTiasuok(this BinaryReader agawipiDoaj)
        {
            Coords3 coords;
            coords.X = agawipiDoaj.ReadSingle();
            coords.Y = agawipiDoaj.ReadSingle();
            coords.Z = agawipiDoaj.ReadSingle();
            return coords;
        }

        public static float MegujumepiawiIpeamimei(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if ((uwononiec != null) && (uwononiec._kapetUxu != 0))
            {
                throw new Exception("TypeCode.Single Unknown meta.Bits");
            }
            return agawipiDoaj.ReadSingle();
        }

        public static string MoecaebiuMeusomi(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if ((uwononiec != null) && (uwononiec._kapetUxu != 0))
            {
                throw new Exception("TypeCode.String Unknown meta.Bits");
            }
            return agawipiDoaj.Boutoehaidiad();
        }

        public static ushort Peiha(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if ((uwononiec != null) && (uwononiec._kapetUxu != 0x10))
            {
                throw new Exception("TypeCode.UInt16 Unknown meta.Bits");
            }
            return agawipiDoaj.ReadUInt16();
        }

        public static void Qouceofoxesoq(this StringBuilder ahafaewitJoawo, string amoquikosaXewe, params object[] osucomaesaIjugegaes)
        {
            ahafaewitJoawo.AppendFormat(amoquikosaXewe, osucomaesaIjugegaes);
            ahafaewitJoawo.AppendLine();
        }

        public static void Qouceofoxesoq(this StringBuilder ahafaewitJoawo, IFormatProvider wokopoaco, string amoquikosaXewe, params object[] osucomaesaIjugegaes)
        {
            ahafaewitJoawo.AppendFormat(wokopoaco, amoquikosaXewe, osucomaesaIjugegaes);
            ahafaewitJoawo.AppendLine();
        }

        public static T Rokevuihatih<T>(this BinaryReader agawipiDoaj) where T: struct
        {
            GCHandle handle = GCHandle.Alloc(agawipiDoaj.ReadBytes(Marshal.SizeOf(typeof(T))), GCHandleType.Pinned);
            T local = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return local;
        }

        public static uint Tuesaupui(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if (uwononiec == null)
            {
                return agawipiDoaj.ReadUInt32();
            }
            byte[] buffer = agawipiDoaj.ReadBytes((0x20 - uwononiec._kapetUxu) >> 3);
            uint num = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                num |= (uint) (buffer[i] << (i * 8));
            }
            return num;
        }

        public static Coords4 UhuheawauwudigDiwikikuiUjautuot(this BinaryReader agawipiDoaj)
        {
            Coords4 coords;
            coords.X = agawipiDoaj.ReadSingle();
            coords.Y = agawipiDoaj.ReadSingle();
            coords.Z = agawipiDoaj.ReadSingle();
            coords.O = agawipiDoaj.ReadSingle();
            return coords;
        }

        public static sbyte Ujemuhoqoap(this BinaryReader agawipiDoaj, Onuseifouveile uwononiec)
        {
            if ((uwononiec != null) && (uwononiec._kapetUxu != 0x18))
            {
                throw new Exception("TypeCode.SByte Unknown meta.Bits");
            }
            return agawipiDoaj.ReadSByte();
        }
    }
}

