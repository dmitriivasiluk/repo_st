using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.MenuFolder
{
    public class ViewMenu : MenuBar
    {
        public ViewMenu(Window mainWindow) : base(mainWindow)
        {
        }
        public override Menu UIElementMenu { get { return MainWindow.Get<Menu>(SearchCriteria.ByText("View")); } }
        //TODO: running tests against SourceTree beta
        //public override Menu UIElementMenu { get { return MainWindow.Get<Menu>(SearchCriteria.ByAutomationId("MenuView")); } }

        #region Methods        
        public void ClickOperations(OperationsView operation)
        {
            UIElementMenu.SubMenu(operation.Value).Click();
        }
        #endregion

    }

    public struct OperationsView
    {
        private OperationsView(string value) { Value = value; }
        public string Value { get; set; }
        public static OperationsView Refresh => new OperationsView("Refresh"); 
        public static OperationsView NextTab => new OperationsView("Next Tab"); 
        public static OperationsView PreviuosTab => new OperationsView("Previuos Tab"); 
        public static OperationsView FileStatusView => new OperationsView("File Status View"); 
        public static OperationsView LogView => new OperationsView("Log View"); 
        public static OperationsView SearchView => new OperationsView("Search View"); 
    }
}
