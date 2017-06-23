using ScreenObjectsHelpers.Windows;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Menu
{
    public abstract class MenuBar : BasicWindow
    {
        public MenuBar(Window mainWindow) : base(mainWindow)
        {
        }

        public abstract TestStack.White.UIItems.MenuItems.Menu UIElementMenu
        {
            get;
        }

    }
}
