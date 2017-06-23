using System.IO;
using System.Threading;
using NUnit.Framework;
using ScreenObjectsHelpers.Windows;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows;

namespace SourceTree.AutomationTests.General.WelcomeWizard
{
    /// <summary>
    /// REQUIRMENTS TO RUN THIS TESTS:
    /// 1. Git is installed on computer
    /// 2. Mercurial is installed on computer
    /// 3. OAuth access to GitHub using credentials from tests
    /// 4. OAuth access to BitBucket using credentials from tests
    /// 5. Browser's autocomplete pop-up is disabled (Save password using IE in Authorization window to Atlassian)
    /// 6. Putty agent is running on computer (when it is running, there is no prompt about adding ssh key)
    /// 7. There are global ignore files (Git/Mercurial) on computers
    /// </summary>
    public class WelcomeWizardTests : AbstractWelcomeWizardTest
    {
        #region Test Variables

        protected string openSourceTreeTestFolder = "OpenSourceTree";
        protected string cloneOAuthGitHubTestFolder = "CloneOAuthGitHub";
        protected string cloneOAuthBitBucketTestFolder = "CloneOAuthBitBucket";
        private string cloneBasicBitBucketTestFolder = "CloneBasicBitBucket";
        private string cloneBasicGitHubTestFolder = "CloneBasicGitHub";
        protected string cloneBasicBitBucketServerTestFolder = "CloneBasicBitBucketServer";
        #endregion

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            RemoveTestFolders();
        }

