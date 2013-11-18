#region

using Highway.Data.Interceptors;

#endregion

namespace Highway.Data.EventManagement.Interfaces
{
    public interface IInterceptor
    {
        int Priority { get; }

        bool AppliesTo(EventType eventType);

        InterceptorResult Apply(IDataContext dataContext, EventType eventType);
    }

    public enum EventType
    {
        BeforeSave,
        AfterSave,
        BeforeQuery,
        AfterQuery,
        BeforeScalar,
        AfterScalar,
        BeforeCommand,
        AfterCommand
    }
}