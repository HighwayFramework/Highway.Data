using System;
using System.Linq;
using Common.Logging;
using Highway.Data.Rest.Configuration;

namespace Highway.Data.Rest.Contexts
{
    public class DataContext : IDataContext
    {
        private readonly string _baseUri;
        private readonly IMappingConfiguration _mapping;
        private readonly IContextConfiguration _contextConfiguration;
        private readonly ILog _logger;
        private ModelDefinitions _model;

        public DataContext(string baseUri, IMappingConfiguration mapping,IContextConfiguration contextConfiguration, ILog logger )
        {
            _baseUri = baseUri;
            _model = mapping.Build(new ModelBuilder()).Compile();
            _contextConfiguration = contextConfiguration;
            _logger = logger;
        }

        public void Dispose() { }
        public IEventManager EventManager { get; set; }
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public T Add<T>(T item) where T : class
        {
            throw new System.NotImplementedException();
        }

        public T Remove<T>(T item) where T : class
        {
            throw new System.NotImplementedException();
        }

        public T Update<T>(T item) where T : class
        {
            throw new System.NotImplementedException();
        }

        public T Reload<T>(T item) where T : class
        {
            throw new System.NotImplementedException();
        }

        public int Commit()
        {
            throw new System.NotImplementedException();
        }
    }
}