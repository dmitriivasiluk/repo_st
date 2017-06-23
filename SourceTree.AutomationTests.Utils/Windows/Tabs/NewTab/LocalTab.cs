using System;
using ScreenObjectsHelpers.Windows;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Tabs.NewTab
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
                catch (NullReferenceException e)
                {
                    Console.WriteLine("**************************************");
                    Console.WriteLine(e.Message);
                    throw new NullReferenceException("ToolbarTab:" + e.Message);                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("**************************************");
                    Console.WriteLine(e.Message);
                    throw new Exception(e.Message);
                }
            }
        }       
    }
}