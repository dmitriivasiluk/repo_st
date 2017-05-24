using System;
using System.IO;
using System.Net;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using ScreenObjectsHelpers.Windows;

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

        [Test, Order(1)]
        public void ValidateLocalRepoNameTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(pathToAllRepos + gitRepoName);
            Assert.AreEqual(createTab.NameRepoTextBox.Text, gitRepoName);
        }

        [Test, Order(2)]
        public void CheckCreateButtonUnavailableOnDestinationEmpty()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            //for sure
            createTab.DestinationPathTextBox.SetValue("");
            Assert.IsFalse(createTab.CreateButton.Visible && createTab.CreateButton.Enabled);
        }

        [Test, Order(3)]
        public void CheckCreateButtonUnavailableOnNoRepoAccount()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(pathToAllRepos + gitRepoName);
            createTab.CheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.DescriptionTextBox.Focus();
            Assert.IsFalse(createTab.CreateButton.Visible && createTab.CreateButton.Enabled);
        }

        //TODO Add dialow window verification
        [Test, Order(4)]
        public void CheckBrowserButtonWorks()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            Assert.IsTrue(createTab.BrowseButton.Enabled && createTab.BrowseButton.Visible);
            Assert.DoesNotThrow(() => createTab.ClickButton(createTab.BrowseButton));
        }

        [Test]
        public void CheckLocalGitRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(pathToAllRepos + gitRepoName);
            mainWindow.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.GitHub);
            createTab.ClickButton(createTab.CreateButton);
            Utils.ThreadWait(2000);
            Assert.IsTrue(Directory.Exists(pathToAllRepos + gitRepoName));
            Assert.IsTrue(Directory.Exists(pathToAllRepos + gitRepoName + "\\" + ConstantsList.dotGitFolder));
            Assert.AreEqual(createTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        public void CheckLocalHgRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(pathToAllRepos + mercurialRepoName);
            mainWindow.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.RepoTypeComboBox.Select(CreateTab.CVS.Mercurial);
            createTab.ClickButton(createTab.CreateButton);
            Utils.ThreadWait(2000);
            Assert.IsTrue(Directory.Exists(pathToAllRepos + mercurialRepoName));
            Assert.IsTrue(Directory.Exists(pathToAllRepos + mercurialRepoName + "\\" + ConstantsList.dotHgFolder));
            Assert.AreEqual(createTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        //[Ignore("Issue SRCTREE-1622")]
        public void CheckLocalNoneRepoCreatedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            Assert.Throws(typeof(TestStack.White.UIItems.UIActionException), () => createTab.RepoTypeComboBox.Select(CreateTab.CVS.None));
        }
    }
}

namespace AutomationTestsSolution.Tests.CreateRemote
{

}