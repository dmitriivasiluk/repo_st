﻿using ScreenObjectsHelpers.Windows.Options;
using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows
{
    public class ProxyAuthenticationWindow : GeneralWindow
    {
        private UIItemContainer proxyAuthenticationWindow;
        private UIItemContainer networkTab;

        public ProxyAuthenticationWindow(Window mainWindow, UIItemContainer networkTab, UIItemContainer proxyAuthenticationWindow)
            : base(mainWindow)
        {
            this.proxyAuthenticationWindow = proxyAuthenticationWindow;
            this.networkTab = networkTab;
        }

        public override void ValidateWindow()
        {
            // Need verify opened tab in this method, need implementation! If validation is fail, throw exception!
            Console.WriteLine("WAIT FOR OPENING _Proxy Authentication_ WINDOW");
        }

        #region UIElements
        public TextBox UsernameTextBox => proxyAuthenticationWindow.Get<TextBox>(SearchCriteria.ByAutomationId("Username"));

        public TextBox PasswordTextBox => proxyAuthenticationWindow.Get<TextBox>(SearchCriteria.ByAutomationId("Password"));

        public CheckBox RememberPasswordCheckBox => proxyAuthenticationWindow.Get<CheckBox>(SearchCriteria.ByAutomationId("RememberPassword"));

        public Button LoginButton => proxyAuthenticationWindow.Get<Button>(SearchCriteria.ByAutomationId("LoginButton"));

        public Button CancelButton => proxyAuthenticationWindow.Get<Button>(SearchCriteria.ByAutomationId("CancelButton"));
        #endregion

        #region Methods
        public NetworkTab LoginClick()
        {
            LoginButton.Click();
            return new NetworkTab(MainWindow, networkTab);
        }

        public NetworkTab CancelClick()
        {
            CancelButton.Click();
            return new NetworkTab(MainWindow, networkTab);
        }
        #endregion
    }
}