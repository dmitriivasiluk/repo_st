using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.MenuFolder
{
    public class EditMenu : MenuBar
    {
        public EditMenu(Window mainWindow) : base(mainWindow)
        {
        }

        public override Menu UIElementMenu { get { return MainWindow.Get<Menu>(SearchCriteria.ByText("Edit")); } }
        //TODO: running tests against SourceTree beta
        //public override Menu UIElementMenu { get { return MainWindow.Get<Menu>(SearchCriteria.ByAutomationId("MenuEdit")); } }


        #region Methods        
        public void ClickOperations(OperationsEdit operation)
        {
            UIElementMenu.SubMenu(operation.Value).Click();
        }
        #endregion
    }

    public struct OperationsEdit
    {
        private OperationsEdit(string value) { Value = value; }
        public string Value { get; set; }
        public static OperationsEdit Undo => new OperationsEdit("Undo"); 
        public static OperationsEdit Redo => new OperationsEdit("Redo"); 
        public static OperationsEdit Cut => new OperationsEdit("Cut"); 
        public static OperationsEdit Copy => new OperationsEdit("Copy");
        public static OperationsEdit Paste => new OperationsEdit("Paste"); 
        public static OperationsEdit SelectAll => new OperationsEdit("Select All"); 
    }
}
