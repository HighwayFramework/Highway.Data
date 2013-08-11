using System;
using System.Linq.Expressions;
using System.Reflection;
using Highway.Data.Rest.Configuration.Conventions;
using Highway.Data.Rest.Configuration.Interfaces;
using Highway.Data.Rest.Expressions;

namespace Highway.Data.Rest.Configuration.Entities
{
    public class RestTypeConfiguration<T> : 
        IKeyConfiguredType,
        IRouteConfiguredType<T>, 
        IRestTypeConfiguration<T>,
        IRestTypeDefinition

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
            KeyProperty = TypeOf<T>.PropertyInfo(id);
            return this;
        }

        public IKeyConfiguredType WithKey<TK>(Expression<Func<T, TK>> selector)
        {
            _key = TypeOf<T>.Property(selector).ToLowerInvariant();
            KeyProperty = TypeOf<T>.PropertyInfo(selector);
            return this;
        }

        public string SingleUri
        {
            get { return string.Format(_defaultConvention.DefaultFormat().Single, _route, _key); }
        }

        public string AllUri
        {
            get { return string.Format(_defaultConvention.DefaultFormat().All, _route, _key); }
        }

        public Type ConfiguredType { get { return typeof (T); } }

        public PropertyInfo KeyProperty { get; private set; }
    }
}