using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs
{
    public class RemoteTab : NewTabWindow
    {
        private readonly UIItemContainer remoteTab;
        public RemoteTab(TestStack.White.UIItems.WindowItems.Window mainWindow) : base(mainWindow)
        {
        }

        public override WPFLabel ToolbarTabButton => MainWindow.Get<WPFLabel>(SearchCriteria.ByText("Remote"));
        public WPFLabel AddAccountButton => MainWindow.Get<WPFLabel>(SearchCriteria.ByText("Add an account..."));
        public Button EditAccountsButton => MainWindow.Get<Button>(SearchCriteria.ByText("Edit Accounts..."));

        public EditHostingAccountWindow ClickAddAccountButton()
        {
            AddAccountButton.Click();

            var editHostingAccountWindow = MainWindow.MdiChild(SearchCriteria.ByText("Edit Hosting Account"));
            
            return new EditHostingAccountWindow(MainWindow, editHostingAccountWindow);
        }
    }
}
