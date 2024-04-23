namespace Highway.Data.Tests.InMemory.BugTests
{
    public class DeviceModel : IIdentifiable<long>
    {
        public string Code { get; set; }

        public long Id { get; set; }

        public string Name { get; set; }
    }
}