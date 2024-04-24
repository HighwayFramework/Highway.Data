using System;

using Highway.Data.EventManagement.Interfaces;

namespace Highway.Data.ReadonlyTests
{
    public abstract class TypeInspectionInterceptor : IInterceptor
    {
        public int Priority => -1;


        public Type InspectedType { get; set; }
    }
}