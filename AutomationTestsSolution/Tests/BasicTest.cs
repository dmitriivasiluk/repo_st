using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using AutomationTestsSolution.Helpers;
using Castle.Core.Internal;
using NUnit.Framework;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;

namespace AutomationTestsSolution.Tests
{
    public abstract class BasicTest
    {
        public static readonly string DefaultSourceTreeInstallPath = Path.Combine(TestContext.CurrentContext.TestDirectory, DateTime.Now.Ticks.ToString());
        public static readonly string DefaultSourceTreeDownloadPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "downloads");
        public static readonly string DefaultSourceTreeArtifactsPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "artifacts");

        public static readonly Version DefaultSourceTreeVersion = new Version("2.1.2.5");
        public static readonly Version DefaulGitVersion = new Version("2.12.2.2");
        public static readonly Version DefaulGitLfsVersion = new Version("1.5.6");
        public static readonly Version DefaulGitLfsMediaAdapaterVersion = new Version("1.0.5");
        public static readonly Version DefaulGcmVersion = new Version("1.8.1.6");
        public static readonly Version DefaultHgVersion = new Version("3.7.3");
        public static readonly Version DefaulAskpassPassthroughVersion = new Version("1.2.0.0");

        private bool ForceCleanRun = false;


        [SetUp]
        public virtual void SetUp()
        {
            CheckRuntimeEnvironment();
            
            // check we have a sourcetree install
            GetSourceTreeHandle();

            // configure sourcetree
            PreConfigureSourceTree();

            PerTestPreConfigureSourceTree();

            RecordUserConfigPaths();

            // run sourcetree
            RunAndAttach();
        }

        private void RecordUserConfigPaths()
        {
            UserConfigPaths = FindUserConfigs();
        }

        private void CleanUserConfigPaths()
        {
            var currentUserConfigPaths = FindUserConfigs();

            var newUserConfigPaths = currentUserConfigPaths.Except(UserConfigPaths);

            newUserConfigPaths.ForEach(p => CleanDirectory(p));
        }

        protected IEnumerable<string> UserConfigPaths { get; private set; }
        protected virtual void PreConfigureSourceTree()
        {
            PreConfigureSourceTreeCore();

            PreConfigureSourceTreeRegistered();
            PreConfigureSourceTreeCheckedForOlderInstalls();
            PreConfigureSourceTreeRegistration();

            InstallGitSystem();
            InstallGitExtras();
            InstallHgSystem();
            InstallHgExtras();
        }

        private void GetSourceTreeHandle()
        {
            if (!FindSourceTree())
            {
                // TODO install if necessary
                if (DownloadSourceTree())
                {
                    if (ExtractSourceTree())
                    {
                        if (!FindSourceTree())
                        {
                            Assert.Fail($"Unable to find SourceTree installation");
                        }
                    }
                }
            }
        }

        private void InstallHgEmbedded()
        {
            var hgHelper = new EmbeddedHgHelper(DefaultSourceTreeDownloadPath, SourceTreeUserDataPath, SourceTreeAppPath, DefaultHgVersion);
            hgHelper.Download(ForceCleanRun);
            if (!hgHelper.InstallHg(ForceCleanRun))
            {
                Assert.Fail("Unable to install hg");
            }

            ExeConfig exeConfig = new ExeConfig(SourceTreeExeConfigPath);
            exeConfig.SetUserSetting("HgSystemPath", hgHelper.InstalledPath);
            exeConfig.SetUserSetting("HgWhichOne", "0");
            exeConfig.SetUserSetting("EnableHgSupport", "True");
            exeConfig.Save();
        }

        private void InstallHgSystem()
        {
            var hgHelper = new EmbeddedHgHelper(DefaultSourceTreeDownloadPath, DefaultSourceTreeDownloadPath, SourceTreeAppPath, DefaultHgVersion);
            hgHelper.Download(ForceCleanRun);
            if (!hgHelper.InstallHg(ForceCleanRun))ar
            {
                Assert.Fail("Unable to install git");
            }

            ExeConfig exeConfig = new ExeConfig(SourceTreeExeConfigPath);
            exeConfig.SetUserSetting("HgSystemPath", hgHelper.InstalledPath);
            exeConfig.SetUserSetting("HgWhichOne", "1");
            exeConfig.SetUserSetting("EnableHgSupport", "True");
            exeConfig.Save();

            var currPath = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", hgHelper.InstalledPath + ";" + currPath);
        }

        private void InstallHgExtras()
        {
            var szHelper = new SevenZipHelper(SourceTreeAppPath);

            // TODO search for a GCM installer to avoid hardcoding the  version
            var gcmPassThroughInstaller = Path.Combine(SourceTreeExtrasPath, $"PortableGcmPassthroughAskpass-{DefaulAskpassPassthroughVersion}.7z");
            if (!szHelper.Unzip(gcmPassThroughInstaller, SourceTreeUserDataHgExtrasPath, ForceCleanRun))
            {
                Assert.Fail("Unable to install hg gcm passthrough");
            }
        }

        private void InstallGitEmbedded()
        {
            var gitHelper = new EmbeddedGitHelper(DefaultSourceTreeDownloadPath, SourceTreeUserDataPath, SourceTreeAppPath, DefaulGitVersion);
            gitHelper.Download(ForceCleanRun);
            if (!gitHelper.InstallGit(ForceCleanRun))
            {
                Assert.Fail("Unable to install git");
            }

            ExeConfig exeConfig = new ExeConfig(SourceTreeExeConfigPath);
            exeConfig.SetUserSetting("GitSystemPath", gitHelper.InstalledPath);
            exeConfig.SetUserSetting("GitWhichOne", "0");
            exeConfig.SetUserSetting("EnableGitSupport", "True");
            exeConfig.Save();
        }

        private void InstallGitSystem()
        {
            var gitHelper = new EmbeddedGitHelper(DefaultSourceTreeDownloadPath, DefaultSourceTreeDownloadPath, SourceTreeAppPath, DefaulGitVersion);
            gitHelper.Download(ForceCleanRun);
            if (!gitHelper.InstallGit(ForceCleanRun))
            {
                Assert.Fail("Unable to install git");
            }

            ExeConfig exeConfig = new ExeConfig(SourceTreeExeConfigPath);
            exeConfig.SetUserSetting("GitSystemPath", gitHelper.InstalledPath);
            exeConfig.SetUserSetting("GitWhichOne", "1");
            exeConfig.SetUserSetting("EnableGitSupport", "True");
            exeConfig.Save();

            var currPath = Environment.GetEnvironmentVariable("PATH");
            Environment.SetEnvironmentVariable("PATH", gitHelper.InstalledPath + ";" + currPath);
        }

        private void InstallGitExtras()
        {
            var szHelper = new SevenZipHelper(SourceTreeAppPath);

            // TODO search for a GCM installer to avoid hardcoding the  version
            var gcmInstaller = Path.Combine(SourceTreeExtrasPath, $"PortableGcmSt-{DefaulGcmVersion}.7z");
            if (!szHelper.Unzip(gcmInstaller, SourceTreeUserDataGitExtrasPath, ForceCleanRun))
            {
                Assert.Fail("Unable to install gcm");
            }

            // TODO search for a GCM installer to avoid hardcoding the  version
            var gitlfsInstaller = Path.Combine(SourceTreeExtrasPath, $"PortableGitLfs-{DefaulGitLfsVersion}.7z");
            if (!szHelper.Unzip(gitlfsInstaller, SourceTreeUserDataGitExtrasPath, ForceCleanRun))
            {
                Assert.Fail("Unable to install git-lfs");
            }

            // TODO search for a GCM installer to avoid hardcoding the  version
            var gitlfsmediaInstaller = Path.Combine(SourceTreeExtrasPath, $"PortableGitLfsBitbucketMediaApi-{DefaulGitLfsMediaAdapaterVersion}.7z");
            if (!szHelper.Unzip(gitlfsmediaInstaller, SourceTreeUserDataGitExtrasPath, ForceCleanRun))
            {
                Assert.Fail("Unable to install git-lfs media adapter");
            }
        }

        protected virtual void PerTestPreConfigureSourceTree()
        {
            // do nothing by default
            // override in TestFixtures for Ficture specific setup
        }

        private void PreConfigureSourceTreeRegistration()
        {
            using (var client = new System.Net.WebClient())
            {
                if (!Directory.Exists(SourceTreeUserDataPath))
                {
                    Directory.CreateDirectory(SourceTreeUserDataPath);
                }

                var accountsJson = Path.Combine(SourceTreeUserDataPath, "accounts.json");
                client.DownloadFile("https://bitbucket.org/atlassian/sourcetreeqaautomation/downloads/sourcetree-test_preregistered_accounts.json", accountsJson);
            }
        }

        protected void PreConfigureSourceTreeCore()
        {
            ExeConfig exeConfig = new ExeConfig(SourceTreeExeConfigPath);
            exeConfig.SetApplicationSetting("IsPortable", "True");
            exeConfig.SetApplicationSetting("PortableDataFolder", @"AppData\local\Atlassian\SourceTree");
            exeConfig.SetApplicationSetting("AutoUpdater", "Disabled");

            exeConfig.SetUserSetting("AutoStartSSHAgent", "False");
            exeConfig.SetUserSetting("SSHClientType", "PuTTY");

            exeConfig.SetUserSetting("ShowWelcome", "False");

            exeConfig.Save();
        }

        private void PreConfigureSourceTreeRegistered()
        {
            ExeConfig exeConfig = new ExeConfig(SourceTreeExeConfigPath);
            exeConfig.SetUserSetting("AgreedToEULA", "True");
            exeConfig.SetUserSetting("FirstLaunchSinceHgAdded", "False");
            exeConfig.SetUserSetting("FirstLaunch", "False");
            exeConfig.SetUserSetting("AgreedToEULAVersion", "20160201");
            exeConfig.Save();
        }

        protected void PreConfigureSourceTreeCheckedForOlderInstalls()
        {
            ExeConfig exeConfig = new ExeConfig(SourceTreeExeConfigPath);
            exeConfig.SetUserSetting("HasCheckedForOlderInstall", "True");
            exeConfig.Save();
        }

        private bool ExtractSourceTree()
        {
            if (Directory.Exists(SourceTreeInstallTempPath))
            {
                CleanDirectory(SourceTreeInstallTempPath);
            }

            System.IO.Compression.ZipFile.ExtractToDirectory(SourceTreeNuPkgPath, SourceTreeInstallTempPath);

            if (Directory.Exists(SourceTreeAppPath))
            {
                CleanDirectory(SourceTreeAppPath);
            }

            Directory.Move(Path.Combine(SourceTreeInstallTempPath, @"lib\net45"), SourceTreeAppPath);
            return true;;
        }

        private void CheckRuntimeEnvironment()
        {
            var stInstallPathEnvVar = Environment.GetEnvironmentVariable("ST_INSTALLPATH");
            SourceTreeInstallPath = stInstallPathEnvVar == null ? DefaultSourceTreeInstallPath : stInstallPathEnvVar;

            Version ver;
            if(Version.TryParse(Environment.GetEnvironmentVariable("ST_TARGETVERSION"), out ver))
            {
                SourceTreeTargetVersion = ver;
            }
            else
            {
                SourceTreeTargetVersion = DefaultSourceTreeVersion;
            }

            var stDownloadPathEnvVar = Environment.GetEnvironmentVariable("ST_DOWNLOADPATH");
            SourceTreeDownloadPath = stDownloadPathEnvVar == null ? DefaultSourceTreeDownloadPath : stDownloadPathEnvVar;
        }

        private bool DownloadSourceTree()
        {
            if (SourceTreeTargetVersion == null)
            {
                Assert.Fail("A Target version is required to install SourceTree");
                return false;
            }

            if (!Directory.Exists(DefaultSourceTreeDownloadPath))
            {
                Directory.CreateDirectory(DefaultSourceTreeDownloadPath);
            }

            SourceTreeNuPkgPath = Path.Combine(DefaultSourceTreeDownloadPath, $"sourcetree.{SourceTreeTargetVersion}.nupkg");

            if (File.Exists(SourceTreeNuPkgPath) && !ForceCleanRun)
            {
                // local cache of the download
                return true;
            }

            using (var client = new System.Net.WebClient())
            {
                var url = $"https://bitbucket.org/atlassian/sourcetreeqaautomation/downloads/SourceTree-{SourceTreeTargetVersion}-full.nupkg";
                try
                {
                    client.DownloadFile(
                        url,
                        SourceTreeNuPkgPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unable to download {url} {e.Message}");
                    Console.WriteLine(e.StackTrace);
                    Assert.Fail($"Unable to download {url}");
                }
            }

            if (!File.Exists(SourceTreeNuPkgPath))
            {
                return false;
            }

            return true;
        }

        private bool FindSourceTree()
        {
            if (!File.Exists(SourceTreeExePath))
            {
                return false;
            }

            SourceTreeVersion = GetSourceTreeVersion(SourceTreeExePath);

            if (SourceTreeVersion != SourceTreeTargetVersion)
            {
                Assert.Warn($"{SourceTreeVersion} != {SourceTreeTargetVersion}");
                return false;
            }

            return true;
        }

        protected Window MainWindow { get; set; }
        protected Process SourceTreeProcess { get; private set; }

        public string SourceTreeInstallPath { get; private set; }
        public string SourceTreeInstallTempPath { get { return Path.Combine(SourceTreeInstallPath, "temp"); } }
        public string SourceTreeScreenShotsPath { get { return Path.Combine(DefaultSourceTreeArtifactsPath, "screenshots"); } }
        public string SourceTreeAppPath { get { return Path.Combine(SourceTreeInstallPath, "app"); } }
        public string SourceTreeExtrasPath { get { return Path.Combine(SourceTreeAppPath, "extras"); } }
        public string SourceTreeNuPkgPath { get; private set; }
        public Version SourceTreeVersion { get; private set; }
        public Version SourceTreeTargetVersion { get; private set; }
        public string SourceTreeExePath { get { return Path.Combine(SourceTreeAppPath, "SourceTree.exe"); } }
        public string SourceTreeExeConfigPath { get { return Path.Combine(SourceTreeAppPath, "SourceTree.exe.config"); } }
        public string SourceTreeUserDataPath {  get {  return Path.Combine(SourceTreeAppPath, @"AppData\local\Atlassian\SourceTree"); } }

        public string SourceTreeUserDataGitExtrasPath { get { return Path.Combine(SourceTreeUserDataPath, @"git_extras"); } }

        public string SourceTreeUserDataHgExtrasPath { get { return Path.Combine(SourceTreeUserDataPath, @"hg_extras"); } }
        public IEnumerable<string> SourceTreeUserConfigs { get; set; }
        public string TestDataPath { get { return Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../TestData"); } }
        public string SourceTreeTestDataPath { get { return Path.Combine(SourceTreeInstallPath, "data"); } }
        public string SourceTreeDownloadPath { get; private set; }

        public static void ReplaceTextInFile(string pathToFile, string oldText, string newText)
        {
            var fileContent = File.ReadAllText(pathToFile);
            fileContent = fileContent.Replace(oldText, newText);
            File.WriteAllText(pathToFile, fileContent);
        }

        protected virtual void RunAndAttach()
        {
            RunAndAttachToSourceTree();
        }

        protected void RunAndAttachToSourceTree()
        { 
            var attempt = 0;

            do
            {
                KillSourceTree();
                RunSourceTree(SourceTreeExePath);
                attempt++;
            }
            while (!IsSourceTreeWindowOpeded() && attempt < 5);
            RunSourceTree(SourceTreeExePath);
            AttachToSourceTree();
        }

        public bool IsSourceTreeWindowOpeded()
        {
            if (SourceTreeProcess.MainWindowTitle.Equals("SourceTree") || SourceTreeProcess.MainWindowTitle.Equals("Welcome"))
            {
                return true;
            }

            return false;
        }

        protected void SetFile(string sourceFile, string targetFile)
        {
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }

            File.Copy(sourceFile, targetFile);
        }

        public static Version GetSourceTreeVersion(string sourceTreeExePath)
        {
            // Get the file version for the file.
            FileVersionInfo stVersionInfo = FileVersionInfo.GetVersionInfo(sourceTreeExePath);

            return new Version(stVersionInfo.FileVersion);
        }

        protected void AttachToSourceTree()
        {
            MainWindow = null;
            MainWindow = Utils.FindNewWindow("SourceTree");
        }

        protected void RunSourceTree(string sourceTreeExe)
        {
            if (string.IsNullOrWhiteSpace(sourceTreeExe) || !File.Exists(sourceTreeExe))
            {
                Assert.Fail($"Unable to start [{sourceTreeExe}]");
            }

            Console.WriteLine($"Starting [{sourceTreeExe}]");
            // run SourceTree
            ProcessStartInfo psi = new ProcessStartInfo(sourceTreeExe);
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = Environment.ExpandEnvironmentVariables("%TEMP%");
            SourceTreeProcess = new Process();
            SourceTreeProcess.StartInfo = psi;

            SourceTreeProcess.Start();
        }

        private void KillSourceTree()
        {
            foreach (var process in Process.GetProcessesByName("SourceTree"))
            {
                try
                {
                    process.CloseMainWindow();
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw new Win32Exception(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw new Win32Exception(e.Message);
                }
            }
        }

        public IEnumerable<string> FindUserConfigs()
        {
            var userConfigHome = Path.Combine(Environment.ExpandEnvironmentVariables("%LocalAppData%"), "Atlassian");
            if (!Directory.Exists(userConfigHome))
            {
                return new List<string>();
            }

            return Directory.GetDirectories(userConfigHome, SourceTreeVersion.ToString(), SearchOption.AllDirectories);
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (MainWindow != null)
            {
                var allChildWindow = MainWindow.ModalWindows();
                foreach (var window in allChildWindow)
                {
                    window.Close();
                }
                MainWindow.Close();
            }

            if (!SourceTreeProcess.HasExited)
            {
                SourceTreeProcess.CloseMainWindow();
                SourceTreeProcess.Close();
            }

            CleanTransitoryDirectories();

            CleanUserConfigPaths();
        }

        private void CleanTransitoryDirectories()
        {
            var inError = false;
            do
            {
                try
                {
                    CleanDirectory(SourceTreeInstallPath);
                    inError = false;
                }
                catch (Exception)
                {
                    inError = true;
                }
            } while (inError);
        }

        public static void CleanDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                CleanDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
    }
}
