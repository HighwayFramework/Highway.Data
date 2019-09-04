using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data.EntityFramework.Extensions
{
    internal static class DbContextExtensions
    {
        public static IDbCommand CreateAdoCommand(this DbContext context, string query, params IDataParameter[] parameters)
        {
            return context.CreateDbCommand(query, CommandType.Text, parameters);
        }

        public static IDbCommand CreateDbCommand(this DbContext context, string commandText, CommandType commandType, params IDataParameter[] parameters)
        {
            var command = context.Database.Connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            parameters?.ForEach(parameter => command.Parameters.Add(parameter));
            return command;
        }

        public static IDbCommand CreateStoredProcedureCommand(this DbContext context, string storedProcedureName, params IDataParameter[] parameters)
        {
            return context.CreateDbCommand(storedProcedureName, CommandType.StoredProcedure, parameters);
        }
    }
}
