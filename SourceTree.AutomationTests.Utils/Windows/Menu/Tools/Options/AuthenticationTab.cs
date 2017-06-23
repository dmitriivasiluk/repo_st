﻿using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Menu.Tools.Options
{
    public class AuthenticationTab : OptionsWindow
    {        
        public AuthenticationTab(Window mainWindow, UIItemContainer optionsWindow) : base(mainWindow, optionsWindow)
        {
        }

        #region UIElements        
        public override UIItem UIElementTab => OptionsWindowContainer.Get<UIItem>(SearchCriteria.ByText("Authentication"));            
        #endregion
    }
}