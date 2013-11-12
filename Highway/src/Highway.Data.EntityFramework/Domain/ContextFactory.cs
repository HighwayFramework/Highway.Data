using System;
using System.Data.Objects;
using System.Linq;

namespace Highway.Data.Domain
{
    public class ContextFactory : IContextFactory
    {
        private readonly IDomain[] _domains;

        public ContextFactory(params IDomain[] domains)
        {
            _domains = domains;
        }

        public IDataContext Create<T>() where T : class, IDomain
        {
            var domain = _domains.OfType<T>().SingleOrDefault();
            return new DomainContext<T>(domain);
        }

        public IDataContext Create(Type type)
        {
            var domain = _domains.FirstOrDefault(x => x.GetType() == type);
            var d1 = typeof(DomainContext<>);
            Type[] typeArgs = { type };
            var makeme = d1.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(makeme,domain);
            return (IDataContext) o;
        }
    }

    public interface IDomain
    {
        string ConnectionString { get; set; }

        IMappingConfiguration Mappings { get; set; }

        IContextConfiguration Context { get; set; }


    }
}