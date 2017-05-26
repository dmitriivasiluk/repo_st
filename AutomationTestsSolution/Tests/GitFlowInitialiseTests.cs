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
            gitFlowInitWindow.ClickCancelButton();
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

            Assert.IsTrue(gitFlowInitWindow.IsDefaultBranchNameCorrect(gitFlowInitWindow.ProductionBranchTextbox, ConstantsList.defaultProductionBranch));
            Assert.IsTrue(gitFlowInitWindow.IsDefaultBranchNameCorrect(gitFlowInitWindow.DevelopmentBranchTextbox, ConstantsList.defaultDevelopmentBranch));
            Assert.IsTrue(gitFlowInitWindow.IsDefaultBranchNameCorrect(gitFlowInitWindow.FeatureBranchTextbox, ConstantsList.defaultFeatureBranch));
            Assert.IsTrue(gitFlowInitWindow.IsDefaultBranchNameCorrect(gitFlowInitWindow.ReleaseBranchTextbox, ConstantsList.defaultReleaseBranch));
            Assert.IsTrue(gitFlowInitWindow.IsDefaultBranchNameCorrect(gitFlowInitWindow.HotfixBranchTextbox, ConstantsList.defaultHotfixBranch));
            Assert.IsTrue(gitFlowInitWindow.IsVersionTagEmpty());
        }

        [Test]
        [Category("GitFlow")]
        public void CheckWhetherDefaultBranchNamesCorrect()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            gitFlowInitWindow = mainWindow.ClickGitFlowButton();

            Assert.IsTrue(gitFlowInitWindow.TextboxDefaultContent(gitFlowInitWindow.ProductionBranchTextbox, ConstantsList.defaultProductionBranch));
            Assert.IsTrue(gitFlowInitWindow.TextboxDefaultContent(gitFlowInitWindow.DevelopmentBranchTextbox, ConstantsList.defaultDevelopmentBranch));
            Assert.IsTrue(gitFlowInitWindow.TextboxDefaultContent(gitFlowInitWindow.FeatureBranchTextbox, ConstantsList.defaultFeatureBranch));
            Assert.IsTrue(gitFlowInitWindow.TextboxDefaultContent(gitFlowInitWindow.ReleaseBranchTextbox, ConstantsList.defaultReleaseBranch));
            Assert.IsTrue(gitFlowInitWindow.TextboxDefaultContent(gitFlowInitWindow.HotfixBranchTextbox, ConstantsList.defaultHotfixBranch));
            Assert.IsTrue(gitFlowInitWindow.IsVersionTagEmpty());
        }
    }
}