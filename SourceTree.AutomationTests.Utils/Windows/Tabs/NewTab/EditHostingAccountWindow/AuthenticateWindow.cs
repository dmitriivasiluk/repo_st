﻿using ScreenObjectsHelpers.Windows;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab.EditHostingAccountWindow
{
    public class AuthenticateWindow : GeneralWindow
    {
        private UIItemContainer authenticationWindow;


        public AuthenticateWindow(Window mainWindow, UIItemContainer authenticationWindow)
            : base(mainWindow)
        {
            this.authenticationWindow = authenticationWindow;
        }

        #region UIItems
        public UIItem PasswordField => authenticationWindow.Get<UIItem>(SearchCriteria.ByAutomationId("Password"));        
        public Button LoginButton => authenticationWindow.Get<Button>(SearchCriteria.ByText("Login"));
        public CheckBox RememberPasswordCheckBox => authenticationWindow.Get<CheckBox>(SearchCriteria.ByText("Remember password"));
        #endregion

        #region Methods
        public EditHostingAccountWindow ClickCancelButton()
        {
            ClickButton(CancelButton);
            return new EditHostingAccountWindow(MainWindow, authenticationWindow);
        }

        public EditHostingAccountWindow ClickLoginButton()
        {
            ClickButton(LoginButton);
            return new EditHostingAccountWindow(MainWindow, authenticationWindow);
        }
        #endregion
    }
}
