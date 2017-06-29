using System;
using SourceTree.AutomationTests.Utils.Windows.MenuFolder;
using SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Menu
{
    public class FileMenu : MenuBar
    {
        private const string cloneNew = "Clone / New...";
        private const string open = "Open...";
        private const string exitSourceTree = "Exit SourceTree";

        public FileMenu(Window mainWindow) : base(mainWindow)
        {
        }

        public override TestStack.White.UIItems.MenuItems.Menu UIElementMenu
        {
            get
            {
                return MainWindow.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByText("File"));
                //TODO: running tests against SourceTree beta
                //return MainWindow.Get<Menu>(SearchCriteria.ByAutomationId("MenuFile"));
            }
        }

        public CloneTab OpenCloneNew()
        {
            UIElementMenu.SubMenu(cloneNew).Click();
            CloneTab newTab = new CloneTab(MainWindow);
            return newTab;
        }

        public object OpenRepository()
        {
            UIElementMenu.SubMenu(open).Click();
            throw new NotImplementedException("No corresponding class");
        }

        public void ExitSourceTree()
        {
            UIElementMenu.SubMenu(exitSourceTree).Click();
        }
    }
}
