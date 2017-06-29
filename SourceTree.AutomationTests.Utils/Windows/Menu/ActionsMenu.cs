using SourceTree.AutomationTests.Utils.Windows.Menu.Action;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Menu
{
    public class ActionsMenu : MenuBar
    {
        public ActionsMenu(Window mainWindow) : base(mainWindow)
        {
        }

        public override TestStack.White.UIItems.MenuItems.Menu UIElementMenu { get { return MainWindow.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByText("Actions")); } }

        public ResolveConflict OpenResolveConflictMenu()
        {
            return new ResolveConflict(MainWindow);
        }

    }

}
