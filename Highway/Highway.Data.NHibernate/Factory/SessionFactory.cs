using System;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Highway.Data.NHibernate.Factory
{
    public class SessionFactory
    {
        public static ISessionFactory Create<T>(Func<IPersistenceConfigurer> dbConfig,
                                                Func<AutoPersistenceModel> autoConfig = null)
        {
            var sessionFactory = Fluently.Configure().Database(dbConfig)
                .Mappings(m =>
                              {
                                  m.HbmMappings.AddFromAssemblyOf<T>();
                                  m.FluentMappings.AddFromAssemblyOf<T>();
                                  if (autoConfig != null) m.AutoMappings.Add(autoConfig());
                              })
                .BuildSessionFactory();
            return sessionFactory;
        }
    }
}