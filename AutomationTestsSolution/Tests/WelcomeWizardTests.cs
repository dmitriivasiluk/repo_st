using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using ScreenObjectsHelpers.Windows.Repository;

namespace AutomationTestsSolution.Tests
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
    class WelcomeWizardTests : AbstractWelcomeWizardTest
    {
        #region Test Variables
        private string openSourceTreeTestFolder = "OpenSourceTree";
        private string cloneOAuthGitHubTestFolder = "CloneOAuthGitHub";
        private string cloneOAuthBitBucketTestFolder = "CloneOAuthBitBucket";
        private string cloneBasicBitBucketTestFolder = "CloneBasicBitBucket";
        private string cloneBasicGitHubTestFolder = "CloneBasicGitHub";
        private string cloneBasicBitBucketServerTestFolder = "CloneBasicBitBucketServer";
        #endregion

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();

            RemoveTestFolders();
        }

        private void RemoveTestFolders()
        {
            Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, openSourceTreeTestFolder));
            Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneOAuthGitHubTestFolder));
            Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneOAuthBitBucketTestFolder));
            Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneBasicBitBucketTestFolder));
            Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneBasicGitHubTestFolder));
            Utils.RemoveDirectory(Path.Combine(SourceTreeTestDataPath, cloneBasicBitBucketServerTestFolder));
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
        //[Ignore("Investigate stability issue")]
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

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "http://HostURL.com", "username", "password")]
        [Ignore("BitBucket server link needed")]
        [Category("WelcomeWizard")]
        public void ConnectBitbucketServerAccountTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string hostUrl,
            string bitBucketLogin,
            string bitbucketPassword)
        {
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);

            installWindow.ClickContinueButton();

            // Don't have any HostURL to BitbucketServer for check this functionality.
            installWindow.FillAuthenticationBitBucketServer(hostUrl, bitBucketLogin, bitbucketPassword);

            //string actualTitleOfNextStep = installWindow.DownloadingVersionText();
            // This is ensure that authentication was successful, because we are located on next step "Install tools"
            //Assert.AreEqual(actualTitleOfNextStep, "Downloading version control systems...");
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
        //[Ignore("Investigate stability issue")]

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

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "SourceTree")]
        [Category("WelcomeWizard")]
        [Ignore("Fix needed")]
        public void SkipSetupButtonClosesConfigurationTest(
               string atlassianLoginEmail,
               string atlassianPassword,
               string expectedTitle)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SkipSetupButtonClosesConfigurationTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SkipSetupButtonClosesConfigurationTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SkipSetupButtonClosesConfigurationTest));

            installWindow.ClickContinueButton();

            LocalTab mainWindow = installWindow.SkipSetup();

            string actualTitle = mainWindow.GetTitle();

            Assert.AreEqual(actualTitle, expectedTitle);
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
            Utils.RemoveDirectory(pathToNewFolder);

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
            bool actualIsRepositoryCloned = Utils.IsFolderGit(pathToNewFolder);

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
            Utils.RemoveDirectory(pathToNewFolder);

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
            bool isRepositoryCloned = Utils.IsFolderGit(pathToNewFolder);

            Assert.IsTrue(isRepositoryCloned);
        }

        /// <summary>
        /// Verify that you have permission to access SourceTreeForWindows by atlassian to GitHub account.
        /// </summary>
        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "github-public")]
        [Category("WelcomeWizard")]
        [Ignore("Odd behaviour when there are existing sessions")]
        [Category("OAuth")]
        public void CloneGitHubRepositoryUsingOAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string nameOfRepo)
        {
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, cloneOAuthGitHubTestFolder);
            Utils.RemoveDirectory(pathToNewFolder);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneGitHubRepositoryUsingOAuthTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneGitHubRepositoryUsingOAuthTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneGitHubRepositoryUsingOAuthTest));

            installWindow.ClickContinueButton();

            installWindow.ChooseGitHubAccount();

            installWindow.ClickContinueButton(); // step remotes

            installWindow.WaitCompleteInstallToolsProgressBar();

            installWindow.ClickContinueButton(); // step install tools

            installWindow.SelectRepositoryByName(nameOfRepo);
            Directory.CreateDirectory(pathToNewFolder);
            installWindow.BrowseDestinationPath(pathToNewFolder);

            installWindow.ClickContinueButton();

            Thread.Sleep(2000);
            bool isRepositoryCloned = Utils.IsFolderGit(pathToNewFolder);

            Assert.IsTrue(isRepositoryCloned);
        }

        /// <summary>
        /// Verify that you have permission to access SourceTreeForWindows by atlassian to BitBucket account.
        /// </summary>
        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "bitbucket-public")]
        [Category("WelcomeWizard")]
        [Ignore("Odd behaviour when there are existing sessions")]
        [Category("OAuth")]
        public void CloneBitBucketRepositoryUsingOAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string nameOfRepo)
        {
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, cloneOAuthBitBucketTestFolder);
            Utils.RemoveDirectory(pathToNewFolder);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneBitBucketRepositoryUsingOAuthTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneBitBucketRepositoryUsingOAuthTest));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneBitBucketRepositoryUsingOAuthTest));

            installWindow.ClickContinueButton();

            installWindow.ChooseBitBucketAccount();

            installWindow.ClickContinueButton(); // step remotes

            installWindow.WaitCompleteInstallToolsProgressBar();

            installWindow.ClickContinueButton(); // step install tools

            installWindow.SelectRepositoryByName(nameOfRepo);
            Directory.CreateDirectory(pathToNewFolder);
            installWindow.BrowseDestinationPath(pathToNewFolder);

            installWindow.ClickContinueButton();

            Thread.Sleep(2000);
            bool isRepositoryCloned = Utils.IsFolderGit(pathToNewFolder);

            Assert.IsTrue(isRepositoryCloned);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "https://Server.com.ua", "incorrectLogin", "incorrectPassword", "bitbucket-public")]
        [Ignore("BitBucket server link needed")]
        [Category("WelcomeWizard")]
        public void CloneBitBucketServerRepositoryUsingBasicAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string urlServer,
            string bitBucketLogin,
            string bitBucketPassword,
            string nameOfRepo)
        {
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, cloneBasicBitBucketServerTestFolder);
            Utils.RemoveDirectory(pathToNewFolder);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(CloneBitBucketServerRepositoryUsingBasicAuthTest));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);

            installWindow.ClickContinueButton();

            installWindow.FillAuthenticationBitBucketServer(urlServer, bitBucketLogin, bitBucketPassword);

            // WE need any BitBucket server to pass this test!

            //installWindow.ClickContinueButton(); // step remotes

            //installWindow.WaitCompleteInstallToolsProgressBar();

            //installWindow.ClickContinueButton(); // step install tools

            //installWindow.SelectRepositoryByName(nameOfRepo);
            //Directory.CreateDirectory(pathToNewFolder);
            //installWindow.BrowseDestinationPath(pathToNewFolder);

            //installWindow.ClickContinueButton();

            //Thread.Sleep(2000);
            //bool isRepositoryCloned = WindowsFilesHelper.IsGitRepositoryByPath(pathToNewFolder);

            //Assert.IsTrue(isRepositoryCloned);
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

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "bitbucketfaketest", "123BitBucketFake", "bitbucket-public")]
        [Category("WelcomeWizard")]
        [Ignore("Fix needed")]
        public void SourceTreeOpensAfterFinishConfiguration(
            string atlassianLoginEmail,
            string atlassianPassword,
            string bitbucketbLogin,
            string bitbucketPassword,
            string nameOfRepo)
        {
            // Pre-condition
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, openSourceTreeTestFolder);
            Utils.RemoveDirectory(pathToNewFolder);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));
            InstallationWindow installWindow = new InstallationWindow(MainWindow);
            installWindow.CheckLicenceAgreementCheckbox();
            installWindow.ClickContinueButton();
            AuthenticationWindow authentication = installWindow.ClickUseExistingAccount();
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));

            installWindow = authentication.SignIn(atlassianLoginEmail, atlassianPassword);
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));

            installWindow.ClickContinueButton();

            installWindow.FillBasicAuthenticationBitbucket(bitbucketbLogin, bitbucketPassword);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));
            installWindow.ClickContinueButton();

            installWindow.WaitCompleteInstallToolsProgressBar();

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));
            installWindow.ClickContinueButton();

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));
            installWindow.SelectRepositoryByName(nameOfRepo);

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));
            Directory.CreateDirectory(pathToNewFolder);
            installWindow.BrowseDestinationPath(pathToNewFolder);

            installWindow.ClickContinueButton();

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));
            RepositoryTab mainWindow = installWindow.ClickContinueAtTheLatestStepButton();

            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(SourceTreeOpensAfterFinishConfiguration));
            string actualTitle = mainWindow.GetTitle();

            Assert.AreEqual(actualTitle, "SourceTree");

        }
        protected override void PerTestPreConfigureSourceTree()
        {
            
        }
    }
}


