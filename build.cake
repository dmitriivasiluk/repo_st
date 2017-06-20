#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool nuget:?package=7-Zip.CommandLine&version=9.20.0
#addin nuget:?package=SharpZipLib&version=0.86.0
#addin "Cake.Compression"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories etc
var testVersion = new Version("2.0.20.1");
var userConfigFileName = "user.config";
var buildDir = Directory("./AutomationTestsSolution/bin") + Directory(configuration);
var testFolder = Environment.GetEnvironmentVariable("ST_testFolder") != null ? Environment.GetEnvironmentVariable("ST_testFolder") : "./test";
var initialDataUrl = Environment.GetEnvironmentVariable("ST_TESTDATAURL") != null ? Environment.GetEnvironmentVariable("ST_testFolder") : "https://bitbucket.org/atlassian/sourcetreeqaautomation/downloads/SourceTree-" + testVersion + "-full.nupkg";
var initialDataZip = System.IO.Path.Combine(testFolder, "sourcetree.nupkg");

var installPath = System.IO.Path.Combine(testFolder,"SourceTree", "Installation");
var userDataFolder = System.IO.Path.Combine(installPath, @"AppData\local\Atlassian\SourceTree");
var exePath = System.IO.Path.Combine(installPath, @"SourceTree.exe");
var exeConfig = System.IO.Path.Combine(installPath, @"SourceTree.exe.config");
var testDataRuntimeFolder = System.IO.Path.Combine(testFolder,"RuntimeData");
        

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore("./SourceTreeAutomation.sln");
});

Task("Init")
    .Does(() =>
{
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("./SourceTreeAutomation.sln", new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        ToolVersion = MSBuildToolVersion.VS2015 ,
        Configuration = configuration,
        });
    }
    else
    {
      // Use XBuild
      XBuild("./SourceTreeAutomation.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests-Core")
    .IsDependentOn("Build")
    .Does(() =>
{
    //Environment.SetEnvironmentVariable("ST_EXE", exePath);
    //Environment.SetEnvironmentVariable("ST_VERSION", testVersion.ToString() );
    //Environment.SetEnvironmentVariable("ST_USERDATAFOLDER", userDataFolder );
    //Environment.SetEnvironmentVariable("ST_TESTDATARUNTIMEFOLDER", testDataRuntimeFolder );
    //Environment.SetEnvironmentVariable("ST_USERCONFIG", userConfig );

    Error("Using "+ testVersion + " @ " + exePath);
    NUnit3("./**/bin/" + configuration + "/AutomationTest*.dll", 
        new NUnit3Settings {
            NoResults = true,
            Test = "AutomationTestsSolution.Tests.HelpMenuTests"
        });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
