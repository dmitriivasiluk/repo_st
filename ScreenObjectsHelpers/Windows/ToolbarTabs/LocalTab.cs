using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs
{
    public class LocalTab : NewTabWindow
    {
        public LocalTab(Window mainWindow) : base(mainWindow)
        {
        }

        public override WPFLabel ToolbarTabButton
        {
            get
            {
                try
                {
                    return MainWindow.Get<WPFLabel>(SearchCriteria.ByText("Local"));
                }
                catch (AutomationException)
                {
                    return null;
                }
            }
        }       
    }
}