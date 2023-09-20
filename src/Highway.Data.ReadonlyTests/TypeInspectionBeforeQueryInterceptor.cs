using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.ReadonlyTests
{
    public class TypeInspectionBeforeQueryInterceptor : TypeInspectionInterceptor, IEventInterceptor<BeforeQuery>
    {
        public bool AppliesTo(BeforeQuery eventArgs)
        {
            return true;
        }

        public InterceptorResult Apply(IDataSource context, BeforeQuery eventArgs)
        {
            InspectedType = eventArgs.Query.GetType();

            return InterceptorResult.Succeeded();
        }
    }
}
