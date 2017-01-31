namespace NHibernate.Json.Tests.Data
{
    public class ExampleEntityRepository : Repository<ExampleEntity>
    {
        public ExampleEntityRepository(ISession session)
            : base(session) {}
    }
}