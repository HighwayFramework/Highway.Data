using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using Highway.Data.Rest.Configuration;
using Highway.Data.Rest.Configuration.Interfaces;
using Highway.Data.Rest.Extensions;

namespace Highway.Data.Rest.Contexts
{
    public class DataContext : IDataContext
    {
        private readonly IMappingConfiguration _mapping;
        private readonly IHttpClientAdapter _httpClientAdapter;
        private readonly IContextConfiguration _contextConfiguration;
        private readonly ILog _logger;
        private ModelBuilder _model = new ModelBuilder();
        private ModelDefinitions _config;

        public DataContext(IMappingConfiguration mapping, IHttpClientAdapter httpClientAdapter, ILog logger)
        {
            mapping.Build(_model);
            _httpClientAdapter = httpClientAdapter;
            _logger = logger;
        }

        public DataContext(IMappingConfiguration mapping, IHttpClientAdapter httpClientAdapter) : this(mapping,httpClientAdapter,new NoOpLogger()) { }

        public void Dispose() { }
        public IEventManager EventManager { get; set; }
        public IQueryable<T> AsQueryable<T>() where T : class
        {
            if (_config == null)
            {
                _config = _model.Compile();
            }
            return _httpClientAdapter.All<T>();
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