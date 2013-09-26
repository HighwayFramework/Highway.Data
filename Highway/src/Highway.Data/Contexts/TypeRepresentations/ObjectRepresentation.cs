using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Highway.Data.Contexts
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

        protected virtual Type Type { get { return typeof(object); } }
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

        public Guid Id { get; set; }
        public Dictionary<object, Action> Parents { get; set; }
    }
}