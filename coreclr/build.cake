///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var solutions = GetFiles("./**/project.json");
var solutionPaths = solutions.Select(solution => solution.GetDirectory());

var home = EnvironmentVariable("USERPROFILE") ?? EnvironmentVariable("HOME") ?? ".";
var runtimeVersion = "1.0.0-rc1-update2";
var runtime = "";
var os = "";
var arch = "";

if (IsRunningOnWindows())
{
    runtime = "clr";
    os = "win";
    arch = "x86";
}

if (IsRunningOnUnix())
{
    runtime = "mono";
    os = "osx";
    arch = "x64";
}

var dnx = home + @"\.dnx\runtimes\dnx-" + runtime + "-" + os + "-" + arch + "." + runtimeVersion + @"\bin\dnx.exe ";


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(() =>
{
    // Executed BEFORE the first task.
    Information("Running tasks...");
});

Teardown(() =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    // Clean solution directories.
    foreach(var path in solutionPaths)
    {
        Information("Cleaning {0}", path);
        CleanDirectories(path + "/**/bin/" + configuration);
        CleanDirectories(path + "/**/obj/" + configuration);
    }
});

Task("Restore")
    .Does(() =>
{
    // Restore all NuGet packages.
    DNURestore();
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DNUBuild("./src/*", new DNUBuildSettings
    {
        Configurations = new[] { configuration },
        Quiet = true
    });
    DNUBuild("./test/*", new DNUBuildSettings
    {
        Configurations = new[] { configuration },
        Quiet = true
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    StartProcess(dnx, @"--project .\test\Highway.Data.CoreCLR.Tests test");
    StartProcess(dnx, @"--project .\test\Highway.Data.DotNet.Tests test");
});
///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Test");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
