using Highway.Data.Interceptors.Events;
using System;
using System.Linq;


namespace Highway.Data.Contexts
{
    public class InMemoryDomainContext<T> : InMemoryDataContext, IDomainContext<T> where T : class
    {
    }
}