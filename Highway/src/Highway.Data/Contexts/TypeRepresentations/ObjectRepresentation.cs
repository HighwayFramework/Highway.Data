using System;
using System.Collections.Generic;
using System.Linq;

namespace Highway.Data.Contexts.TypeRepresentations
{
    internal class ObjectRepresentation
    {
        public ObjectRepresentation()
        {
            Parents = new Dictionary<object, Action>();
        }
        internal object Entity { get; set; }

        public bool IsType<T1>()
        {
            return typeof(T1) == Entity.GetType();
        }

        internal IEnumerable<ObjectRepresentation> RelatedEntities { get; set; }

        internal IEnumerable<ObjectRepresentation> AllRelated()
        {
            var items = RelatedEntities.ToList();
            foreach (var objectRepresentationBase in RelatedEntities)
            {
                items.AddRange(objectRepresentationBase.AllRelated());
            }
            return items;
        }

        internal Guid Id { get; set; }
        internal Dictionary<object, Action> Parents { get; set; }
    }
}