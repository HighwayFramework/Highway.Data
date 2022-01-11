This project reads connection strings from user-secrets and from environment variables.  For more information, visit:
https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows

To initialize user secrets, navigate to the projec folder and type:
dotnet user-secrets init (already done for this project)

To set your connection string, run:
dotnet user-secrets set "SqlConnectionString" "Data Source=.;Initial Catalog=[DATABASE_NAME];Integrated Security=false;User ID=sa;Password=[PASSWORD]"

You can use any valid SQL server connection string, so long as you have SA permissions to the server.