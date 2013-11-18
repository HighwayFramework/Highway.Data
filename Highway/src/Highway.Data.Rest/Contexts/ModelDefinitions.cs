#region

using System;
using System.Collections.Generic;
using Highway.Data.Rest.Configuration.Interfaces;

#endregion

namespace Highway.Data.Rest.Contexts
{
    public class ModelDefinitions : Dictionary<Type, IRestTypeDefinition>
    {
        public void Add(IRestTypeDefinition restType)
        {
            Add(restType.ConfiguredType, restType);
        }
    }
}