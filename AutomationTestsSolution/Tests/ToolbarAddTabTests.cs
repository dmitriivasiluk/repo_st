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
        private string pathToTestGitFolder = Environment.ExpandEnvironmentVariables(ConstantsList.gitInitFolderForAddTest);
        private string pathToTestHgFolder = Environment.ExpandEnvironmentVariables(ConstantsList.hgInitFolderForAddTest);
        private string pathToEmptyFolder = Environment.ExpandEnvironmentVariables(ConstantsList.emptyFolderForAddTest);
        #endregion

        [SetUp]
        public override void SetUp()
        {
            RemoveTestFolders();

            CreateTestFolders();

            Repository.Init(pathToTestGitFolder);
            MercurialWrapper.HgRun(MercurialWrapper.HgInit, pathToTestHgFolder);

            base.SetUp();           
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();            

            RemoveTestFolders();
        }

        private void RemoveTestFolders()
        {
            Utils.RemoveDirectory(pathToTestGitFolder);
            Utils.RemoveDirectory(pathToTestHgFolder);
            Utils.RemoveDirectory(pathToEmptyFolder);
        }

        private void CreateTestFolders()
        {
            Directory.CreateDirectory(pathToTestGitFolder);
            Directory.CreateDirectory(pathToTestHgFolder);
            Directory.CreateDirectory(pathToEmptyFolder);
        }

        [Test]
        public void AddGitFolderValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();
            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, pathToTestGitFolder);

            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.gitRepoType));
        }

        [Test]
        public void AddHgFolderValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();
            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, pathToTestHgFolder);

            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.mercurialRepoType));
        }

        [Test]
        public void AddNotRepoFolderValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, pathToEmptyFolder);

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.notValidPath));
            Assert.IsFalse(isAddButtonEnabled);
        }

        [Test]
        [Ignore("Investigate stability issue")]
        public void AddEmptyPathValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, "");            
            
            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.noWorkingPathSupplied));
            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsFalse(isAddButtonEnabled);
        }

        [Test]
        public void CheckAddButtonEnablesWithValidGitFolderTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, pathToTestGitFolder);
            addTab.TriggerValidation();

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(isAddButtonEnabled);
        }

        [Test]
        public void CheckAddButtonEnablesWithValidHgFolderTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, pathToTestHgFolder);
            addTab.TriggerValidation();

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(isAddButtonEnabled);
        }

        [Test]
        public void CheckOpenedRepoTitleAfterAddGitFolderTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, pathToTestGitFolder);
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

            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, pathToTestHgFolder);
            addTab.TriggerValidation();
            var repoName = addTab.NameTextBox.Text;
            RepositoryTab repoTab = addTab.ClickAddButton();

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }
    }
}