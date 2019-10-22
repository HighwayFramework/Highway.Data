
using System;


namespace Highway.Data.Contexts.TypeRepresentations
{
    internal class Accessor
    {
        public Accessor(Action removeAction, Func<object, object, object> getterFunc)
        {
            RemoveAction = removeAction;
            GetterFunc = getterFunc;
        }

        internal Action RemoveAction { get; set; }
        internal Func<object, object, object> GetterFunc { get; set; }
    }
}