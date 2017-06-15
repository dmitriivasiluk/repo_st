﻿using System;
using System.IO;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using ScreenObjectsHelpers.Windows.Repository;
using System.Threading;

namespace AutomationTestsSolution.Tests.CreateLocal
{
    class ToolbarCreateTabTestLocal : BasicTest
    {
         #region Test Variables
        string gitRepoName = ConstantsList.testGitRepoBookmarkName;
        string mercurialRepoName = ConstantsList.testHgRepoBookmarkName;
        string pathToAllRepos = Environment.ExpandEnvironmentVariables(ConstantsList.pathToDocumentsFolder);
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
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateLocalRepoNameTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateLocalRepoNameTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToAllRepos + gitRepoName);
            
            Assert.AreEqual(createTab.NameRepoTextBox.Text, gitRepoName);
        }

        [Test]
        [Category("CreateRepoUI")]
        public void CheckCreateRepoButtonUnavailableOnDestinationEmpty()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCreateRepoButtonUnavailableOnDestinationEmpty));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCreateRepoButtonUnavailableOnDestinationEmpty));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, "");
            
            Assert.IsFalse(createTab.CreateButton.Enabled);
        }

        [Test]
        [Category("CreateRepoUI")]
        public void CheckCreateRepoButtonUnavailableOnNoRepoAccount()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCreateRepoButtonUnavailableOnNoRepoAccount));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCreateRepoButtonUnavailableOnNoRepoAccount));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            createTab.SetTextboxContent(createTab.DestinationPathTextBox, Path.Combine(pathToAllRepos, gitRepoName));
            createTab.CheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.DescriptionTextBox.Focus();
            
            Assert.IsFalse(createTab.CreateButton.Enabled);
        }

        [Test]
        [Category("CreateRepoUI")]
        public void CheckBrowserButtonWorks()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckBrowserButtonWorks));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckBrowserButtonWorks));
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
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalGitRepoCreatedTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalGitRepoCreatedTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            RepositoryTab repoTab = createTab.ClickCreateButton();
            Thread.Sleep(2000);
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalHgRepoCreatedTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalHgRepoCreatedTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalHgRepoCreatedTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            RepositoryTab repoTab = createTab.ClickCreateButton();
            Thread.Sleep(2000);
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
            Assert.AreEqual(repoTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInEmptyFolderPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInEmptyFolderPositiveTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInEmptyFolderNegativeTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInEmptyFolderPositiveTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
        public void CheckLocalRepoCreateHgInEmptyFolderNegativeTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInEmptyFolderNegativeTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInNotEmptyFolderPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInNotEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInNotEmptyFolderPositiveTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInNotEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInNotEmptyFolderNegativeTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInNotEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInNotEmptyFolderPositiveTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInNotEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInNotEmptyFolderNegativeTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        //[Ignore("Not stable")]
        public void CheckLocalRepoCreateGitInExistRepoPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInExistRepoPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateGitInExistRepoPositiveTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            LibGit2Sharp.Repository.Init(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInExistRepoPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckLocalRepoCreateHgInExistRepoPositiveTest));
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(pathToAllRepos, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            var repo = new Mercurial.Repository(pathToRepo);
            repo.Init();
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
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
            // Returned to create repo tab - all elements are accessible
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
        #endregion
    }
}

namespace AutomationTestsSolution.Tests.CreateRemote
{

}