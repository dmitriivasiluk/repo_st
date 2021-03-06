﻿using System.IO;
using System.Threading;
using AutomationTestsSolution.Helpers;
using NUnit.Framework;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows.Menu;
using SourceTree.AutomationTests.Utils.Windows.Menu.Repository;
using SourceTree.AutomationTests.Utils.Windows.MenuFolder;

namespace SourceTree.AutomationTests.General.Menu.Repository.AddSubmodule
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
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(PathToClonedGitRepo);
        }

        [Test]
        [Category("Submodules")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void IsOkButtonDisabledWithEmptySourcePath()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(IsOkButtonDisabledWithEmptySourcePath));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddSubmoduleWindow>(RepositoryMenu.OperationsRepositoryMenu.AddSubmodule);

            Assert.IsFalse(addSubmoduleWindow.IsOkButtonEnabled());
        }

        [Test]
        [Category("Submodules")]
        [Category("General")]
        [Category("StartWithRepoOpened")]
        public void IsOkButtonEnabledWithEnteredSourcePath()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(IsOkButtonEnabledWithEnteredSourcePath));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddSubmoduleWindow>(RepositoryMenu.OperationsRepositoryMenu.AddSubmodule);

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
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourcePathFieldValidateWrongInputTest));
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddSubmoduleWindow>(RepositoryMenu.OperationsRepositoryMenu.AddSubmodule);

            addSubmoduleWindow.SetTextboxContent(addSubmoduleWindow.SourcePathTextbox, testString);
            var isValidationMessageCorrect = addSubmoduleWindow.GetValidationMessage(AddSubmoduleWindow.LinkValidationMessage.notValidPath);

            Assert.IsTrue(isValidationMessageCorrect);
        }

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
    }
}
