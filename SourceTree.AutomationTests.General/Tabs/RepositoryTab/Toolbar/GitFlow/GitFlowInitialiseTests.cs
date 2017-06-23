using System.IO;
using AutomationTestsSolution.Helpers;
using LibGit2Sharp;
using NUnit.Framework;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows.Menu.Repository;

namespace SourceTree.AutomationTests.General.Tabs.RepositoryTab.Toolbar.GitFlow
{
    class GitFlowInitialiseTests : BasicTest
    {
        #region Test Variables

        public string PathToClonedGitRepo { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.testGitRepoBookmarkName); } }

        private string userprofileToBeReplaced = ConstantsList.currentUserProfile;
        private string testString = "123";
        private GitFlowInitialiseWindow gitFlowInitWindow;
        #endregion

        [TearDown]
        public override void TearDown()
        {
            gitFlowInitWindow.ClickButtonToGetRepository(gitFlowInitWindow.CancelButton);
            base.TearDown();
            RemoveTestFolder();
        }
        private void CreateTestFolder()
        {
            Directory.CreateDirectory(PathToClonedGitRepo);
        }
        private void RemoveTestFolder()
        {
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(PathToClonedGitRepo);
        }

        [Test]
        [Category("GitFlow")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void CheckUseDefaultsButtonResetTextboxesTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckUseDefaultsButtonResetTextboxesTest));
            Utils.Windows.Menu.Repository.RepositoryTab mainWindow = new Utils.Windows.Menu.Repository.RepositoryTab(MainWindow);            
            gitFlowInitWindow = mainWindow.ClickGitFlowButton();

            gitFlowInitWindow.SetAllTextboxes(testString);
            gitFlowInitWindow.ClickUseDefaultsButton();

            Assert.AreEqual(gitFlowInitWindow.ProductionBranchTextbox.Text, ConstantsList.defaultProductionBranch);
            Assert.AreEqual(gitFlowInitWindow.DevelopmentBranchTextbox.Text, ConstantsList.defaultDevelopmentBranch);
            Assert.AreEqual(gitFlowInitWindow.FeatureBranchTextbox.Text, ConstantsList.defaultFeatureBranch);
            Assert.AreEqual(gitFlowInitWindow.ReleaseBranchTextbox.Text, ConstantsList.defaultReleaseBranch);
            Assert.AreEqual(gitFlowInitWindow.HotfixBranchTextbox.Text, ConstantsList.defaultHotfixBranch);
            Assert.IsTrue(gitFlowInitWindow.IsVersionTagEmpty());
        }

        [Test]
        [Category("GitFlow")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void CheckDefaultBranchNamesTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckDefaultBranchNamesTest));
            Utils.Windows.Menu.Repository.RepositoryTab mainWindow = new Utils.Windows.Menu.Repository.RepositoryTab(MainWindow);            
            gitFlowInitWindow = mainWindow.ClickGitFlowButton();

            Assert.AreEqual(gitFlowInitWindow.ProductionBranchTextbox.Text, ConstantsList.defaultProductionBranch);
            Assert.AreEqual(gitFlowInitWindow.DevelopmentBranchTextbox.Text, ConstantsList.defaultDevelopmentBranch);
            Assert.AreEqual(gitFlowInitWindow.FeatureBranchTextbox.Text, ConstantsList.defaultFeatureBranch);
            Assert.AreEqual(gitFlowInitWindow.ReleaseBranchTextbox.Text, ConstantsList.defaultReleaseBranch);
            Assert.AreEqual(gitFlowInitWindow.HotfixBranchTextbox.Text, ConstantsList.defaultHotfixBranch);

            Assert.IsTrue(gitFlowInitWindow.IsVersionTagEmpty());
        }

        protected override void PerTestPreConfigureSourceTree()
        {
            // init repo
            RemoveTestFolder();
            CreateTestFolder();
            Repository.Init(PathToClonedGitRepo);

            // open tab
            var openTabsPath = Path.Combine(SourceTreeUserDataPath, ConstantsList.opentabsXml);
            var openTabsXml = new OpenTabsXml(openTabsPath);
            openTabsXml.SetOpenTab(PathToClonedGitRepo);
            openTabsXml.Save();

        }
    }
}