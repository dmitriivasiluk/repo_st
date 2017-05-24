using ScreenObjectsHelpers.Windows.Repository;
using System.Windows.Automation;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.MenuFolder
{
    public class RepositoryMenu : MenuBar
    {
        private const string AddSubmodule = "Add Submodule...";
        public RepositoryMenu(Window mainWindow) : base(mainWindow)
        {
        }

        public override Menu UIElementMenu { get { return MainWindow.Get<Menu>(SearchCriteria.ByAutomationId("MenuRepository")); } }

        #region Methods        
        public void ClickOperations(OperationsHelp operation)
        {
            UIElementMenu.SubMenu(operation.Value).Click();
        }

        public AddSubmoduleWindow OpenAddSubmoduleWindow()
        {
            UIElementMenu.SubMenu(AddSubmodule).Click();

            return new AddSubmoduleWindow(MainWindow);
        }
        #endregion        
    }

}
