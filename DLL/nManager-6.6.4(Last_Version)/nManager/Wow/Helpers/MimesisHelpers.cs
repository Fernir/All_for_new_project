namespace nManager.Wow.Helpers
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;

    public class MimesisHelpers
    {
        public static T BytesToObject<T>(byte[] arrBytes)
        {
            MemoryStream serializationStream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            serializationStream.Write(arrBytes, 0, arrBytes.Length);
            serializationStream.Seek(0L, SeekOrigin.Begin);
            return (T) formatter.Deserialize(serializationStream);
        }

        public static string BytesToString(byte[] bytes)
        {
            char[] dst = new char[bytes.Length / 2];
            Buffer.BlockCopy(bytes, 0, dst, 0, bytes.Length);
            return new string(dst);
        }

        public static T BytesToStruct<T>(byte[] arr)
        {
            T structure = (T) Activator.CreateInstance(typeof(T));
            int cb = Marshal.SizeOf(structure);
            IntPtr destination = Marshal.AllocHGlobal(cb);
            Marshal.Copy(arr, 0, destination, cb);
            structure = (T) Marshal.PtrToStructure(destination, structure.GetType());
            Marshal.FreeHGlobal(destination);
            return structure;
        }

        public static byte[] ObjectToBytes(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream serializationStream = new MemoryStream();
            formatter.Serialize(serializationStream, obj);
            return serializationStream.ToArray();
        }

        public static byte[] StringToBytes(string str)
        {
            byte[] dst = new byte[str.Length * 2];
            Buffer.BlockCopy(str.ToCharArray(), 0, dst, 0, dst.Length);
            return dst;
        }

        public static byte[] StructToBytes(object str)
        {
            int cb = Marshal.SizeOf(str);
            byte[] destination = new byte[cb];
            IntPtr ptr = Marshal.AllocHGlobal(cb);
            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, destination, 0, cb);
            Marshal.FreeHGlobal(ptr);
            return destination;
        }

        public enum eventType
        {
            none,
            pickupQuest,
            turninQuest,
            interactObject,
            mount,
            taxi
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        public struct MimesisEvent
        {
            public uint SerialNumber;
            public MimesisHelpers.eventType eType;
            public int EventValue1;
            public int EventValue2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=100)]
            public string EventString1;
        }

        public enum opCodes
        {
            Disconnect = 9,
            QueryEvent = 2,
            QueryGuid = 3,
            QueryPosition = 1,
            ReplyEvent = 0x15,
            ReplyGrouping = 0x33,
            ReplyGuid = 0x1f,
            ReplyPosition = 11,
            RequestGrouping = 4
        }
    }
}

