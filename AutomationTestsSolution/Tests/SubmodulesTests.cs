using LibGit2Sharp;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Repository;
using ScreenObjectsHelpers.Windows.MenuFolder;
using System;
using System.IO;
using System.Threading;

namespace AutomationTestsSolution.Tests
{
    class SubmodulesTests : BasicTest
    {
        #region Test Variables
        private string pathToClonedGitRepo = Environment.ExpandEnvironmentVariables(ConstantsList.pathToClonedGitRepo);
        private string currentUserProfile = Environment.ExpandEnvironmentVariables(ConstantsList.currentUserProfile);
        // opentabs configuration
        private string openTabsPath = Environment.ExpandEnvironmentVariables(Path.Combine(ConstantsList.pathToDataFolder, ConstantsList.opentabsXml));
        private string resourceName = Resources.opentabs_for_clear_repo;

        private string userprofileToBeReplaced = ConstantsList.currentUserProfile;
        private string testString = "123";
        private AddSubmoduleWindow addSubmoduleWindow;
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
            addSubmoduleWindow.ClickCancelButton();
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
        [Category("Submodules")]
        public void IsOkButtonDisabledWithEmptySourcePath()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().OpenAddSubmoduleWindow();

            Assert.IsFalse(addSubmoduleWindow.IsOkButtonEnabled());
        }

        [Test]
        [Category("Submodules")]
        public void IsOkButtonEnabledWithEnteredSourcePath()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().OpenAddSubmoduleWindow();

            addSubmoduleWindow.SetSourcePath(pathToClonedGitRepo);
            addSubmoduleWindow.LocalRelativePathTextboxFocus();
            addSubmoduleWindow.WaitWhileElementAvaliable(addSubmoduleWindow.OKButton);

            Assert.IsTrue(addSubmoduleWindow.IsOkButtonEnabled());
        }

        [Test]
        [Category("Submodules")]
        public void SourcePathFieldValidateWrongInputTest()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().OpenAddSubmoduleWindow();

            addSubmoduleWindow.SourcePathTextbox.SetValue(testString);
            var isValidationMessageCorrect = addSubmoduleWindow.GetValidationMessage(AddSubmoduleWindow.LinkValidationMessage.notValidPath);

            Assert.IsTrue(isValidationMessageCorrect);
        }
    }
}
