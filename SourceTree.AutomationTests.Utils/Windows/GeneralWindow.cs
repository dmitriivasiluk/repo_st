using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Windows.MenuFolder;

namespace ScreenObjectsHelpers.Windows
{
    /// <summary>
    /// This is general class for give other window common method like Menu Bar, workings with tab etc.
    /// </summary>
    public abstract class GeneralWindow : BasicWindow
    {
        public GeneralWindow(Window mainWindow) : base(mainWindow)
        {

        }

        public T OpenMenu<T>() where T : MenuBar
        {
            return (T)Activator.CreateInstance(typeof(T), MainWindow);
        }

        public Button NewTabButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("DefaultAddButton").AndByClassName("Button"));
    }
}
