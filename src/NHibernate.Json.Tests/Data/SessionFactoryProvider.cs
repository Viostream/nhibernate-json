namespace NHibernate.Json.Tests.Data
{
    namespace UnitTests
    {
        using Cfg;
        using FluentNHibernate.Cfg;
        using FluentNHibernate.Cfg.Db;
        using Tool.hbm2ddl;

        public class SessionFactoryProvider
        {
            private static SessionFactoryProvider _instance;
            private Configuration _configuration;

            private ISessionFactory _sessionFactory;

            private SessionFactoryProvider() {}

            public static SessionFactoryProvider Instance
            {
                get { return _instance ?? (_instance = new SessionFactoryProvider()); }
            }

            public void Initialize()
            {
                _sessionFactory = CreateSessionFactory();
            }

            private ISessionFactory CreateSessionFactory()
            {
                return Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<SessionFactoryProvider>())
                    .ExposeConfiguration(cfg => _configuration = cfg)
                    .BuildSessionFactory();
            }

            public ISession OpenSession()
            {
                ISession session = _sessionFactory.OpenSession();

                var export = new SchemaExport(_configuration);
                export.Execute(true, true, false, session.Connection, null);

                return session;
            }

            public void Dispose()
            {
                if (_sessionFactory != null)
                    _sessionFactory.Dispose();

                _sessionFactory = null;
                _configuration = null;
            }
        }
    }
}