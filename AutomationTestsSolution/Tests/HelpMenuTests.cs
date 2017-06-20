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
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void AboutWindowTest()
        {
            ScreenshotsTaker.TakeScreenShot(nameof(AboutWindowTest));
            LocalTab mainWindow = new LocalTab(MainWindow);
            AboutWindow aboutWindow = mainWindow.OpenMenu<HelpMenu>().OpenAbout();

            string aboutWindowHeader = aboutWindow.GetHeader();
            string copyrightCaption = aboutWindow.GetCopyrightCaption();
            Assert.AreEqual(aboutWindowHeader, ConstantsList.aboutWindowHeader);
            Assert.AreEqual(copyrightCaption, ConstantsList.copyrightCaption);
            Assert.That(aboutWindow.HasAppVersion(SourceTreeVersion), Is.True);
        }

        protected override void PerTestPreConfigureSourceTree()
        {
            // nothing todo
        }
    }
}