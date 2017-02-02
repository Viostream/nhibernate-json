namespace NHibernate.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Xunit;

    public class JsonColumnTypeTests
    {

        [Fact]
        public void DeepCopy_Success()
        {
            var id = Guid.NewGuid();
            var value = (object)(new ExampleJsonModel(id, new List<string>{"1","2"}));
            var type = new JsonColumnType<ExampleJsonModel>();
            var result = type.DeepCopy(value) as ExampleJsonModel;

            Assert.NotNull(result);
            Assert.Equal(id, result.Identifier);
        }

        [Fact]
        public void DeepCopy_Null()
        {
            object value = null;
            var type = new JsonColumnType<ExampleJsonModel>();

            var result = type.DeepCopy(value);
            Assert.Null(result);
        }

        [Fact]
        public void Serialise_Success()
        {
            var id = new Guid("7D1E8984-15FD-4FCD-85FF-DF248C1E056B");
            var type = new JsonColumnType<ExampleJsonModel>();
            var value = new ExampleJsonModel(id, new List<string> { "1", "2" });
            var result = type.Serialise(value);
            Assert.NotNull(result);
            Assert.Equal("{\"identifier\":\"7d1e8984-15fd-4fcd-85ff-df248c1e056b\",\"list\":[\"1\",\"2\"]}", result);
        }

        [Fact]
        public void Serialise_Null()
        {
            var type = new JsonColumnType<ExampleJsonModel>();
            var result = type.Serialise(null);
            Assert.NotNull(result);
            Assert.Equal("{}", result);
        }

        [Fact]
        public void Deserialise_Success()
        {
            var id = new Guid("7D1E8984-15FD-4FCD-85FF-DF248C1E056B");
            var value  = "{\"identifier\":\""+id+"\",\"list\":[\"1\",\"2\"]}";
            var type = new JsonColumnType<ExampleJsonModel>();
            var result = type.Deserialise(value);
            Assert.NotNull(result);
            Assert.Equal(id, result.Identifier);
        }

        [Fact]
        public void Deserialise_Null()
        {
            var type = new JsonColumnType<ExampleJsonModel>();
            var result = type.Deserialise(null);
            Assert.NotNull(result);
        }

        [Fact]
        public void Deserialise_EmptyString()
        {
            var type = new JsonColumnType<ExampleJsonModel>();
            var result = type.Deserialise("");
            Assert.NotNull(result);
        }

        [Fact]
        public void Equals_Success()
        {
            var id = Guid.NewGuid();
            var type = new JsonColumnType<ExampleJsonModel>();
            var result = type.Equals(new ExampleJsonModel(id, new List<string>()), new ExampleJsonModel(id, new List<string>()));
            Assert.True(result);
        }

        [Fact]
        public void Equals_Fail()
        {
            var id = Guid.NewGuid();
            var type = new JsonColumnType<ExampleJsonModel>();
            var result = type.Equals(string.Empty, new ExampleJsonModel(id, new List<string>()));
            Assert.False(result);
        }
    }
}