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

        [Test, Category("CreateRepoUI")]
        public void ValidateLocalRepoNameTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(pathToAllRepos + gitRepoName);
            Assert.AreEqual(createTab.NameRepoTextBox.Text, gitRepoName);
        }

        [Test, Category("CreateRepoUI")]
        public void CheckCreateRepoButtonUnavailableOnDestinationEmpty()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            //for sure
            createTab.DestinationPathTextBox.SetValue("");
            Assert.IsFalse(createTab.CreateButton.Enabled);
        }

        [Test, Category("CreateRepoUI")]
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
        [Test, Category("CreateRepoUI")]
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

        [Test, Category("CreateRepoLocal")]
        public void CheckLocalGitRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, gitRepoName));
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            RepositoryTab repoTab = createTab.ClickCreateButton();
            Utils.ThreadWait(2000);
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, gitRepoName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, gitRepoName, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test, Category("CreateRepoLocal")]
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

        [Test, Category("CreateRepoLocal")]
        [Ignore("Issue SRCTREE-1622")]
        public void CheckLocalNoneRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            Assert.Throws(typeof(UIActionException), 
                () => createTab.RepoTypeComboBox.Select(CreateTab.CVS.None));
        }

        [Test, Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInEmptyFolderPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            CreateRepoDirecory(Path.Combine(pathToAllRepos, gitRepoName));
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, gitRepoName));
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            RepositoryTab repoTab = warning.ClickYesButton();
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, gitRepoName)));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, gitRepoName, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test, Category("CreateRepoLocal")]
        public void CheckLocalRepoCreateGitInEmptyFolderNegativeTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            CreateRepoDirecory(Path.Combine(pathToAllRepos, gitRepoName));
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, gitRepoName));
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToAllRepos, gitRepoName)));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToAllRepos, gitRepoName, ConstantsList.dotGitFolder)));
        }

        [Test, Category("CreateRepoLocal")]
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

        [Test, Category("CreateRepoLocal")]
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

        #region ServiceMethods
        private bool CreateRepoDirecory(string path)
        {
            if (Directory.Exists(path)) return false;
            Directory.CreateDirectory(path);
            return true;
        }
        #endregion
    }
}

namespace AutomationTestsSolution.Tests.CreateRemote
{

}