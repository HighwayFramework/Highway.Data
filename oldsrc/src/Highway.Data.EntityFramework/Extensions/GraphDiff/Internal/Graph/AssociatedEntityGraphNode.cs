﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Reflection;

namespace Highway.Data.EntityFramework.Extensions.GraphDiff.Internal.Graph
{
    internal class AssociatedEntityGraphNode : GraphNode
    {
        internal AssociatedEntityGraphNode(GraphNode parent, PropertyInfo accessor)
                : base(parent, accessor)
        {
        }

        public override void Update<T>(DbContext context, T persisted, T updating)
        {
            var dbValue = GetValue<object>(persisted);
            var newValue = GetValue<object>(updating);

            if (newValue == null)
            {
                SetValue(persisted, null);
                return;
            }

            // do nothing if the key is already identical
            if (IsKeyIdentical(context, newValue, dbValue))
            {
                return;
            }

            newValue = AttachAndReloadAssociatedEntity(context, newValue);

            SetValue(persisted, newValue);
        }

        protected override IEnumerable<string> GetRequiredNavigationPropertyIncludes(DbContext context)
        {
            return Accessor != null
                    ? GetRequiredNavigationPropertyIncludes(context, Accessor.PropertyType, IncludeString)
                    : new string[0];
        }
    }
}
