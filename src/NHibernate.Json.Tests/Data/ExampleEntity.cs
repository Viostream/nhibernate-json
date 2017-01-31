namespace NHibernate.Json.Tests.Data
{
    using System;
    using System.Collections.Generic;
    using FluentNHibernate.Mapping;

    public class ExampleEntity
    {
        protected ExampleEntity()
        {
            
        }
        
        public ExampleEntity(int id, string title, ExampleJsonModel json)
        {
            Id = id;
            Title = title;
            Json = json;
        }

        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual ExampleJsonModel Json { get; set; }
    }

    public class ExampleJsonModel
    {
        public ExampleJsonModel(Guid identifier, List<string> list)
        {
            Identifier = identifier;
            List = list;
        }

        public Guid Identifier { get; set; }
        public List<string> List { get; set; }
    }

    public class ExampleEntityMap : ClassMap<ExampleEntity>
    {
        public ExampleEntityMap()
        {
            Table("ExampleEntities");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Title);
            Map(x => x.Json).CustomType<JsonColumnType<ExampleJsonModel>>().Nullable();
        }
    }
}