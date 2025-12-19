using System;
using System.IO;
using NHibernate.Json;

namespace NHibernate.Json.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || (args[0] != "compress" && args[0] != "decompress"))
            {
                Console.Error.WriteLine("Usage: NHibernate.Json.Cli [compress|decompress]");
                Environment.Exit(1);
            }

            if (args[0] == "compress")
            {
                // Read all input as string
                using var reader = new StreamReader(Console.OpenStandardInput());
                string inputText = reader.ReadToEnd();
                string compressed = JsonCompressor.Compress(inputText);
                Console.Out.Write(compressed);
            }
            else if (args[0] == "decompress")
            {
                using var reader = new StreamReader(Console.OpenStandardInput());
                string base64Input = reader.ReadToEnd();
                string decompressed = JsonCompressor.Decompress(base64Input);
                Console.Out.Write(decompressed);
            }
        }
    }
}
