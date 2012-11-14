using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace Highway.Data
{
    public class DbContext<T> : IDocumentSession where T: IDocumentSession
    {
        private readonly IDocumentSession _session;

        public DbContext(IDocumentSession session)
        {
            _session = session;
        }

        public void Dispose()
        {
            _session.Dispose();
        }

        public void Delete<T1>(T1 entity)
        {
            _session.Delete(entity);
        }

        public T1 Load<T1>(string id)
        {
            return _session.Load<T1>(id);
        }

        public T1[] Load<T1>(params string[] ids)
        {
            return _session.Load<T1>(ids);
        }

        public T1[] Load<T1>(IEnumerable<string> ids)
        {
            return _session.Load<T1>(ids);
        }

        public T1 Load<T1>(ValueType id)
        {
            return _session.Load<T1>(id);
        }

        public IRavenQueryable<T1> Query<T1>(string indexName)
        {
            return _session.Query<T1>(indexName);
        }

        public IRavenQueryable<T1> Query<T1>()
        {
            return _session.Query<T1>();
        }

        public IRavenQueryable<T1> Query<T1, TIndexCreator>() where TIndexCreator : AbstractIndexCreationTask, new()
        {
            return _session.Query<T1, TIndexCreator>();
        }

        public ILoaderWithInclude<object> Include(string path)
        {
            return _session.Include(path);
        }

        public ILoaderWithInclude<T1> Include<T1>(Expression<Func<T1, object>> path)
        {
            return _session.Include(path);
        }

        public ILoaderWithInclude<T1> Include<T1, TInclude>(Expression<Func<T1, object>> path)
        {
            return _session.Include(path);
        }

        public void SaveChanges()
        {
            _session.SaveChanges();
        }

        public void Store(object entity, Guid etag)
        {
            _session.Store(entity,etag);
        }

        public void Store(object entity, Guid etag, string id)
        {
            _session.Store(entity,etag,id);
        }

        public void Store(dynamic entity)
        {
            _session.Store(entity);
        }

        public void Store(dynamic entity, string id)
        {
            _session.Store(entity,id);
        }

        public ISyncAdvancedSessionOperation Advanced { get; private set; }
    }
}