using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.Finders;
using System.Windows.Automation;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Repository;
using System.Threading;
using TestStack.White.UIItems.TabItems;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs
{
    public class CloneTab : NewTabWindow
    {
        public CloneTab(TestStack.White.UIItems.WindowItems.Window mainWindow) : base(mainWindow)
        {
        }

        #region UIElements
        public override TabPage ToolbarTab => MainWindow.Get<TabPage>(SearchCriteria.ByAutomationId("CloneRepoTab"));
        public Label RemoteAccountLabel => MainWindow.Get<Label>(SearchCriteria.ByAutomationId("remote account"));
        public TextBox SourcePathTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("SourceTextBox"));
        private Button DetailsButton => MainWindow.Get<Button>(SearchCriteria.ByText("Details..."));
        public ComboBox LocalFolderComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("CloneBookmarksFolderList"));
        public Button CloneButton => MainWindow.Get<Button>(SearchCriteria.ByText("Clone"));
        public Button AdvancedOptionsButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("HeaderSite"));
        public CheckBox RecurseSubmodulesCheckBox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Recurse submodules"));
        public CheckBox NoHardlinksCheckBox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("No hardlinks"));

        //AutomationID_required
        
        public TextBox DestinationPathTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(1));

        public TextBox NameTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(2));

        /*
        public Button SourcePathButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId(""));

        public Button DestinationPathButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId(""));

        public ComboBox CheckoutBranchComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId(""));

        public TextBox CloneDepthTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId(""));
        */
        #endregion

        #region Methods 

        public Boolean IsAdvancedOptionsExpanded()
        {
            if (AdvancedOptionsButton.State == ToggleState.On)
            {
                return true;
            }
            return false;
        }

        public void AdvancedOptionsExpand()
        {
            if (IsAdvancedOptionsExpanded())
            {
                return;
            }
            else
            {
                AdvancedOptionsButton.Click();
            }
        }

        public void AdvancedOptionsCollapse()
        {
            if (!IsAdvancedOptionsExpanded())
            {
                return;
            }
            else
            {
                AdvancedOptionsButton.Click();
            }
        }

        public void TriggerValidation()
        {
            //AutomationID_required - temporary workaround
            DestinationPathTextBox.Focus();
            Thread.Sleep(2000);
            NameTextBox.Focus();
        }

        public bool IsCloneButtonEnabled()
        {
            return CloneButton.Enabled;
        }
        
        public RepositoryTab ClickCloneButton()
        {
            if (IsCloneButtonEnabled())
            {
                CloneButton.Click();
                Thread.Sleep(3000);
            }
            return new RepositoryTab(MainWindow);
        }

        public bool GetValidationMessage(string text)
        {
            DestinationPathTextBox.Focus();

            if (GetWithWait<WPFLabel>(MainWindow, SearchCriteria.ByText(text)) == null)
            {
                return false;
            }

            NameTextBox.Focus();

            return true;
        }

        public struct LinkValidationMessage
        {
            public static string noPathSupplied = "No path / URL supplied";
            public static string notValidPath = "This is not a valid source path / URL";
            public static string gitRepoType = "This is a Git repository";
            public static string mercurialRepoType = "This is a Mercurial repository";
            public static string checkingSource = "Checking source...";
        }
        #endregion
    }
}