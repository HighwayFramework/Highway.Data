namespace Highway.Data
{
    /// <summary>
    /// Testable interface for our adapter
    /// </summary>
    public interface IDbModelBuilderAdapter
    {
        IConfigurationRegistrarAdapater Configurations { get; }
    }
}