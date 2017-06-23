using ScreenObjectsHelpers.Windows;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TabItems;

namespace SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab
{
    public class RemoteTab : NewTabWindow
    {        
        public RemoteTab(TestStack.White.UIItems.WindowItems.Window mainWindow) : base(mainWindow)
        {
        }

        public override TabPage ToolbarTab => MainWindow.Get<TabPage>(SearchCriteria.ByAutomationId("RemoteRepoListTab"));
        public WPFLabel AddAccountButton => MainWindow.Get<WPFLabel>(SearchCriteria.ByText("Add an account..."));
        public Button EditAccountsButton => MainWindow.Get<Button>(SearchCriteria.ByText("Edit Accounts..."));

        public EditHostingAccountWindow.EditHostingAccountWindow ClickAddAccountButton()
        {
            AddAccountButton.Click();

            var editHostingAccountWindow = MainWindow.MdiChild(SearchCriteria.ByText("Edit Hosting Account"));
            
            return new EditHostingAccountWindow.EditHostingAccountWindow(MainWindow, editHostingAccountWindow);
        }
    }
}
