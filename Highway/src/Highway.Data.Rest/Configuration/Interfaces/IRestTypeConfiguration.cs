using System;
using System.Linq.Expressions;

namespace Highway.Data.Rest.Configuration.Interfaces
{
    public interface IRestTypeConfiguration<T>
    {
        IRouteConfiguredType<T> WithRoute(string value);
        IKeyConfiguredType  WithKey(string id);
        IKeyConfiguredType WithKey<TK>(Expression<Func<T, TK>> selector);
    }
}