namespace nManager.Helpful
{
    using System;
    using System.IO;
    using System.IO.Compression;

    public class GZip
    {
        public static bool Compress(string filename, string toFile)
        {
            try
            {
                byte[] buffer = new byte[0x1000];
                if (!File.Exists(filename))
                {
                    return false;
                }
                using (FileStream stream = File.Open(filename, FileMode.Open))
                {
                    using (FileStream stream2 = File.Create(toFile))
                    {
                        using (GZipStream stream3 = new GZipStream(stream2, CompressionMode.Compress))
                        {
                            int num;
                            while ((num = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                stream3.Write(buffer, 0, num);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Logging.WriteError("Compress(string filename, string toFile): " + exception, true);
                return false;
            }
        }

        public static bool Decompress(string filename)
        {
            try
            {
                using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    string str = filename;
                    string path = str.Remove(str.Length - 3);
                    using (FileStream stream2 = File.Create(path))
                    {
                        using (GZipStream stream3 = new GZipStream(stream, CompressionMode.Decompress))
                        {
                            stream3.CopyTo(stream2);
                            return File.Exists(path);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.WriteError("Decompress(string filename): " + exception, true);
            }
            return false;
        }
    }
}

