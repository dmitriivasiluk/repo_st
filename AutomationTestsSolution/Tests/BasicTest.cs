using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;
using System.Threading;

namespace AutomationTestsSolution.Tests
{
    class BasicTest
    {
        private string BackupSuffix = "st_ui_test_bak";
        protected Window MainWindow;
        protected string sourceTreeExePath;
        protected string sourceTreeVersion;
        protected string sourceTreeUserConfigPath;
        protected string sourceTreeDataPath = Environment.ExpandEnvironmentVariables(ConstantsList.pathToDataFolder);
        protected Process sourceTreeProcess;
        private string testDataFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, @"../../TestData");
        private string emptyAutomationFolder = Environment.ExpandEnvironmentVariables(ConstantsList.emptyAutomationFolder);

        private Tuple<string, string> exeAndVersion = FindSourceTree();

        private static readonly string sourceTreeTypeEnvVar = Environment.GetEnvironmentVariable("ST_UI_TEST_TYPE"); // "Beta", "Alpha" ....

        [SetUp]
        public virtual void SetUp()
        {
            BackupConfigs();
            UseTestConfigAndAccountJson(sourceTreeDataPath);
            RunAndAttachToSourceTree();
        }

        protected void UseTestConfigAndAccountJson(string dataFolder)
        {
            var testAccountsJson = Path.Combine(testDataFolder, ConstantsList.accountsJson);
            var sourceTreeAccountsJsonPath = Path.Combine(dataFolder, ConstantsList.accountsJson);

            SetFile(testAccountsJson, sourceTreeAccountsJsonPath);

            UseTestUserConfig();

            Thread.Sleep(1000);
        }

        protected void UseTestUserConfig()
        {
            var testUserConfig = Path.Combine(testDataFolder, ConstantsList.userConfig);

            SetFile(testUserConfig, sourceTreeUserConfigPath);

            UserProfileExpandVariables();

            Thread.Sleep(1000);
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

        protected void BackupConfigs()
        {
            sourceTreeVersion = exeAndVersion.Item2;
            sourceTreeUserConfigPath = FindSourceTreeUserConfig(sourceTreeVersion);

            BackupFile(sourceTreeUserConfigPath);

            BackupData(sourceTreeDataPath);

            Thread.Sleep(1000);
        }

        protected void RunAndAttachToSourceTree()
        {
            RunSourceTree();
            AttachToSourceTree();
        }

        protected void RunSourceTree()
        {
            sourceTreeExePath = exeAndVersion.Item1;
            RunSourceTree(sourceTreeExePath);
        }

        private void BackupData(string dataFolder)
        {
            BackupFile(Path.Combine(dataFolder, ConstantsList.bookmarksXml));
            BackupFile(Path.Combine(dataFolder, ConstantsList.opentabsXml));
            BackupFile(Path.Combine(dataFolder, ConstantsList.accountsJson));
        }

        private void SetFile(string sourceFile, string targetFile)
        {
            File.Copy(sourceFile, targetFile);
        }

        private void RestoreData(string dataFolder)
        {
            RestoreFile(Path.Combine(dataFolder, ConstantsList.bookmarksXml));
            RestoreFile(Path.Combine(dataFolder, ConstantsList.opentabsXml));
            RestoreAccount(Path.Combine(sourceTreeDataPath, ConstantsList.accountsJson));
        }

        private void BackupFile(string fileName)
        {

            Utils.RemoveFile(fileName + BackupSuffix);
            Thread.Sleep(1000);
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

        private string FindSourceTreeUserConfig(string version)
        {
            var sourceTreeInstallParentDir =
                Environment.ExpandEnvironmentVariables(ConstantsList.pathToAtlassianFolder);
            var userConfigDirectories = Directory.GetDirectories(sourceTreeInstallParentDir, version,
                    SearchOption.AllDirectories);
            if (userConfigDirectories.Count(d => !d.Contains("vshost")) != 1)
            {
                throw new Exception("Unexpected number of user.config folders");
            }

            Array.Sort(userConfigDirectories);
            var folder = userConfigDirectories.Last(d => !d.Contains("vshost"));
            return Path.Combine(folder, ConstantsList.userConfig);
        }

        public string GetSourceTreeVersion()
        {
            var pathToConfig = FindSourceTreeUserConfig(sourceTreeVersion);
            var version = Path.GetDirectoryName(pathToConfig).Split('\\').LastOrDefault();

            return version;
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

        protected static Tuple<string, string> FindSourceTree()
        {
            // Allowing Environment Variables to override defaults  lets us test against GA, Beta, Alpha with runtime changes etc.
            var sourceTreeType = string.IsNullOrWhiteSpace(sourceTreeTypeEnvVar) ? string.Empty : sourceTreeTypeEnvVar;

            var sourceTreeInstallParentDir =
                //Environment.ExpandEnvironmentVariables(@"%localappdata%\SourceTreeBeta" + sourceTreeType);
                Environment.ExpandEnvironmentVariables(@"%localappdata%\SourceTree" + sourceTreeType);
            // TODO find SourceTree
            // assumption that it is a squirrel install.
            string[] sourceTreeAppDirs = Directory.GetDirectories(sourceTreeInstallParentDir, "app-*",
                SearchOption.TopDirectoryOnly);
            Array.Sort(sourceTreeAppDirs);
            string sourceTreeAppDir = sourceTreeAppDirs.Last();
            string version = new DirectoryInfo(sourceTreeAppDir).Name.Substring("app-".Length);

            // TODO reset config to known state
            // TODO run SourceTree
            return new Tuple<string, string>(Path.Combine(sourceTreeAppDir, "SourceTree.exe"), version);
        }

        protected void RemoveTestFolders(string[] testFolderArray)
        {
            foreach (var testFolder in testFolderArray)
            {
                Utils.RemoveDirectory(testFolder);
            }
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
            }

            Thread.Sleep(2000);

            RestoreFile(sourceTreeUserConfigPath);
            RestoreData(sourceTreeDataPath);
        }
    }
}