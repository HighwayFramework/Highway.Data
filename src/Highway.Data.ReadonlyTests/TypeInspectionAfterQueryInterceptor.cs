using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;

namespace Highway.Data.ReadonlyTests
{
    public class TypeInspectionAfterQueryInterceptor : TypeInspectionInterceptor, IEventInterceptor<AfterQuery>
    {
        public bool AppliesTo(AfterQuery eventArgs)
        {
            return true;
        }

        public InterceptorResult Apply(IDataSource context, AfterQuery eventArgs)
        {
            InspectedType = eventArgs.Result.GetType();

            return InterceptorResult.Succeeded();
        }
    }
}