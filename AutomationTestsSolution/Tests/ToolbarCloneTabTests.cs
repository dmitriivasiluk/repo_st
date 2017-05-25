using System;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;

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

            Utils.ThreadWait(2000);

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
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();
            
            cloneTab.SourcePathTextBox.SetValue(ConstantsList.gitRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType));
        }

        [Test]
        [Category("CloneTab")]
        public void ValidateMercurialRepoLinkTest() // Mercurial should be installed
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue(ConstantsList.mercurialRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType));
        }

        [Test]
        [Category("CloneTab")]
        public void ValidateInvalidRepoLinkTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue(ConstantsList.notValidRepoLink);

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.notValidPath));
        }

        [Test]
        [Category("CloneTab")]
        public void CheckNoPathSuppliedMessageDisplayed()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue("");

            Assert.IsTrue(cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.noPathSupplied));
        }

        [Test]
        [Category("CloneTab")]
        public void CheckCloneButtonEnabledTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue(ConstantsList.gitRepoLink);            
            cloneTab.TriggerValidation();

            Assert.IsTrue(cloneTab.IsCloneButtonEnabled());
        }

        [Test]
        [Category("CloneTab")]
        public void CheckCloneGitRepoTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue(gitRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType);
            cloneTab.ClickCloneButton();

            var isFolderInitialized = GitWrapper.GetRepositoryByPath(pathToClonedGitRepo);            

            Assert.IsNotNull(isFolderInitialized);
        }

        [Test]
        [Category("CloneTab")]
        public void CheckCloneMercurialRepoTest()  // Mercurial should be installed
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue(mercurialRepoToClone);

            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType);
            cloneTab.ClickCloneButton();

            bool isDotHgExistByPath = Utils.IsFolderMercurial(pathToClonedMercurialRepo);

            Assert.IsTrue(isDotHgExistByPath);
        }

        [Test]
        [Category("CloneTab")]
        public void CheckBookmarkAppearedAfterGitRepoClonedTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue(gitRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.gitRepoType);
            cloneTab.ClickCloneButton();
            var localtab = mainWindow.OpenTab<LocalTab>();
            var bookmarkAdded = localtab.IsTestGitRepoBookmarkAdded();
            Assert.IsTrue(bookmarkAdded);
        }

        [Test]
        [Category("CloneTab")]
        public void CheckBookmarkAppearedAfterHgRepoClonedTest()  // Mercurial should be installed
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            CloneTab cloneTab = mainWindow.OpenTab<CloneTab>();

            cloneTab.SourcePathTextBox.SetValue(mercurialRepoToClone);
            cloneTab.GetValidationMessage(CloneTab.LinkValidationMessage.mercurialRepoType);
            cloneTab.ClickCloneButton();
            var localtab = mainWindow.OpenTab<LocalTab>();
            var bookmarkAdded = localtab.IsTestHgRepoBookmarkAdded();
            Assert.IsTrue(bookmarkAdded);
        }
    }
}