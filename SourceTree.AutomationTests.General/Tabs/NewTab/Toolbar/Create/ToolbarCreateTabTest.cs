using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;

namespace SourceTree.AutomationTests.General.Tabs.NewTab.Toolbar.Create
{
    class ToolbarCreateTabTestLocal : BasicTest
    {
         #region Test Variables
        string gitRepoName = ConstantsList.testGitRepoBookmarkName;
        string mercurialRepoName = ConstantsList.testHgRepoBookmarkName;
        #endregion

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, gitRepoName));
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, mercurialRepoName));
        }

        [Test]
        [Category("CreateRepoUI")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void ValidateLocalRepoNameTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ValidateLocalRepoNameTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(Path.Combine(SourceTreeTestDataPath, gitRepoName));
            
            Assert.AreEqual(createTab.NameRepoTextBox.Text, gitRepoName);
        }

        [Test]
        [Category("CreateRepoUI")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckCreateRepoButtonUnavailableOnDestinationEmpty()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckCreateRepoButtonUnavailableOnDestinationEmpty));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, "");
            
            Assert.IsFalse(createTab.CreateButton.Enabled);
        }

        [Test]
        [Category("CreateRepoUI")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckCreateRepoButtonUnavailableOnNoRepoAccount()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckCreateRepoButtonUnavailableOnNoRepoAccount));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            createTab.DestinationPathTextBox.SetValue(Path.Combine(SourceTreeTestDataPath, gitRepoName));
            createTab.CheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.DescriptionTextBox.Focus();
            
            Assert.IsFalse(createTab.CreateButton.Enabled);
        }

        [Test]
        [Category("CreateRepoUI")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckBrowserButtonWorks()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckBrowserButtonWorks));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            Assert.IsTrue(createTab.BrowseButton.Enabled && createTab.BrowseButton.Visible);
            Assert.DoesNotThrow(() => createTab.ClickButton(createTab.BrowseButton));

            DialogSelectDestination dialog = new DialogSelectDestination(MainWindow);
            var titleBar = dialog.titleBar.Name;
            dialog.ClickCancelButton();
            
            Assert.AreEqual(ConstantsList.dialogSelectDestinationTitle, titleBar);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalGitRepoCreatedTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalGitRepoCreatedTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, gitRepoName);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.GitHub);
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = createTab.ClickCreateButton();
            Thread.Sleep(2000);
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalHgRepoCreatedTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalHgRepoCreatedTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(SourceTreeTestDataPath, mercurialRepoName);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.Mercurial);
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = createTab.ClickCreateButton();
            Thread.Sleep(2000);
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
            Assert.AreEqual(repoTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateGitInEmptyFolderPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateGitInEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = warning.ClickYesButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateGitInEmptyFolderNegativeTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateGitInEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateHgInEmptyFolderPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateHgInEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(SourceTreeTestDataPath, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = warning.ClickYesButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
            Assert.AreEqual(repoTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateHgInEmptyFolderNegativeTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateHgInEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(SourceTreeTestDataPath, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateGitInNotEmptyFolderPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateGitInNotEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();

            var pathToRepo = Path.Combine(SourceTreeTestDataPath, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = warning.ClickYesButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateGitInNotEmptyFolderNegativeTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateGitInNotEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateHgInNotEmptyFolderPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateHgInNotEmptyFolderPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = warning.ClickYesButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
            Assert.AreEqual(repoTab.TabTextHg.Name, mercurialRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateHgInNotEmptyFolderNegativeTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateHgInNotEmptyFolderNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            CreateFile(Path.Combine(pathToRepo, ConstantsList.fileForNotEmptyFolder));
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickNoButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsFalse(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateGitInExistRepoPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateGitInExistRepoPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, gitRepoName);
            CreateRepoDirecory(pathToRepo);
            LibGit2Sharp.Repository.Init(pathToRepo);
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.GitHub);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            Utils.Windows.Menu.Repository.RepositoryTab repoTab = warning.ClickYesButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotGitFolder)));
            //New thumb is opened and is accessible
            Assert.DoesNotThrow(() => repoTab.TabThumb.Click());
            //Supposed to be TabName
            Assert.AreEqual(repoTab.TabTextGit.Name, gitRepoName);
        }

        [Test]
        [Category("CreateRepoLocal")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void CheckLocalRepoCreateHgInExistRepoPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CheckLocalRepoCreateHgInExistRepoPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            CreateTab createTab = mainWindow.OpenTab<CreateTab>();
            var pathToRepo = Path.Combine(SourceTreeTestDataPath, mercurialRepoName);
            CreateRepoDirecory(pathToRepo);
            var repo = new Mercurial.Repository(pathToRepo);
            repo.Init();
            createTab.SetTextboxContent(createTab.DestinationPathTextBox, pathToRepo);
            createTab.UncheckCheckbox(createTab.CreateRemoteCheckBox);
            createTab.SetComboboxValue(createTab.RepoTypeComboBox, CreateTab.CVS.Mercurial);
            WarningExistingEmptyFolder warning = createTab.ClickCreateButtonCallsWarning();
            warning.ClickYesButton();
            WarningExistingRepoMercurial warningHg = new WarningExistingRepoMercurial(MainWindow);
            var warnTitle = warningHg.titleBar.Name;
            warningHg.ClickCloseButton();
            
            Assert.IsTrue(Directory.Exists(pathToRepo));
            Assert.IsTrue(Directory.Exists(Path.Combine(pathToRepo, ConstantsList.dotHgFolder)));
            Assert.AreEqual(ConstantsList.mercurialExistRepoWarnTitle, warnTitle);
            // Returned to create repo tab - all elements are accessible
            Assert.DoesNotThrow(() => createTab.DestinationPathTextBox.Focus());
        }

        #region ServiceMethods
        private bool CreateRepoDirecory(string path)
        {
            if (Directory.Exists(path)) return false;
            Directory.CreateDirectory(path);
            return true;
        }

        private bool CreateFile(string path)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("string");
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        protected override void PerTestPreConfigureSourceTree()
        {
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, gitRepoName));
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, mercurialRepoName));
        }
    }
}