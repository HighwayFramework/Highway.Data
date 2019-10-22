using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Highway.Data.EntityFramework.Interceptors.DynamicFilters
{
    public static class DynamicFilterExtensions
    {
        /// <summary>
        ///     Key: Filter Name
        ///     Value: The parameters for the filter
        /// </summary>
        private static readonly ConcurrentDictionary<string, DynamicFilterParameters> GlobalParameterValues =
            new ConcurrentDictionary<string, DynamicFilterParameters>();

        /// <summary>
        ///     Key: The DbContext to which the scoped parameter values belong
        ///     Values: A dictionary defined as _GlobalParameterValues that contains the scoped parameter values for the DbContext
        /// </summary>
        private static readonly ConcurrentDictionary<DbContext, ConcurrentDictionary<string, DynamicFilterParameters>>
            ScopedParameterValues =
                new ConcurrentDictionary<DbContext, ConcurrentDictionary<string, DynamicFilterParameters>>();

        private static bool _initialized;

        /// <summary>
        ///     Initialize the Dynamic Filters.  Adding a Filter now automatically calls this method.
        /// </summary>
        /// <param name="context"></param>
        public static void InitializeDynamicFilters(this DbContext context)
        {
            if (_initialized)
                return;

            _initialized = true;
            DbInterception.Add(new DynamicFilterCommandInterceptor());
            DbInterception.Add(new DynamicFilterInterceptor());
        }

        public static void Filter<TEntity, TProperty>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, TProperty>> path, Func<object> globalFuncValue) where TEntity : class
        {
            modelBuilder.Filter(filterName, path, (object) globalFuncValue);
        }

        public static void Filter<TEntity, TProperty>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, TProperty>> path, object globalValue = null)
        {
            InitializeDynamicFilters(null);

            filterName = ScrubFilterName(filterName);

            //  If ParseColumnNameFromExpression returns null, path is a lambda expression, not a single column expression. 
            LambdaExpression predicate = null;
            var columnName = ParseColumnNameFromExpression(path);
            if (columnName == null)
                predicate = path;

            modelBuilder.Conventions.Add(new DynamicFilterConvention(filterName, typeof (TEntity), predicate, columnName));

            //  Always add the filter to _GlobalParameterValues - need it to be able to disable it
            GlobalParameterValues.TryAdd(filterName, new DynamicFilterParameters());

            if (globalValue != null)
                SetFilterGlobalParameterValue(null, filterName, columnName, globalValue);
        }

        public static void Filter<TEntity, T0>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, bool>> predicate, Func<T0> value0) where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0);
        }

        public static void Filter<TEntity, T0>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, bool>> predicate, T0 value0) where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0);
        }

        public static void Filter<TEntity, T0, T1>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, bool>> predicate, Func<T0> value0, Func<T1> value1) where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1);
        }

        public static void Filter<TEntity, T0, T1>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, bool>> predicate, T0 value0, T1 value1) where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1);
        }

        public static void Filter<TEntity, T0, T1, T2>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, T2, bool>> predicate, Func<T0> value0, Func<T1> value1, Func<T2> value2)
            where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1, (object) value2);
        }

        public static void Filter<TEntity, T0, T1, T2>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, T2, bool>> predicate, T0 value0, T1 value1, T2 value2)
            where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1, (object) value2);
        }

        public static void Filter<TEntity, T0, T1, T2, T3>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, T2, T3, bool>> predicate, Func<T0> value0, Func<T1> value1, Func<T2> value2,
            Func<T3> value3) where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1, (object) value2,
                (object) value3);
        }

        public static void Filter<TEntity, T0, T1, T2, T3>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, T2, T3, bool>> predicate, T0 value0, T1 value1, T2 value2, T3 value3)
            where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1, (object) value2,
                (object) value3);
        }

        public static void Filter<TEntity, T0, T1, T2, T3, T4>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, T2, T3, T4, bool>> predicate, Func<T0> value0, Func<T1> value1,
            Func<T2> value2, Func<T3> value3, Func<T4> value4) where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1, (object) value2,
                (object) value3, (object) value4);
        }

        public static void Filter<TEntity, T0, T1, T2, T3, T4>(this DbModelBuilder modelBuilder, string filterName,
            Expression<Func<TEntity, T0, T1, T2, T3, T4, bool>> predicate, T0 value0, T1 value1, T2 value2, T3 value3,
            T4 value4) where TEntity : class
        {
            Filter<TEntity>(modelBuilder, filterName, predicate, (object) value0, (object) value1, (object) value2,
                (object) value3, (object) value4);
        }

        private static void Filter<TEntity>(DbModelBuilder modelBuilder, string filterName, LambdaExpression predicate,
            params object[] valueList)
        {
            InitializeDynamicFilters(null);

            filterName = ScrubFilterName(filterName);

            modelBuilder.Conventions.Add(new DynamicFilterConvention(filterName, typeof (TEntity), predicate));

            //  Always add the filter to _GlobalParameterValues - need it to be able to disable it
            GlobalParameterValues.TryAdd(filterName, new DynamicFilterParameters());

            var numParams = predicate.Parameters.Count;
            var numValues = valueList == null ? 0 : valueList.Length;
            for (var i = 1; i < numParams; i++)
            {
                var value = ((i - 1) < numValues) ? valueList[i - 1] : null;
                SetFilterGlobalParameterValue(null, filterName, predicate.Parameters[i].Name, value);
            }
        }

        //  Setting a parameter to null will also disable that parameter

        /// <summary>
        ///     Enable the filter.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        public static void EnableFilter(this DbContext context, string filterName)
        {
            var filterParams = GetOrCreateScopedFilterParameters(context, filterName);
            filterParams.Enabled = true;
        }

        /// <summary>
        ///     Enable all filters.
        /// </summary>
        /// <param name="context"></param>
        public static void EnableAllFilters(this DbContext context)
        {
            foreach (var filterName in GlobalParameterValues.Keys.ToList())
                EnableFilter(context, filterName);
        }

        /// <summary>
        ///     Disable the filter within the current DbContext scope.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        public static void DisableFilter(this DbContext context, string filterName)
        {
            var filterParams = GetOrCreateScopedFilterParameters(context, filterName);
            filterParams.Enabled = false;
        }

        /// <summary>
        ///     Disable all filters within the current DbContext scope.
        /// </summary>
        /// <param name="context"></param>
        public static void DisableAllFilters(this DbContext context)
        {
            foreach (var filterName in GlobalParameterValues.Keys.ToList())
                DisableFilter(context, filterName);
        }

        /// <summary>
        ///     Globally disable the filter.  Can be enabled as needed via DbContext.EnableFilter().
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="filterName"></param>
        public static void DisableFilterGlobally(this DbModelBuilder modelBuilder, string filterName)
        {
            filterName = ScrubFilterName(filterName);

            GlobalParameterValues.AddOrUpdate(filterName,
                f =>
                {
                    var newValues = new DynamicFilterParameters();
                    newValues.Enabled = false;
                    return newValues;
                },
                (f, currValues) =>
                {
                    currValues.Enabled = false;
                    return currValues;
                });
        }

        /// <summary>
        ///     Set the parameter for a filter within the current DbContext scope.  Once the DbContext is disposed, this
        ///     parameter will no longer be in scope and will be removed.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <param name="func">
        ///     A delegate that returns the value of the parameter.  This will be evaluated each time
        ///     the parameter value is needed.
        /// </param>
        public static void SetFilterScopedParameterValue(this DbContext context, string filterName, Func<object> func)
        {
            context.SetFilterScopedParameterValue(filterName, null, (object) func);
        }

        /// <summary>
        ///     Set the parameter for a filter within the current DbContext scope.  Once the DbContext is disposed, this
        ///     parameter will no longer be in scope and will be removed.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <param name="value"></param>
        public static void SetFilterScopedParameterValue(this DbContext context, string filterName, object value)
        {
            context.SetFilterScopedParameterValue(filterName, null, value);
        }

        /// <summary>
        ///     Set the parameter for a filter within the current DbContext scope.  Once the DbContext is disposed, this
        ///     parameter will no longer be in scope and will be removed.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <param name="parameterName"></param>
        /// <param name="func"></param>
        public static void SetFilterScopedParameterValue(this DbContext context, string filterName, string parameterName,
            Func<object> func)
        {
            context.SetFilterScopedParameterValue(filterName, parameterName, (object) func);
        }

        /// <summary>
        ///     Set the parameter for a filter within the current DbContext scope.  Once the DbContext is disposed, this
        ///     parameter will no longer be in scope and will be removed.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        public static void SetFilterScopedParameterValue(this DbContext context, string filterName, string parameterName,
            object value)
        {
            filterName = ScrubFilterName(filterName);

            var filterParams = GetOrCreateScopedFilterParameters(context, filterName);

            if (string.IsNullOrEmpty(parameterName))
                parameterName = GetDefaultParameterNameForFilter(filterName);

            var globalFilterParams = GlobalParameterValues[filterName]; //  Already validated that this exists.
            if (!globalFilterParams.ParameterValues.ContainsKey(parameterName))
                throw new ApplicationException(string.Format("Parameter {0} not found in Filter {1}", parameterName,
                    filterName));

            filterParams.SetParameter(parameterName, value);
        }

        /// <summary>
        ///     Set the parameter value for a filter with global scope.  If a scoped parameter value is not found, this
        ///     value will be used.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <param name="func">
        ///     A delegate that returns the value of the parameter.  This will be evaluated each time
        ///     the parameter value is needed.
        /// </param>
        public static void SetFilterGlobalParameterValue(this DbContext context, string filterName, Func<object> func)
        {
            context.SetFilterGlobalParameterValue(filterName, (object) func);
        }

        /// <summary>
        ///     Set the parameter value for a filter with global scope.  If a scoped parameter value is not found, this
        ///     value will be used.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <param name="value"></param>
        public static void SetFilterGlobalParameterValue(this DbContext context, string filterName, object value)
        {
            context.SetFilterGlobalParameterValue(filterName, null, value);
        }

        public static void SetFilterGlobalParameterValue(this DbContext context, string filterName, string parameterName,
            Func<object> func)
        {
            context.SetFilterGlobalParameterValue(filterName, parameterName, (object) func);
        }

        public static void SetFilterGlobalParameterValue(this DbContext context, string filterName, string parameterName,
            object value)
        {
            filterName = ScrubFilterName(filterName);

            if (string.IsNullOrEmpty(parameterName))
                parameterName = GetDefaultParameterNameForFilter(filterName);

            GlobalParameterValues.AddOrUpdate(filterName,
                f =>
                {
                    var newValues = new DynamicFilterParameters();
                    newValues.SetParameter(parameterName, value);
                    return newValues;
                },
                (f, currValues) =>
                {
                    currValues.SetParameter(parameterName, value);
                    return currValues;
                });
        }

        /// <summary>
        ///     Returns the value for the filter.  If a scoped value exists within this DbContext, that is returned.
        ///     Otherwise, a global parameter value will be returned.  If the parameter was set with a delegate, the
        ///     delegate is evaluated and the result is returned.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static object GetFilterParameterValue(this DbContext context, string filterName, string parameterName)
        {
            //  First check to see if this the Disabled parameter.
            if (parameterName == DynamicFilterConstants.FilterDisabledName)
                return context.IsFilterEnabled(filterName) ? null : (object) true;

            filterName = ScrubFilterName(filterName);
            if (parameterName == null)
                parameterName = string.Empty;

            ConcurrentDictionary<string, DynamicFilterParameters> contextFilters;
            DynamicFilterParameters filterParams;
            object value;

            //  First try to get the value from _ScopedParameterValues
            if (ScopedParameterValues.TryGetValue(context, out contextFilters))
            {
                if (contextFilters.TryGetValue(filterName, out filterParams))
                {
                    if (filterParams.ParameterValues.TryGetValue(parameterName, out value))
                    {
                        var func = value as Func<object>;
                        return (func == null) ? value : func();
                    }
                }
            }

            //  Then try _GlobalParameterValues
            if (GlobalParameterValues.TryGetValue(filterName, out filterParams))
            {
                if (filterParams.ParameterValues.TryGetValue(parameterName, out value))
                {
                    var func = value as MulticastDelegate;
                    return (func == null) ? value : func.DynamicInvoke();
                }
            }

            //  Not found anywhere???
            return null;
        }

        /// <summary>
        ///     Checks to see if the filter is currently enabled based on the DbContext or global settings.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filterName"></param>
        /// <returns></returns>
        public static bool IsFilterEnabled(this DbContext context, string filterName)
        {
            filterName = ScrubFilterName(filterName);

            ConcurrentDictionary<string, DynamicFilterParameters> contextFilters;
            DynamicFilterParameters filterParams;

            //  If specifically enabled/disabled on local scope, that overrides anything global
            if (ScopedParameterValues.TryGetValue(context, out contextFilters))
            {
                if (contextFilters.TryGetValue(filterName, out filterParams))
                {
                    if (filterParams.Enabled.HasValue)
                        return filterParams.Enabled.Value;
                }
            }

            //  Otherwise, we look to the global Enabled flag.
            if (GlobalParameterValues.TryGetValue(filterName, out filterParams))
            {
                if (filterParams.Enabled.HasValue)
                    return filterParams.Enabled.Value;
            }

            //  Otherwise, default to true
            return true;
        }

        /// <summary>
        ///     Clear all parameter values within the DbContext scope.
        /// </summary>
        /// <param name="context"></param>
        public static void ClearScopedParameters(this DbContext context)
        {
            ConcurrentDictionary<string, DynamicFilterParameters> contextFilters;
            ScopedParameterValues.TryRemove(context, out contextFilters);

            Debug.Print("Cleared scoped parameters.  Have {0} scopes", ScopedParameterValues.Count);
        }

        internal static void SetSqlParameters(this DbContext context, DbCommand command)
        {
            foreach (DbParameter param in command.Parameters)
            {
                if (!param.ParameterName.StartsWith(DynamicFilterConstants.ParameterNamePrefix))
                    continue;

                //  parts are:
                //  1 = Fixed string constant (DynamicFilterConstants.PARAMETER_NAME_PREFIX)
                //  2 = Filter Name (delimiter char is scrubbed from this field when creating a filter)
                //  3+ = Column Name (this can contain the delimiter char)
                var parts = param.ParameterName.Split(new[] {DynamicFilterConstants.Delimeter}, StringSplitOptions.None);
                if (parts.Length < 3)
                    continue;

                //  If Column contains _ character, it will be split up.  Need to re-combine it.  Delimiter has to be
                //  a character that is valid in a C# variable name (or EF will throw an exception when setting it).
                var columnName = parts[2];
                if (parts.Length > 3)
                {
                    for (var i = 3; i < parts.Length; i++)
                        columnName = string.Concat(columnName, "_", parts[i]);
                }

                var value = context.GetFilterParameterValue(parts[1], columnName); //  Middle is the filter name

                //  If not found, leave as the default that EF assigned (which will be a DBNull)
                if (value == null)
                    continue;

                //  Check to see if it's a collection.  If so, this is an "In" parameter
                var valueType = value.GetType();
                if (valueType.IsGenericType && typeof (IEnumerable).IsAssignableFrom(valueType))
                {
                    //  Generic collection.  The EF query created for this collection was an '=' condition.
                    //  We need to convert this into an 'in' clause so that we can dynamically set the
                    //  values in the collection.
                    SetParameterList(value as IEnumerable, param, command);
                }
                else
                    param.Value = value;
            }
        }

        /// <summary>
        ///     Set a collection of parameter values in place of a single parameter.  The '=' condition in the sql
        ///     command is changed to an 'in' expression.
        /// </summary>
        /// <param name="paramValueCollection"></param>
        /// <param name="param"></param>
        /// <param name="command"></param>
        private static void SetParameterList(IEnumerable paramValueCollection, DbParameter param, DbCommand command)
        {
            var paramStartIdx = command.CommandText.IndexOf(param.ParameterName);
            var startIdx = command.CommandText.LastIndexOf("=", paramStartIdx);

            var inCondition = new StringBuilder();
            inCondition.Append("in (@");
            inCondition.Append(param.ParameterName);

            var isFirst = true;
            foreach (var singleValue in paramValueCollection)
            {
                var value = singleValue;
                if (singleValue != null)
                {
                    //  If this is an Enum, need to cast it to an int
                    if (singleValue.GetType().IsEnum)
                        value = (int) singleValue;
                }

                if (isFirst)
                {
                    //  The first item in the list is set as the value of the sql parameter
                    param.Value = value;
                }
                else
                {
                    //  Remaining valus must be inserted directly into the sql 'in' clause
                    inCondition.AppendFormat(", {0}", QuotedValue(param, value));
                }

                isFirst = false;
            }

            inCondition.Append(")");

            command.CommandText = string.Concat(command.CommandText.Substring(0, startIdx),
                inCondition,
                command.CommandText.Substring(paramStartIdx + param.ParameterName.Length));
        }

        private static string QuotedValue(DbParameter param, object value)
        {
            if (value == null)
                return "null";

            if (value is bool)
                return (bool) value ? "1" : "0";
            if (value is DateTime)
                return string.Format("'{0:yyyy-MM-dd HH:mm:ss.FFF}'", (DateTime) value);

            if (DbTypeIsNumeric(param.DbType))
                return value.ToString();
            return string.Format("'{0}'", value);
        }

        private static bool DbTypeIsNumeric(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Byte:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.Single:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.VarNumeric:
                    return true;
            }

            return false;
        }

        private static string ScrubFilterName(string filterName)
        {
            //  Do not allow the delimiter char in the filter name at all because it will interfere with us parsing out
            //  the filter name from the parameter name.  Doesn't matter in column name though.
            return filterName.Replace(DynamicFilterConstants.Delimeter, "");
        }

        private static DynamicFilterParameters GetOrCreateScopedFilterParameters(DbContext context, string filterName)
        {
            filterName = ScrubFilterName(filterName);

            if (!GlobalParameterValues.ContainsKey(filterName))
                throw new ApplicationException(string.Format("Filter name {0} not found", filterName));

            var newContextFilters = new ConcurrentDictionary<string, DynamicFilterParameters>();
            var contextFilters = ScopedParameterValues.GetOrAdd(context, newContextFilters);
            var filterParams = contextFilters.GetOrAdd(filterName, p => new DynamicFilterParameters());

            if (contextFilters == newContextFilters)
            {
                Debug.Print("Created new scoped filter params.  Have {0} scopes", ScopedParameterValues.Count);

                //  We created new filter params for this scope.  Add an event handler to the OnDispose to clean them up when
                //  the context is disposed.
                var internalContext = typeof (DbContext)
                    .GetProperty("InternalContext", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetGetMethod(true)
                    .Invoke(context, null);

                var eventInfo = internalContext.GetType()
                    .GetEvent("OnDisposing", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                eventInfo.AddEventHandler(internalContext,
                    new EventHandler<EventArgs>((o, e) => context.ClearScopedParameters()));
            }

            return filterParams;
        }

        private static string GetDefaultParameterNameForFilter(string filterName)
        {
            //  If the parameter name is not specified, find it from the global parameters.  There must be exactly
            //  1 parameter in the filter for this to work.  If multiple parameters are used, the parameterName
            //  must be specified when the value is set.
            filterName = ScrubFilterName(filterName);

            DynamicFilterParameters globalFilterParams;
            if (!GlobalParameterValues.TryGetValue(filterName, out globalFilterParams))
                throw new ApplicationException(string.Format("Filter name {0} not found", filterName));

            if (globalFilterParams.ParameterValues.Count != 1)
                throw new ApplicationException(
                    "Attempted to set Scoped Parameter without specifying Parameter Name and when filter does not contain exactly 1 parameter");

            return globalFilterParams.ParameterValues.Keys.FirstOrDefault();
        }

        private static string ParseColumnNameFromExpression(LambdaExpression expression)
        {
            if (expression == null)
                throw new ArgumentNullException("Lambda expression is null");

            var body = expression.Body as MemberExpression;
            if ((body == null) || string.IsNullOrEmpty(body.Member.Name))
            {
                //  The expression does not specify a column - it's a lambda expression/predicate that will need to
                //  be expanded by LabdaToDbExprssionVisitor during the query evaluation.
                return null;
            }

            var propertyExpression = body.Expression as MemberExpression;
            if ((propertyExpression != null) && (propertyExpression.Member.Name != body.Member.Name))
            {
                //  The expression is a property accessor - i.e. field.HasValue.  It's a lambda expression/predicate
                return null;
            }

            return body.Member.Name;
        }

    }
}