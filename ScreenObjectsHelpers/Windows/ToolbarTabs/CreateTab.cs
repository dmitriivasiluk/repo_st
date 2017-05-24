using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;

namespace ScreenObjectsHelpers.Windows.ToolbarTabs
{
    public class CreateTab : NewTabWindow
    {
        public CreateTab(TestStack.White.UIItems.WindowItems.Window mainWindow) : base(mainWindow)
        {
        }

        #region UIElements

        public override UIItem ToolbarTabButton => MainWindow.Get<UIItem>(SearchCriteria.ByText("Create"));
        public Button BrowseButton => MainWindow.Get<Button>(SearchCriteria.ByText("Browse"));
        public TextBox DestinationPathTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("CreateRepoDestinationPath"));
        public TextBox NameRepoTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("CreateRepoName"));
        public ComboBox RepoTypeComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("CreateRepoTypeDropdown"));
        //AutomationID is needed
        public CheckBox CreateRemoteCheckBox =>
            MainWindow.Get<CheckBox>(SearchCriteria.ByClassName("CheckBox").AndByText("Create Repository On Account:"));
        //=================================Opens when CreateRemoteCheckBox is checked - Start
        public ComboBox RemoteAccountsComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("LocalRemoteTypeDropdown"));
        //AutomationID is needed
        public TextBox DescriptionTextBox => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndAutomationId(""));
        public CheckBox IsPrivateCheckBox => MainWindow.Get<CheckBox>(SearchCriteria.ByClassName("CheckBox").AndByText("Is Private"));
        //=================================Opens when CreateRemoteCheckBox is checked - End
        public Button CreateButton => MainWindow.Get<Button>(SearchCriteria.ByClassName("Button").AndByText("CreateRepoButton"));
        //New window is opened after successfull repo creation
        public UIItem TabTextGit => MainWindow.Get<UIItem>(SearchCriteria.ByText(ConstantsList.testGitRepoBookmarkName));
        public UIItem TabTextHg => MainWindow.Get<UIItem>(SearchCriteria.ByText(ConstantsList.testHgRepoBookmarkName));

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
            Utils.ThreadWait(1000);
            return RemoteAccountsComboBox.Enabled && RemoteAccountsComboBox.Visible &&
                DescriptionTextBox.Enabled && DescriptionTextBox.Visible &&
                IsPrivateCheckBox.Enabled && IsPrivateCheckBox.Visible;
        }
        
        #endregion

        //TODO: Put this struct to  ScreenObjectsHelpers / Windows / ToolbarTabs / 
        //EditHostingAccountWindow / EditHostingAccountWindow.cs
        public struct CVS
        {
            public static string GitHub = "Git";
            public static string Mercurial = "Mercurial";
            //somesing strange
            public static string None = "None";
        }
    }
}
