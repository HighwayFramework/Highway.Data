#region

using NHibernate;

#endregion

namespace Highway.Data.NHibernate
{
    public interface ISessionBuilder
    {
        ISession GetSession();
        ISession GetExistingWebSession();
    }
}