using System;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs
{
    public class LocalTab : NewTabWindow
    {
        public LocalTab(Window mainWindow) : base(mainWindow)
        {
        }

        public override TabPage ToolbarTab
        {
            get
            {
                try
                {
                    return MainWindow.Get<TabPage>(SearchCriteria.ByAutomationId("LocalRepoListTab"));
                }
                catch (AutomationException e)
                {                    
                    return null;
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine("**************************************");
                    Console.WriteLine(e.Message);
                    throw new NullReferenceException("ToolbarTab:" + e.Message);                    
                }
            }
        }       
    }
}