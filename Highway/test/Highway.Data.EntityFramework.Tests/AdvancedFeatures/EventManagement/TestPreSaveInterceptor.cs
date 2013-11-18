#region

using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;

#endregion

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    public class TestPreSaveInterceptor : IInterceptor
    {
        public bool WasCalled { get; set; }

        public int Priority
        {
            get { return 10; }
        }

        public bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.BeforeSave;
        }

        public InterceptorResult Apply(IDataContext dataContext, EventType eventType)
        {
            WasCalled = true;
            return InterceptorResult.Succeeded();
        }
    }
}