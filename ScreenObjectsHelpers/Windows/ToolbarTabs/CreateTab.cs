using System;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.Repository;
using System.Threading;
using TestStack.White.UIItems.TabItems;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs
{
    public class CreateTab : NewTabWindow
    {
        public CreateTab(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIElements

        public override TabPage ToolbarTab => MainWindow.Get<TabPage>(SearchCriteria.ByAutomationId("CreateRepoTab"));
        public Button BrowseButton => MainWindow.Get<Button>(SearchCriteria.ByText("Browse"));
        public TextBox DestinationPathTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("CreateRepoDestinationPath"));
        public TextBox NameRepoTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("CreateRepoName"));
        public ComboBox RepoTypeComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("CreateRepoTypeDropdown"));
        //AutomationID_required
        public CheckBox CreateRemoteCheckBox =>
            MainWindow.Get<CheckBox>(SearchCriteria.ByClassName("CheckBox").AndByText("Create Repository On Account:"));
        //=================================Opens when CreateRemoteCheckBox is checked - Start
        public ComboBox RemoteAccountsComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("LocalRemoteTypeDropdown"));
        //AutomationID is needed
        public TextBox DescriptionTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(2));
        public CheckBox IsPrivateCheckBox => MainWindow.Get<CheckBox>(SearchCriteria.ByClassName("CheckBox").AndByText("Is Private"));
        //=================================Opens when CreateRemoteCheckBox is checked - End
        public Button CreateButton => MainWindow.Get<Button>(SearchCriteria.ByClassName("Button").AndByText("CreateRepoButton"));

        public struct CVS
        {
            public static string GitHub = "Git";
            public static string Mercurial = "Mercurial";
        }

        #endregion

        #region Methods
        //TODO Refactor to BasicWindow
        public Boolean IsCreateRemoteChecked()
        {
            return CreateRemoteCheckBox.Checked;
        }
        //TODO Refactor to BasicWindow
        public Boolean IsPrivateChecked()
        {
            return IsPrivateCheckBox.Checked;
        }

        public Boolean IsRepoSettingsAvailable()
        {
            CheckCheckbox(CreateRemoteCheckBox);
            Thread.Sleep(1000);
            return RemoteAccountsComboBox.Enabled && RemoteAccountsComboBox.Visible &&
                DescriptionTextBox.Enabled && DescriptionTextBox.Visible &&
                IsPrivateCheckBox.Enabled && IsPrivateCheckBox.Visible;
        }

        public RepositoryTab ClickCreateButton()
        {
            ClickButton(CreateButton);
            return new RepositoryTab(MainWindow);
        }

        public WarningExistingEmptyFolder ClickCreateButtonCallsWarning()
        {
            ClickButton(CreateButton);
            Thread.Sleep(2000);
            return new WarningExistingEmptyFolder(MainWindow);
        }

        #endregion

    }
    
    //TODO add separate folder for dialogs
    public class DialogSelectDestination : GeneralWindow
    {
        public DialogSelectDestination(Window mainWindow) : base(mainWindow)
        {
            dialogWindow = mainWindow.ModalWindow("Select Destination Path");
        }

        public Window dialogWindow { get; }

        #region UIElements

        public TitleBar titleBar => dialogWindow.TitleBar;
        //ToDo Add proper ToolBar identification
        public TextBox destinationFolder => dialogWindow.Get<TextBox>(SearchCriteria.ByAutomationId("1152"));
        public Button selectFolderButton => dialogWindow.Get<Button>(SearchCriteria.ByAutomationId("1"));
        public Button cancelButton => dialogWindow.Get<Button>(SearchCriteria.ByAutomationId("2"));

        #endregion

        #region Methods

        public void ClickSelectButton()
        {
            ClickButton(selectFolderButton);
        }
        
        public void ClickCancelButton()
        {
            ClickButton(cancelButton);
        }

        #endregion
    }

    //TODO separate folder for warnings
    public class WarningExistingEmptyFolder : GeneralWindow
    {
        public WarningExistingEmptyFolder(Window mainWindow) : base(mainWindow)
        {
            WarningWindowContainer = mainWindow.ModalWindow(SearchCriteria.ByAutomationId("window"));
        }

        public Window WarningWindowContainer { get; }

        #region UIItems
        public TextBox titleBar
        {
            get
            {
                var controlElement = WarningWindowContainer.GetElement(SearchCriteria.ByText("Problem with destination directory").AndControlType(ControlType.Text));
                return controlElement != null ? new TextBox(controlElement, WarningWindowContainer.ActionListener) : null;
            }
        }

        public Button closeButton => WarningWindowContainer.Get<Button>(SearchCriteria.ByAutomationId("close"));
        public Button yesButton => WarningWindowContainer.Get<Button>(SearchCriteria.ByText("Yes"));
        public Button noButton => WarningWindowContainer.Get<Button>(SearchCriteria.ByText("No"));
        #endregion

        #region Methods

        public void ClickCloseButton()
        {
            ClickButton(closeButton);
        }

        public RepositoryTab ClickYesButton()
        {
            ClickButton(yesButton);
            return new RepositoryTab(MainWindow);
        }

        public void ClickNoButton()
        {
            ClickButton(noButton);
        }

        #endregion
    }

    public class WarningExistingRepoMercurial : GeneralWindow
    {
        public WarningExistingRepoMercurial(Window mainWindow) : base(mainWindow)
        {
            WarningWindowContainer = mainWindow.ModalWindow(SearchCriteria.ByAutomationId("window"));
        }
        public Window WarningWindowContainer { get; }

        #region UIItems
        public TextBox titleBar
        {
            get
            {
                var controlElement = WarningWindowContainer
                    .GetElement(SearchCriteria.ByText("Failed to create local repository")
                    .AndControlType(ControlType.Text));
                return controlElement != null ? new TextBox(controlElement, WarningWindowContainer.ActionListener) : null;
            }
        }
        public Button closeButton => WarningWindowContainer.Get<Button>(SearchCriteria.ByText("Cancel"));
        #endregion

        #region Methods
        public void ClickCloseButton()
        {
            ClickButton(closeButton);
        }
        #endregion
    }
}
