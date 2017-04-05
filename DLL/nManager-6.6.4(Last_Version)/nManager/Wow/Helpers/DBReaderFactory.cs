namespace nManager.Wow.Helpers
{
    using System;
    using System.IO;
    using System.Text;

    public class DBReaderFactory
    {
        public static IClientDBReader GetReader(string file, Table def)
        {
            IClientDBReader reader = null;
            string str = Path.GetExtension(file).ToUpperInvariant();
            if (!(str == ".DB2"))
            {
                throw new InvalidDataException(string.Format("Unknown file type {0}", str));
            }
            uint maxValue = uint.MaxValue;
            using (FileStream stream = new FileStream(file, FileMode.Open))
            {
                using (BinaryReader reader2 = new BinaryReader(stream, Encoding.UTF8))
                {
                    maxValue = reader2.ReadUInt32();
                }
            }
            switch (maxValue)
            {
                case 0x35424457:
                    return reader;
            }
            return new AnielOcoihij(file);
        }
    }
}

