using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Options;
using ScreenObjectsHelpers.Windows.MenuFolder;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using System.IO;
using System;

namespace AutomationTestsSolution.Tests
{
    class CustomActionsTests : BasicTest
    {
        [Test]
        [Category("CustomActions")]
        public void AddCustomAction()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            OptionsWindow optionsWindows = mainWindow.OpenMenu<ToolsMenu>().OpenOptions();
            CustomActionsTab customActionsTab = optionsWindows.OpenTab<CustomActionsTab>();

            var isMenuCaptionExistsBeforeTest = customActionsTab.IsMenuCaptionExists(ConstantsList.addCustomActionName);
            var editCustomActionWindow = customActionsTab.ClickAddCustomActionButton();

            editCustomActionWindow.SetMenuCaption(ConstantsList.addCustomActionName);
            editCustomActionWindow.SetScriptToRun(ConstantsList.addCustomActionName);
            editCustomActionWindow.ClickOKButton();

            Assert.IsFalse(isMenuCaptionExistsBeforeTest);
            Assert.IsTrue(customActionsTab.IsMenuCaptionExists(ConstantsList.addCustomActionName));
        }

        [Test]
        [Category("CustomActions")]
        public void DeleteCustomAction()
        {
            LocalTab mainWindow = new LocalTab(MainWindow);
            OptionsWindow optionsWindows = mainWindow.OpenMenu<ToolsMenu>().OpenOptions();
            CustomActionsTab customActionsTab = optionsWindows.OpenTab<CustomActionsTab>();

            var isDeleteCustomActionButtonEnabled = customActionsTab.IsDeleteCustomActionButtonEnabled();
            var isMenuCaptionExistsBeforeTest = customActionsTab.IsMenuCaptionExists(ConstantsList.customActionToBeDeleted);

            customActionsTab.ClickDeleteCustomActionButton();

            var confirmDeletionWindow = customActionsTab.SwitchToConfirmDeletionDialogWindow();

            confirmDeletionWindow.ClickOkButton();

            var isMenuCaptionExistsAfterTest = customActionsTab.IsMenuCaptionExists(ConstantsList.customActionToBeDeleted);

            Assert.IsFalse(isDeleteCustomActionButtonEnabled);
            Assert.IsTrue(isMenuCaptionExistsBeforeTest);
            Assert.IsFalse(isMenuCaptionExistsAfterTest);
        }

        protected override void PerTestPreConfigureSourceTree()
        {
            var resourceName = Resources.customactions;
            var customActionsFilePath = Path.Combine(SourceTreeUserDataPath, "customactions.xml");
            File.WriteAllText(customActionsFilePath, resourceName);
        }
    }
}