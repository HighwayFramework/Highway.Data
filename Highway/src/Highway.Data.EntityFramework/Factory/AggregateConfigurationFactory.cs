using Microsoft.Practices.ServiceLocation;

namespace Highway.Data
{
    public abstract class AggregateConfigurationFactory
    {
        public static IAggregateConfiguration GetConfigurationFor<T1>()
        {
            return ServiceLocator.Current.GetInstance<IAggregateConfiguration>(typeof (T1).FullName);
        }

        public static IAggregateConfiguration GetConfigurationFor<T1, T2>()
        {
            return
                ServiceLocator.Current.GetInstance<IAggregateConfiguration>(string.Format("{0},{1}",
                                                                                          typeof (T1).FullName,
                                                                                          typeof (T2).FullName));
        }

        public static IAggregateConfiguration GetConfigurationFor<T1, T2, T3>()
        {
            return
                ServiceLocator.Current.GetInstance<IAggregateConfiguration>(string.Format("{0},{1},{2}",
                                                                                          typeof (T1).FullName,
                                                                                          typeof (T2).FullName,
                                                                                          typeof (T3).FullName));
        }

        public static IAggregateConfiguration GetConfigurationFor<T1, T2, T3, T4>()
        {
            return
                ServiceLocator.Current.GetInstance<IAggregateConfiguration>(string.Format("{0},{1},{2},{3}",
                                                                                          typeof (T1).FullName,
                                                                                          typeof (T2).FullName,
                                                                                          typeof (T3).FullName,
                                                                                          typeof (T4).FullName));
        }
    }
}