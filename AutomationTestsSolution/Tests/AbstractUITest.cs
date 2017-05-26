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
    public abstract class AbstractUITest
    {
        private string BackupSuffix = "st_ui_test_bak";
        protected Window MainWindow;
        protected string sourceTreeExePath;
        protected string sourceTreeVersion;
        protected string sourceTreeUserConfigPath;

        protected Process sourceTreeProcess;
        private string testDataFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../TestData");
        private string emptyAutomationFolder = Environment.ExpandEnvironmentVariables(ConstantsList.emptyAutomationFolder);

        //private Tuple<string, string> exeAndVersion = FindSourceTree();

        private static readonly string sourceTreeTypeEnvVar = Environment.GetEnvironmentVariable("ST_UI_TEST_TYPE"); // "Beta", "Alpha" ....

        public static readonly string DefaultSourceTreeInstallPath = Path.Combine(TestContext.CurrentContext.TestDirectory, DateTime.Now.Ticks.ToString());
        public static readonly string DefaultSourceTreeDownloadPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "downloads");

        public static readonly Version DefaultSourceTreeVersion = new Version("2.0.20.1");

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
            InstallHgSystem();
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
            var hgHelper = new EmbeddedHgHelper(DefaultSourceTreeDownloadPath, SourceTreeUserDataPath, SourceTreeAppPath, new Version("3.7.3"));
            hgHelper.DownloadGit(ForceCleanRun);
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
            var hgHelper = new EmbeddedHgHelper(DefaultSourceTreeDownloadPath, DefaultSourceTreeDownloadPath, SourceTreeAppPath, new Version("3.7.3"));
            hgHelper.DownloadGit(ForceCleanRun);
            if (!hgHelper.InstallHg(ForceCleanRun))
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

        private void InstallGitEmbedded()
        {
            var gitHelper = new EmbeddedGitHelper(DefaultSourceTreeDownloadPath, SourceTreeUserDataPath, SourceTreeAppPath, new Version("2.12.2.2"));
            gitHelper.DownloadGit(ForceCleanRun);
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
            var gitHelper = new EmbeddedGitHelper(DefaultSourceTreeDownloadPath, DefaultSourceTreeDownloadPath, SourceTreeAppPath, new Version("2.12.2.2"));
            gitHelper.DownloadGit(ForceCleanRun);
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

            SourceTreeNuPkgPath = Path.Combine(DefaultSourceTreeDownloadPath, "sourcetree.nupkg");

            if (File.Exists(SourceTreeNuPkgPath) && !ForceCleanRun)
            {
                // local cache of the download
                return true;
            }

            using (var client = new System.Net.WebClient())
            {
                client.DownloadFile($"https://bitbucket.org/atlassian/sourcetreeqaautomation/downloads/SourceTree-{SourceTreeTargetVersion}-full.nupkg", SourceTreeNuPkgPath);
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


        public string SourceTreeInstallPath { get; private set; }
        public string SourceTreeInstallTempPath { get { return Path.Combine(SourceTreeInstallPath, "temp"); } }
        public string SourceTreeAppPath { get { return Path.Combine(SourceTreeInstallPath, "app"); } }
        public string SourceTreeNuPkgPath { get; private set; }
        public Version SourceTreeVersion { get; private set; }
        public Version SourceTreeTargetVersion { get; private set; }
        public string SourceTreeExePath { get { return Path.Combine(SourceTreeAppPath, "SourceTree.exe"); } }
        public string SourceTreeExeConfigPath { get { return Path.Combine(SourceTreeAppPath, "SourceTree.exe.config"); } }
        public string SourceTreeUserDataPath {  get {  return Path.Combine(SourceTreeAppPath, @"AppData\local\Atlassian\SourceTree"); } }
        public IEnumerable<string> SourceTreeUserConfigs { get; set; }
        public string TestDataPath { get { return testDataFolder; } }
        public string SourceTreeDownloadPath { get; private set; }
        protected void UseTestConfigAndAccountJson(string dataFolder)
        {
            var testAccountsJson = Path.Combine(testDataFolder, ConstantsList.accountsJson);
            var sourceTreeAccountsJsonPath = Path.Combine(dataFolder, ConstantsList.accountsJson);            

            SetFile(testAccountsJson, sourceTreeAccountsJsonPath);

            UseTestUserConfig();

            Utils.ThreadWait(1000);
        }

        protected void UseTestUserConfig()
        {
            var testUserConfig = Path.Combine(testDataFolder, ConstantsList.userConfig);

            SetFile(testUserConfig, sourceTreeUserConfigPath);

            UserProfileExpandVariables();

            Utils.ThreadWait(1000);
        }

        public static void ReplaceTextInFile(string pathToFile, string oldText, string newText)
        {
            var fileContent = File.ReadAllText(pathToFile);
            fileContent = fileContent.Replace(oldText, newText);
            File.WriteAllText(pathToFile, fileContent);
        }

        public void UserProfileExpandVariables()
        {
            var localappdataNewValue = Environment.ExpandEnvironmentVariables(ConstantsList.pathToLocalappdata);
            ReplaceTextInFile(sourceTreeUserConfigPath, ConstantsList.pathToLocalappdata, localappdataNewValue);

            var userprofileNewValue = Environment.ExpandEnvironmentVariables(ConstantsList.pathToUserprofile);
            ReplaceTextInFile(sourceTreeUserConfigPath, ConstantsList.pathToUserprofile, userprofileNewValue);
        }

        protected virtual void RunAndAttach()
        {
            RunAndAttachToSourceTree();
        }

        protected void RunAndAttachToSourceTree()
        {
            RunSourceTree(SourceTreeExePath);
            AttachToSourceTree();
        }

        private void BackupData(string dataFolder)
        {
            BackupFile(Path.Combine(dataFolder, ConstantsList.bookmarksXml));
            BackupFile(Path.Combine(dataFolder, ConstantsList.opentabsXml));
            BackupFile(Path.Combine(dataFolder, ConstantsList.accountsJson));            
        }

        protected void SetFile(string sourceFile, string targetFile)
        {
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }

            File.Copy(sourceFile, targetFile);
        }

        private void BackupFile(string fileName)
        {

            Utils.RemoveFile(fileName + BackupSuffix);
            Utils.ThreadWait(1000);
            if (File.Exists(fileName))
            {
                File.Move(fileName, fileName + BackupSuffix);
            }
        }

        protected void RestoreFile(string fileName)
        {
            Utils.RemoveFile(fileName);

            if (File.Exists(fileName + BackupSuffix))
            {
                File.Move(fileName + BackupSuffix, fileName);
            }
        }

        private void RestoreAccount(string account)
        {
            RestoreFile(account);
        }

        


        //public string GetSourceTreeVersion()
        //{
        //    var pathToConfig = FindSourceTreeUserConfig(sourceTreeVersion);
        //    var version = Path.GetDirectoryName(pathToConfig).Split('\\').LastOrDefault();

        //    return version;
        //}

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
            // run SourceTree
            ProcessStartInfo psi = new ProcessStartInfo(sourceTreeExe);
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            Directory.CreateDirectory(emptyAutomationFolder);
            psi.WorkingDirectory = emptyAutomationFolder;
            sourceTreeProcess = new Process();
            sourceTreeProcess.StartInfo = psi;

            sourceTreeProcess.Start();
        }

        //protected static Tuple<string, string> FindSourceTree()
        //{
        //    // Allowing Environment Variables to override defaults  lets us test against GA, Beta, Alpha with runtime changes etc.
        //    var sourceTreeType = string.IsNullOrWhiteSpace(sourceTreeTypeEnvVar) ? string.Empty : sourceTreeTypeEnvVar;

        //    var sourceTreeInstallParentDir =
        //        //Environment.ExpandEnvironmentVariables(@"%localappdata%\SourceTreeBeta" + sourceTreeType);
        //        Environment.ExpandEnvironmentVariables(@"%localappdata%\SourceTree" + sourceTreeType);
        //    // TODO find SourceTree
        //    // assumption that it is a squirrel install.
        //    string[] sourceTreeAppDirs = Directory.GetDirectories(sourceTreeInstallParentDir, "app-*",
        //        SearchOption.TopDirectoryOnly);
        //    Array.Sort(sourceTreeAppDirs);
        //    string sourceTreeAppDir = sourceTreeAppDirs.Last();
        //    string version = new DirectoryInfo(sourceTreeAppDir).Name.Substring("app-".Length);

        //    // TODO reset config to known state
        //    // TODO run SourceTree
        //    return new Tuple<string, string>(Path.Combine(sourceTreeAppDir, "SourceTree.exe"), version);
        //}

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

            if (!sourceTreeProcess.HasExited)
            {
                sourceTreeProcess.CloseMainWindow();
                sourceTreeProcess.Close();
                //sourceTreeProcess.Kill();
            }

            //sourceTreeProcess.WaitForExit();

            //Utils.ThreadWait(1000);

            //RestoreSourceTreeExeConfig();
            //RestoreFile(sourceTreeUserConfigPath);
            //RestoreData(sourceTreeDataPath);
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
