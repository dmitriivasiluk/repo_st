using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows
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
        public Button CancelButton => authenticationWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
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
