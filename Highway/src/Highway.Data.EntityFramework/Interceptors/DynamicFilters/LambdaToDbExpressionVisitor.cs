﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Highway.Data.EntityFramework.Interceptors.DynamicFilters
{
    internal class LambdaToDbExpressionVisitor : ExpressionVisitor
    {
        
        private readonly DynamicFilterDefinition _filter;
        private readonly DbExpressionBinding _binding;
        private readonly ObjectContext _objectContext;

        private readonly Dictionary<Expression, DbExpression> _expressionToDbExpressionMap = new Dictionary<Expression, DbExpression>();
        private readonly Dictionary<string, DbPropertyExpression> _properties = new Dictionary<string, DbPropertyExpression>();
        private readonly Dictionary<string, DbParameterReferenceExpression> _parameters = new Dictionary<string, DbParameterReferenceExpression>();

        
        
        public static DbExpression Convert(DynamicFilterDefinition filter, DbExpressionBinding binding, ObjectContext objectContext)
        {
            var visitor = new LambdaToDbExpressionVisitor(filter, binding, objectContext);
            var expression = visitor.Visit(filter.Predicate) as LambdaExpression;

            return visitor.GetDbExpressionForExpression(expression.Body);
        }

        private LambdaToDbExpressionVisitor(DynamicFilterDefinition filter, DbExpressionBinding binding, ObjectContext objectContext)
        {
            _filter = filter;
            _binding = binding;
            _objectContext = objectContext;
        }

        
        
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var expression = base.VisitBinary(node) as BinaryExpression;

            DbExpression dbExpression;

            //  Need special handling for comparisons against the null constant.  If we don't translate these
            //  using an "IsNull" expression, EF will convert it literally as "= null" which doesn't work in SQL Server.
            if (IsNullConstantExpression(expression.Right))
                dbExpression = MapNullComparison(expression.Left, expression.NodeType);
            else if (IsNullConstantExpression(expression.Left))
                dbExpression = MapNullComparison(expression.Right, expression.NodeType);
            else
            {
                DbExpression leftExpression = GetDbExpressionForExpression(expression.Left);
                DbExpression rightExpression = GetDbExpressionForExpression(expression.Right);

                switch (expression.NodeType)
                {
                    case ExpressionType.Equal:
                        //  DbPropertyExpression = class property that has been mapped to a database column
                        //  DbParameterReferenceExpression = lambda parameter
                        if (IsNullableExpressionOfType<DbPropertyExpression>(leftExpression) && IsNullableExpressionOfType<DbParameterReferenceExpression>(rightExpression))
                            dbExpression = CreateEqualComparisonOfNullablePropToNullableParam(leftExpression, rightExpression);
                        else if (IsNullableExpressionOfType<DbPropertyExpression>(rightExpression) && IsNullableExpressionOfType<DbParameterReferenceExpression>(leftExpression))
                            dbExpression = CreateEqualComparisonOfNullablePropToNullableParam(rightExpression, leftExpression);
                        else
                            dbExpression = leftExpression.Equal(rightExpression);
                        break;
                    case ExpressionType.NotEqual:
                        dbExpression = leftExpression.NotEqual(rightExpression);
                        break;
                    case ExpressionType.GreaterThan:
                        dbExpression = leftExpression.GreaterThan(rightExpression);
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        dbExpression = leftExpression.GreaterThanOrEqual(rightExpression);
                        break;
                    case ExpressionType.LessThan:
                        dbExpression = leftExpression.LessThan(rightExpression);
                        break;
                    case ExpressionType.LessThanOrEqual:
                        dbExpression = leftExpression.LessThanOrEqual(rightExpression);
                        break;

                    case ExpressionType.AndAlso:
                        dbExpression = leftExpression.And(rightExpression);
                        break;
                    case ExpressionType.OrElse:
                        dbExpression = leftExpression.Or(rightExpression);
                        break;

                    default:
                        throw new NotImplementedException(string.Format("Unhandled NodeType of {0} in LambdaToDbExpressionVisitor.VisitBinary", expression.NodeType));
                }
            }

            MapExpressionToDbExpression(expression, dbExpression);

            return expression;
        }

        /// <summary>
        /// Creates an Equal comparison of a nullable property (db column) to a nullable parameter (lambda param)
        /// that adds the necessary "is null" checks to support a filter like "e.TenantId = tenantId".
        /// Results in sql: (e.TenantID is null and @tenantID is null) or (e.TenantID is not null and e.TenantID = @tenantID)
        /// which will support parmeter values that are "null" or a specific value and will correctly filter on columns that
        /// are "null" or a specific value.
        /// </summary>
        /// <param name="propExpression"></param>
        /// <param name="paramExpression"></param>
        /// <returns></returns>
        private DbExpression CreateEqualComparisonOfNullablePropToNullableParam(DbExpression propExpression, DbExpression paramExpression)
        {
            var condition1 = propExpression.IsNull().And(paramExpression.IsNull());
            var condition2 = propExpression.IsNull().Not().And(propExpression.Equal(paramExpression));
            return condition1.Or(condition2);
        }

        /// <summary>
        /// Maps a comparison of an expression to a "null" constant.
        /// </summary>
        /// <returns></returns>
        private DbExpression MapNullComparison(Expression expression, ExpressionType comparisonType)
        {
            DbExpression dbExpression = GetDbExpressionForExpression(expression).IsNull();

            switch (comparisonType)
            {
                case ExpressionType.Equal:
                    return dbExpression;
                case ExpressionType.NotEqual:
                    //  Creates expression: !([expression] is null)
                    return dbExpression.Not();
            }

            throw new NotImplementedException(string.Format("Unhandled comparisonType of {0} in LambdaToDbExpressionVisitor.MapNullComparison", comparisonType));
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            var expression = base.VisitConstant(node);

            var type = node.Type;
            if (IsNullableType(type))
            {
                var genericArgs = type.GetGenericArguments();
                if ((genericArgs != null) && (genericArgs.Length == 1))
                    type = genericArgs[0];
            }

            if (type == typeof(byte[]))
                MapExpressionToDbExpression(expression, DbExpression.FromBinary((byte[])node.Value));
            else if (type == typeof(bool))
                MapExpressionToDbExpression(expression, DbExpression.FromBoolean((bool?)node.Value));
            else if (type == typeof(byte))
                MapExpressionToDbExpression(expression, DbExpression.FromByte((byte?)node.Value));
            else if (type == typeof(DateTime))
                MapExpressionToDbExpression(expression, DbExpression.FromDateTime((DateTime?)node.Value));
            else if (type == typeof(DateTimeOffset))
                MapExpressionToDbExpression(expression, DbExpression.FromDateTimeOffset((DateTimeOffset?)node.Value));
            else if (type == typeof(decimal))
                MapExpressionToDbExpression(expression, DbExpression.FromDecimal((decimal?)node.Value));
            else if (type == typeof(double))
                MapExpressionToDbExpression(expression, DbExpression.FromDouble((double?)node.Value));
            else if (type == typeof(Guid))
                MapExpressionToDbExpression(expression, DbExpression.FromGuid((Guid?)node.Value));
            else if (type == typeof(Int16))
                MapExpressionToDbExpression(expression, DbExpression.FromInt16((Int16?)node.Value));
            else if (type == typeof(Int32))
                MapExpressionToDbExpression(expression, DbExpression.FromInt32((Int32?)node.Value));
            else if (type.IsEnum)
                MapExpressionToDbExpression(expression, DbExpression.FromInt32((Int32)node.Value));
            else if (type == typeof(Int64))
                MapExpressionToDbExpression(expression, DbExpression.FromInt64((Int64?)node.Value));
            else if (type == typeof(float))
                MapExpressionToDbExpression(expression, DbExpression.FromSingle((float?)node.Value));
            else if (type == typeof(string))
                MapExpressionToDbExpression(expression, DbExpression.FromString((string)node.Value));
            else
                throw new NotImplementedException(string.Format("Unhandled Type of {0} for Constant value {1} in LambdaToDbExpressionVisitor.VisitConstant", node.Type.Name, node.Value ?? "null"));

            return expression;
        }

        /// <summary>
        /// Visit a Member Expression.  Creates a mapping of the MemberExpression to a DbPropertyExpression
        /// which is a reference to the table/column name that matches the MemberExpression.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            var expression = base.VisitMember(node) as MemberExpression;

            if ((expression.Expression.NodeType == ExpressionType.Parameter) && (expression.Expression.Type.IsClass || expression.Expression.Type.IsInterface))
            {
                //  expression is a reference to a class/interface property.  Need to map it to a sql parameter or look up
                //  the existing parameter.
                //  The class/interface is defined by expression.Expression while the property is in expression.Member.
                string propertyName;
                if (IsNullableType(expression.Member.ReflectedType))
                {
                    var subExpression = expression.Expression as MemberExpression;
                    propertyName = subExpression.Member.Name;
                }
                else
                    propertyName = expression.Member.Name;

                //  TODO: To support different class/interfaces, can we figure out the correct binding from what's in expression.Expression?
                var edmType = _binding.VariableType.EdmType as EntityType;

                DbPropertyExpression propertyExpression;
                if (!_properties.TryGetValue(propertyName, out propertyExpression))
                {
                    //  Not created yet

                    //  Need to map through the EdmType properties to find the actual database/cspace name for the entity property.
                    //  It may be different from the entity property!
                    var edmProp = edmType.Properties.FirstOrDefault(p => p.MetadataProperties.Any(m => m.Name == "PreferredName" && m.Value.Equals(propertyName)));
                    if (edmProp == null)
                    {
                        //  Accessing properties outside the main entity is not supported and will cause this exception.
                        throw new ApplicationException(string.Format("Property {0} not found in Entity Type {1}", propertyName, expression.Expression.Type.Name));
                    }
                    //  database column name is now in edmProp.Name.  Use that instead of filter.ColumnName

                    propertyExpression = _binding.VariableType.Variable(_binding.VariableName).Property(edmProp.Name);
                    _properties.Add(propertyName, propertyExpression);
                }

                //  Nothing else to do here
                MapExpressionToDbExpression(expression, propertyExpression);
                return expression;
            }

            //  We are accessing a member property such that expression.Expression is the object and expression.Member is the property.
            //  And the property is one that requires special handling.  Regular class properties are all handled up above.
            var objectExpression = GetDbExpressionForExpression(expression.Expression);

            DbExpression dbExpression;
            switch(expression.Member.Name)
            {
                case "HasValue":
                    //  Map HasValue to !IsNull
                    dbExpression = objectExpression.IsNull().Not();
                    break;
                case "Value":
                    //  This is a nullable Value accessor so just map to the object itself and it will be mapped for us
                    dbExpression = objectExpression;
                    break;
                default:
                    throw new ApplicationException(string.Format("Unhandled property accessor in expression: {0}", expression));
            }

            MapExpressionToDbExpression(expression, dbExpression);
            return expression;
        }

        /// <summary>
        /// Visit a Parameter Expression.  Creates a mapping from the ParameterExpression to a
        /// DbParameterReferenceExpression which is a reference to a SQL Parameter bound to the
        /// table being queries (_Binding).
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitParameter(ParameterExpression node)
        {
            var expression = base.VisitParameter(node);

            if (node.Type.IsClass || node.Type.IsInterface)
                return expression;      //  Ignore class or interface param

            if (_parameters.ContainsKey(node.Name))
                return expression;      //  Already created sql parameter for this node.Name

            //  Create a new DbParameterReferenceExpression for this parameter.
            var param = CreateParameter(node.Name, node.Type);

            MapExpressionToDbExpression(expression, param);
            return expression;
        }

        private DbParameterReferenceExpression CreateParameter(string name, Type type)
        {
            DbParameterReferenceExpression param;
            if (_parameters.TryGetValue(name, out param))
                return param;

            var typeUsage = TypeUsageForPrimitiveType(type);
            string dynFilterParamName = _filter.CreateDynamicFilterName(name);
            param = typeUsage.Parameter(dynFilterParamName);

            _parameters.Add(name, param);

            return param;
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            var expression = base.VisitUnary(node) as UnaryExpression;

            DbExpression operandExpression = GetDbExpressionForExpression(expression.Operand);

            switch (expression.NodeType)
            {
                case ExpressionType.Not:
                    MapExpressionToDbExpression(expression, operandExpression.Not());
                    break;
                case ExpressionType.Convert:
                    MapExpressionToDbExpression(expression, operandExpression.CastTo(TypeUsageForPrimitiveType(expression.Type)));
                    break;
                default:
                    throw new NotImplementedException(string.Format("Unhandled NodeType of {0} in LambdaToDbExpressionVisitor.VisitUnary", expression.NodeType));
            }

            return expression;
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            throw new NotImplementedException("Conditionals in Lambda expressions are not supported");
            //return base.VisitConditional(node);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var expression = base.VisitMethodCall(node) as MethodCallExpression;

            switch(node.Method.Name)
            {
                case "Contains":
                    MapContainsExpression(expression);
                    break;
                case "StartsWith":
                    MapStartsWithExpression(expression);
                    break;
                default:
                    throw new NotImplementedException(string.Format("Unhandled Method of {0} in LambdaToDbExpressionVisitor.VisitMethodCall", node.Method.Name));
            }

            return expression;
        }

        private void MapContainsExpression(MethodCallExpression expression)
        {
            DbExpression argExpression = GetDbExpressionForExpression(expression.Arguments[0]);

            DbExpression dbExpression;

            var collectionObjExp = expression.Object as ParameterExpression;
            if (collectionObjExp != null)
            {
                //  collectionObjExp is a parameter expression.  This means the content of the collection is
                //  dynamic.  DbInExpression only supports a fixed size list of constant values.
                //  So the only way to handle a dynamic collection is for us to create a single Equals expression
                //  with a DbParameterReference.  Then when we intercept that parameter, we will see that it's
                //  for a collection and we will modify the SQL to change it from an "=" to an "in".  The single
                //  Parameter Reference is set to the first value in the collection and the rest of the values
                //  are inserted into the SQL "in" clause.
                string paramName = collectionObjExp.Name;
                Type paramType = PrimitiveTypeForType(collectionObjExp.Type);

                var param = CreateParameter(paramName, paramType);
                dbExpression = argExpression.Equal(param);
            }
            else
            {
                var listExpression = expression.Object as ListInitExpression;
                if (listExpression == null)
                    throw new NotSupportedException(string.Format("Unsupported object type used in Contains() - type = {0}", expression.Object.GetType().Name));

                //  This is a fixed size list that may contain parameter references or constant values.
                //  This can be handled using either a DbInExpression (if all are constants) or with
                //  a series of OR conditions.
                //  Find all of the constant & parameter expressions.
                var constantExpressionList = listExpression.Initializers
                    .Select(i => i.Arguments.FirstOrDefault() as ConstantExpression)
                    .Where(c => (c != null) && (c.Value != null))       //  null not supported - can only use DbConstant in "In" expression
                    .Select(c => CreateConstantExpression(c.Value))
                    .ToList();
                constantExpressionList.AddRange(listExpression.Initializers
                    .Select(i => i.Arguments.FirstOrDefault() as UnaryExpression)
                    .Where(c => (c != null) && (c.Operand is ConstantExpression))
                    .Select(c => CreateConstantExpression(((ConstantExpression)c.Operand).Value)));
                var parameterExpressionList = listExpression.Initializers
                    .Select(i => i.Arguments.FirstOrDefault() as ParameterExpression)
                    .Where(c => c != null)
                    .Select(c => CreateParameter(c.Name, c.Type))
                    .ToList();

                if (constantExpressionList.Count + parameterExpressionList.Count != listExpression.Initializers.Count)
                    throw new NotSupportedException(string.Format("Unrecognized parameters in Contains list.  Null parameters not supported."));

                if (parameterExpressionList.Any())
                {
                    //  Have parameters so need to build a series of OR conditions so that we can include the
                    //  DbParameterReferences.  EF will optimize this into an "in" condition but with our
                    //  DbParameterReferences preserved (which is not possible with a DbInExpression).
                    //  The DbParameterReferences will be intercepted as any other parameter.
                    dbExpression = null;
                    var allExpressions = parameterExpressionList.Cast<DbExpression>().Union(constantExpressionList.Cast<DbExpression>());
                    foreach (var paramReference in allExpressions)
                    {
                        var equalsExpression = argExpression.Equal(paramReference);
                        if (dbExpression == null)
                            dbExpression = equalsExpression;
                        else
                            dbExpression = dbExpression.Or(equalsExpression);
                    }
                }
                else
                {
                    //  All values are constants so can use DbInExpression
                    dbExpression = argExpression.In(constantExpressionList);
                }
            }

            MapExpressionToDbExpression(expression, dbExpression);
        }

        private void MapStartsWithExpression(MethodCallExpression expression)
        {
            if ((expression.Arguments == null) || (expression.Arguments.Count != 1))
                throw new ApplicationException("Did not find exactly 1 Argument to StartsWith function");

            DbExpression srcExpression = GetDbExpressionForExpression(expression.Object);

            DbExpression dbExpression;

            if (expression.Arguments[0] is ConstantExpression)
            {
                var constantExpression = GetDbExpressionForExpression(expression.Arguments[0]) as DbConstantExpression;
                if ((constantExpression == null) || (constantExpression.Value == null))
                    throw new NullReferenceException("Parameter to StartsWith cannot be null");

                dbExpression = srcExpression.Like(DbExpressionBuilder.Constant(constantExpression.Value.ToString() + "%"));
            }
            else
            {
                var argExpression = GetDbExpressionForExpression(expression.Arguments[0]);

                //  Note: Can also do this using StartsWith function on srcExpression (which avoids having to hardcode the % character).
                //  It works but generates some crazy conditions using charindex which I don't think will use indexes as well as "like"...
                //dbExpression = DbExpressionBuilder.Equal(DbExpressionBuilder.True, srcExpression.StartsWith(argExpression));

                dbExpression = srcExpression.Like(argExpression.Concat(DbExpressionBuilder.Constant("%")));
            }

            MapExpressionToDbExpression(expression, dbExpression);
        }

        private DbConstantExpression CreateConstantExpression(object value)
        {
            //  This is not currently supported (DbExpressionBuilder.Constant throws exceptions).  But, DbConstant has
            //  other constructors available that do take nullable types.  Maybe those would work.
            if (value == null)
                throw new ApplicationException("null is not convertable to a DbConstantExpression");

            //  Must map Enums to an int or EF/SQL will not know what to do with them.
            if (value.GetType().IsEnum)
                return DbExpressionBuilder.Constant((int)value);

            return DbExpressionBuilder.Constant(value);
        }

        
        
        private void MapExpressionToDbExpression(Expression expression, DbExpression dbExpression)
        {
            _expressionToDbExpressionMap[expression] = dbExpression;
        }

        private DbExpression GetDbExpressionForExpression(Expression expression)
        {
            var paramExpression = expression as ParameterExpression;
            if (paramExpression != null)
            {
                string paramName = paramExpression.Name;
                Type paramType = PrimitiveTypeForType(paramExpression.Type);

                return CreateParameter(paramName, paramType);
            }

            DbExpression dbExpression;
            if (!_expressionToDbExpressionMap.TryGetValue(expression, out dbExpression))
                throw new FormatException(string.Format("DbExpression not found for expression: {0}", expression));

            return dbExpression;
        }

        private TypeUsage TypeUsageForPrimitiveType(Type type)
        {
            return TypeUsageForPrimitiveType(type, _objectContext);
        }

        public static TypeUsage TypeUsageForPrimitiveType(Type type, ObjectContext objectContext)
        {
            bool isNullable = IsNullableType(type);
            type = PrimitiveTypeForType(type);

            //  Find equivalent EdmType in CSpace.  This is a 1-to-1 mapping to CLR types except for the Geometry/Geography types
            //  (so not supporting those atm).
            var primitiveTypeList = objectContext.MetadataWorkspace
                .GetPrimitiveTypes(DataSpace.CSpace)
                .Where(p => p.ClrEquivalentType == type)
                .ToList();
            if (primitiveTypeList.Count != 1)
                throw new ApplicationException(string.Format("Unable to map parameter of type {0} to TypeUsage.  Found {1} matching types", type.Name, primitiveTypeList.Count));
            var primitiveType = primitiveTypeList.FirstOrDefault();

            var facetList = new List<Facet>();
            if (isNullable)
            {
                //  May not even be necessary to specify these Facets, but just to be safe.  And only way to create them is to call the internal Create method...
                var createMethod = typeof(Facet).GetMethod("Create", BindingFlags.NonPublic | BindingFlags.Static, null, new Type[] { typeof(FacetDescription), typeof(object) }, null);

                var facetDescription = MetadataItem.GetGeneralFacetDescriptions().FirstOrDefault(fd => fd.FacetName == "Nullable");
                if (facetDescription != null)
                    facetList.Add((Facet)createMethod.Invoke(null, new object[] { facetDescription, true }));

                facetDescription = MetadataItem.GetGeneralFacetDescriptions().FirstOrDefault(fd => fd.FacetName == "DefaultValue");
                if (facetDescription != null)
                    facetList.Add((Facet)createMethod.Invoke(null, new object[] { facetDescription, null }));
            }

            return TypeUsage.Create(primitiveType, facetList);
        }

        /// <summary>
        /// Returns the primitive type of the Type.  If this is a collection type, this is the type of the objects inside the collection.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Type PrimitiveTypeForType(Type type)
        {
            if (type.IsEnum)
                type = typeof(Int32);

            if (type.IsGenericType)
            {
                //  Generic type of some sort.  Could be IEnumerable<T> or INullable<T>.  The primitive type is in the
                //  Generic Arguments.
                var genericArgs = type.GetGenericArguments();
                if ((genericArgs != null) && (genericArgs.Length == 1))
                    return genericArgs[0];
            }

            if ((typeof(IEnumerable).IsAssignableFrom(type) && (type != typeof(String))) || (type == typeof(object)))
            {
                //  Non-generic collection (such as ArrayList) are not supported.  We require that we are able to enforce correct
                //  type matching between the collection and the db column.
                throw new NotSupportedException("Non generic collections or System.Object types are not supported");
            }

            return type;
        }

        /// <summary>
        /// Returns true if the expression is a DbPropertyExpression (i.e. a class property that has been mapped
        /// to a database column) and the type is a Nullable type.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private static bool IsNullableExpressionOfType<T>(DbExpression expression)
            where T : DbExpression
        {
            if (expression == null)
                return false;

            if (!typeof(T).IsAssignableFrom(expression.GetType()))
                return false;

            return expression.ResultType.Facets.Any(f => f.Name == "Nullable" && ((bool)f.Value));
        }

        private static bool IsNullableType(Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Returns true if the expression is for the "null" constant
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private bool IsNullConstantExpression(Expression expression)
        {
            UnaryExpression unaryExpr = expression as UnaryExpression;
            if (unaryExpr == null)
                return false;

            var constantExpr = unaryExpr.Operand as ConstantExpression;
            if (constantExpr == null)
                return false;

            return (constantExpr.Value == null);
        }

            }
}
