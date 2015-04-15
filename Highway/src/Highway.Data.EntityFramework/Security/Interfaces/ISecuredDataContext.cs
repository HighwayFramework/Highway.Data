namespace Highway.Data.EntityFramework.Security.Interfaces
{
    internal interface ISecuredDataContext
    {
        IProvideEntitlements EntitlementProvider { get; }
        SecuredRelationshipCache SecuredRelationshipCache { get; }
    }
}