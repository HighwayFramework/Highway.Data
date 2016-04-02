namespace Highway.Data.EntityFramework.Tests.Properties {
    public class Settings {
        public static Settings Default { get; } = new Settings();

        public string Connection { get; } = "Data Source=.;Initial Catalog=Highway.Test;Integrated Security=True";
    }
}
