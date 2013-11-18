#region

using System;
using System.Threading;
using Highway.Data.EventManagement.Interfaces;
using Highway.Data.Interceptors;

#endregion

namespace Highway.Data.EntityFramework.Tests.AdvancedFeatures.EventManagement
{
    public class TestPreSaveInterceptor : TestInterceptorBase
    {
        public TestPreSaveInterceptor(int priority = 10)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.BeforeSave;
        }
    }

    public class TestPostSaveInterceptor : TestInterceptorBase
    {
        public TestPostSaveInterceptor(int priority)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.AfterSave;
        }

    }

    public class TestPreQueryInterceptor : TestInterceptorBase
    {
        public TestPreQueryInterceptor(int priority)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.BeforeQuery;
        }

    }

    public class TestPostQueryInterceptor : TestInterceptorBase
    {
        public TestPostQueryInterceptor(int priority)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.AfterQuery;
        }

    }

    public class TestPreScalarInterceptor : TestInterceptorBase
    {
        public TestPreScalarInterceptor(int priority)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.BeforeScalar;
        }

    }

    public class TestPostScalarInterceptor : TestInterceptorBase
    {
        public TestPostScalarInterceptor(int priority)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.AfterScalar;
        }

    }

    public class TestPreCommandInterceptor : TestInterceptorBase
    {
        public TestPreCommandInterceptor(int priority)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.BeforeCommand;
        }
    }

    public class TestPostCommandInterceptor : TestInterceptorBase
    {
        public TestPostCommandInterceptor(int priority)
            : base(priority)
        {
        }

        public override bool AppliesTo(EventType eventType)
        {
            return eventType == EventType.AfterCommand;
        }

    }

    public class TestInterceptorBase : IInterceptor
    {
        public TestInterceptorBase(int priority)
        {
            Priority = priority;
        }

        public bool WasCalled { get; set; }

        public int Priority { get; set; }
        public DateTime CallTime { get; set; }

        public virtual bool AppliesTo(EventType eventType)
        {
            return false;
        }

        public InterceptorResult Apply(IDataContext dataContext, EventType eventType)
        {
            WasCalled = true;
            Thread.Sleep(1);
            CallTime = DateTime.Now;
            return InterceptorResult.Succeeded();
        }
    }
}