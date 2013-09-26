using System;

namespace Highway.Data.Contexts
{
    internal class TypeObjectRepresentation<T> : ObjectRepresentationBase
    {
        internal T Entity { get; set; }

        protected override Type Type
        {
            get { return typeof(T); }
        }
    }
}