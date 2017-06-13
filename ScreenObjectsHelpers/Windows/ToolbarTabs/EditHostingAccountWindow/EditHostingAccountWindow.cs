using System.Windows.Automation;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;
using System.Threading;

namespace ScreenObjectsHelpers.Windows
{
    public class EditHostingAccountWindow : GeneralWindow
    {
        private UIItemContainer editHostingAccountWindow;

        public EditHostingAccountWindow(Window mainWindow, UIItemContainer editHostingAccountWindow)
            : base(mainWindow)
        {
            this.editHostingAccountWindow = editHostingAccountWindow;
        }

        #region UIItems
        public ComboBox HostingSeviceComboBox => editHostingAccountWindow.Get<ComboBox>(SearchCriteria.ByControlType(ControlType.ComboBox).AndIndex(0));
        public ComboBox PreferredProtocolComboBox => editHostingAccountWindow.Get<ComboBox>(SearchCriteria.ByControlType(ControlType.ComboBox).AndIndex(1));
        public ComboBox AuthenticationComboBox => editHostingAccountWindow.Get<ComboBox>(SearchCriteria.ByControlType(ControlType.ComboBox).AndIndex(2));
        public TextBox HostUrlTextBox => editHostingAccountWindow.Get<TextBox>(SearchCriteria.Indexed(0));
        public TextBox UsernameTextBox => editHostingAccountWindow.Get<TextBox>(SearchCriteria.Indexed(1));
        public Button RefreshTokenButton => editHostingAccountWindow.Get<Button>(SearchCriteria.ByText("Refresh OAuth Token"));
        public Button RefreshPasswordButton => editHostingAccountWindow.Get<Button>(SearchCriteria.ByText("Refresh Password"));
        #endregion

        #region ComboBox Variables
        public struct HostingService
        {
            public static string GitHub = "GitHub";
            public static string Bitbucket = "Bitbucket";
            public static string BitbucketServer = "Bitbucket Server";
        }

        public struct Protocol
        {
            public static string HTTPS = "HTTPS";
            public static string SSH = "SSH";
        }

        public struct Authentication
        {
            public static string Basic = "Basic";
            public static string OAuth = "OAuth";
        }
        #endregion

        #region Methods
        public AuthenticateWindow ClickRefreshPasswordButton()
        {
            Thread.Sleep(1000);

            if (RefreshPasswordButton.Enabled)
            {
                ClickButton(RefreshPasswordButton);
                return new AuthenticateWindow(MainWindow, editHostingAccountWindow);
            }

            return null;
        }

        public void ClickRefreshTokenButton()
        {
            Thread.Sleep(1000);

            if (RefreshTokenButton.Enabled)
            {
                ClickButton(RefreshTokenButton);
                Thread.Sleep(4000);
            }
        }

        public RemoteTab ClickCancelButton()
        {
            ClickButton(CancelButton);
            return new RemoteTab(MainWindow);
        }

        public RemoteTab ClickOkButton()
        {
            ClickButton(OKButton);
            return new RemoteTab(MainWindow);
        }

        public bool IsValidationMessageDisplayed(string text)
        {
            if (editHostingAccountWindow.Get<WPFLabel>(SearchCriteria.ByText(text)).Visible)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Auth Validation Text variables
        public string authFailed = "Authentication failed";
        public string authOk = "Authentication OK";
        public string loginFailed = "Login failed";
        #endregion
    }
}
