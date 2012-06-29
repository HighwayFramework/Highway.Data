using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using Highway.Data.Interfaces;

namespace Highway.Data.EntityFramework.Tests.UnitTests
{
    public class EFFailureContext : IDataContext
    {
        public IQueryable<T> AsQueryable<T>()
        {
            throw new NotImplementedException();
        }

        public T Add<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Add<T>(T item, bool changeState) where T : class
        {
            throw new NotImplementedException();
        }

        public T Remove<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Update<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Attach<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Detach<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public T Reload<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Reload<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public int Commit()
        {
            throw new NotImplementedException();
        }

        public int ExecuteFunction(string procedureName, params ObjectParameter[] dbParams)
        {
            throw new NotImplementedException();
        }

        public IEventManager EventManager
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int ExecuteSqlCommand(string sql, params DbParameter[] dbParams)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        IQueryable<T> IDataContext.AsQueryable<T>()
        {
            throw new NotImplementedException();
        }
    }
}