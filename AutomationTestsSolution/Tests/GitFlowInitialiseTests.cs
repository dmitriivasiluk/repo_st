using LibGit2Sharp;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using System.IO;
using System;
using ScreenObjectsHelpers.Windows.Repository;

namespace AutomationTestsSolution.Tests
{
    class GitFlowInitialiseTests : BasicTest
    {
        #region Test Variables
        private string pathToClonedGitRepo = Environment.ExpandEnvironmentVariables(ConstantsList.pathToClonedGitRepo);
        private string currentUserProfile = Environment.ExpandEnvironmentVariables(ConstantsList.currentUserProfile);
        
        // opentabs configuration
        private string openTabsPath = Environment.ExpandEnvironmentVariables(Path.Combine(ConstantsList.pathToDataFolder, ConstantsList.opentabsXml));
        private string resourceName = Resources.opentabs_for_clear_repo;

        private string userprofileToBeReplaced = ConstantsList.currentUserProfile;
        private string testString = "123";
        private GitFlowInitialiseWindow gitFlowInitWindow;
        #endregion

        [SetUp]
        public override void SetUp()
        {
            RemoveTestFolder();
            CreateTestFolder();
            Repository.Init(pathToClonedGitRepo);
            base.BackupConfigs();
            base.UseTestConfigAndAccountJson(sourceTreeDataPath);
            resourceName = resourceName.Replace(userprofileToBeReplaced, currentUserProfile);
            File.WriteAllText(openTabsPath, resourceName);
            base.RunAndAttachToSourceTree();
        }

        [TearDown]
        public override void TearDown()
        {
            gitFlowInitWindow.ClickButtonToGetRepository(gitFlowInitWindow.CancelButton);
            base.TearDown();
            RemoveTestFolder();
        }
        private void CreateTestFolder()
        {
            Directory.CreateDirectory(pathToClonedGitRepo);
        }
        private void RemoveTestFolder()
        {
            Utils.RemoveDirectory(pathToClonedGitRepo);
        }

        [Test]
        [Category ("GitFlow")]
        public void CheckUseDefaultsButtonResetTextboxesTest()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
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
        public void CheckDefaultBranchNamesTest()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);            
            gitFlowInitWindow = mainWindow.ClickGitFlowButton();

            Assert.AreEqual(gitFlowInitWindow.ProductionBranchTextbox.Text, ConstantsList.defaultProductionBranch);
            Assert.AreEqual(gitFlowInitWindow.DevelopmentBranchTextbox.Text, ConstantsList.defaultDevelopmentBranch);
            Assert.AreEqual(gitFlowInitWindow.FeatureBranchTextbox.Text, ConstantsList.defaultFeatureBranch);
            Assert.AreEqual(gitFlowInitWindow.ReleaseBranchTextbox.Text, ConstantsList.defaultReleaseBranch);
            Assert.AreEqual(gitFlowInitWindow.HotfixBranchTextbox.Text, ConstantsList.defaultHotfixBranch);

            Assert.IsTrue(gitFlowInitWindow.IsVersionTagEmpty());
        }
    }
}