using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Options
{
    public class UpdatesTab : OptionsWindow
    {

        public UpdatesTab(Window mainWindow, UIItemContainer optionsWindow) : base(mainWindow, optionsWindow)
        {
        }


        #region UIElements
        public override UIItem UIElementTab => OptionsWindowContainer.Get<UIItem>(SearchCriteria.ByText("Updates"));
            
        public Button CheckForUpdatesButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Check For Updates"));
            
        #endregion

        #region Methods        
        public void CheckForUpdate()
        {
            this.ClickButton(CheckForUpdatesButton);
        }
        #endregion
    }
}
