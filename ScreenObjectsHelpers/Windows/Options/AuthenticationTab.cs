using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Options
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