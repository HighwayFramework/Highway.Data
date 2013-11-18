#region

using System;
using System.Data;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;

#endregion

namespace Highway.Data
{
    public partial class DataContext : ISession
    {
        public void Flush()
        {
            _session.Flush();
        }

        public IDbConnection Disconnect()
        {
            return _session.Disconnect();
        }

        public void Reconnect()
        {
            _session.Reconnect();
        }

        public void Reconnect(IDbConnection connection)
        {
            _session.Reconnect(connection);
        }

        public IDbConnection Close()
        {
            return _session.Close();
        }

        public void CancelQuery()
        {
            _session.CancelQuery();
        }

        public bool IsDirty()
        {
            return _session.IsDirty();
        }

        public bool IsReadOnly(object entityOrProxy)
        {
            return _session.IsReadOnly(entityOrProxy);
        }

        public void SetReadOnly(object entityOrProxy, bool readOnly)
        {
            _session.SetReadOnly(entityOrProxy, readOnly);
        }

        public object GetIdentifier(object obj)
        {
            return _session.GetIdentifier(obj);
        }

        public bool Contains(object obj)
        {

            {
                return _session.Contains(obj);
            }
        }

        public void Evict(object obj)
        {

            {
                _session.Evict(obj);
            }
        }

        public object Load(Type theType, object id, LockMode lockMode)
        {

            {
                return _session.Load(theType, id, lockMode);
            }
        }

        public object Load(string entityName, object id, LockMode lockMode)
        {

            {
                return _session.Load(entityName, id, lockMode);
            }
        }

        public object Load(Type theType, object id)
        {

            {
                return _session.Load(theType, id);
            }
        }

        public T Load<T>(object id, LockMode lockMode)
        {

            {
                return _session.Load<T>(id, lockMode);
            }
        }

        public T Load<T>(object id)
        {

            {
                return _session.Load<T>(id);
            }
        }

        public object Load(string entityName, object id)
        {

            {
                return _session.Load(entityName, id);
            }
        }

        public void Load(object obj, object id)
        {

            {
                _session.Load(obj, id);
            }
        }

        public void Replicate(object obj, ReplicationMode replicationMode)
        {

            {
                _session.Replicate(obj, replicationMode);
            }
        }

        public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
        {

            {
                _session.Replicate(entityName, obj, replicationMode);
            }
        }

        public object Save(object obj)
        {

            {
                return _session.Save(obj);
            }
        }

        public void Save(object obj, object id)
        {

            {
                _session.Save(obj, id);
            }
        }

        public object Save(string entityName, object obj)
        {

            {
                return _session.Save(entityName, obj);
            }
        }

        public void SaveOrUpdate(object obj)
        {

            {
                _session.SaveOrUpdate(obj);
            }
        }

        public void SaveOrUpdate(string entityName, object obj)
        {

            {
                _session.SaveOrUpdate(entityName, obj);
            }
        }

        public void Update(object obj)
        {

            {
                _session.Update(obj);
            }
        }

        public void Update(object obj, object id)
        {

            {
                _session.Update(obj, id);
            }
        }

        public void Update(string entityName, object obj)
        {

            {
                _session.Update(entityName, obj);
            }
        }

        public object Merge(object obj)
        {

            {
                return _session.Merge(obj);
            }
        }

        public object Merge(string entityName, object obj)
        {

            {
                return _session.Merge(entityName, obj);
            }
        }

        public T Merge<T>(T entity) where T : class
        {

            {
                return _session.Merge(entity);
            }
        }

        public T Merge<T>(string entityName, T entity) where T : class
        {

            {
                return _session.Merge(entityName, entity);
            }
        }

        public void Persist(object obj)
        {

            {
                _session.Persist(obj);
            }
        }

        public void Persist(string entityName, object obj)
        {

            {
                _session.Persist(entityName, obj);
            }
        }

        public object SaveOrUpdateCopy(object obj)
        {

            {
                return _session.SaveOrUpdateCopy(obj);
            }
        }

        public object SaveOrUpdateCopy(object obj, object id)
        {

            {
                return _session.SaveOrUpdateCopy(obj, id);
            }
        }

        public void Delete(object obj)
        {

            {
                _session.Delete(obj);
            }
        }

        public void Delete(string entityName, object obj)
        {

            {
                _session.Delete(entityName, obj);
            }
        }

        public int Delete(string query)
        {

            {
                return _session.Delete(query);
            }
        }

