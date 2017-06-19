using LibGit2Sharp;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Repository;
using ScreenObjectsHelpers.Windows.MenuFolder;
using System;
using System.IO;
using System.Threading;
using AutomationTestsSolution.Helpers;

namespace AutomationTestsSolution.Tests
{
    class SubmodulesTests : AbstractUITest
    {
        #region Test Variables
        private string pathToClonedGitRepo = Environment.ExpandEnvironmentVariables(ConstantsList.pathToClonedGitRepo);
        private string currentUserProfile = Environment.ExpandEnvironmentVariables(ConstantsList.currentUserProfile);
        // opentabs configuration
        private string resourceName = Resources.opentabs_for_clear_repo;

        private string userprofileToBeReplaced = ConstantsList.currentUserProfile;
        private string testString = "123";
        private AddSubmoduleWindow addSubmoduleWindow;
        #endregion


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

            addSubmoduleWindow.SourcePathTextbox.SetValue(pathToClonedGitRepo);
            addSubmoduleWindow.LocalRelativePathTextbox.Focus();
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

        protected override void PerTestPreConfigureSourceTree()
        {
            RemoveTestFolder();
            CreateTestFolder();
            Repository.Init(pathToClonedGitRepo);

            
            var openTabsPath = Path.Combine(SourceTreeUserDataPath, ConstantsList.opentabsXml);
            var openTabsXml = new OpenTabsXml(openTabsPath);
            openTabsXml.SetOpenTab(pathToClonedGitRepo);
        }
    }
}
