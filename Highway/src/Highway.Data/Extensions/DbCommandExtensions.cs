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
            try
            {
                // ERIC:  Code defensively, or assumed closed connection?
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public static T ExecuteWithResult<T>(this IDbCommand cmd, Func<IDbCommand, T> mapResults)
        {
            T result;
            try
            {
                // ERIC:  Code defensively, or assumed closed connection?
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                result = mapResults(cmd);
            }
            finally
            {
                cmd.Connection.Close();
            }

            return result;
        }
        
        public static T ExecuteWithResult<T>(this IDbCommand cmd, Func<IDataReader, T> mapResult)
        {
            T result;
            try
            {
                cmd.Connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    result = mapResult(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return result;
        }

        public static IQueryable<T> ExecuteWithResults<T>(this IDbCommand cmd, Func<IDataReader, IEnumerable<T>> mapResults)
        {
            IQueryable<T> results;
            try
            {
                cmd.Connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    results = mapResults(reader).ToList().AsQueryable();
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return results;
        }
    }
}
