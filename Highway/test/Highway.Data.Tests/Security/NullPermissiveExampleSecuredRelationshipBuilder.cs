using System;
using System.Collections.Generic;
using Highway.Data.Security.DataEntitlements;
using Highway.Data.Security.DataEntitlements.Expressions;

namespace Highway.Data.Tests.Security
{
    internal class NullPermissiveExampleSecuredRelationshipBuilder : ISecuredRelationshipBuilder
    {
        public IEnumerable<SecuredRelationship> BuildSecuredRelationships()
        {
            yield return this.Secure<ExampleLeaf>().By<ExampleRoot, long>(x => x.SecuredRoot, WhenNull.Allow);
            yield return this.Secure<ExampleLeaf>().ByCollection<ExampleRoot, long>(x => x.SecuredRoots, WhenNull.Allow);
        }
    }
}