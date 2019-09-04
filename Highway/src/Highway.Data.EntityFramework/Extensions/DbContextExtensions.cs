using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data.EntityFramework.Extensions
{
    internal static class DbContextExtensions
    {
        public static IDbCommand CreateSqlCommand(this DbContext context, string sql, params IDataParameter[] parameters)
        {
            var command = context.Database.Connection.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            parameters?.ForEach(parameter => command.Parameters.Add(parameter));
            return command;
        }

        public static IDbCommand CreateStoredProcedureCommand(this DbContext context, string storedProcedureName, params IDataParameter[] parameters)
        {
            var command = context.Database.Connection.CreateCommand();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;
            parameters?.ForEach(parameter => command.Parameters.Add(parameter));
            return command;
        }
    }
}
