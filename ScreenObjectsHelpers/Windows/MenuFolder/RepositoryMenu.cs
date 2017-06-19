﻿using System;
using System.Windows.Automation;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.MenuFolder
{
    public class RepositoryMenu : MenuBar
    {
        public RepositoryMenu(Window mainWindow) : base(mainWindow)
        {
        }
        public override Menu UIElementMenu { get { return MainWindow.Get<Menu>(SearchCriteria.ByText("Repository")); } }
        public struct OperationsRepositoryMenu
        {
            private OperationsRepositoryMenu(string value) { Value = value; }
            public string Value { get; set; }
            public static OperationsRepositoryMenu AddSubmodule => new OperationsRepositoryMenu("Add Submodule...");
            public static OperationsRepositoryMenu AddLinkSubtree => new OperationsRepositoryMenu("Add/Link Subtree...");
        }
        #region Methods        
        public T ClickOperationToReturnWindow<T>(OperationsRepositoryMenu windowType) where T : GeneralWindow
        {
            try
            {
                UIElementMenu.SubMenu(windowType.Value).Click();
                return (T)Activator.CreateInstance(typeof(T), MainWindow);
            }
            catch (ElementNotAvailableException e)
            {
                Console.WriteLine("No menu options available", e.Message);
            }

            throw new ElementNotAvailableException("No menu options available");
        }

        #endregion        
    }

}
