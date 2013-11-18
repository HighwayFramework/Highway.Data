#region

using System.Web;
using Common.Logging;
using Common.Logging.Simple;
using NHibernate;

#endregion

namespace Highway.Data.NHibernate
{
    public class HybridSessionBuilder : ISessionBuilder
    {
        private static ISession _currentSession;
        private static ISessionFactory _sessionFactory;
        private readonly ILog _logger;

        public HybridSessionBuilder(ISessionFactory sessionFactory, ILog logger)
        {
            _sessionFactory = sessionFactory;
            _logger = logger ?? new NoOpLogger();
        }

        public ISession GetSession()
        {
            ISession session = GetExistingOrNewSession();
            _logger.Debug("Using ISession " + session.GetHashCode());
            return session;
        }

        public ISession GetExistingWebSession()
        {
            return HttpContext.Current.Items[GetType().FullName] as ISession;
        }

        private ISession GetExistingOrNewSession()
        {
            if (HttpContext.Current != null)
            {
                ISession session = GetExistingWebSession();
                if (session == null)
                {
                    session = OpenSessionAndAddToContext();
                }
                else if (!session.IsOpen)
                {
                    session = OpenSessionAndAddToContext();
                }

                return session;
            }

            if (_currentSession == null)
            {
                _currentSession = _sessionFactory.OpenSession();
            }
            else if (!_currentSession.IsOpen)
            {
                _currentSession = _sessionFactory.OpenSession();
            }

            return _currentSession;
        }

        private ISession OpenSessionAndAddToContext()
        {
            ISession session = _sessionFactory.OpenSession();
            HttpContext.Current.Items.Remove(GetType().FullName);
            HttpContext.Current.Items.Add(GetType().FullName, session);
            return session;
        }

        public static void ResetSession()
        {
            var builder = new HybridSessionBuilder(_sessionFactory, null);
            builder.GetSession().Dispose();
        }
    }
}