        private void RemoveTestFolders()
        {
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, openSourceTreeTestFolder));
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneOAuthGitHubTestFolder));
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneOAuthBitBucketTestFolder));
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneBasicBitBucketTestFolder));
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneBasicGitHubTestFolder));
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneBasicBitBucketServerTestFolder));
        }

        [TestCase]
        [Category("WelcomeWizard")]
        public void ContinueButtonIsNotActiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ContinueButtonIsNotActiveTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.UncheckLicenceAgreementCheckbox();

            bool isContinueButtonActive = installWindow.IsContinueButtonActive();
            Assert.IsFalse(isContinueButtonActive);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree")]
        [Category("WelcomeWizard")]
        public void ValidRegistrationTest(string loginEmailToAtlassian, string passwordToAtlassian)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ValidRegistrationTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ValidRegistrationTest));

            installWindow = authentication.SignIn(loginEmailToAtlassian, passwordToAtlassian);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ValidRegistrationTest));

            bool isContinueButtonActive = installWindow.IsContinueButtonActive();
            string ActualtextRegistrationComplete = installWindow.CompleteText();
            //string ActualLoggedAsEmail = installWindow.LoggedAsEmail();       // Need AutomationId for this element, because get element by text - text dynamically changes
            Assert.AreEqual(ActualtextRegistrationComplete, "Registration Complete!");
            //Assert.AreEqual(ActualLoggedAsEmail, loginEmail);
            Assert.IsTrue(isContinueButtonActive);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "githubfaketesting", "123GitHubFake")]
        [Category("WelcomeWizard")]
        public void ConnectGitHubAccountTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string gitHubLogin,
            string gitHubPassword)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubAccountTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubAccountTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubAccountTest));
            installWindow.ClickContinueButton();
            installWindow.FillBasicAuthenticationGithub(gitHubLogin, gitHubPassword);
            installWindow.ClickContinueButton();

            string actualTitleOfNextStep = installWindow.DownloadingVersionText();
            // This is ensure that authentication was successful, because we are located on next step "Install tools"
            Assert.AreEqual(actualTitleOfNextStep, "Downloading version control systems...");
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "bitbucketfaketest", "123BitBucketFake")]
        [Category("WelcomeWizard")]
        public void ConnectBitbucketAccountTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string bitBucketLogin,
            string bitbucketPassword)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketAccountTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketAccountTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketAccountTest));

            installWindow.ClickContinueButton();

            installWindow.FillBasicAuthenticationBitbucket(bitBucketLogin, bitbucketPassword);

            installWindow.ClickContinueButton();

            string actualTitleOfNextStep = installWindow.DownloadingVersionText();
            // This is ensure that authentication was successful, because we are located on next step "Install tools"
            Assert.AreEqual(actualTitleOfNextStep, "Downloading version control systems...");
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "incorrectLogin", "incorrectPassword")]
        [Category("WelcomeWizard")]
        public void ConnectGitHubIncorrectCredentialsNegativeTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string gitHubLogin,
            string gitHubPassword)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubIncorrectCredentialsNegativeTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubIncorrectCredentialsNegativeTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubIncorrectCredentialsNegativeTest));

            installWindow.ClickContinueButton();

            installWindow.FillBasicAuthenticationGithub(gitHubLogin, gitHubPassword);

            installWindow.ClickContinueButton();

            bool actualIsContinueButtonActive = installWindow.IsContinueButtonActive();
            ErrorDialogWindow error = installWindow.SwitchToErrorDialogWindow();
            string actualTitleError = error.GetTitle();
            Assert.AreEqual(actualTitleError, "Login failed");
            Assert.IsFalse(actualIsContinueButtonActive);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "incorrectLogin", "IncorrectPassword")]
        [Category("WelcomeWizard")]
        public void ConnectBitbucketIncorrectCredentialsNegativeTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string bitBucketLogin,
            string bitBucketPassword)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketIncorrectCredentialsNegativeTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketIncorrectCredentialsNegativeTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketIncorrectCredentialsNegativeTest));

            installWindow.ClickContinueButton();

            installWindow.FillBasicAuthenticationBitbucket(bitBucketLogin, bitBucketPassword);

            installWindow.ClickContinueButton();

            bool actualIsContinueButtonActive = installWindow.IsContinueButtonActive();
            ErrorDialogWindow error = installWindow.SwitchToErrorDialogWindow();
            string actualTitleError = error.GetTitle();
            Assert.AreEqual(actualTitleError, "Login failed");
            Assert.IsFalse(actualIsContinueButtonActive);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "http://IncorrectHost.com", "test", "test")]
        [Category("WelcomeWizard")]
        public void ConnectBitbucketServerIncorrectCredentialsNegativeTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string hostUrl,
            string bitBucketLogin,
            string bitBucketPassword)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketServerIncorrectCredentialsNegativeTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);

            installWindow.ClickContinueButton();

            installWindow.FillAuthenticationBitBucketServer(hostUrl, bitBucketLogin, bitBucketPassword);

            installWindow.ClickContinueButton();

            bool actualIsContinueButtonActive = installWindow.IsContinueButtonActive();
            ErrorDialogWindow error = installWindow.SwitchToErrorDialogWindow();
            string actualTitleError = error.GetTitle();
            Assert.AreEqual(actualTitleError, "Login failed");
            Assert.IsFalse(actualIsContinueButtonActive);

        }

        /// <summary>
        /// Verify that you have permission to access SourceTreeForWindows by atlassian to GitHub account.
        /// </summary>
        [TestCase("testdesktopapplication@20minute.email", "123SourceTree")]
        [Category("WelcomeWizard")]
        [Category("OAuth")]
        public void ConnectGitHubViaOAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubViaOAuthTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubViaOAuthTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectGitHubViaOAuthTest));

            installWindow.ClickContinueButton();

            installWindow.ChooseGitHubAccount();

            installWindow.ClickContinueButton();

            Thread.Sleep(5000); // Wait connection via OAuth

            string actualTitleOfNextStep = installWindow.DownloadingVersionText();
            // This is ensure that authentication was successful, because we are located on next step "Install tools"
            Assert.AreEqual(actualTitleOfNextStep, "Downloading version control systems...");
        }

        /// <summary>
        /// Verify that you have permission to access SourceTreeForWindows by atlassian to BitBucket account.
        /// </summary>
        [TestCase("testdesktopapplication@20minute.email", "123SourceTree")]
        [Category("WelcomeWizard")]
        [Category("OAuth")]
        public void ConnectBitbucketViaOAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketViaOAuthTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketViaOAuthTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(ConnectBitbucketViaOAuthTest));

            installWindow.ClickContinueButton();

            installWindow.ChooseBitBucketAccount();

            installWindow.ClickContinueButton();

            Thread.Sleep(5000); // Wait connection via OAuth

            string actualTitleOfNextStep = installWindow.DownloadingVersionText();
            // This is ensure that authentication was successful, because we are located on next step "Install tools"
            Assert.AreEqual(actualTitleOfNextStep, "Downloading version control systems...");
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "githubfaketesting", "123GitHubFake", "github-public")]
        [Category("WelcomeWizard")]
        public void CloneGitHubRepositoryUsingBasicAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string gitHubLogin,
            string gitHubPassword,
            string nameOfRepo)
        {
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, cloneBasicGitHubTestFolder);
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(pathToNewFolder);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneGitHubRepositoryUsingBasicAuthTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneGitHubRepositoryUsingBasicAuthTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneGitHubRepositoryUsingBasicAuthTest));

            installWindow.ClickContinueButton();

            installWindow.FillBasicAuthenticationGithub(gitHubLogin, gitHubPassword);

            installWindow.ClickContinueButton(); // step remotes

            installWindow.WaitCompleteInstallToolsProgressBar();

            installWindow.ClickContinueButton(); // step install tools

            installWindow.SelectRepositoryByName(nameOfRepo);
            Directory.CreateDirectory(pathToNewFolder);
            installWindow.BrowseDestinationPath(pathToNewFolder);

            installWindow.ClickContinueButton();

            Thread.Sleep(10000); // Time for cloning 
            bool actualIsRepositoryCloned = SourceTree.AutomationTests.Utils.Helpers.Utils.IsFolderGit(pathToNewFolder);

            Assert.IsTrue(actualIsRepositoryCloned);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "bitbucketfaketest", "123BitBucketFake", "bitbucket-public")]
        [Category("WelcomeWizard")]
        public void CloneBitBucketRepositoryUsingBasicAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string bitbucketLogin,
            string bitbucketPassword,
            string nameOfRepo)
        {
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, cloneBasicBitBucketTestFolder);
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(pathToNewFolder);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneBitBucketRepositoryUsingBasicAuthTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneBitBucketRepositoryUsingBasicAuthTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneBitBucketRepositoryUsingBasicAuthTest));

            installWindow.ClickContinueButton();

            installWindow.FillBasicAuthenticationBitbucket(bitbucketLogin, bitbucketPassword);

            installWindow.ClickContinueButton(); // step remotes

            installWindow.WaitCompleteInstallToolsProgressBar();

            installWindow.ClickContinueButton(); // step install tools

            installWindow.SelectRepositoryByName(nameOfRepo);
            Directory.CreateDirectory(pathToNewFolder);

            installWindow.BrowseDestinationPath(pathToNewFolder);

            installWindow.ClickContinueButton();

            Thread.Sleep(2000);
            bool isRepositoryCloned = SourceTree.AutomationTests.Utils.Helpers.Utils.IsFolderGit(pathToNewFolder);

            Assert.IsTrue(isRepositoryCloned);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "githubfaketesting", "123GitHubFake", "github-public")]
        [Category("WelcomeWizard")]
        public void SearchInStartingRepositoryStepTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string gitHubLogin,
            string gitHubPassword,
            string searchCondition)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SearchInStartingRepositoryStepTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SearchInStartingRepositoryStepTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SearchInStartingRepositoryStepTest));

            installWindow.ClickContinueButton();

            installWindow.FillBasicAuthenticationGithub(gitHubLogin, gitHubPassword);

            installWindow.ClickContinueButton(); // step remotes

            installWindow.WaitCompleteInstallToolsProgressBar();

            installWindow.ClickContinueButton(); // step install tools

            installWindow.TypeSearchCondition(searchCondition);

            int actualAmountOfRepos = installWindow.CountOfRepositroyInList();
            string nameOfRepository = installWindow.GetTextOfFirstRepository();

            Assert.AreEqual(actualAmountOfRepos, 1);
            Assert.IsTrue(nameOfRepository.Contains(searchCondition));
        }

        protected override void PerTestPreConfigureSourceTree()
        {
            
        }
    }
}


