using System.IO;
using System.Threading;
using NUnit.Framework;
using ScreenObjectsHelpers.Windows;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Windows;
using SourceTree.AutomationTests.Utils.Windows.Menu.Repository;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;

namespace SourceTree.AutomationTests.Complex.WelcomeWizard
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
    class WelcomeWizardTests : General.WelcomeWizard.WelcomeWizardTests
    {
        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "http://HostURL.com", "username", "password")]
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

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "SourceTree")]
        [Category("WelcomeWizard")]
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

        /// <summary>
        /// Verify that you have permission to access SourceTreeForWindows by atlassian to GitHub account.
        /// </summary>
        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "github-public")]
        [Category("WelcomeWizard")]
        [Category("OAuth")]
        public void CloneGitHubRepositoryUsingOAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string nameOfRepo)
        {
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, cloneOAuthGitHubTestFolder);
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(pathToNewFolder);

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
            bool isRepositoryCloned = SourceTree.AutomationTests.Utils.Helpers.Utils.IsFolderGit(pathToNewFolder);

            Assert.IsTrue(isRepositoryCloned);
        }

        /// <summary>
        /// Verify that you have permission to access SourceTreeForWindows by atlassian to BitBucket account.
        /// </summary>
        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "bitbucket-public")]
        [Category("WelcomeWizard")]
        [Category("OAuth")]
        public void CloneBitBucketRepositoryUsingOAuthTest(
            string atlassianLoginEmail,
            string atlassianPassword,
            string nameOfRepo)
        {
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, cloneOAuthBitBucketTestFolder);
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(pathToNewFolder);

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
            bool isRepositoryCloned = SourceTree.AutomationTests.Utils.Helpers.Utils.IsFolderGit(pathToNewFolder);

            Assert.IsTrue(isRepositoryCloned);
        }

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "https://Server.com.ua", "incorrectLogin", "incorrectPassword", "bitbucket-public")]
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
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(pathToNewFolder);

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

        [TestCase("testdesktopapplication@20minute.email", "123SourceTree", "bitbucketfaketest", "123BitBucketFake", "bitbucket-public")]
        [Category("WelcomeWizard")]
        public void SourceTreeOpensAfterFinishConfiguration(
            string atlassianLoginEmail,
            string atlassianPassword,
            string bitbucketbLogin,
            string bitbucketPassword,
            string nameOfRepo)
        {
            // Pre-condition
            string pathToNewFolder = Path.Combine(SourceTreeTestDataPath, openSourceTreeTestFolder);
            SourceTree.AutomationTests.Utils.Helpers.Utils.RemoveDirectory(pathToNewFolder);

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
    }
}


