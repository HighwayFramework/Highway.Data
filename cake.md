# Super-duper quick cake getting started guide.

## To run dotnet cake

The default target is "Pack."  This restores, cleans, builds, and tests Debug and Release configurations.  Then it packs the Release configuration to ./packages.

1. From the root of the project, run (PowerShell or bash)
   `dotnet tool restore`
2. To prevent integration tests from failing, follow the instructions to set up your SqlConnectionString for the Highway.Data.ReadonlyTests project.  Those are located in ./src/Highway.Data.ReadonlyTests/readme.md
3. Run the following command (PowerShell or bash)
   `dotnet cake`

To push nuget packages, you need to do a few more things:

1. Create an environment variable named "NUGET_API_KEY"
   PowerShell:  Run from an admin prompt:
   `[Environment]::SetEnvironmentVariable("NUGET_API_KEY", "[VALUE]", [System.EnvironmentVariableTarget]::User)`


   bash:  Add the following to your .bashrc file:
   `export NUGET_API_KEY="[VALUE]"`

2. run dotnet cake with the push target specified.  It is not specified by default because you would only want to push packages deliberately, never by default.
   `dotnet cake --target=Push`

