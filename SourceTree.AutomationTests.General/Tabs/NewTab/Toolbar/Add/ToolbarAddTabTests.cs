using System.IO;
using LibGit2Sharp;
using NUnit.Framework;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;

namespace SourceTree.AutomationTests.General.Tabs.NewTab.Toolbar.Add
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
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(PathToTestGitFolder);
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(PathToTestHgFolder);
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(PathToEmptyFolder);
        }

        private void CreateTestFolders()
        {
            Directory.CreateDirectory(PathToTestGitFolder);
            Directory.CreateDirectory(PathToTestHgFolder);
            Directory.CreateDirectory(PathToEmptyFolder);
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void AddGitFolderValidationMessageTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();
            addTab.WorkingCopyPathTextBox.SetValue(PathToTestGitFolder);

            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.gitRepoType));
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void AddHgFolderValidationMessageTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AddHgFolderValidationMessageTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();
            addTab.WorkingCopyPathTextBox.SetValue(PathToTestHgFolder);

            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.mercurialRepoType));
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void AddNotRepoFolderValidationMessageTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AddNotRepoFolderValidationMessageTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToEmptyFolder);

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.notValidPath));
            Assert.IsFalse(isAddButtonEnabled);
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void AddEmptyPathValidationMessageTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AddEmptyPathValidationMessageTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.SetTextboxContent(addTab.WorkingCopyPathTextBox, "");            
            
            Assert.IsTrue(addTab.GetValidationMessage(AddTab.RepoValidationMessage.noWorkingPathSupplied));
            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsFalse(isAddButtonEnabled);
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckAddButtonEnablesWithValidGitFolderTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckAddButtonEnablesWithValidGitFolderTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestGitFolder);
            addTab.TriggerValidation();

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(isAddButtonEnabled);
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckAddButtonEnablesWithValidHgFolderTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckAddButtonEnablesWithValidGitFolderTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestHgFolder);
            addTab.TriggerValidation();

            bool isAddButtonEnabled = addTab.AddButton.Enabled;
            Assert.IsTrue(isAddButtonEnabled);
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckOpenedRepoTitleAfterAddGitFolderTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckOpenedRepoTitleAfterAddGitFolderTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestGitFolder);
            addTab.TriggerValidation();
            var repoName = addTab.NameTextBox.Text;
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = addTab.ClickAddButton();           

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }

        [Test]
        [Category("AddTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckOpenedRepoTitleAfterAddHgFolderTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckOpenedRepoTitleAfterAddHgFolderTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            AddTab addTab = mainWindow.OpenTab<AddTab>();

            addTab.WorkingCopyPathTextBox.SetValue(PathToTestHgFolder);
            addTab.TriggerValidation();
            var repoName = addTab.NameTextBox.Text;
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = addTab.ClickAddButton();

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