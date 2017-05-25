using System;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows
{
    /// <summary>
    /// This is main Window with opened New tab.
    /// This class inherit Menu from General class and this first main real window object which not abstract.
    /// </summary>
    public abstract class NewTabWindow : GeneralWindow
    {
        private UIItemContainer newTab;
        public NewTabWindow(Window mainWindow) : base(mainWindow)
        {
            //Button gotItButton = GotItButton;
            //if (gotItButton != null && gotItButton.Visible)
            //{
            //    ClickButton(gotItButton);
            //}
            OpenToolbarTab();
        }

        public abstract UIItem ToolbarTabButton
        {
            get;
        }

        public Button GotItButton
        {
            get
            {
                try
                {
                    return MainWindow.Get<Button>(SearchCriteria.ByText("Got it"));
                }
                catch (AutomationException)
                {
                    return null;
                }
            }
        }

        public override void ValidateWindow()
        {
            // I guess this method should close all opened tab and open new one. Then validate that is all right. 
            // If validation is fail, throw exception!
            Console.WriteLine("WAIT FOR OPENING TAB");
        }

        public T OpenTab<T>() where T : NewTabWindow
        {
            return (T)Activator.CreateInstance(typeof(T), MainWindow);
        }

        public virtual void OpenToolbarTab()
        {
            if (ToolbarTabButton == null)
            {
                ClickButton(NewTabButton);
            }
            ToolbarTabButton.Click();
        }

        public string GetTitle()
        {
            return MainWindow.Title;
        }
    }
}
