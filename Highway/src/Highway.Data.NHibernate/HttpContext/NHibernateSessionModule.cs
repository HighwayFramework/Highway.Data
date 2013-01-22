using System;
using System.Web;
using Common.Logging;
using NHibernate;
using NHibernate.Cfg;

namespace Highway.Data.NHibernate
{
    public class NHibernateSessionModule : IHttpModule
    {
        private readonly ISessionBuilder _sessionBuilder;
        private readonly ILog _logger;

        public NHibernateSessionModule(ISessionBuilder sessionBuilder, ILog logger)
        {
            _sessionBuilder = sessionBuilder;
            _logger = logger;
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += context_EndRequest;
        }

        public void Dispose()
        {
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            ISession session = _sessionBuilder.GetExistingWebSession();
            if (session != null)
            {
                _logger.Debug("Disposing of ISession " + session.GetHashCode());
                session.Dispose();
            }
        } 
    }
}