using ScreenObjectsHelpers.Windows.Options;
using System;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows
{
    public class EditCustomActionWindow : GeneralWindow
    {
        private UIItemContainer customActionsWindow;
        private UIItemContainer customActionsTab;

        public EditCustomActionWindow(Window mainWindow, UIItemContainer customActionsTab, UIItemContainer customActionsWindow)
            : base(mainWindow)
        {
            this.customActionsWindow = customActionsWindow;
            this.customActionsTab = customActionsTab;
        }

        #region UIItems
        public TextBox MenuCaption => customActionsWindow.Get<TextBox>(SearchCriteria.ByAutomationId("CaptionBox"));
        public CheckBox OpenInASeparateWindow => customActionsWindow.Get<CheckBox>(SearchCriteria.ByText("Open in a separate window"));
        public CheckBox ShowFullOutput => customActionsWindow.Get<CheckBox>(SearchCriteria.ByText("Show Full Output"));
        public TextBox ScriptToRun => customActionsWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(1));
        public TextBox Parameters => customActionsWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(2));
        
        #endregion

        #region Methods
        public CustomActionsTab ClickOKButton()
        {
            ClickButton(OKButton);
            return new CustomActionsTab(MainWindow, customActionsTab);
        }
        public CustomActionsTab ClickCancelButton()
        {
            ClickButton(CancelButton);
            return new CustomActionsTab(MainWindow, customActionsTab);
        }

        #endregion
    }
}
