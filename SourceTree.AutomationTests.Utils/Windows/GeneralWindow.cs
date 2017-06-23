using System;
using SourceTree.AutomationTests.Utils.Windows.Menu;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows
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
