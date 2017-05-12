using System;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;
using System.Windows.Automation;
using TestStack.White.InputDevices;

namespace ScreenObjectsHelpers.Windows.Repository
{
    public class AddSubmoduleWindow : GeneralWindow
    {
        public AddSubmoduleWindow(Window mainWindow) : base(mainWindow)
        {
        }
        public override void ValidateWindow()
        {
            // Need verify opened tab in this method, need implementation! If validation is fail, throw exception!
            Console.WriteLine("AddSubmoduleWindow opened");
        }

        #region UIItems
        public TextBox SourcePathTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("SourceTextBox"));
        public Button OKButton => MainWindow.Get<Button>(SearchCriteria.ByText("OK"));
        public Button CancelButton => MainWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
        #endregion

        #region Methods
        public void ClickOkButton()
        {
            OKButton.Click();
        }
        public RepositoryTab ClickCancelButton()
        {
            CancelButton.Click();
            return new RepositoryTab(MainWindow);
        }
        #endregion
    }
}