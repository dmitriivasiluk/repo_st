using ScreenObjectsHelpers.Helpers;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs
{
    public class AddTab : NewTabWindow
    {
        public AddTab(TestStack.White.UIItems.WindowItems.Window mainWindow) : base(mainWindow)
        {
        }

        #region UIElements
        public override UIItem ToolbarTabButton => MainWindow.Get<UIItem>(SearchCriteria.ByText("Add"));

        public Button BrowseButton => MainWindow.Get<Button>(SearchCriteria.ByText("Browse"));

        public Button AddButton => MainWindow.Get<Button>(SearchCriteria.ByText("Add"));

        //AutomationID_required Temporary workaround
        public TextBox WorkingCopyPathTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(0));

        public TextBox NameTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(1));

        //AutomationID_required
        /*
        public TextBox WorkingCopyPathTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId(""));

        public TextBox NameTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId(""));

        public ComboBox LocalFolderComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId(""));
        */
        #endregion

        #region Methods
        public void TriggerValidation()
        {
            NameTextBox.Focus();
            Utils.ThreadWait(2000);
        }

        //TODO return OpenWorkingCopyWindow class
        public void ClickBrowseButton()
        {
            ClickButton(BrowseButton);
        }

        public bool GetValidationMessage(string text)
        {
            TriggerValidation();

            if (GetWithWait<WPFLabel>(MainWindow, SearchCriteria.ByText(text)) == null)
            {
                return false;
            }            

            return true;
        }

        public struct RepoValidationMessage
        {
            public static string noWorkingPathSupplied = "No working copy path supplied";
            public static string notValidPath = "This is not a valid working copy path.";
            public static string gitRepoType = "This is a Git repository";
            public static string mercurialRepoType = "This is a Mercurial repository";
        }

        #endregion
    }
}
