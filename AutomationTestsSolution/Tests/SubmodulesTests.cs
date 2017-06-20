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
    class SubmodulesTests : BasicTest
    {
        #region Test Variables
        public string PathToClonedGitRepo { get { return Path.Combine(SourceTreeTestDataPath, ConstantsList.testGitRepoBookmarkName); } }

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
            Directory.CreateDirectory(PathToClonedGitRepo);
        }
        private void RemoveTestFolder()
        {
            Utils.RemoveDirectory(PathToClonedGitRepo);
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

            addSubmoduleWindow.SourcePathTextbox.SetValue(PathToClonedGitRepo);
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
            Repository.Init(PathToClonedGitRepo);
            
            var openTabsPath = Path.Combine(SourceTreeUserDataPath, ConstantsList.opentabsXml);
            var openTabsXml = new OpenTabsXml(openTabsPath);
            openTabsXml.SetOpenTab(PathToClonedGitRepo);
            openTabsXml.Save();
        }
    }
}
