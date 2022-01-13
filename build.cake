//////////////////////////////////////////////////////////////////////
// CONFIGURATION
//////////////////////////////////////////////////////////////////////
var debugConfiguration = Argument("configuration", "Debug");
var nugetApiKeyEnvironmentVariable = "NUGET_API_KEY";
var nugetPackageSource = "https://api.nuget.org/v3/index.json";
var packagePath = "./packages/";
var releaseConfiguration = Argument("configuration", "Release");
var solutionFile = "./src/Highway.Data.sln";
var target = Argument("target", "Pack");

//////////////////////////////////////////////////////////////////////
// TASKS - Build
//////////////////////////////////////////////////////////////////////
Task("BuildAll")
    .IsDependentOn("BuildDebug")
    .IsDependentOn("BuildRelease");

Task("BuildDebug")
    .Does(() =>
    {
        DotNetBuild(solutionFile, new DotNetBuildSettings
        {
            Configuration = debugConfiguration,
            NoRestore = true,
        });
    });

Task("BuildRelease")
    .Does(() =>
    {
        DotNetBuild(solutionFile, new DotNetBuildSettings
        {
            Configuration = releaseConfiguration,
            NoRestore = true,
        });
    });

//////////////////////////////////////////////////////////////////////
// TASKS - Clean
//////////////////////////////////////////////////////////////////////
Task("CleanAll")
    .IsDependentOn("CleanDebug")
    .IsDependentOn("CleanRelease");

Task("CleanDebug")
    .Does(() =>
    {
        DotNetClean(solutionFile, new DotNetCleanSettings
        {
            Configuration = debugConfiguration,
        });
    });

Task("CleanRelease")
    .Does(() =>
    {
        DotNetClean(solutionFile, new DotNetCleanSettings
        {
            Configuration = releaseConfiguration,
        });
    });

//////////////////////////////////////////////////////////////////////
// TASKS - Nuget
//////////////////////////////////////////////////////////////////////
Task("Unpack")
    .Does(() => {
        CleanDirectory("./packages/");
    });

Task("Pack")
    .IsDependentOn("Unpack")
    .IsDependentOn("TestAll")
    .Does(() => 
    {
        DotNetPack(solutionFile, new DotNetPackSettings{
            Configuration = releaseConfiguration,
            OutputDirectory = packagePath
        });
    });

Task("Push")
    .IsDependentOn("Pack")
    .Does(() => {
        var tagProgetApiKey = EnvironmentVariable<string>(nugetApiKeyEnvironmentVariable, null);

        if (tagProgetApiKey == null){
            throw new InvalidOperationException($"Necessary Nuget API Key not read from environment variable: {nugetApiKeyEnvironmentVariable}");
        }
        
        var packages = GetFiles(packagePath + "*.nupkg");
        NuGetPush(packages, new NuGetPushSettings{
            ApiKey = tagProgetApiKey,
            SkipDuplicate = true,
            Source = nugetPackageSource,
        });
    });

//////////////////////////////////////////////////////////////////////
// TASKS - Rebuild
//////////////////////////////////////////////////////////////////////
Task("RebuildAll")
    .IsDependentOn("RebuildDebug")
    .IsDependentOn("RebuildRelease");

Task("RebuildDebug")
    .IsDependentOn("CleanDebug")
    .Does(() =>
    {
        DotNetBuild(solutionFile, new DotNetBuildSettings
        {
            Configuration = debugConfiguration,
        });
    });

Task("RebuildRelease")
    .IsDependentOn("CleanRelease")
    .Does(() =>
    {
        DotNetBuild(solutionFile, new DotNetBuildSettings
        {
            Configuration = releaseConfiguration,
        });
    });

//////////////////////////////////////////////////////////////////////
// TASKS - Restore
//////////////////////////////////////////////////////////////////////
Task("Restore")
    .Does(() => {
        DotNetRestore(solutionFile);
    });

//////////////////////////////////////////////////////////////////////
// TASKS - Test
//////////////////////////////////////////////////////////////////////
Task("TestAll")
    .IsDependentOn("TestDebug")
    .IsDependentOn("TestRelease");

Task("TestDebug")
    .IsDependentOn("RebuildDebug")
    .Does(() =>
    {
        DotNetTest(solutionFile, new DotNetTestSettings
        {
            Configuration = debugConfiguration,
            NoBuild = true,
        });
    });

Task("TestRelease")
    .IsDependentOn("RebuildRelease")
    .Does(() =>
    {
        DotNetTest(solutionFile, new DotNetTestSettings
        {
            Configuration = releaseConfiguration,
            NoBuild = true,
        });
    });

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////
RunTarget(target);
