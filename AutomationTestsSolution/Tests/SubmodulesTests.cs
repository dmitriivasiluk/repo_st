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
        public void IsOkButtonDisabledWithEmptySourcePath()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().OpenAddSubmoduleWindow();

            Assert.IsFalse(addSubmoduleWindow.IsOkButtonEnabled());
        }
        [Test]
        public void IsOkButtonEnabledWithEnteredSourcePath()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().OpenAddSubmoduleWindow();

            addSubmoduleWindow.SetSourcePath(pathToClonedGitRepo);
            addSubmoduleWindow.LocalRelativePathTextboxFocus();
            Thread.Sleep(4000);

            Assert.IsTrue(addSubmoduleWindow.IsOkButtonEnabled());
        }

        //[Test]
        //public void SourcePathFieldValidateCorrectGitFolderTest()
        //{
        //    RepositoryTab mainWindow = new RepositoryTab(MainWindow);
        //    addSubmoduleWindow = mainWindow.OpenMenu<RepositoryMenu>().OpenAddSubmoduleWindow();

        //    addSubmoduleWindow.SourcePathTextbox.SetValue(testString);
        //    addSubmoduleWindow.LocalRelativePathTextboxFocus();
        //    var a = addSubmoduleWindow.WrongPathValidationMessage.Text;

        //    addSubmoduleWindow.AdvancedOptions.Click();
        //}
    }
}
