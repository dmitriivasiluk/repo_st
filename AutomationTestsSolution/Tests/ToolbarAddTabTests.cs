using System;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using System.IO;
using LibGit2Sharp;
using ScreenObjectsHelpers.Windows.Repository;

namespace AutomationTestsSolution.Tests
{
    class ToolbarAddTabTests : BasicTest
    {
        #region Test Variables

        private string PathToTestGitFolder { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.gitInitFolderForAddTest); } }
        private string PathToTestHgFolder { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.hgInitFolderForAddTest); } }
        private string PathToEmptyFolder { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.emptyFolderForAddTest); } }
        #endregion

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();            

            RemoveTestFolders();
        }

        private void RemoveTestFolders()
        {
            Utils.RemoveDirectory(PathToTestGitFolder);
            Utils.RemoveDirectory(PathToTestHgFolder);
            Utils.RemoveDirectory(PathToEmptyFolder);
        }

        private void CreateTestFolders()
        {
            Directory.CreateDirectory(PathToTestGitFolder);
            Directory.CreateDirectory(PathToTestHgFolder);
            Directory.CreateDirectory(PathToEmptyFolder);
        }

        [Test]
        public void AddGitFolderValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();
            addTab.WorkingCopyPathTextBox.SetValue(PathToTestGitFolder);

            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.gitRepoType));
        }

        [Test]
        public void AddHgFolderValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();
            addTab.WorkingCopyPathTextBox.SetValue(PathToTestHgFolder);

            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.mercurialRepoType));
        }

        [Test]
        public void AddNotRepoFolderValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToEmptyFolder);

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.notValidPath));
            Assert.IsFalse(isAddButtonEnabled);
        }

        [Test]
        public void AddEmptyPathValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue("");            
            
            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.noWorkingPathSupplied));
            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsFalse(isAddButtonEnabled);
        }

        [Test]
        public void CheckAddButtonEnablesWithValidGitFolderTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestGitFolder);
            addTab.TriggerValidation();

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(isAddButtonEnabled);
        }

        [Test]
        public void CheckAddButtonEnablesWithValidHgFolderTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestHgFolder);
            addTab.TriggerValidation();

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(isAddButtonEnabled);
        }

        [Test]
        public void CheckOpenedRepoTitleAfterAddGitFolderTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestGitFolder);
            addTab.TriggerValidation();
            var repoName = addTab.NameTextBox.Text;
            RepositoryTab repoTab = addTab.ClickAddButton();           

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }

        [Test]
        public void CheckOpenedRepoTitleAfterAddHgFolderTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestHgFolder);
            addTab.TriggerValidation();
            var repoName = addTab.NameTextBox.Text;
            RepositoryTab repoTab = addTab.ClickAddButton();

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }
        
        protected override void PerTestPreConfigureSourceTree()
        {
            RemoveTestFolders();

            CreateTestFolders();

            Repository.Init(PathToTestGitFolder);
            var mercurial = new MercurialWrapper(Path.Combine(SourceTreeDownloadPath, "hg_local", "hg.exe"));
            mercurial.Init(PathToTestHgFolder);
        }
    }
}