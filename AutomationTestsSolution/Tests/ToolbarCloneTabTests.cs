using System;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using ScreenObjectsHelpers.Windows.Repository;
using System.Threading;

namespace AutomationTestsSolution.Tests
{
    class ToolbarCloneTabTests : BasicTest
    {
        #region Test Variables
        string gitRepoToClone = ConstantsList.gitRepoToClone;
        string pathToClonedGitRepo = Environment.ExpandEnvironmentVariables(ConstantsList.pathToClonedGitRepo);
        string mercurialRepoToClone = ConstantsList.mercurialRepoToClone;
        string pathToClonedMercurialRepo = Environment.ExpandEnvironmentVariables(ConstantsList.pathToClonedMercurialRepo);
        #endregion

        /// <summary>        
        /// Pre-conditions: 
        /// Test repo folders are removed
        /// Mercurial is installed
        /// 2.0 Welcome - Disabled
        /// </summary>
        [SetUp]
        public override void SetUp()
        {
            RemoveTestFolders();

            base.SetUp();           
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            Thread.Sleep(2000);

            RemoveTestFolders();
        }

        private void RemoveTestFolders()
        {
            Utils.RemoveDirectory(pathToClonedGitRepo);
            Utils.RemoveDirectory(pathToClonedMercurialRepo);
        }

        [Test]
        [Category("CloneTab")]
        public void ValidateGitRepoLinkTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateGitRepoLinkTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateGitRepoLinkTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();
            
            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.gitRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType));
        }

        [Test]
        [Category("CloneTab")]
        public void ValidateMercurialRepoLinkTest() // Mercurial should be installed
        {
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateMercurialRepoLinkTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateMercurialRepoLinkTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.mercurialRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType));
        }

        [Test]
        [Category("CloneTab")]
        public void ValidateInvalidRepoLinkTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateInvalidRepoLinkTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(ValidateInvalidRepoLinkTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.notValidRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.notValidPath));
        }

        [Test]
        [Category("CloneTab")]
        public void CheckNoPathSuppliedMessageDisplayed()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckNoPathSuppliedMessageDisplayed));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckNoPathSuppliedMessageDisplayed));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, "");

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.noPathSupplied));
        }

        [Test]
        [Category("CloneTab")]
        public void CheckCloneButtonEnabledTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCloneButtonEnabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCloneButtonEnabledTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, ConstantsList.gitRepoLink);            
            cloneTab.TriggerValidation();

            Assert.IsTrue(cloneTab.IsCloneButtonEnabled());
        }

        [Test]
        [Category("CloneTab")]
        public void CheckCloneGitRepoTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCloneGitRepoTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCloneGitRepoTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, gitRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType);
            cloneTab.ClickCloneButton();

            var isFolderInitialized = GitWrapper.GetRepositoryByPath(pathToClonedGitRepo);            

            Assert.IsNotNull(isFolderInitialized);
        }

        [Test]
        [Category("CloneTab")]
        public void CheckCloneMercurialRepoTest()  // Mercurial should be installed
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCloneMercurialRepoTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckCloneMercurialRepoTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, mercurialRepoToClone);

            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType);
            cloneTab.ClickCloneButton();

            bool isDotHgExistByPath = Utils.IsFolderMercurial(pathToClonedMercurialRepo);

            Assert.IsTrue(isDotHgExistByPath);
        }

        [Test]
        [Category("CloneTab")]
        //[Ignore("Investigate stability issue")]
        public void CheckGitRepoOpenedAfterCloneTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckGitRepoOpenedAfterCloneTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckGitRepoOpenedAfterCloneTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, gitRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType);            
            var repoName = cloneTab.NameTextBox.Text;

            RepositoryTab repoTab = cloneTab.ClickCloneButton();

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }

        [Test]
        [Category("CloneTab")]
        //[Ignore("Investigate stability issue")]
        public void CheckHgRepoOpenedAfterCloneTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(CheckHgRepoOpenedAfterCloneTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            ScreenshotsTaker.TakeScreenShot(nameof(CheckHgRepoOpenedAfterCloneTest));
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SetTextboxContent(cloneTab.SourcePathTextBox, mercurialRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType);
            var repoName = cloneTab.NameTextBox.Text;

            RepositoryTab repoTab = cloneTab.ClickCloneButton();

            Assert.IsTrue(repoTab.IsRepoTabTitledWithText(repoName));
        }
    }
}