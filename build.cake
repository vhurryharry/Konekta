#addin "nuget:?package=NuGet.Core"
#addin "Cake.FileHelpers"
#addin "Cake.Incubator"
#addin "Cake.Npm"
#tool "nuget:?package=vswhere"

var target        = Argument("target", "Default");
var buildNumber   = Argument("buildnumber", "0");
var dropPath      = Argument("drop_path", "./drop");
var stagingPath   = "./staging";
var buildDir      = Directory("./artifacts");
var configuration = "Release";
var solution      = "./WCA.sln";

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
    MSBuild(solution, settings => settings
        .SetConfiguration(Argument("Configuration", configuration))
        .SetVerbosity(Verbosity.Minimal)
        .UseToolVersion(MSBuildToolVersion.VS2017)
        .WithTarget("Clean")
    );
});

Task("RestorePackages")
    .Does(() =>
{
    // NuGetRestore(solution);
    DotNetCoreRestore();

    NpmInstall(settings => settings.FromPath("./src/WCA.Web/"));
});

Task("Build")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
    // Build backend / dotnet
    MSBuild(solution, settings => settings
        .SetConfiguration(Argument("Configuration", configuration))
        .SetVerbosity(Verbosity.Minimal)
        .UseToolVersion(MSBuildToolVersion.VS2017)
    );

    // Build frontend SPA with webpack
    NpmRunScript("webpack", settings => settings.FromPath("./src/WCA.Web/"));
});

Task("RunTests")
    .IsDependentOn("Build")
    .Does(() =>
{

    var settings = new DotNetCoreTestSettings
    {
        Logger = "trx",
        NoBuild = true, // Don't build because we're dependent on build anyway
        Configuration = Argument("Configuration", configuration)
    };

    // Only run unit tests here. Integration tests must be called separately due to
    // their dependence on configuration.
    DotNetCoreTest("./test/WCA.UnitTests/", settings);
});

Task("Publish")
    .IsDependentOn("RunTests")
    .Does(() =>
{
    if (DirectoryExists(stagingPath))
        CleanDirectory(stagingPath);

    if (DirectoryExists(dropPath))
        CleanDirectory(dropPath);

    EnsureDirectoryExists(stagingPath);
    EnsureDirectoryExists(dropPath);

    var webPublishPath = stagingPath + "/WCA.Web";
    DotNetCorePublish("./src/WCA.Web", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        OutputDirectory = webPublishPath,
        NoRestore = true
    });
    Zip(webPublishPath, dropPath + "/WCA.Web.zip");

    var funcPublishPath = stagingPath + "/WCA.AzureFunctions";
    DotNetCorePublish("./src/WCA.AzureFunctions", new DotNetCorePublishSettings
    {
        Configuration = configuration,
        OutputDirectory = funcPublishPath,
        NoRestore = true
    });
    Zip(funcPublishPath, dropPath + "/WCA.AzureFunctions.zip");
});

Task("Default")
    .IsDependentOn("RunTests");

RunTarget(target);