namespace NHibernate.Json.Tests
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Newtonsoft.Json;
    using Util;
    using Xunit;

    public class JsonConvertorTests
    {
        [Fact]
        public void CanConfigureProperty()
        {
            JsonConvertor.Configure(x => x.TypeNameHandling = TypeNameHandling.None);

            Assert.Equal(TypeNameHandling.None, JsonConvertor.Settings.TypeNameHandling);
        }

        [Fact]
        public void CanSerialiseAsExpected()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var model = new ExampleJsonModel(id, new List<string> { "a", "b" });

            var serialised = JsonConvertor.Serialize(model);

            Assert.Equal("{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}", serialised);
        }

        [Fact]
        public void CanDeserialiseAsExpected()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var json = "{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}";
        
            var model = JsonConvertor.Deserialize<ExampleJsonModel>(json);
            
            Assert.Equal(id, model.Identifier);
            Assert.Equal("a", model.List.First());
        }

        [Fact]
        public void CanPopulateObject()
        {
            var id = new Guid("209EBBA5-F195-495F-BA68-48C32173BEC5");
            var json = "{\"identifier\":\"" + id + "\",\"list\":[\"a\",\"b\"]}";

            var model = new ExampleJsonModel(Guid.NewGuid(), new List<string>());
            JsonConvertor.PopulateObject(json, model);

            Assert.Equal(id, model.Identifier);
            Assert.Equal("a", model.List.First());
        }
    }
}