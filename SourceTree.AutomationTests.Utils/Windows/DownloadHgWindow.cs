using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows
{
    public class DownloadHgWindow : GeneralWindow
    {
        public DownloadHgWindow(Window mainWindow, UIItemContainer downloadHgWindow) : base(mainWindow)
        {
            DownloadHgWindowContainer = downloadHgWindow;
        }

        public UIItemContainer DownloadHgWindowContainer { get; }
    }
}
