using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs.AddTabWindows
{
    class OpenWorkingCopyWindow : BasicWindow
    {
        public OpenWorkingCopyWindow(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIElements        

        public UIItem FolderTextBox => MainWindow.Get<UIItem>(SearchCriteria.ByText("Folder:"));

        public Button SelectFolderButton => MainWindow.Get<Button>(SearchCriteria.ByText("Select Folder"));       

        #endregion

        #region Methods

        //TODO methods should return add tab
        public void SelectFolderButtonClick()
        {
            ClickButton(SelectFolderButton);            
        }

        public void CancelButtonClick()
        {
            ClickButton(CancelButton);
        }

        #endregion
    }
}
