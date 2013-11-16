using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    public class TestPreSaveInterceptor : IInterceptor
    {
        public int Priority { get { return 10; } }
        public bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.BeforeSave;
        }

        public InterceptorResult Apply(IDataContext dataContext, EventType eventType)
        {
            this.WasCalled = true;
            return InterceptorResult.Succeeded();
        }

        public bool WasCalled { get; set; }
    }
}