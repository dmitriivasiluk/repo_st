using System;
using SourceTree.AutomationTests.Utils.Windows.Menu.Tools.Options;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Menu
{
    public class ToolsMenu : MenuBar
    {
        private const string launchSSHAgent = "Launch SSH Agent...";
        private const string createImportSSHKeys = "Create or Import SSH Keys";
        private const string options = "Options";

        public ToolsMenu(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIElements        
        public override TestStack.White.UIItems.MenuItems.Menu UIElementMenu => MainWindow.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByText("Tools"));
        //TODO: running tests against SourceTree beta
        //public override Menu UIElementMenu => MainWindow.Get<Menu>(SearchCriteria.ByAutomationId("MenuTools"));
        #endregion

        #region Methods        
        public void LaunchSSHAgent()
        {
            UIElementMenu.SubMenu(launchSSHAgent).Click();
        }

        public object CreateImportSSHKeys()
        {
            UIElementMenu.SubMenu(createImportSSHKeys).Click();
            throw new NotImplementedException("No corresponding class");
        }

        public OptionsWindow OpenOptions()
        {
            UIElementMenu.SubMenu(options).Click();
            var optionsWindow = MainWindow.MdiChild(SearchCriteria.ByText(options));
            return new GeneralTab(MainWindow, optionsWindow);
        }
        #endregion
    }
}
