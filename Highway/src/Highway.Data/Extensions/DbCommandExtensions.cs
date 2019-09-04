using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Highway.Data.Extensions
{
    public static class DbCommandExtensions
    {
        public static void Execute(this IDbCommand cmd)
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();
        }

        public static T ExecuteWithResult<T>(this IDbCommand cmd, Func<IDbCommand, T> mapResults)
        {
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            cmd.ExecuteNonQuery();
            var result = mapResults(cmd);

            return result;
        }

        public static T ExecuteWithResult<T>(this IDbCommand cmd, Func<IDataReader, T> mapResult)
        {
            T result;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            using (var reader = cmd.ExecuteReader())
            {
                result = mapResult(reader);
            }

            return result;
        }

        public static IQueryable<T> ExecuteWithResults<T>(this IDbCommand cmd,
            Func<IDataReader, IEnumerable<T>> mapResults)
        {
            IQueryable<T> results;
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }

            using (var reader = cmd.ExecuteReader())
            {
                results = mapResults(reader).ToList().AsQueryable();
            }

            return results;
        }
    }
}