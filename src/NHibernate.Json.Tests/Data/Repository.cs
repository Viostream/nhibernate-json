namespace NHibernate.Json.Tests.Data
{
    using System;
    using System.Collections.Generic;

    public class Repository<T>
    {
        public Repository(ISession session)
        {
            Session = session;
        }

        public ISession Session { get; private set; }

        public IList<T> FindAll()
        {
            return Session.CreateCriteria(typeof (T))
                .List<T>();
        }

        public T Save(T entity)
        {
            Session.Save(entity);
            return entity;
        }

        public void Flush()
        {
            Session.Flush();
        }

        public T Find(object id)
        {
            return Session.Get<T>(id);
        }

        #region "Disposing"

        private bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Repository()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing) {}
                _disposed = true;
            }
        }

        #endregion
    }
}