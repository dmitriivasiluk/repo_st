using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.MenuFolder
{
    public abstract class MenuBar : BasicWindow
    {
        public MenuBar(Window mainWindow) : base(mainWindow)
        {
        }

        public abstract Menu UIElementMenu
        {
            get;
        }

    }
}
