﻿using System;
using System.Windows.Automation;
using ScreenObjectsHelpers.Windows;
using SourceTree.AutomationTests.Utils.Helpers;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows
{
    public class AboutWindow : GeneralWindow
    {
        public AboutWindow(Window mainWindow, UIItemContainer aboutWindow) : base(mainWindow)
        {
            AboutWindowContainer = aboutWindow;
        }

        public UIItemContainer AboutWindowContainer { get; }

        #region UIItems

        public TextBox HeaderOfAboutWindow
        {
            get
            {
                var controlElement = AboutWindowContainer.GetElement(SearchCriteria.ByText("About SourceTree").AndControlType(ControlType.Text));
                return controlElement != null ? new TextBox(controlElement, AboutWindowContainer.ActionListener) : null;
            }
        }
        public TextBox AppVersion 
        {
            get
            {
                var controlElement = AboutWindowContainer.GetElement(SearchCriteria.ByText(ConstantsList.appVersion).AndControlType(ControlType.Text));
                return controlElement != null ? new TextBox(controlElement, AboutWindowContainer.ActionListener) : null;
            }
        }
        public TextBox CopyrightCaption
        {
            get
            {
                var controlElement = AboutWindowContainer.GetElement(SearchCriteria.ByText("Copyright Atlassian 2012-2017. All Rights Reserved.").AndControlType(ControlType.Text));
                return controlElement != null ? new TextBox(controlElement, AboutWindowContainer.ActionListener) : null;
            }
        }
        public Button CloseAboutWindowButton => AboutWindowContainer.Get<Button>(SearchCriteria.ByAutomationId("CloseWindow"));
        #endregion
        
        #region Methods

        public string GetHeader()
        {
            return HeaderOfAboutWindow.Name;
        }
        public bool HasAppVersion(Version version)
        {
            var controlElement = AboutWindowContainer.GetElement(SearchCriteria.ByText($"Version {version}").AndControlType(ControlType.Text));
            return controlElement != null;
        }
        public string GetCopyrightCaption()
        {
            return CopyrightCaption.Name;
        }
        public LocalTab CloseAboutWindowButtonClick()
        {
            CloseAboutWindowButton.Click();
            return new LocalTab(MainWindow);
        }
        #endregion
    }
}