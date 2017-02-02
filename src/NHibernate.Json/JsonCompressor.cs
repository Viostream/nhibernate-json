namespace NHibernate.Json
{
    using System;
    using Compression;

    public class JsonCompressor
    {
        public static int CompressionThreshold = 2000;

        public static string Compress(string value, Compressor compressor = Compressor.Zlib, int threshold = 0)
        {
            threshold = threshold > 0 ? threshold : CompressionThreshold;
            if (!string.IsNullOrEmpty(value) && value.Length < threshold)
                return value;

            return compressor == Compressor.Gzip ? GzipStreamCompressor.Compress(value) : DeflateStreamCompressor.Compress(value);
        }

        public static string Decompress(string value, Compressor compressor = Compressor.Zlib)
        {
            if (value == null)
                return "{}";

            if (value.StartsWith("{"))
                return value;

            byte[] bytes;
            var isZipped = value.StartsWith("H4sI");

            if (isZipped)
                compressor = Compressor.Gzip;
            try
            {
                bytes = Convert.FromBase64String(value);
            }
            catch (Exception)
            {
                return value;
            }

            return compressor == Compressor.Gzip ? GzipStreamCompressor.Decompress(bytes) : DeflateStreamCompressor.Decompress(bytes);
        }
    }

    public enum Compressor
    {
        Gzip,
        Zlib
    }
}