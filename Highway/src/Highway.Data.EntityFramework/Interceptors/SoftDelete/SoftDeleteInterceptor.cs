using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;

namespace Highway.Data.EntityFramework.Interceptors.SoftDelete
{
    public class SoftDeleteInterceptor : IDbCommandTreeInterceptor
    {
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace == DataSpace.SSpace)
            {
                var queryCommand = interceptionContext.Result as DbQueryCommandTree;
                if (queryCommand != null)
                {
                    var newQuery = queryCommand.Query.Accept(new SoftDeleteQueryVisitor());
                    interceptionContext.Result = new DbQueryCommandTree(
                        queryCommand.MetadataWorkspace,
                        queryCommand.DataSpace,
                        newQuery);
                }


                var deleteCommand = interceptionContext.OriginalResult as DbDeleteCommandTree;
                if (deleteCommand != null)
                {
                    var column = SoftDeleteAttribute.GetSoftDeleteColumnName(deleteCommand.Target.VariableType.EdmType);
                    if (column != null)
                    {
                        // Just because the entity has the soft delete annotation doesn't mean that  
                        // this particular table has the column. This occurs in situation like TPT 
                        // inheritance mapping and entity splitting where one type maps to multiple  
                        // tables. 
                        // If the table doesn't have the column we just want to leave the row unchanged 
                        // since it will be joined to the table that does have the column during query. 
                        // We can't no-op, so we just generate an UPDATE command that doesn't set anything. 
                        var setClauses = new List<DbModificationClause>();
                        var table = (EntityType)deleteCommand.Target.VariableType.EdmType;
                        if (table.Properties.Any(p => p.Name == column))
                        {
                            setClauses.Add(DbExpressionBuilder.SetClause(
                                DbExpressionBuilder.Property(DbExpressionBuilder.Variable(deleteCommand.Target.VariableType, deleteCommand.Target.VariableName), column),
                                DbExpression.FromBoolean(true)));
                        }


                        var update = new DbUpdateCommandTree(
                            deleteCommand.MetadataWorkspace,
                            deleteCommand.DataSpace,
                            deleteCommand.Target,
                            deleteCommand.Predicate,
                            setClauses.AsReadOnly(),
                            null);


                        interceptionContext.Result = update;
                    }
                }
            }
        }
    }
}