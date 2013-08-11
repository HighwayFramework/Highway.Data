namespace Highway.Data.Rest.Configuration.Defaults
{
    public class DefaultContextConfiguration : IContextConfiguration
    {
        public DefaultContextConfiguration(string baseUri)
        {
            BaseUri = baseUri;
        }

        public string BaseUri { get; set; }
    }
}