        public int Delete(string query, object value, IType type)
        {

            {
                return _session.Delete(query, value, type);
            }
        }

        public int Delete(string query, object[] values, IType[] types)
        {

            {
                return _session.Delete(query, values, types);
            }
        }

        public void Lock(object obj, LockMode lockMode)
        {

            {
                _session.Lock(obj, lockMode);
            }
        }

        public void Lock(string entityName, object obj, LockMode lockMode)
        {

            {
                _session.Lock(entityName, obj, lockMode);
            }
        }

        public void Refresh(object obj)
        {

            {
                _session.Refresh(obj);
            }
        }

        public void Refresh(object obj, LockMode lockMode)
        {

            {
                _session.Refresh(obj, lockMode);
            }
        }

        public LockMode GetCurrentLockMode(object obj)
        {

            {
                return _session.GetCurrentLockMode(obj);
            }
        }

        public ITransaction BeginTransaction()
        {

            {
                return _session.BeginTransaction();
            }
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {

            {
                return _session.BeginTransaction(isolationLevel);
            }
        }

        public ICriteria CreateCriteria<T>() where T : class
        {

            {
                return _session.CreateCriteria<T>();
            }
        }

        public ICriteria CreateCriteria<T>(string alias) where T : class
        {

            {
                return _session.CreateCriteria<T>(alias);
            }
        }

        public ICriteria CreateCriteria(Type persistentClass)
        {

            {
                return _session.CreateCriteria(persistentClass);
            }
        }

        public ICriteria CreateCriteria(Type persistentClass, string alias)
        {

            {
                return _session.CreateCriteria(persistentClass, alias);
            }
        }

        public ICriteria CreateCriteria(string entityName)
        {

            {
                return _session.CreateCriteria(entityName);
            }
        }

        public ICriteria CreateCriteria(string entityName, string alias)
        {

            {
                return _session.CreateCriteria(entityName, alias);
            }
        }

        public IQueryOver<T, T> QueryOver<T>() where T : class
        {

            {
                return _session.QueryOver<T>();
            }
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {

            {
                return _session.QueryOver(alias);
            }
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {

            {
                return _session.QueryOver<T>(entityName);
            }
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
        {

            {
                return _session.QueryOver(entityName, alias);
            }
        }

        public IQuery CreateQuery(string queryString)
        {

            {
                return _session.CreateQuery(queryString);
            }
        }

        public IQuery CreateFilter(object collection, string queryString)
        {

            {
                return _session.CreateFilter(collection, queryString);
            }
        }

        public IQuery GetNamedQuery(string queryName)
        {

            {
                return _session.GetNamedQuery(queryName);
            }
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {

            {
                return _session.CreateSQLQuery(queryString);
            }
        }

        public void Clear()
        {

            {
                _session.Clear();
            }
        }

        public object Get(Type clazz, object id)
        {

            {
                return _session.Get(clazz, id);
            }
        }

        public object Get(Type clazz, object id, LockMode lockMode)
        {

            {
                return _session.Get(clazz, id, lockMode);
            }
        }

        public object Get(string entityName, object id)
        {

            {
                return _session.Get(entityName, id);
            }
        }

        public T Get<T>(object id)
        {

            {
                return _session.Get<T>(id);
            }
        }

        public T Get<T>(object id, LockMode lockMode)
        {

            {
                return _session.Get<T>(id, lockMode);
            }
        }

        public string GetEntityName(object obj)
        {

            {
                return _session.GetEntityName(obj);
            }
        }

        public IFilter EnableFilter(string filterName)
        {

            {
                return _session.EnableFilter(filterName);
            }
        }

        public IFilter GetEnabledFilter(string filterName)
        {

            {
                return _session.GetEnabledFilter(filterName);
            }
        }

        public void DisableFilter(string filterName)
        {

            {
                _session.DisableFilter(filterName);
            }
        }

        public IMultiQuery CreateMultiQuery()
        {

            {
                return _session.CreateMultiQuery();
            }
        }

        public ISession SetBatchSize(int batchSize)
        {

            {
                return _session.SetBatchSize(batchSize);
            }
        }

        public ISessionImplementor GetSessionImplementation()
        {

            {
                return _session.GetSessionImplementation();
            }
        }

        public IMultiCriteria CreateMultiCriteria()
        {

            {
                return _session.CreateMultiCriteria();
            }
        }

        public ISession GetSession(EntityMode entityMode)
        {

            {
                return _session.GetSession(entityMode);
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