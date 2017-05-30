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
        
        public Button OkButton => customActionsWindow.Get<Button>(SearchCriteria.ByText("OK"));
        public Button CancelButton => customActionsWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
        #endregion

        #region Methods
        public void SetMenuCaption(string menuCaption)
        {
            MenuCaption.Text = menuCaption;
        }
        public void SetScriptToRun(string scriptToRun)
        {
            ScriptToRun.Text = scriptToRun;
        }
        public void SetParameters(string parameters)
        {
            Parameters.Text = parameters;
        }

        public CustomActionsTab ClickOKButton()
        {
            OkButton.Click();
            return new CustomActionsTab(MainWindow, customActionsTab);
        }
        public CustomActionsTab ClickCancelButton()
        {
            CancelButton.Click();
            return new CustomActionsTab(MainWindow, customActionsTab);
        }
        #endregion
    }
}
