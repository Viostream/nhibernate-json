using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Json.Tests
{
    using Data;
    using Data.UnitTests;
    using Xunit;

    public class JsonColumnPersistanceTests : IDisposable
    {
        public JsonColumnPersistanceTests()
        {
            SessionFactoryProvider.Instance.Initialize();
        }

        [Fact]
        public void CanSetupSession()
        {
            var session = SessionFactoryProvider.Instance.OpenSession();

            var repo = new ExampleEntityRepository(session);
            Assert.NotNull(repo);
        }

        [Fact]
        public void Persist_NoCompression()
        {
            var session = SessionFactoryProvider.Instance.OpenSession();

            var id = 12;
            var identifer = Guid.NewGuid();
            var repo = new ExampleEntityRepository(session);
            var entity = new ExampleEntity(id, "Something", new ExampleJsonModel(identifer, new List<string>()));
            repo.Save(entity);
            repo.Flush();

            var found = repo.FindAll();

            Assert.Equal(1, found.Count);

            Assert.Equal(id, entity.Id);
            Assert.Equal("Something", found.First().Title);
            Assert.Equal(identifer, entity.Json.Identifier);
            session.Dispose();
        }

        [Fact]
        public void Persist_Compression()
        {
            var session = SessionFactoryProvider.Instance.OpenSession();
            JsonCompressor.CompressionThreshold = 10;
            var id = 12;
            var identifer = Guid.NewGuid();
            var repo = new ExampleEntityRepository(session);
            var entity = new ExampleEntity(id, "Something", new ExampleJsonModel(identifer, new List<string> {"AAAAAA","BBBBBBBB","CCCCCCC", "DDDDDDD"}));
            repo.Save(entity);
            repo.Flush();

            var found = repo.FindAll();

            Assert.Equal(1, found.Count);

            Assert.Equal(id, entity.Id);
            Assert.Equal("Something", found.First().Title);
            Assert.Equal(identifer, entity.Json.Identifier);
            Assert.Equal("AAAAAA", entity.Json.List.First());
            session.Dispose();
            JsonCompressor.CompressionThreshold = 10000;
        }

        public void Dispose()
        {
            SessionFactoryProvider.Instance.Dispose();
        }
    }
}
