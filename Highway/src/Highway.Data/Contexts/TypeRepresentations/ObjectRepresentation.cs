using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Highway.Data.Contexts
{
    internal class ObjectRepresentation
    {
        internal object Entity { get; set; }

        public bool IsType<T1>()
        {
            return typeof(T1) == Entity.GetType();
        }

        protected virtual Type Type { get { return typeof(object); } }
        internal IEnumerable<ObjectRepresentation> RelatedEntities { get; set; }
        public Action EntityRemove { get; set; }
        public object Parent { get; set; }

        internal IEnumerable<ObjectRepresentation> AllRelated()
        {
            var items = RelatedEntities.ToList();
            foreach (var objectRepresentationBase in RelatedEntities)
            {
                items.AddRange(objectRepresentationBase.AllRelated());
            }
            return items;
        }

        public Guid Id { get; set; }

    }
}