#region

using System;
using System.Threading;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;
using Highway.Data.Interceptors.Events;

#endregion

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    public class TestEventInterceptor<T> : IEventInterceptor<T>
    {
        public TestEventInterceptor(int priority)
        {
            Priority = priority;
        }

        public bool WasCalled { get; set; }

        public int Priority { get; set; }
        public DateTime CallTime { get; set; }

        public InterceptorResult Apply(IDataContext dataContext, T eventArgs)
        {
            WasCalled = true;
            Thread.Sleep(1);
            CallTime = DateTime.Now;
            return InterceptorResult.Succeeded();
        }
    }
}