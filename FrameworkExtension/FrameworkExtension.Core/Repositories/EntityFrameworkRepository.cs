using System;
using System.Collections.Generic;
using System.Linq;
using FrameworkExtension.Core.EventManagement;
using FrameworkExtension.Core.Interfaces;

namespace FrameworkExtension.Core.Repositories
{
    public class EntityFrameworkRepository : IRepository
    {
        public EntityFrameworkRepository(IDataContext context, IEventManager eventManager)
        {
            Context = context;
            this.EventManager = eventManager;
            var observableDataContext = context as IObservableDataContext;
            if(observableDataContext != null)
            {
                ((EventManager) this.EventManager).Context = observableDataContext;
            }
        }

        public IDataContext Context { get; private set; }
        public IEventManager EventManager { get; private set; }

        public T Get<T>(IQuery<T> query) where T : class
        {
            return query.Execute(Context).FirstOrDefault();
        }

        public T Get<T>(IScalarObject<T> query)
        {
            return query.Execute(Context);
        }

        public void Execute(ICommandObject command)
        {
            command.Execute(Context);
        }

        public IEnumerable<T> Find<T>(IQuery<T> query) where T : class
        {
            return query.Execute(Context);
        }
    }
}