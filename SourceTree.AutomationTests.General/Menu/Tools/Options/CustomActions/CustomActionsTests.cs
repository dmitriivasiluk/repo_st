using System.IO;
using System.Threading;
using NUnit.Framework;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Tests;
using SourceTree.AutomationTests.Utils.Windows.Menu;
using SourceTree.AutomationTests.Utils.Windows.Menu.Tools.Options;
using SourceTree.AutomationTests.Utils.Windows.MenuFolder;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;

namespace SourceTree.AutomationTests.General.Menu.Tools.Options.CustomActions
{
    class CustomActionsTests : BasicTest
    {
        [Test]
        [Category("CustomActions")]
        [Category("General")]
        public void AddCustomAction()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(AddCustomAction));
            LocalTab mainWindow = new LocalTab(MainWindow);
            OptionsWindow optionsWindows = mainWindow.OpenMenu<ToolsMenu>().OpenOptions();
            CustomActionsTab customActionsTab = optionsWindows.OpenTab<CustomActionsTab>();

            Thread.Sleep(3000);

            var editCustomActionWindow = customActionsTab.ClickAddCustomActionButton();

            editCustomActionWindow.SetTextboxContent(editCustomActionWindow.MenuCaption, ConstantsList.addCustomActionName);
            editCustomActionWindow.SetTextboxContent(editCustomActionWindow.ScriptToRun, ConstantsList.addCustomActionName);
            editCustomActionWindow.ClickOKButton();

            bool isCustomActionAdded = customActionsTab.IsMenuCaptionExists(ConstantsList.addCustomActionName);

            Assert.IsTrue(isCustomActionAdded);
        }

        [Test]
        [Category("CustomActions")]
        [Category("General")]
        public void EditCustomAction()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(EditCustomAction));
            LocalTab mainWindow = new LocalTab(MainWindow);
            OptionsWindow optionsWindows = mainWindow.OpenMenu<ToolsMenu>().OpenOptions();
            CustomActionsTab customActionsTab = optionsWindows.OpenTab<CustomActionsTab>();

            Thread.Sleep(3000);

            var editCustomActionWindow = customActionsTab.ClickEditCustomActionButton();

            editCustomActionWindow.SetTextboxContent(editCustomActionWindow.MenuCaption, ConstantsList.editedCustomActionName);
            editCustomActionWindow.ClickOKButton();

            bool isCustomActionEdited = customActionsTab.IsMenuCaptionExists(ConstantsList.editedCustomActionName);

            Assert.IsTrue(isCustomActionEdited);
        }

        [Test]
        [Category("CustomActions")]
        [Category("General")]
        public void DeleteCustomAction()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeScreenShotsPath, nameof(DeleteCustomAction));
            LocalTab mainWindow = new LocalTab(MainWindow);
            OptionsWindow optionsWindows = mainWindow.OpenMenu<ToolsMenu>().OpenOptions();
            CustomActionsTab customActionsTab = optionsWindows.OpenTab<CustomActionsTab>();

            Thread.Sleep(3000);

            customActionsTab.ClickDeleteCustomActionButton();

            var confirmDeletionWindow = customActionsTab.SwitchToConfirmDeletionDialogWindow();

            confirmDeletionWindow.ClickOkButton();

            bool isCustomActionDeleted = customActionsTab.IsMenuCaptionExists(ConstantsList.customActionToBeDeleted);

            Assert.IsFalse(isCustomActionDeleted);
        }

        protected override void PerTestPreConfigureSourceTree()
        {
            // TODO replace resource with writing file on the fly
            var resourceName = Resources.customactions;
            var customActionsFilePath = Path.Combine(SourceTreeUserDataPath, "customactions.xml");
            File.WriteAllText(customActionsFilePath, resourceName);
        }
    }
}