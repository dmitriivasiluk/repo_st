﻿using System;
using TestStack.White.UIItems;
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
            OpenToolbarTab();
        }

        public abstract UIItem ToolbarTabButton
        {
            get;
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
                ClickOnButton(NewTabButton);
            }
            ToolbarTabButton.Click();
        }

        public string GetTitle()
        {
            return MainWindow.Title;
        }
    }
}
