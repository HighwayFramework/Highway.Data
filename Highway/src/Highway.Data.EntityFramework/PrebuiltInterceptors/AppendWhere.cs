using System;
using System.Linq;
using System.Linq.Expressions;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.PrebuiltInterceptors
{
    public class AppendWhere<T> : IEventInterceptor<BeforeQuery>
    {
        private readonly Expression<Func<T, bool>> _clauseToAppend;
        private readonly Type[] _typesToApplyTo;

        public AppendWhere(int priority, Expression<Func<T, bool>> clauseToAppend, params Type[] typesToApplyTo)
        {
            _clauseToAppend = clauseToAppend;
            _typesToApplyTo = typesToApplyTo;
            Priority = 1;
        }

        public InterceptorResult Apply(IDataContext context, BeforeQuery eventArgs)
        {
            var query = eventArgs.Query as IQuery<T>;
            query.Where(_clauseToAppend);
            return InterceptorResult.Succeeded();
        }

        public bool AppliesTo(BeforeQuery eventArgs)
        {
            return _typesToApplyTo.Any(type => eventArgs.Query.GetType() == type);
        }

        public int Priority { get; private set; }
    }

}
