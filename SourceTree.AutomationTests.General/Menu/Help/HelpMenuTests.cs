using NUnit.Framework;
using ScreenObjectsHelpers.Windows;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows;
using SourceTree.AutomationTests.Utils.Windows.Menu;
using SourceTree.AutomationTests.Utils.Windows.MenuFolder;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;

namespace SourceTree.AutomationTests.General.Menu.Help
{
    class HelpMenuTests : BasicTest
    {
        [Test]
        [Category("HelpMenu")]
        [Category("General")]
        [Category("StartWithNewTabOpened")]
        public void AboutWindowTest()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AboutWindowTest));
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