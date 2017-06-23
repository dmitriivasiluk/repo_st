using ScreenObjectsHelpers.Windows.MenuFolder.ActionMenu;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.MenuFolder
{
    public class ActionsMenu : MenuBar
    {
        public ActionsMenu(Window mainWindow) : base(mainWindow)
        {
        }

        public override Menu UIElementMenu { get { return MainWindow.Get<Menu>(SearchCriteria.ByText("Actions")); } }

        public ResolveConflict OpenResolveConflictMenu()
        {
            return new ResolveConflict(MainWindow);
        }

    }

}
