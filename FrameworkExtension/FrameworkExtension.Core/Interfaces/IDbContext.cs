using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Data.Objects;
using System.Linq.Expressions;
using System.Data;

namespace FrameworkExtension.Core.Interfaces
{
    public interface IDbContext : IDisposable
    {
        IQueryable<T> AsQueryable<T>() where T : class;
        T Add<T>(T item) where T : class;
        T Add<T>(T item, bool changeState) where T : class;
        T Remove<T>(T item) where T : class;
        T Update<T>(T item) where T : class;
        T Attach<T>(T item) where T : class;
        T Detach<T>(T item) where T : class;
        T Reload<T>(T item) where T : class;
        void Reload<T>() where T : class;
        int Commit();

        IEnumerable<T> ExecuteSqlQuery<T>(string sql, params DbParameter[] dbParams);
        int ExecuteSqlCommand(string sql, params DbParameter[] dbParams);
        int ExecuteFunction(string procedureName, params ObjectParameter[] dbParams);
    }
}
