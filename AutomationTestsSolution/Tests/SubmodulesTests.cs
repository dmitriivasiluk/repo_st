using LibGit2Sharp;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Repository;
using ScreenObjectsHelpers.Windows.MenuFolder;
using static ScreenObjectsHelpers.Windows.MenuFolder.RepositoryMenu;
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
            addSubmoduleWindow.ClickButtonToGetRepository(addSubmoduleWindow.CancelButton);
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
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void IsOkButtonDisabledWithEmptySourcePath()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeInstallArtifactsPath, nameof(IsOkButtonDisabledWithEmptySourcePath));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddSubmoduleWindow>(OperationsRepositoryMenu.AddSubmodule);

            Assert.IsFalse(addSubmoduleWindow.IsOkButtonEnabled());
        }

        [Test]
        [Category("Submodules")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void IsOkButtonEnabledWithEnteredSourcePath()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeInstallArtifactsPath, nameof(IsOkButtonEnabledWithEnteredSourcePath));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddSubmoduleWindow>(OperationsRepositoryMenu.AddSubmodule);

            addSubmoduleWindow.SourcePathTextbox.SetValue(PathToClonedGitRepo);
            addSubmoduleWindow.LocalRelativePathTextbox.Focus();
            Thread.Sleep(4000);

            Assert.IsTrue(addSubmoduleWindow.IsOkButtonEnabled());
        }

        [Test]
        [Category("Submodules")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void SourcePathFieldValidateWrongInputTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeInstallArtifactsPath, nameof(SourcePathFieldValidateWrongInputTest));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddSubmoduleWindow>(OperationsRepositoryMenu.AddSubmodule);

            addSubmoduleWindow.SetTextboxContent(addSubmoduleWindow.SourcePathTextbox, testString);
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
