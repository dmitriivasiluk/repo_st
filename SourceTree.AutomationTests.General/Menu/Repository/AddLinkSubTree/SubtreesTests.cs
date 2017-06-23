using System.IO;
using System.Threading;
using AutomationTestsSolution.Helpers;
using AutomationTestsSolution.Tests;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.MenuFolder;
using ScreenObjectsHelpers.Windows.Repository;

namespace AutomationTestsSolution.Menu.Repository.AddLinkSubTree
{
    class SubtreesTests : BasicTest
    {
        #region Test Variables
        public string PathToClonedGitRepo { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.testGitRepoBookmarkName); } }

        private string testString = "123";
        private AddLinkSubtreeWindow addLinkSubtree;
        #endregion

        protected override void PerTestPreConfigureSourceTree()
        {
            RemoveTestFolder();
            CreateTestFolder();
            LibGit2Sharp.Repository.Init(PathToClonedGitRepo);

            var openTabsPath = Path.Combine(SourceTreeUserDataPath, ConstantsList.opentabsXml);
            var openTabsXml = new OpenTabsXml(openTabsPath);
            openTabsXml.SetOpenTab(PathToClonedGitRepo);
            openTabsXml.Save();
        }

        [TearDown]
        public override void TearDown()
        {
            addLinkSubtree.ClickButtonToGetRepository(addLinkSubtree.CancelButton);
            base.TearDown();
            RemoveTestFolder();
        }
        private void CreateTestFolder()
        {
            Directory.CreateDirectory(PathToClonedGitRepo);
        }
        private void RemoveTestFolder()
        {
            Utils.RemoveDirectory(PathToClonedGitRepo);
        }

        [Test]
        [Category("Subtrees")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void IsOkButtonDisabledWithEmptySourcePath()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(IsOkButtonDisabledWithEmptySourcePath));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            
            addLinkSubtree = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddLinkSubtreeWindow>(RepositoryMenu.OperationsRepositoryMenu.AddLinkSubtree);
            
            Assert.IsFalse(addLinkSubtree.IsOkButtonEnabled());
        }

        [Test]
        [Category("Subtrees")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void IsOkButtonEnabledAfterCorrectDataSet()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(IsOkButtonEnabledAfterCorrectDataSet));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);

            addLinkSubtree = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddLinkSubtreeWindow>(RepositoryMenu.OperationsRepositoryMenu.AddLinkSubtree);

            addLinkSubtree.SetTextboxContent(addLinkSubtree.SourcePathTextbox, PathToClonedGitRepo);
            addLinkSubtree.SetTextboxContent(addLinkSubtree.LocalRelativePathTextbox, testString);
            Thread.Sleep(3000);
            addLinkSubtree.SetTextboxContent(addLinkSubtree.BranchCommitTextbox, testString);
            addLinkSubtree.SourcePathTextbox.Focus();
            Thread.Sleep(3000);

            Assert.IsTrue(addLinkSubtree.IsOkButtonEnabled());
        }
    }
}
