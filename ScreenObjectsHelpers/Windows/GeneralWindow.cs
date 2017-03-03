﻿using ScreenObjectsHelpers.Windows.Options;
using ScreenObjectsHelpers.Windows.MenuFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;


namespace ScreenObjectsHelpers.Windows
{
    /// <summary>
    /// This is general class for give other window common method like Menu Bar, workings with tab etc.
    /// </summary>
    public abstract class GeneralWindow : BasicWindow
    {

        public GeneralWindow(Window mainWindow) : base(mainWindow)
        {
            ValidateWindow();
        }

        public abstract void ValidateWindow();

        public T OpenMenu<T>() where T : MenuBar
        {
            return (T)Activator.CreateInstance(typeof(T), MainWindow);
        }

    }
}
