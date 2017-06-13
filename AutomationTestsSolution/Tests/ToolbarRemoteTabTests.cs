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
        public void AuthBitbucketHttpsBasicPositiveTest(string login, string password)
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            Thread.Sleep(1000); // wait is needed because of the issue 1090, reported earlier
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000); // wait is needed for authentication
            
            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [TestCase("staccount", "incorrectPassword")]
        [Category("Authentication")]
        public void AuthBitbucketHttpsBasicNegativeTest(string login, string password)
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            Thread.Sleep(1000);
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authFailed));
        }

        [TestCase("githubst", "123456test")]
        [Category("Authentication")]
        public void AuthGithubHttpsBasicPositiveTest(string login, string password)
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.GitHub);
            Thread.Sleep(1000); // wait is needed for combobox selecting 
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            Thread.Sleep(1000);
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [TestCase("githubst", "incorrectPassword")]
        [Category("Authentication")]
        public void AuthGithubHttpsBasicNegativeTest(string login, string password)
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.GitHub);
            Thread.Sleep(1000);
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            Thread.Sleep(1000);
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue(password);
            addAccount = auth.ClickLoginButton();
            Thread.Sleep(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.loginFailed));
        }

        [Test]
        [Category("Authentication")]
        [Ignore ("Investigate stability issue")]
        public void AuthBitbucketHttpsOauthPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();
            Thread.Sleep(3000); // wait needed for OAuth in browser

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        [Category("Authentication")]
        [Ignore("Investigate stability issue")]
        public void AuthGithubHttpsOauthPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.GitHub);
            Thread.Sleep(1000);
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();
            Thread.Sleep(3000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        [Category("Authentication")]
        public void AuthOkButtonDisabledTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();

            Assert.IsFalse(addAccount.OKButton.Enabled);
        }

        [TestCase("RandomUsername")]
        [Category("Authentication")]
        public void AuthRefreshPasswordButtonEnabledTest(string login)
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);
            Thread.Sleep(1000);

            Assert.IsTrue(addAccount.RefreshPasswordButton.Enabled);
        }

        [Test]
        [Category("Authentication")]
        public void AuthRefreshPasswordButtonDisabledTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);

            Assert.IsFalse(addAccount.RefreshPasswordButton.Enabled);
        }
    }
}