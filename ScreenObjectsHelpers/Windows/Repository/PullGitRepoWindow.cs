using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using System.Windows.Automation;
using TestStack.White.UIItems.ListBoxItems;

namespace ScreenObjectsHelpers.Windows.Repository
{
    public class PullGitRepoWindow : GeneralWindow
    {
        public PullGitRepoWindow(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIItems
        //AutomationID_required
        public ComboBox PullFromRemoteCombobox => MainWindow.Get<ComboBox>(SearchCriteria.ByAutomationId("RemoteNameCombo"));

        public TextBox RemoteRepoPathTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(1));

        public ComboBox RemoteBranchToPullCombobox => MainWindow.Get<ComboBox>(SearchCriteria.ByControlType(ControlType.ComboBox).AndIndex(1));

        public CheckBox CommitMergedChangesImmediatelyCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Commit merged changes immediately"));

        public CheckBox IncludeMessagesFromCommitBeingMergedInMergeCommitCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Include messages from commits being merged in merge commit"));

        public CheckBox CreateANewCommitCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Create a new commit even if fast-forward is possible"));

        public CheckBox RebaseInsteadOfMergeCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Rebase instead of merge (WARNING: make sure you haven't pushed your changes)"));

        public Button RefreshButton => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Refresh"));
        #endregion
    }
}
