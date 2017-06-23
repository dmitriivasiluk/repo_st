using System.IO;
using System.Threading;
using AutomationTestsSolution.Tests;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Repository;
using ScreenObjectsHelpers.Windows.ToolbarTabs;

namespace AutomationTestsSolution.Tabs.NewTab.Toolbar.Clone
{
    class ToolbarCloneTabTests : BasicTest
    {
        #region Test Variables
        string gitRepoToClone = ConstantsList.gitRepoToClone;
        public string PathToClonedGitRepo { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.testGitRepoBookmarkName); } }
        public string PathToClonedHgRepo { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.testHgRepoBookmarkName); } }

        string mercurialRepoToClone = ConstantsList.mercurialRepoToClone;
        #endregion

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            Thread.Sleep(2000);

            RemoveTestFolders();
        }

        private void RemoveTestFolders()
        {
            Utils.RemoveDirectory(PathToClonedGitRepo);
            Utils.RemoveDirectory(PathToClonedHgRepo);
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void ValidateGitRepoLinkTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ValidateGitRepoLinkTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();
            
            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.gitRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType));
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void ValidateMercurialRepoLinkTest() // Mercurial should be installed
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ValidateMercurialRepoLinkTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.mercurialRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType));
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void ValidateInvalidRepoLinkTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ValidateInvalidRepoLinkTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.notValidRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.notValidPath));
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckNoPathSuppliedMessageDisplayed()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckNoPathSuppliedMessageDisplayed));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, "");

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.noPathSupplied));
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckCloneButtonEnabledTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckCloneButtonEnabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.gitRepoLink);            
            cloneTab.TriggerValidation();

            Assert.IsTrue(cloneTab.IsCloneButtonEnabled());
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckCloneGitRepoTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckCloneGitRepoTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, gitRepoToClone);
            cloneTab.DestinationPathTextBox.SetValue(PathToClonedGitRepo);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType);
            cloneTab.ClickCloneButton();

            var isFolderInitialized = GitWrapper.GetRepositoryByPath(PathToClonedGitRepo);            

            Assert.IsNotNull(isFolderInitialized);
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckCloneMercurialRepoTest()  // Mercurial should be installed
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckCloneMercurialRepoTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, mercurialRepoToClone);
            cloneTab.DestinationPathTextBox.SetValue(PathToClonedHgRepo);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckCloneMercurialRepoTest));
            cloneTab.ClickCloneButton();

            bool isDotHgExistByPath = Utils.IsFolderMercurial(PathToClonedHgRepo);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckCloneMercurialRepoTest));
            Assert.IsTrue(isDotHgExistByPath);
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        //[Ignore("Investigate stability issue")]
        public void CheckGitRepoOpenedAfterCloneTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckGitRepoOpenedAfterCloneTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, gitRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType);            
            var repoName = cloneTab.NameTextBox.Text;

            ScreenObjectsHelpers.Windows.Repository.RepositoryTab repoTab = cloneTab.ClickCloneButton();

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }

        [Test]
        [Category("CloneTab")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        //[Ignore("Investigate stability issue")]
        public void CheckHgRepoOpenedAfterCloneTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckHgRepoOpenedAfterCloneTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, mercurialRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType);
            var repoName = cloneTab.NameTextBox.Text;

            ScreenObjectsHelpers.Windows.Repository.RepositoryTab repoTab = cloneTab.ClickCloneButton();

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }
        
        protected override void PerTestPreConfigureSourceTree()
        {
            RemoveTestFolders();
        }
    }
}