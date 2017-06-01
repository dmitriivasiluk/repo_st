using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using NUnit.Framework;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using ScreenObjectsHelpers.Windows.Repository;
using Microsoft.Win32;
using System.Diagnostics;

namespace AutomationTestsSolution.Tests.CreateLocal
{
    class ToolbarCreateTabTestLocal : BasicTest
    {
         #region Test Variables
        string gitRepoName = ConstantsList.testGitRepoBookmarkName;
        string mercurialRepoName = ConstantsList.testHgRepoBookmarkName;
        string pathToAllRepos = Environment.ExpandEnvironmentVariables(ConstantsList.pathToCreateLocalRepos);
        #endregion

        [SetUp]
        public override void SetUp()
        {
            //if folders exist for some reason
            RemoveTestFolders(new string[] { pathToAllRepos + gitRepoName, pathToAllRepos + mercurialRepoName });
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            RemoveTestFolders(new string[] { pathToAllRepos + gitRepoName, pathToAllRepos + mercurialRepoName });
        }

        [Test]
        [Category("CreateRepoUI")]
        public void ValidateLocalRepoNameTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(pathToAllRepos + gitRepoName);
            Assert.AreEqual(createTab.NameRepoTextBox.Text, gitRepoName);
        }

        [Test]
        [Category("CreateRepoUI")]
        public void CheckCreateRepoButtonUnavailableOnDestinationEmpty()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            //for sure
            createTab.DestinationPathTextBox.SetValue("");
            Assert.IsFalse(createTab.CreateButton.Enabled);
        }

        [Test]
        [Category("CreateRepoUI")]
        public void CheckCreateRepoButtonUnavailableOnNoRepoAccount()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, gitRepoName));
            createTab.CheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.DescriptionTextBox.Focus();
            Assert.IsFalse(createTab.CreateButton.Enabled);
        }

        //TODO Add dialow window verification
        [Test]
        [Category("CreateRepoUI")]
        public void CheckBrowserButtonWorks()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            Assert.IsTrue(createTab.BrowseButton.Enabled && createTab.BrowseButton.Visible);
            Assert.DoesNotThrow(() => createTab.ClickButton(createTab.BrowseButton));
            DialogSelectDestination dialog = new DialogSelectDestination(MainWindow);
            var titleBar = dialog.titleBar.Name;
            dialog.ClickCancelButton();
            Assert.AreEqual(ConstantsList.dialogSelectDestinationTitle, titleBar);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalGitRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            RepositoryTab repoTab = createTab.ClickCreateButton();
            Utils.ThreadWait(2000);
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalHgRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, mercurialRepoName));
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            RepositoryTab repoTab = createTab.ClickCreateButton();
            Utils.ThreadWait(2000);
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, mercurialRepoName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, mercurialRepoName, ConstantsList.dotHgFolder)));
            Assert.AreEqual(repoTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Ignore("Issue SRCTREE-1622")]
        public void CheckLocalNoneRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            Assert.Throws(typeof(UIActionException), 
                () => createTab.RepoTypeComboBox.Select(CreateTab.CVS.None));
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInEmptyFolderPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            RepositoryTab repoTab = warning.ClickYesButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInEmptyFolderNegativeTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateHgInEmptyFolderPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            CreateRepoDirecory(Path.Combine(pathToAllRepos, mercurialRepoName));
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, mercurialRepoName));
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            RepositoryTab repoTab = warning.ClickYesButton();
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, mercurialRepoName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, mercurialRepoName, ConstantsList.dotHgFolder)));
            Assert.AreEqual(repoTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateHgInEmptyFolderNegativeTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            CreateRepoDirecory(Path.Combine(pathToAllRepos, mercurialRepoName));
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, mercurialRepoName));
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, mercurialRepoName)));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToAllRepos, mercurialRepoName, ConstantsList.dotHgFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInNotEmptyFolderPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            RepositoryTab repoTab = warning.ClickYesButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInNotEmptyFolderNegativeTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateHgInNotEmptyFolderPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            RepositoryTab repoTab = warning.ClickYesButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
            Assert.AreEqual(repoTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateHgInNotEmptyFolderNegativeTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Ignore("Not stable yet")]
        public void CheckLocalRepoCreateGitInExistRepoPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            GitInit(pathToRepo);
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            RepositoryTab repoTab = warning.ClickYesButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            //New thumb is opened and is accessible
            Assert.DoesNotThrow(() => repoTab.TabThumb.Click());
            //Supposed to be TabName
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateHgInExistRepoPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            var repo = new Mercurial.Repository(pathToRepo);
            repo.Init();
            createTab.DestinationPathTextBox.SetValue(pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickYesButton();
            WarningExistingRepoMercurial warningHg = new WarningExistingRepoMercurial(MainWindow);
            var warnTitle = warningHg.titleBar.Name;
            warningHg.ClickCloseButton();
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
            Assert.AreEqual(ConstantsList.mercurialExistRepoWarnTitle, warnTitle);
            // Retirned to create repo tab - all elements are accessible
            Assert.DoesNotThrow(() => createTab.DestinationPathTextBox.Focus());
        }

        #region ServiceMethods
        private bool CreateRepoDirecory(string path)
        {
            if (Directory.Exists(path)) return false;
            Directory.CreateDirectory(path);
            return true;
        }

        private bool CreateFile(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("string");
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        private bool IsValidGitRepo(string path)
        {
            return GitInteraction.IsValidRepo(path);
        }

        private void GitInit(string path)
        {
            if (!GitInteraction.IsInitSucessully(path)) throw new Exception("Cannot init git in " + path);
        }

        //TODO place this class in some another method as Git2Sharp substitute
        private class GitInteraction
        {
            private readonly Process _gitProcess;

            public static bool IsValidRepo(string path)
            {
                var repo = new GitInteraction(path, gitPath);
                return repo.IsGitRepository;
            }

            public static bool IsInitSucessully(string path)
            {
                var repo = new GitInteraction(path, gitPath);
                return repo.IsInitGit;
            }

            private bool IsGitRepository => !String.IsNullOrWhiteSpace(RunCommand("log -1"));
            private bool IsInitGit => !String.IsNullOrWhiteSpace(RunCommand("init"));
            private static string gitPath => GetGitPath();

            private GitInteraction(string path, string gitPath)
            {
                var processInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    RedirectStandardOutput = true,
                    FileName = Directory.Exists(gitPath) ? gitPath : "git.exe",
                    CreateNoWindow = true,
                    WorkingDirectory = (path != null && Directory.Exists(path)) ? path : Environment.CurrentDirectory
                };
                _gitProcess = new Process();
                _gitProcess.StartInfo = processInfo;
            }

            private string RunCommand(string args)
            {
                _gitProcess.StartInfo.Arguments = args;
                _gitProcess.Start();
                string output = _gitProcess.StandardOutput.ReadToEnd().Trim();
                _gitProcess.WaitForExit();
                return output;
            }

            private static string GetGitPath()
            {
                var programName = "Git ";
                foreach (var item in Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall")
                    .GetSubKeyNames())
                {
                    string program = Registry.LocalMachine.OpenSubKey(
                        "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\"
                        + item).GetValue("DisplayName") as string;
                    if (program != null && program.StartsWith(programName))
                    {
                        return Registry.LocalMachine.OpenSubKey(
                        "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\"
                        + item).GetValue("InstallLocation") as string;
                    }
                }
                throw new Exception("Git doesn't installed!");
            }
        }
        #endregion
    }
}

namespace AutomationTestsSolution.Tests.CreateRemote
{

}