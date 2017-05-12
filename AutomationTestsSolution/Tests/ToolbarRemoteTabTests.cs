using System;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows;
using ScreenObjectsHelpers.Windows.ToolbarTabs;

namespace AutomationTestsSolution.Tests
{
    class ToolbarRemoteTabTests : BasicTest
    {
        [SetUp]
        public override void SetUp()
        {
            AttachToSourceTree();
        }

        [TearDown]
        public override void TearDown()
        {

        }

        [Test]
        public void AuthBitbucketHttpsBasicPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.UsernameTextBox.SetValue("staccount");
            Utils.ThreadWait(1000);
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue("123456test");
            addAccount = auth.ClickLoginButton();
            Utils.ThreadWait(2000);
            
            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        public void AuthBitbucketHttpsBasicNegativeTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.UsernameTextBox.SetValue("staccount");
            Utils.ThreadWait(1000);
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue("incorrectPassword");
            addAccount = auth.ClickLoginButton();
            Utils.ThreadWait(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authFailed));
        }

        [Test]
        public void AuthGithubHttpsBasicPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.GitHub);
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.UsernameTextBox.SetValue("githubst");
            Utils.ThreadWait(1000);
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue("123456test");
            addAccount = auth.ClickLoginButton();
            Utils.ThreadWait(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        public void AuthGithubHttpsBasicNegativeTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.GitHub);
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.UsernameTextBox.SetValue("githubst");
            Utils.ThreadWait(1000);
            var auth = addAccount.ClickRefreshPasswordButton();
            auth.PasswordField.SetValue("incorrectPassword");
            addAccount = auth.ClickLoginButton();
            Utils.ThreadWait(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.loginFailed));
        }

        [Test]
        public void AuthBitbucketHttpsOauthPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();
            Utils.ThreadWait(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        public void AuthGithubHttpsOauthPositiveTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.GitHub);
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();
            Utils.ThreadWait(2000);

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        public void AuthOkButtonDisabledTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();

            Assert.IsFalse(addAccount.OkButton.Enabled);
        }

        [Test]
        public void AuthRefreshPasswordButtonEnabledTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);
            addAccount.UsernameTextBox.SetValue("RandomUsername");
            Utils.ThreadWait(1000);

            Assert.IsTrue(addAccount.RefreshPasswordButton.Enabled);
        }

        [Test]
        public void AuthRefreshPasswordButtonDisabledTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.Basic);

            Assert.IsFalse(addAccount.RefreshPasswordButton.Enabled);
        }

        [Test]
        public void AddAccountGithubHttpsOauthTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.GitHub);
            addAccount.AuthenticationComboBox.Select(EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();
            Utils.ThreadWait(2000);
            remoteTab = addAccount.ClickOkButton();

            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [Test]
        public void AllTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();
            
            //Console.WriteLine();

            //addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.BitbucketServer);
            //addAccount.PreferredProtocolComboBox.Select(EditHostingAccountWindow.Protocol.HTTPS);

            //addAccount.HostUrlTextBox.SetValue("dfgggd");

            //remoteTab = addAccount.ClickCancelButton();
            //remoteTab.EditAccountsButton.Click();
        }

        //[Test]
        //public void AuthTest()
        //{
        //    LocalTab mainWindow = new LocalTab(MainWindow);
        //    RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

        //    var addAccount = remoteTab.ClickAddAccountButton();

        //    //addAccount.HostingSeviceComboBox.Select(EditHostingAccountWindow.HostingService.BitbucketServer);
        //    //addAccount.PreferredProtocolComboBox.Select(EditHostingAccountWindow.Protocol.HTTPS);

        //    //addAccount.HostUrlTextBox.SetValue("dfgggd");

        //    //remoteTab = addAccount.ClickCancelButton();
        //    //remoteTab.EditAccountsButton.Click();
        //}
    }
}