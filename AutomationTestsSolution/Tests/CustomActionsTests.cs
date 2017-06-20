﻿using NUnit.Framework;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Options;
using ScreenObjectsHelpers.Windows.MenuFolder;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using System.IO;
using System;
using System.Threading;

namespace AutomationTestsSolution.Tests
{
    class CustomActionsTests : BasicTest
    {
        [Test]
        [Category("CustomActions")]
        [Category("General")]
        public void AddCustomAction()
        {
            ScreenshotsTaker.TakeScreenShot(SourceTreeInstallArtifactsPath, nameof(AddCustomAction));
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
            ScreenshotsTaker.TakeScreenShot(SourceTreeInstallArtifactsPath, nameof(EditCustomAction));
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
            ScreenshotsTaker.TakeScreenShot(SourceTreeInstallArtifactsPath, nameof(DeleteCustomAction));
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
            var resourceName = Resources.customactions;
            var customActionsFilePath = Path.Combine(SourceTreeUserDataPath, "customactions.xml");
            File.WriteAllText(customActionsFilePath, resourceName);
        }
    }
}