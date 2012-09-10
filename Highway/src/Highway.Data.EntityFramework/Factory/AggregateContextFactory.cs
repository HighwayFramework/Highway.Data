using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Highway.Data.Interfaces;
using Highway.Data;
using Microsoft.Practices.ServiceLocation;

namespace Highway.Data
{
    public class AggregateContextFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataContext Create<T1>()
            where T1 : class
        {
            var mappings = ServiceLocator.Current.GetInstance<IAggregateConfiguration<T1>>();
            var context = new AggregateContext();
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataContext Create<T1, T2>() 
            where T1 : class 
            where T2 : class
        {
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataContext Create<T1, T2, T3>()
            where T1 : class
            where T2 : class
            where T3 : class
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDataContext Create<T1, T2, T3, T4>()
            where T1 : class
            where T2 : class
            where T3 : class
            where T4 : class
        {
            return null;
        }
    }
}
