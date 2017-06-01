﻿using LibGit2Sharp;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Repository;
using ScreenObjectsHelpers.Windows.MenuFolder;
using System;
using System.IO;
using static ScreenObjectsHelpers.Windows.MenuFolder.RepositoryMenu;

namespace AutomationTestsSolution.Tests
{
    class SubtreesTests : BasicTest
    {
        #region Test Variables
        private string pathToClonedGitRepo = Environment.ExpandEnvironmentVariables(ConstantsList.pathToClonedGitRepo);
        private string currentUserProfile = Environment.ExpandEnvironmentVariables(ConstantsList.currentUserProfile);
        // opentabs configuration
        private string openTabsPath = Environment.ExpandEnvironmentVariables(Path.Combine(ConstantsList.pathToDataFolder, ConstantsList.opentabsXml));
        private string resourceName = Resources.opentabs_for_clear_repo;

        private string userprofileToBeReplaced = ConstantsList.currentUserProfile;
        private string testString = "123";
        private AddLinkSubtreeWindow addLinkSubtree;
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
            addLinkSubtree.ClickButtonToGetRepository(addLinkSubtree.CancelButton);
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
        [Category("Subtrees")]
        public void IsOkButtonDisabledWithEmptySourcePath()
        {
            RepositoryTab mainWindow = new RepositoryTab(MainWindow);
            
            addLinkSubtree = mainWindow.OpenMenu<RepositoryMenu>().ClickOperationToReturnWindow<AddLinkSubtreeWindow>(OperationsRepositoryMenu.AddLinkSubtree);
            
            Assert.IsFalse(addLinkSubtree.IsOkButtonEnabled());
        }

        
    }
}
