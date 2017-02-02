namespace NHibernate.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Data;
    using Xunit;
    using Xunit.Sdk;

    public class JsonCompressorTests
    {
        [Fact]
        public void ConfigureThreshold_Success()
        {
            JsonCompressor.CompressionThreshold = 10000;

            Assert.Equal(10000, JsonCompressor.CompressionThreshold);
        }

        [Fact]
        public void CompressZlib_Success()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised, threshold:10);
            Assert.DoesNotContain("{\"identifier\":", compressed);
        }

        [Fact]
        public void CompressZlib_Success_UnderThreshold()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", compressed);
        }

        [Fact]
        public void DecrompressZlib_Success()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised, threshold:10);
            var decompressed = JsonCompressor.Decompress(compressed);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }

        [Fact]
        public void DecompressZlib_Success_UnderThreshold()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised);
            var decompressed = JsonCompressor.Decompress(compressed);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }

        [Fact]
        public void CompressGzip_Success()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised, Compressor.Gzip, 10);
            Assert.DoesNotContain("{\"identifier\":", compressed);
        }

        [Fact]
        public void CompressGzip_Success_UnderThreshold()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised, Compressor.Gzip);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", compressed);
        }

        [Fact]
        public void DecompressGzip_Success()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised, Compressor.Gzip, 10);
            var decompressed = JsonCompressor.Decompress(compressed, Compressor.Gzip);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }

        [Fact]
        public void DecompressGzip_Success_UnderThreshold()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonWorker.Serialize(model);
            var compressed = JsonCompressor.Compress(serialised, Compressor.Gzip, 10);
            var decompressed = JsonCompressor.Decompress(compressed, Compressor.Gzip);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }
    }
}