using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Options
{
    public class GeneralTab : OptionsWindow
    {
        private readonly UIItemContainer generalTab;
        public GeneralTab(Window mainWindow, UIItemContainer optionsWindow) : base(mainWindow, optionsWindow)
        {
            generalTab = optionsWindow;
        }

        #region UI Elements
        public override UIItem UIElementTab => OptionsWindowContainer.Get<UIItem>(SearchCriteria.ByText("General"));

        #region Checkboxes
        public CheckBox AllowModifyGlobalConfigCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Allow SourceTree to modify your global Git and Mercurial config files"));
        public CheckBox UseVersionForUriCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Use this version of SourceTree for URI association"));
        public CheckBox AutoStartSshCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Automatically start SSH agent when SourceTree opens"));       
        public CheckBox KeepBackupsCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Keep backups on destructive operations"));        
        public CheckBox RefreshAutomaticallyCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Refresh automatically when files change"));       
        public CheckBox CheckRemotesCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Check default remotes for updates every"));
        public CheckBox ReopenReposAtStartupCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Re-open repository tabs at startup"));
        public CheckBox AlwaysDisplayConsoleCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Always display full console output"));
        public CheckBox DisplayColumnGuideAtCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Display a column guide in commit message at "));
        public CheckBox SpellCheckCommitMessagesCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Spell check commit messages"));
        public CheckBox AutoCleanUpDictionaryCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Automatically clean up custom dictionary files."));
        public CheckBox PushToDefaultRemoteCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Push to default/origin remote when committing"));
        public CheckBox UseFixedFontCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Use fixed-width font for commit messages"));
        public CheckBox AfterCommitStayInDialogCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("After committing, stay in commit dialog if there are still pending changes"));           
        public CheckBox LoadAvatarFromGravatarCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Load avatar images from gravatar.com"));
        public CheckBox DontPromptNewInstallationCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Don't prompt about the new SourceTree installation."));
        public CheckBox HelpImproveSendDataUsageCheckBox => generalTab.Get<CheckBox>(SearchCriteria.ByText("Help improve SourceTree by sending data about your usage"));
        #endregion

        // AutomationID_required
        /*
        #region TextBoxes
        public TextBox FullNameTextBox => generalTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));
        public TextBox EmailTextBox => generalTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));
        public TextBox SshKeyTextBox => generalTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));
        public TextBox ProjectFolderTextBox => generalTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));
        public TextBox CheckRemotesTextBox => generalTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));
        public TextBox DisplayColumnGuideAtTextBox => generalTab.Get<TextBox>(SearchCriteria.ByAutomationId(""));
        #endregion

        #region Buttons
        public Button EllipsisSshKeyButton => generalTab.Get<Button>(SearchCriteria.ByAutomationId(""));
        public Button EllipsisProjectFolderButton => generalTab.Get<Button>(SearchCriteria.ByAutomationId(""));
        #endregion        

        #region ComboBoxes
        public ComboBox SshClientComboBox => generalTab.Get<ComboBox>(SearchCriteria.ByAutomationId(""));
        public ComboBox LanguageComboBox => generalTab.Get<ComboBox>(SearchCriteria.ByAutomationId(""));
        public ComboBox DefaultTextEncodingComboBox => generalTab.Get<ComboBox>(SearchCriteria.ByAutomationId(""));
        public ComboBox SpellCheckLanguageComboBox => generalTab.Get<ComboBox>(SearchCriteria.ByAutomationId(""));
        #endregion
        */

        #endregion

        #region Methods

        #endregion
    }
}


