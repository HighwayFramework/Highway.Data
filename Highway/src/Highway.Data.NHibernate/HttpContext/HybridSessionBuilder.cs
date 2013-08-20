using Common.Logging;
using Common.Logging.Simple;
using NHibernate;

namespace Highway.Data.NHibernate
{
    public class HybridSessionBuilder : ISessionBuilder
    {
        public HybridSessionBuilder(ISessionFactory sessionFactory, ILog logger)
        {
            _sessionFactory = sessionFactory;
            _logger = logger ?? new NoOpLogger();
        }

        private static ISession _currentSession;
        private static ISessionFactory _sessionFactory;
        private readonly ILog _logger;

        public ISession GetSession()
        {
            ISession session = GetExistingOrNewSession();
            _logger.Debug("Using ISession " + session.GetHashCode());
            return session;
        }

        private ISession GetExistingOrNewSession()
        {
            if (System.Web.HttpContext.Current != null)
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

        public ISession GetExistingWebSession()
        {
            return System.Web.HttpContext.Current.Items[GetType().FullName] as ISession;
        }

        private ISession OpenSessionAndAddToContext()
        {
            ISession session = _sessionFactory.OpenSession();
            System.Web.HttpContext.Current.Items.Remove(GetType().FullName);
            System.Web.HttpContext.Current.Items.Add(GetType().FullName, session);
            return session;
        }

        public static void ResetSession()
        {
            var builder = new HybridSessionBuilder(_sessionFactory, null);
            builder.GetSession().Dispose();
        }
    }
}