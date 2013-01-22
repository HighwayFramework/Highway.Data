using System;
using System.Data;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;

namespace Highway.Data
{
    public partial class DataContext : ISession
    {
        public void Flush()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Flush();
            }
        }

        public IDbConnection Disconnect()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Disconnect();
            }
        }

        public void Reconnect()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Reconnect();
            }
        }

        public void Reconnect(IDbConnection connection)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Reconnect(connection);
            }
        }

        public IDbConnection Close()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Close();
            }
        }

        public void CancelQuery()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.CancelQuery();
            }
        }

        public bool IsDirty()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.IsDirty();
            }
        }

        public bool IsReadOnly(object entityOrProxy)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.IsReadOnly(entityOrProxy);
            }
        }

        public void SetReadOnly(object entityOrProxy, bool readOnly)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.SetReadOnly(entityOrProxy, readOnly);
            }
        }

        public object GetIdentifier(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.GetIdentifier(obj);
            }
        }

        public bool Contains(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Contains(obj);
            }
        }

        public void Evict(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Evict(obj);
            }
        }

        public object Load(Type theType, object id, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Load(theType, id, lockMode);
            }
        }

        public object Load(string entityName, object id, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Load(entityName, id, lockMode);
            }
        }

        public object Load(Type theType, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Load(theType, id);
            }
        }

        public T Load<T>(object id, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Load<T>(id, lockMode);
            }
        }

        public T Load<T>(object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Load<T>(id);
            }
        }

        public object Load(string entityName, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Load(entityName, id);
            }
        }

        public void Load(object obj, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Load(obj, id);
            }
        }

        public void Replicate(object obj, ReplicationMode replicationMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Replicate(obj, replicationMode);
            }
        }

        public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Replicate(entityName, obj, replicationMode);
            }
        }

        public object Save(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Save(obj);
            }
        }

        public void Save(object obj, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Save(obj, id);
            }
        }

        public object Save(string entityName, object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Save(entityName, obj);
            }
        }

        public void SaveOrUpdate(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(obj);
            }
        }

        public void SaveOrUpdate(string entityName, object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.SaveOrUpdate(entityName, obj);
            }
        }

        public void Update(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Update(obj);
            }
        }

        public void Update(object obj, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Update(obj, id);
            }
        }

        public void Update(string entityName, object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Update(entityName, obj);
            }
        }

        public object Merge(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Merge(obj);
            }
        }

        public object Merge(string entityName, object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Merge(entityName, obj);
            }
        }

        public T Merge<T>(T entity) where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Merge(entity);
            }
        }

        public T Merge<T>(string entityName, T entity) where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Merge(entityName, entity);
            }
        }

        public void Persist(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Persist(obj);
            }
        }

        public void Persist(string entityName, object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Persist(entityName, obj);
            }
        }

        public object SaveOrUpdateCopy(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.SaveOrUpdateCopy(obj);
            }
        }

        public object SaveOrUpdateCopy(object obj, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.SaveOrUpdateCopy(obj, id);
            }
        }

        public void Delete(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Delete(obj);
            }
        }

        public void Delete(string entityName, object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Delete(entityName, obj);
            }
        }

        public int Delete(string query)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Delete(query);
            }
        }

        public int Delete(string query, object value, IType type)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Delete(query, value, type);
            }
        }

        public int Delete(string query, object[] values, IType[] types)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Delete(query, values, types);
            }
        }

        public void Lock(object obj, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Lock(obj, lockMode);
            }
        }

        public void Lock(string entityName, object obj, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Lock(entityName, obj, lockMode);
            }
        }

        public void Refresh(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Refresh(obj);
            }
        }

        public void Refresh(object obj, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Refresh(obj, lockMode);
            }
        }

        public LockMode GetCurrentLockMode(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.GetCurrentLockMode(obj);
            }
        }

        public ITransaction BeginTransaction()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.BeginTransaction();
            }
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.BeginTransaction(isolationLevel);
            }
        }

        public ICriteria CreateCriteria<T>() where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateCriteria<T>();
            }
        }

        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateCriteria<T>(alias);
            }
        }

        public ICriteria CreateCriteria(Type persistentClass)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateCriteria(persistentClass);
            }
        }

        public ICriteria CreateCriteria(Type persistentClass, string alias)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateCriteria(persistentClass, alias);
            }
        }

        public ICriteria CreateCriteria(string entityName)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateCriteria(entityName);
            }
        }

        public ICriteria CreateCriteria(string entityName, string alias)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateCriteria(entityName, alias);
            }
        }

        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<T>();
            }
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver(alias);
            }
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<T>(entityName);
            }
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver(entityName, alias);
            }
        }

        public IQuery CreateQuery(string queryString)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateQuery(queryString);
            }
        }

        public IQuery CreateFilter(object collection, string queryString)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateFilter(collection, queryString);
            }
        }

        public IQuery GetNamedQuery(string queryName)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.GetNamedQuery(queryName);
            }
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateSQLQuery(queryString);
            }
        }

        public void Clear()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.Clear();
            }
        }

        public object Get(Type clazz, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get(clazz, id);
            }
        }

        public object Get(Type clazz, object id, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get(clazz, id, lockMode);
            }
        }

        public object Get(string entityName, object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get(entityName, id);
            }
        }

        public T Get<T>(object id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public T Get<T>(object id, LockMode lockMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<T>(id, lockMode);
            }
        }

        public string GetEntityName(object obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.GetEntityName(obj);
            }
        }

        public IFilter EnableFilter(string filterName)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.EnableFilter(filterName);
            }
        }

        public IFilter GetEnabledFilter(string filterName)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.GetEnabledFilter(filterName);
            }
        }

        public void DisableFilter(string filterName)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                session.DisableFilter(filterName);
            }
        }

        public IMultiQuery CreateMultiQuery()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateMultiQuery();
            }
        }

        public ISession SetBatchSize(int batchSize)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.SetBatchSize(batchSize);
            }
        }

        public ISessionImplementor GetSessionImplementation()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.GetSessionImplementation();
            }
        }

        public IMultiCriteria CreateMultiCriteria()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.CreateMultiCriteria();
            }
        }

        public ISession GetSession(EntityMode entityMode)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.GetSession(entityMode);
            }
        }

        public EntityMode ActiveEntityMode { get; private set; }
        public FlushMode FlushMode { get; set; }
        public CacheMode CacheMode { get; set; }
        public ISessionFactory SessionFactory { get; private set; }
        public IDbConnection Connection { get; private set; }
        public bool IsOpen { get; private set; }
        public bool IsConnected { get; private set; }
        public bool DefaultReadOnly { get; set; }
        public ITransaction Transaction { get; private set; }
        public ISessionStatistics Statistics { get; private set; }
    }
}