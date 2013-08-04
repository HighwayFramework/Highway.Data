using System;
using System.Linq.Expressions;
using Highway.Data.Rest.Configuration.Conventions;
using Highway.Data.Rest.Configuration.Interfaces;
using Highway.Data.Rest.Expressions;

namespace Highway.Data.Rest.Configuration.Entities
{
    public class RestTypeConfiguration<T> : 
        IKeyConfiguredType,
        IRouteConfiguredType<T>, 
        IRestTypeConfiguration<T>
    {
        private readonly IRestConvention _defaultConvention;
        private string _key;
        private string _route;

        public RestTypeConfiguration(IRestConvention convention)
        {
            _defaultConvention = convention;
            _route = convention.DefaultRoute(typeof(T));
            _key = convention.DefaultKey(typeof(T));
        }

        public IRouteConfiguredType<T> WithRoute(string value)
        {
            _route = value;
            return this;
        }

        public IKeyConfiguredType  WithKey(string id)
        {
            _key = id;
            return this;
        }

        public IKeyConfiguredType WithKey<TK>(Expression<Func<T, TK>> selector)
        {
            _key = TypeOf<T>.Property(selector).ToLowerInvariant();
            return this;
        }

        public string Uri
        {
            get { return string.Format(_defaultConvention.DefaultFormat(), _route, _key); }
        }
    }
}