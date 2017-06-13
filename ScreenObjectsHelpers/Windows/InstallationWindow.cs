using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using TestStack.White;
using System.Windows.Automation;
using TestStack.White.UIItems.ListBoxItems;
using ScreenObjectsHelpers.Helpers;
using ScreenObjectsHelpers.Windows.ToolbarTabs;

namespace ScreenObjectsHelpers.Windows
{

    public class InstallationWindow : BasicWindow
    {

        public InstallationWindow(Window mainWindow) : base(mainWindow)
        {
            ValidateWindow();
        }

        public void ValidateWindow()
        {

        }

        #region UIElements
        public Button ContinueButton => MainWindow.Get<Button>(SearchCriteria.ByText("Continue"));
        public Button UseExistingAccount => MainWindow.Get<Button>(SearchCriteria.ByText("Use an existing account"));
        public Button SkipSetupButton => MainWindow.Get<Button>(SearchCriteria.ByText("Skip Setup"));
        public Button BrowseDestinationPathButton => MainWindow.Get<Button>(SearchCriteria.ByText("..."));
        public CheckBox LicenceAgreementCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("I agree to the "));
        public CheckBox ConfigureAutomaticLineEndingCheckBox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Configure automatic line ending handling by default (recommended)"));
        public Label RegistrationCompleteText => MainWindow.Get<Label>(SearchCriteria.ByText("Registration Complete!"));
        public Label EmailLoggedAs => MainWindow.Get<Label>(SearchCriteria.ByText("")); 
        public Label DownloadingVersionControlLabel => (Label) MainWindow.Get(SearchCriteria.ByText("Downloading version control systems..."), TimeSpan.FromSeconds(10));
        public Label ToolInstallCompletedLabel => MainWindow.Get<Label>(SearchCriteria.ByText("Tool installation completed."));
        public ListItem GitHubImageButton => MainWindow.Get<ListBox>(SearchCriteria.ByControlType(ControlType.List)).Item("GitHub");
        public ListItem BitbucketImageButton => MainWindow.Get<ListBox>(SearchCriteria.ByControlType(ControlType.List)).Item("Bitbucket");
        public ListItem BitbucketServerImageButton => MainWindow.Get<ListBox>(SearchCriteria.ByControlType(ControlType.List)).Item("Bitbucket Server");
        public ComboBox AuthenticationComboBox => MainWindow.Get<ComboBox>(SearchCriteria.ByControlType(ControlType.ComboBox));
        public TextBox UsernameField => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(1));
        public TextBox PasswordField => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("Password"));
        public TextBox HostUrlField => MainWindow.Get<TextBox>(SearchCriteria.ByClassName("TextBox").AndIndex(0));
        public TextBox SearchField => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit));
        public ProgressBar InstallTollsProgressBar => MainWindow.Get<ProgressBar>(SearchCriteria.ByControlType(ControlType.ProgressBar));
        public ListView RepoListView => MainWindow.Get<ListView>(SearchCriteria.ByAutomationId("RepoList"));
        #endregion

        #region Methods
        public void ClickContinueButton()
        {
            ClickButton(ContinueButton);
        }

        public AuthenticationWindow ClickUseExistingAccount()
        {
            ClickButton(UseExistingAccount);
            Window authenticationWindow = Desktop.Instance.Windows().FirstOrDefault(c =>
            {
                var found = false;
                try
                {
                    var item = c.Get<Button>(SearchCriteria.ByText("Log in with Google"));
                    found = true;
                }
                catch (Exception)
                {
                    // ignored
                }
                return c.IsModal == false && found;
            }); // Login window is opened without Name (title), so it is best way to find a window.
            return new AuthenticationWindow(MainWindow, authenticationWindow);
        }

        public void FillBasicAuthenticationGithub(string userName, string password)
        {
            ChooseGitHubAccount();
            AuthenticationComboBox.Select("Basic");
            UsernameField.Text = userName;
            PasswordField.Text = password;
        }

        public void FillAuthenticationBitBucketServer(string hostURL, string userName, string password)
        {
            ChooseBitBucketServerAccount();
            HostUrlField.Text = hostURL;
            UsernameField.Text = userName;
            PasswordField.Text = password;
        }

        public void FillBasicAuthenticationBitbucket(string userName, string password)
        {
            ChooseBitBucketAccount();
            AuthenticationComboBox.Select("Basic");
            UsernameField.Text = userName;
            PasswordField.Text = password;
        }

        public string DownloadingVersionText()
        {
            return DownloadingVersionControlLabel.Text;
        }

        public string ToolInstallCompleteText()
        {
            return ToolInstallCompletedLabel.Text;
        }

        public void CheckConfigureAutomaticLineEncodingCheckbox()
        {
            if (!ConfigureAutomaticLineEndingCheckBox.Checked)
            {
                ConfigureAutomaticLineEndingCheckBox.Click();
            }
        }

        public void WaitCompleteInstallToolsProgressBar()
        {
            var currentProcent = InstallTollsProgressBar.Value;
            var secondPass = 0;
            while (currentProcent < InstallTollsProgressBar.Maximum)
            {
                Thread.Sleep(1000);
                secondPass++;
                currentProcent = InstallTollsProgressBar.Value;
                if (secondPass > 180) // pass 3 minutes
                {
                    throw new TimeoutException();
                }
            }
        }

        public void UncheckConfigureAutomaticLineEncodingCheckbox()
        {
            if (ConfigureAutomaticLineEndingCheckBox.Checked)
            {
                ConfigureAutomaticLineEndingCheckBox.Click();
            }
        }

        public string LoggedAsEmail()
        {
            return EmailLoggedAs.Text;
        }

        public string CompleteText()
        {
            return RegistrationCompleteText.Text;
        }

        public bool IsContinueButtonActive() {
            return ContinueButton.Enabled;
        }

        public void CheckLicenceAgreementCheckbox() {
            bool isChecked = LicenceAgreementCheckbox.Checked;
            if (!isChecked)
            {
                LicenceAgreementCheckbox.Click();
            }
        }

        public void UncheckLicenceAgreementCheckbox()
        {
            bool isChecked = LicenceAgreementCheckbox.Checked;
            if (isChecked)
            {
                LicenceAgreementCheckbox.Click();
            }
        }

        public void ChooseGitHubAccount()
        {
            GitHubImageButton.Click();
        }

        public void ChooseBitBucketAccount()
        {
            BitbucketImageButton.Click();
        }

        public void ChooseBitBucketServerAccount()
        {
            BitbucketServerImageButton.Click();
        }

        public ErrorDialogWindow SwitchToErrorDialogWindow()
        {
            SearchCriteria searchCriteria = SearchCriteria.ByAutomationId("window");
            var errorDialog = this.WaitMdiChildAppears(searchCriteria, 10);
            return new ErrorDialogWindow(this, errorDialog);
        }

        public LocalTab SkipSetup()
        {
            ClickButton(SkipSetupButton);
            Window mercurialWindow = Utils.FindNewWindow("SourceTree: Mercurial not found", 10);
            if (mercurialWindow != null)
            {
                Button IDontWantUseMercurial = mercurialWindow.Get<Button>(SearchCriteria.ByAutomationId("CommandLink_2003"));
                IDontWantUseMercurial.Click();
            }
            Window mainWindow = Utils.FindNewWindow("SourceTree");
            return new LocalTab(mainWindow);
        }

        public LocalTab ClickContinueAtTheLatestStepButton()
        {
            try
            {
                ClickButton(ContinueButton);
            }
            catch (TimeoutException)
            {
                // Empty, expect that Configuration window is closed (the latest step in configuration, clone) and SourceTree is opened 
            }
            Window mainWindow = Utils.FindNewWindow("SourceTree");
            return new LocalTab(mainWindow);
        }

        public IgnoreFileDialogWindow GetInstallGlobalIgnoreFileDialogWindow()
        {
            SearchCriteria searchCriteria = SearchCriteria.ByAutomationId("window");
            var dialogWindow = this.WaitMdiChildAppears(searchCriteria, 10);
            return new IgnoreFileDialogWindow(this, dialogWindow);
        }

        public void SelectRepositoryByName(string repoName)
        {
            WaitListRemoteIsLoaded();
            for (int i = 0; i < RepoListView.Items.Count; i++)
            {
                if (RepoListView.Items[i].Contains(repoName))
                {
                    RepoListView.Select(i);
                    return;
                }
            }
            throw new ElementNotAvailableException($"This {repoName} is not found");
        }

        private void WaitListRemoteIsLoaded()
        {
            List<string> repositories = RepoListView.Items;
            int countLoop = 0;
            int secondToWait = 20;
            while (repositories.Count == 0 && countLoop < secondToWait)
            {
                Thread.Sleep(1000);
                countLoop++;
                repositories = RepoListView.Items;
            }
        }

        public void BrowseDestinationPath(string path)
        {
            ClickButton(BrowseDestinationPathButton);
            SearchCriteria searchCriteria = SearchCriteria.ByText("Select Destination Path");
            var selectDestinationWindow = this.WaitMdiChildAppears(searchCriteria, 10);
            var browseWindow = new BrowseDestinationPath(this, selectDestinationWindow);
            browseWindow.ChooseDestinationFolder(path);
            browseWindow.ClickSelectFolder();
        }

        public void TypeSearchCondition(string searchCondition)
        {
            SearchField.Enter(searchCondition);
        }

        public int CountOfRepositroyInList()
        {
            WaitListRemoteIsLoaded();
            return RepoListView.Items.Count;
        }

        public string GetTextOfFirstRepository()
        {
            WaitListRemoteIsLoaded();
            return RepoListView.Items.Count > 0 ? RepoListView.Items[0] : ""; // should we throw some Exception here or can we return just nothing, empty string?
        }
        #endregion
    }

    public class SSHKey
    {
        private Window sshkeyWindow;

        public SSHKey(Window sshkeyWindow)
        {
            this.sshkeyWindow = sshkeyWindow;
        }

        public Button YesButton => sshkeyWindow.Get<Button>(SearchCriteria.ByText("Yes"));

        public LocalTab clickNoButton()
        {
            YesButton.Click();
            Window sourceTreeWindow = Utils.FindNewWindow("SourceTree");
            return new LocalTab(sourceTreeWindow);
        }
    }

    public class BrowseDestinationPath
    {
        private readonly UIItemContainer _browsePath;
        private readonly InstallationWindow _installWindow;

        public BrowseDestinationPath(InstallationWindow installWindow, UIItemContainer browsePath)
        {
            this._installWindow = installWindow;
            this._browsePath = browsePath;
        }

        public TextBox FolderEditField => _browsePath.Get<TextBox>(SearchCriteria.ByAutomationId("1152"));
        public Button SelectFolder => _browsePath.Get<Button>(SearchCriteria.ByAutomationId("1"));

        public InstallationWindow ClickSelectFolder()
        {
            SelectFolder.Click();
            return _installWindow;
        }

        public void ChooseDestinationFolder(string path)
        {
            FolderEditField.Enter(path);
        }

    }

    public class IgnoreFileDialogWindow
    {
        private readonly UIItemContainer _dialogWindow;
        private readonly InstallationWindow _installWindow;

        public IgnoreFileDialogWindow(InstallationWindow installWindow, UIItemContainer dialogWindow)
        {
            this._installWindow = installWindow;
            this._dialogWindow = dialogWindow;
        }

        public Label TitleOfWindowLabel => _dialogWindow.Get<Label>(SearchCriteria.ByText("Login failed"));
        public Button YesButton => _dialogWindow.Get<Button>(SearchCriteria.ByText("_Yes"));
        public Button NoButton => _dialogWindow.Get<Button>(SearchCriteria.ByText("No"));

        public InstallationWindow ClickYesButton()
        {
            YesButton.Click();
            return _installWindow;
        }

        public InstallationWindow ClickCancelButton()
        {
            NoButton.Click();
            return _installWindow;
        }

        public string GetTitle()
        {
            return TitleOfWindowLabel.Text;
        }
    }

    public class ErrorDialogWindow
    {
        private readonly UIItemContainer _errorWindow;
        private readonly InstallationWindow _installationWindow;

        public ErrorDialogWindow(InstallationWindow installationWindow, UIItemContainer errorWindow)
        {
            this._installationWindow = installationWindow;
            this._errorWindow = errorWindow;
        }

        public Label TitleOfMessageError => _errorWindow.Get<Label>(SearchCriteria.ByText("Bad credentials"));
        public Label TitleOfWindowLabel => _errorWindow.Get<Label>(SearchCriteria.ByText("Login failed"));
        public Button CancelButton => _errorWindow.Get<Button>(SearchCriteria.ByText("Cancel"));

        public string GetTitleOfMessageError()
        {
            return TitleOfMessageError.Text;
        }

        public InstallationWindow ClickCancelButton()
        {
            CancelButton.Click();
            return _installationWindow;
        }

        public string GetTitle()
        {
            return TitleOfWindowLabel.Text;
        }
    }
}
