using System.Data;
using System.Data.Entity;
using Highway.Data.Extensions;

namespace Highway.Data.EntityFramework.Extensions
{
    internal static class DbContextExtensions
    {
        public static IDbCommand CreateAdoCommand(this DbContext context, string query, params IDataParameter[] dataParameters)
        {
            return context.CreateDbCommand(query, CommandType.Text, dataParameters);
        }

        public static IDbCommand CreateDbCommand(this DbContext context, string commandText, CommandType commandType, params IDataParameter[] dataParameters)
        {
            var command = context.Database.Connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            dataParameters?.ForEach(dataParameter => command.Parameters.Add(dataParameter));
            return command;
        }

        public static IDbCommand CreateStoredProcedureCommand(this DbContext context, string storedProcedureName, params IDataParameter[] dataParameters)
        {
            return context.CreateDbCommand(storedProcedureName, CommandType.StoredProcedure, dataParameters);
        }
    }
}
