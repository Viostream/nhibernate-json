namespace NHibernate.Json.Compression
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    internal class DeflateStreamCompressor
    {
        public static string Compress(string value)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var zipStream = new DeflateStream(memoryStream, CompressionMode.Compress))
                using (var writer = new StreamWriter(zipStream, Encoding.Unicode))
                {
                    writer.Write(value);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        public static string Decompress(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            using (var zipStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(zipStream, Encoding.Unicode))
            {
                return reader.ReadToEnd();
            }
        }
    }
}