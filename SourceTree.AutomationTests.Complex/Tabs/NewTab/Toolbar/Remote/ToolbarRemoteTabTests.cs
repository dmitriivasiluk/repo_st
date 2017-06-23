using AutomationTestsSolution.Tests;
using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows;
using ScreenObjectsHelpers.Windows.ToolbarTabs;

namespace SourceTree.AutomationTests.Complex.Tabs.NewTab.Toolbar.Remote
{
    class ToolbarRemoteTabTests : AutomationTestsSolution.Tabs.NewTab.Toolbar.Remote.ToolbarRemoteTabTests
    {
        [Test]
        [Category("Authentication")]
        [Category("OAuth")]
        public void AuthGithubHttpsOauthPositiveTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthGithubHttpsOauthPositiveTest));
            LocalTab mainWindow = new LocalTab(MainWindow);            
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.HostingSeviceComboBox, EditHostingAccountWindow.HostingService.GitHub);            
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.OAuth);
            addAccount.ClickRefreshTokenButton();
            
            Assert.IsTrue(addAccount.IsValidationMessageDisplayed(addAccount.authOk));
        }

        [TestCase("RandomUsername")]
        [Category("Authentication")]
        [Category("General")]
        public void AuthRefreshPasswordButtonEnabledTest(string login)
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AuthRefreshPasswordButtonEnabledTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            RemoteTab remoteTab = mainWindow.OpenTab<RemoteTab>();

            var addAccount = remoteTab.ClickAddAccountButton();
            addAccount.SetComboboxValue(addAccount.AuthenticationComboBox, EditHostingAccountWindow.Authentication.Basic);
            addAccount.SetTextboxContent(addAccount.UsernameTextBox, login);            

            Assert.IsTrue(addAccount.RefreshPasswordButton.Enabled);
        }
    }
}