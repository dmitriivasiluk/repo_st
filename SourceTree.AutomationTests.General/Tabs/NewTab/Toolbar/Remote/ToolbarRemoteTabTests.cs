﻿using System.Threading;
using NUnit.Framework;
using ScreenObjectsHelpers.Windows;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab.EditHostingAccountWindow;

namespace SourceTree.AutomationTests.General.Tabs.NewTab.Toolbar.Remote
{
    public class ToolbarRemoteTabTests : BasicTest
    {

        [TestCase("staccount", "123456test")]
        [Category("Authentication")]
        [Category("General")]
        public void AuthBitbucketHttpsBasicPositiveTest(string login, string password)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthBitbucketHttpsBasicPositiveTest));
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
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthBitbucketHttpsBasicNegativeTest));
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
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthGithubHttpsBasicPositiveTest));
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
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthGithubHttpsBasicNegativeTest));
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
        public void AuthBitbucketHttpsOauthPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthBitbucketHttpsOauthPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();            

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        [Category("Authentication")]
        [Category("General")]
        public void AuthOkButtonDisabledTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthOkButtonDisabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();

            Assert.IsFalse(addAccount.OKButton.Enabled);
        }

        [Test]
        [Category("Authentication")]
        [Category("General")]
        public void AuthRefreshPasswordButtonDisabledTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthRefreshPasswordButtonDisabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);

            Assert.IsFalse(addAccount.RefreshPasswordButton.Enabled);
        }
    }
}