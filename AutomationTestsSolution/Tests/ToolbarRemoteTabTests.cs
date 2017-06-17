using System;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using System.Threading;

namespace AutomationTestsSolution.Tests
{
    class ToolbarRemoteTabTests : BasicTest
    {

        [TestCase("staccount", "123456test")]
        [Category("Authentication")]
        [Category("General")]
        public void AuthBitbucketHttpsBasicPositiveTest(string login, string password)
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthBitbucketHttpsBasicPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000); // wait is needed for authentication
            
            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [TestCase("staccount", "incorrectPassword")]
        [Category("Authentication")]
        [Category("General")]
        public void AuthBitbucketHttpsBasicNegativeTest(string login, string password)
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthBitbucketHttpsBasicNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authFailed));
        }

        [TestCase("githubst", "123456test")]
        [Category("Authentication")]
        [Category("General")]
        public void AuthGithubHttpsBasicPositiveTest(string login, string password)
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthGithubHttpsBasicPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.HostingSeviceComboBox, EditHostingAccountWindow.HostingService.GitHub);            
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [TestCase("githubst", "incorrectPassword")]
        [Category("Authentication")]
        [Category("General")]
        public void AuthGithubHttpsBasicNegativeTest(string login, string password)
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthGithubHttpsBasicNegativeTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.HostingSeviceComboBox, EditHostingAccountWindow.HostingService.GitHub);            
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.loginFailed));
        }

        [Test]
        [Category("Authentication")]
        [Category("OAuth")]
        //[Ignore ("Investigate stability issue")]
        public void AuthBitbucketHttpsOauthPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthBitbucketHttpsOauthPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();            

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        [Category("Authentication")]
        [Category("OAuth")]
        //[Ignore("Investigate stability issue")]
        public void AuthGithubHttpsOauthPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthGithubHttpsOauthPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.HostingSeviceComboBox, EditHostingAccountWindow.HostingService.GitHub);            
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();
            
            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        [Category("Authentication")]
        [Category("General")]
        public void AuthOkButtonDisabledTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthOkButtonDisabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();

            Assert.IsFalse(addAccount.OKButton.Enabled);
        }

        [TestCase("RandomUsername")]
        [Category("Authentication")]
        [Category("General")]
        public void AuthRefreshPasswordButtonEnabledTest(string login)
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthRefreshPasswordButtonEnabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);            

            Assert.IsTrue(addAccount.RefreshPasswordButton.Enabled);
        }

        [Test]
        [Category("Authentication")]
        [Category("General")]
        public void AuthRefreshPasswordButtonDisabledTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AuthRefreshPasswordButtonDisabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);

            Assert.IsFalse(addAccount.RefreshPasswordButton.Enabled);
        }
    }
}