using System;
using System.IO;
using System.Net;
using NUnit.Framework;
using TestStack.White.UIItems;
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
        }

        [Test, Category("CreateRepoLocal")]
        public void CheckLocalGitRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(Path.Combine(pathToAllRepos, gitRepoName));
            mainWindow.UncheckCheckbox(createTab.CreateRemoteCheckBox);
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
            mainWindow.UncheckCheckbox(createTab.CreateRemoteCheckBox);
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
    }
}

namespace AutomationTestsSolution.Tests.CreateRemote
{

}