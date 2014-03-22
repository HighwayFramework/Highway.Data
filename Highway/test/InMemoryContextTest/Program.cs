namespace InMemoryContextTest
{
    using Highway.Data;
    using Highway.Data.Contexts;

    class Program
    {
        static void Main(string[] args)
        {
            var repo = new Repository(new InMemoryDataContext());
            var specification = new CreatePollingDeviceSpecification() { DeviceModel = "Test" };

            var deviceModel = new DeviceModel()
            {
                Id = 1,
                Code = specification.DeviceModel,
                Name = specification.DeviceModel
            };
            repo.Context.Add(deviceModel);
            repo.Context.Commit();
        }
    }

    public class DeviceModel : IIdentifiable<long>
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }

    public class CreatePollingDeviceSpecification
    {
        public string DeviceModel { get; set; }
    }
}
