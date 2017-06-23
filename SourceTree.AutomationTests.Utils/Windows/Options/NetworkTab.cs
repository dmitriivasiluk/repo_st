using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Options
{
    public class NetworkTab : OptionsWindow
    {
        private readonly UIItemContainer networkTab;
        public NetworkTab(Window mainWindow, UIItemContainer optionsWindow) : base(mainWindow, optionsWindow)
        {
            networkTab = optionsWindow;
        }

        #region UIElements        
        public override UIItem UIElementTab => OptionsWindowContainer.Get<UIItem>(SearchCriteria.ByText("Network"));

        public RadioButton UseDefaultProxyRadioButton => networkTab.Get<RadioButton>(SearchCriteria.ByText("Use default operating system settings"));            

        public RadioButton UseCustomProxyRadioButton => networkTab.Get<RadioButton>(SearchCriteria.ByText("Use custom proxy settings"));

        // AutomationID_required
        /*
        public TextBox ServerTextBox => networkTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));            

        public TextBox PortTextBox => networkTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));            
        */

        public CheckBox ProxyRequiresPasswordCheckBox => networkTab.Get<CheckBox>(SearchCriteria.ByText("Proxy server requires username and password"));            

        public CheckBox AddProxyConfigurationCheckBox => networkTab.Get<CheckBox>(SearchCriteria.ByText("Add proxy server configuration to Git / Mercurial"));            

        public CheckBox EnableSslCheckBox => networkTab.Get<CheckBox>(SearchCriteria.ByText("Enable SSL3"));            

        public CheckBox EnableTls11CheckBox => networkTab.Get<CheckBox>(SearchCriteria.ByText("Enable TLS 1.1"));            

        public CheckBox EnableTls12CheckBox => networkTab.Get<CheckBox>(SearchCriteria.ByText("Enable TLS 1.2"));            

        public Button UsernameAndPasswordButton => networkTab.Get<Button>(SearchCriteria.ByText("Username and Password..."));            
        #endregion

        #region Methods        
        public ProxyAuthenticationWindow OpenUsernameAndPassword()
        {
            ClickButton(UsernameAndPasswordButton);
            var proxyAuthenticationWindow = MainWindow.MdiChild(SearchCriteria.ByText("Authenticate"));
            return new ProxyAuthenticationWindow(MainWindow, OptionsWindowContainer, proxyAuthenticationWindow);
        }
        #endregion
    }
}