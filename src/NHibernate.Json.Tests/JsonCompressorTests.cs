namespace NHibernate.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Xunit;

    public class JsonCompressorTests
    {
        [Fact]
        public void CanConfigureProperty()
        {
            JsonCompressor.CompressionThreshold = 10;

            Assert.Equal(10, JsonCompressor.CompressionThreshold);
        }

        [Fact]
        public void CanZlibCompressAsExpected()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 10;
            var compressed = JsonCompressor.Compress(serialised);
            //Assert.Equal();
            Assert.DoesNotContain("{\"identifier\":", compressed);
        }

        [Fact]
        public void CanZlibCompressWhenUnderThreshold()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 100000;
            var compressed = JsonCompressor.Compress(serialised);
            //Assert.Equal();
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", compressed);
        }

        [Fact]
        public void CanZlibDecompressAsExpected()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 10;
            var compressed = JsonCompressor.Compress(serialised);

            var decompressed = JsonCompressor.Decompress(compressed);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }

        [Fact]
        public void CanZlibDecompressWhenNotCompressed()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 10;

            var decompressed = JsonCompressor.Decompress(serialised);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }

        [Fact]
        public void CanGzipCompressAsExpected()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 10;
            var compressed = JsonCompressor.Compress(serialised, Compressor.Gzip);
            //Assert.Equal();
            Assert.DoesNotContain("{\"identifier\":", compressed);
        }

        [Fact]
        public void CanGzipCompressWhenUnderThreshold()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 100000;
            var compressed = JsonCompressor.Compress(serialised, Compressor.Gzip);
            //Assert.Equal();
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", compressed);
        }

        [Fact]
        public void CanGzipDecompressAsExpected()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 10;
            var compressed = JsonCompressor.Compress(serialised, Compressor.Gzip);

            var decompressed = JsonCompressor.Decompress(compressed, Compressor.Gzip);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }

        [Fact]
        public void CanGzipDecompressWhenNotCompressed()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);
            JsonCompressor.CompressionThreshold = 10;

            var decompressed = JsonCompressor.Decompress(serialised, Compressor.Gzip);
            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", decompressed);
        }
    }
}