#region

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

#endregion

namespace Highway.Data
{
    public partial class DataContext : IDocumentSession
    {
        private readonly IDocumentSession _session;

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

        public void Store(object entity, Etag etag)
        {
            _session.Store(entity, etag);
        }

        public void Store(object entity, Etag etag, string id)
        {
            _session.Store(entity, etag, id);
        }

        public void Store(dynamic entity)
        {
            _session.Store(entity);
        }

        public void Store(dynamic entity, string id)
        {
            _session.Store(entity, id);
        }

        public ISyncAdvancedSessionOperation Advanced { get; private set; }


        public TResult[] Load<TTransformer, TResult>(IEnumerable<string> ids, Action<ILoadConfiguration> configure)
            where TTransformer : AbstractTransformerCreationTask, new()
        {
            return _session.Load<TTransformer, TResult>(ids, configure);
        }

        public TResult[] Load<TTransformer, TResult>(params string[] ids)
            where TTransformer : AbstractTransformerCreationTask, new()
        {
            return _session.Load<TTransformer, TResult>(ids);
        }

        public TResult Load<TTransformer, TResult>(string id, Action<ILoadConfiguration> configure)
            where TTransformer : AbstractTransformerCreationTask, new()
        {
            return _session.Load<TTransformer, TResult>(id, configure);
        }

        public TResult Load<TTransformer, TResult>(string id)
            where TTransformer : AbstractTransformerCreationTask, new()
        {
            return _session.Load<TTransformer, TResult>(id);
        }

        public T[] Load<T>(IEnumerable<ValueType> ids)
        {
            return _session.Load<T>(ids);
        }

        public T[] Load<T>(params ValueType[] ids)
        {
            return _session.Load<T>(ids);
        }

        public IRavenQueryable<T> Query<T>(string indexName, bool isMapReduce = false)
        {
            return _session.Query<T>(indexName, isMapReduce);
        }

        public IRavenQueryable<T1> Query<T1>(string indexName)
        {
            return _session.Query<T1>(indexName);
        }
    }
}