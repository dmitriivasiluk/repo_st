using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows;
using ScreenObjectsHelpers.Windows.MenuFolder;
using ScreenObjectsHelpers.Windows.ToolbarTabs;

namespace AutomationTestsSolution.Tests
{
    class HelpMenuTests : BasicTest
    {
        [Test]
        [Category("HelpMenu")]
        public void AboutWindowTest()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            AboutWindow aboutWindow = mainWindow.OpenMenu<HelpMenu>().OpenAbout();

            string aboutWindowHeader = aboutWindow.GetHeader();
            string copyrightCaption = aboutWindow.GetCopyrightCaption();
            string appVersion = aboutWindow.GetAppVersion();
            Assert.AreEqual(aboutWindowHeader, ConstantsList.aboutWindowHeader);
            Assert.AreEqual(copyrightCaption, ConstantsList.copyrightCaption);
            Assert.That(appVersion, Does.Contain(GetSourceTreeVersion()));
        }
    }
}