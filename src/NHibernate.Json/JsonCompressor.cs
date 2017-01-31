namespace NHibernate.Json
{
    using System;
    using Compression;

    public class JsonCompressor
    {
        public static int CompressionThreshold = 2000;

        public static string Compress(string value, Compressor compressor = Compressor.Zlib)
        {
            if (!string.IsNullOrEmpty(value) && value.Length < CompressionThreshold)
                return value;

            switch (compressor)
            {
                case Compressor.Gzip:
                    return GzipStreamCompressor.Compress(value);
                case Compressor.Zlib:
                    return DeflateStreamCompressor.Compress(value);
                default:
                    return DeflateStreamCompressor.Compress(value);
            }
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

            switch (compressor)
            {
                case Compressor.Gzip:
                    return GzipStreamCompressor.Decompress(bytes);
                case Compressor.Zlib:
                    return DeflateStreamCompressor.Decompress(bytes);
                default:
                    return DeflateStreamCompressor.Decompress(bytes);
            }
        }
    }

    public enum Compressor
    {
        Gzip,
        Zlib
    }
}