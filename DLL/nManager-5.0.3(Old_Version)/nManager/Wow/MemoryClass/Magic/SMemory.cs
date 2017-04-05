namespace nManager.Wow.MemoryClass.Magic
{
    using nManager.Wow.Class;
    using System;
    using System.Runtime.InteropServices;

    public static class SMemory
    {
        public const byte ASCII_CHAR_LENGTH = 1;
        public const int DEFAULT_MEMORY_SIZE = 0x1000;
        public const byte UNICODE_CHAR_LENGTH = 2;

        public static uint AllocateMemory(IntPtr hProcess)
        {
            return AllocateMemory(hProcess, 0x1000);
        }

        public static uint AllocateMemory(IntPtr hProcess, int nSize)
        {
            return AllocateMemory(hProcess, nSize, 0x1000, 0x40);
        }

        public static uint AllocateMemory(IntPtr hProcess, int nSize, uint dwAllocationType, uint dwProtect)
        {
            return Imports.VirtualAllocEx(hProcess, 0, nSize, dwAllocationType, dwProtect);
        }

        public static bool FreeMemory(IntPtr hProcess, uint dwAddress)
        {
            return FreeMemory(hProcess, dwAddress, 0, 0x8000);
        }

        public static bool FreeMemory(IntPtr hProcess, uint dwAddress, int nSize, uint dwFreeType)
        {
            if (dwFreeType == 0x8000)
            {
                nSize = 0;
            }
            return Imports.VirtualFreeEx(hProcess, dwAddress, nSize, dwFreeType);
        }

        public static string ReadASCIIString(IntPtr hProcess, uint dwAddress, int nLength)
        {
            string str;
            IntPtr zero = IntPtr.Zero;
            try
            {
                int nSize = nLength;
                zero = Marshal.AllocHGlobal((int) (nSize + 1));
                Marshal.WriteByte(zero, nLength, 0);
                if (ReadRawMemory(hProcess, dwAddress, zero, nSize) != nSize)
                {
                    throw new Exception();
                }
                str = Marshal.PtrToStringAnsi(zero);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return str;
        }

        public static byte ReadByte(IntPtr hProcess, uint dwAddress)
        {
            byte[] buffer = ReadBytes(hProcess, dwAddress, 1);
            if (buffer == null)
            {
                throw new Exception("ReadByte failed.");
            }
            return buffer[0];
        }

        public static byte[] ReadBytes(IntPtr hProcess, uint dwAddress, int nSize)
        {
            byte[] buffer;
            IntPtr zero = IntPtr.Zero;
            try
            {
                zero = Marshal.AllocHGlobal(nSize);
                int length = ReadRawMemory(hProcess, dwAddress, zero, nSize);
                buffer = new byte[nSize];
                Marshal.Copy(zero, buffer, 0, length);
            }
            catch
            {
                return new byte[nSize];
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return buffer;
        }

        public static double ReadDouble(IntPtr hProcess, uint dwAddress)
        {
            return ReadDouble(hProcess, dwAddress, false);
        }

        public static double ReadDouble(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            byte[] array = ReadBytes(hProcess, dwAddress, 8);
            if (array == null)
            {
                throw new Exception("ReadDouble failed.");
            }
            if (bReverse)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToDouble(array, 0);
        }

        public static float ReadFloat(IntPtr hProcess, uint dwAddress)
        {
            return ReadFloat(hProcess, dwAddress, false);
        }

        public static float ReadFloat(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            byte[] array = ReadBytes(hProcess, dwAddress, 4);
            if (array == null)
            {
                throw new Exception("ReadFloat failed.");
            }
            if (bReverse)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToSingle(array, 0);
        }

        public static int ReadInt(IntPtr hProcess, uint dwAddress)
        {
            return ReadInt(hProcess, dwAddress, false);
        }

        public static int ReadInt(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            try
            {
                byte[] array = ReadBytes(hProcess, dwAddress, 4);
                if (array == null)
                {
                    throw new Exception("ReadInt failed.");
                }
                if (bReverse)
                {
                    Array.Reverse(array);
                }
                return BitConverter.ToInt32(array, 0);
            }
            catch
            {
                return 0;
            }
        }

        public static int ReadInt128(IntPtr hProcess, uint dwAddress)
        {
            return ReadInt(hProcess, dwAddress, false);
        }

        public static long ReadInt64(IntPtr hProcess, uint dwAddress)
        {
            return ReadInt64(hProcess, dwAddress, false);
        }

        public static long ReadInt64(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            byte[] array = ReadBytes(hProcess, dwAddress, 8);
            if (array == null)
            {
                throw new Exception("ReadInt64 failed.");
            }
            if (bReverse)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToInt64(array, 0);
        }

        public static object ReadObject(IntPtr hProcess, uint dwAddress, Type objType)
        {
            object obj2;
            IntPtr zero = IntPtr.Zero;
            try
            {
                int cb = Marshal.SizeOf(objType);
                zero = Marshal.AllocHGlobal(cb);
                if (ReadRawMemory(hProcess, dwAddress, zero, cb) != cb)
                {
                    throw new Exception("ReadProcessMemory error in ReadObject.");
                }
                obj2 = Marshal.PtrToStructure(zero, objType);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return obj2;
        }

        public static int ReadRawMemory(IntPtr hProcess, uint dwAddress, IntPtr lpBuffer, int nSize)
        {
            int lpBytesRead = 0;
            try
            {
                Imports.ReadProcessMemory(hProcess, dwAddress, lpBuffer, nSize, out lpBytesRead);
                return lpBytesRead;
            }
            catch
            {
                return 0;
            }
        }

        public static sbyte ReadSByte(IntPtr hProcess, uint dwAddress)
        {
            byte[] buffer = ReadBytes(hProcess, dwAddress, 1);
            if (buffer == null)
            {
                throw new Exception("ReadSByte failed.");
            }
            return (sbyte) buffer[0];
        }

        public static short ReadShort(IntPtr hProcess, uint dwAddress)
        {
            return ReadShort(hProcess, dwAddress, false);
        }

        public static short ReadShort(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            byte[] array = ReadBytes(hProcess, dwAddress, 2);
            if (array == null)
            {
                throw new Exception("ReadShort failed.");
            }
            if (bReverse)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToInt16(array, 0);
        }

        public static uint ReadUInt(IntPtr hProcess, uint dwAddress)
        {
            return ReadUInt(hProcess, dwAddress, false);
        }

        public static uint ReadUInt(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            byte[] array = ReadBytes(hProcess, dwAddress, 4);
            if (array == null)
            {
                throw new Exception("ReadUInt failed.");
            }
            if (bReverse)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToUInt32(array, 0);
        }

        public static UInt128 ReadUInt128(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            try
            {
                byte[] array = ReadBytes(hProcess, dwAddress, Marshal.SizeOf(typeof(UInt128)));
                if (array == null)
                {
                    throw new Exception("ReadInt failed.");
                }
                if (bReverse)
                {
                    Array.Reverse(array);
                }
                return new UInt128(array);
            }
            catch
            {
                return 0;
            }
        }

        public static ulong ReadUInt64(IntPtr hProcess, uint dwAddress)
        {
            return ReadUInt64(hProcess, dwAddress, false);
        }

        public static ulong ReadUInt64(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            byte[] array = ReadBytes(hProcess, dwAddress, 8);
            if (array == null)
            {
                throw new Exception("ReadUInt64 failed.");
            }
            if (bReverse)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToUInt64(array, 0);
        }

        public static string ReadUnicodeString(IntPtr hProcess, uint dwAddress, int nLength)
        {
            string str;
            IntPtr zero = IntPtr.Zero;
            try
            {
                int nSize = nLength * 2;
                zero = Marshal.AllocHGlobal((int) (nSize + 2));
                Marshal.WriteInt16(zero, nLength * 2, (short) 0);
                if (ReadRawMemory(hProcess, dwAddress, zero, nSize) != nSize)
                {
                    throw new Exception();
                }
                str = Marshal.PtrToStringUni(zero);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return str;
        }

        public static ushort ReadUShort(IntPtr hProcess, uint dwAddress)
        {
            return ReadUShort(hProcess, dwAddress, false);
        }

        public static ushort ReadUShort(IntPtr hProcess, uint dwAddress, bool bReverse)
        {
            byte[] array = ReadBytes(hProcess, dwAddress, 2);
            if (array == null)
            {
                throw new Exception("ReadUShort failed.");
            }
            if (bReverse)
            {
                Array.Reverse(array);
            }
            return BitConverter.ToUInt16(array, 0);
        }

        public static bool WriteASCIIString(IntPtr hProcess, uint dwAddress, string Value)
        {
            IntPtr zero = IntPtr.Zero;
            int num = 0;
            int nSize = 0;
            try
            {
                nSize = Value.Length;
                zero = Marshal.StringToHGlobalAnsi(Value);
                num = WriteRawMemory(hProcess, dwAddress, zero, nSize);
                if (nSize != num)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return true;
        }

        public static bool WriteByte(IntPtr hProcess, uint dwAddress, byte Value)
        {
            byte[] lpBytes = new byte[] { Value };
            return WriteBytes(hProcess, dwAddress, lpBytes, 1);
        }

        public static bool WriteBytes(IntPtr hProcess, uint dwAddress, byte[] lpBytes)
        {
            return WriteBytes(hProcess, dwAddress, lpBytes, lpBytes.Length);
        }

        public static bool WriteBytes(IntPtr hProcess, uint dwAddress, byte[] lpBytes, int nSize)
        {
            IntPtr zero = IntPtr.Zero;
            int num = 0;
            try
            {
                zero = Marshal.AllocHGlobal((int) (Marshal.SizeOf(lpBytes[0]) * nSize));
                Marshal.Copy(lpBytes, 0, zero, nSize);
                num = WriteRawMemory(hProcess, dwAddress, zero, nSize);
                if (nSize != num)
                {
                    throw new Exception("WriteBytes failed!  Number of bytes actually written differed from request.");
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return true;
        }

        public static bool WriteDouble(IntPtr hProcess, uint dwAddress, double Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 8);
        }

        public static bool WriteFloat(IntPtr hProcess, uint dwAddress, float Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 4);
        }

        public static bool WriteInt(IntPtr hProcess, uint dwAddress, int Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 4);
        }

        public static bool WriteInt128(IntPtr hProcess, uint dwAddress, UInt128 Value)
        {
            byte[] lpBytes = Value.ToByteArray();
            int nSize = Marshal.SizeOf(typeof(UInt128));
            return WriteBytes(hProcess, dwAddress, lpBytes, nSize);
        }

        public static bool WriteInt64(IntPtr hProcess, uint dwAddress, long Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 8);
        }

        public static bool WriteObject(IntPtr hProcess, uint dwAddress, object objBuffer)
        {
            return WriteObject(hProcess, dwAddress, objBuffer, objBuffer.GetType());
        }

        public static bool WriteObject(IntPtr hProcess, uint dwAddress, object objBuffer, Type objType)
        {
            int cb = 0;
            int num2 = 0;
            IntPtr zero = IntPtr.Zero;
            try
            {
                cb = Marshal.SizeOf(objType);
                zero = Marshal.AllocHGlobal(cb);
                Marshal.StructureToPtr(objBuffer, zero, false);
                num2 = WriteRawMemory(hProcess, dwAddress, zero, cb);
                if (cb != num2)
                {
                    throw new Exception("WriteObject failed!  Number of bytes actually written differed from request.");
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.DestroyStructure(zero, objType);
                    Marshal.FreeHGlobal(zero);
                }
            }
            return true;
        }

        private static int WriteRawMemory(IntPtr hProcess, uint dwAddress, IntPtr lpBuffer, int nSize)
        {
            IntPtr zero = IntPtr.Zero;
            if (!Imports.WriteProcessMemory(hProcess, dwAddress, lpBuffer, nSize, out zero))
            {
                return 0;
            }
            return (int) zero;
        }

        public static bool WriteSByte(IntPtr hProcess, uint dwAddress, sbyte Value)
        {
            byte[] lpBytes = new byte[] { (byte) Value };
            return WriteBytes(hProcess, dwAddress, lpBytes, 1);
        }

        public static bool WriteShort(IntPtr hProcess, uint dwAddress, short Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 2);
        }

        public static bool WriteUInt(IntPtr hProcess, uint dwAddress, uint Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 4);
        }

        public static bool WriteUInt64(IntPtr hProcess, uint dwAddress, ulong Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 8);
        }

        public static bool WriteUnicodeString(IntPtr hProcess, uint dwAddress, string Value)
        {
            IntPtr zero = IntPtr.Zero;
            int num = 0;
            int nSize = 0;
            try
            {
                nSize = Value.Length * 2;
                zero = Marshal.StringToHGlobalUni(Value);
                num = WriteRawMemory(hProcess, dwAddress, zero, nSize);
                if (nSize != num)
                {
                    throw new Exception();
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }
            return true;
        }

        public static bool WriteUShort(IntPtr hProcess, uint dwAddress, ushort Value)
        {
            byte[] bytes = BitConverter.GetBytes(Value);
            return WriteBytes(hProcess, dwAddress, bytes, 2);
        }
    }
}

