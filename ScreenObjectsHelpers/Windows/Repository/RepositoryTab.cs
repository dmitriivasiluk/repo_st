using System;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;

namespace ScreenObjectsHelpers.Windows.Repository
{
    public class RepositoryTab : GeneralWindow
    {
        public RepositoryTab(Window mainWindow) : base(mainWindow)
        {
        }
        public override void ValidateWindow()
        {
            // Need verify opened tab in this method, need implementation! If validation is fail, throw exception!
            Console.WriteLine("WAIT FOR OPENING TAB");
        }
        #region UIItems
        public Button CommitButton => MainWindow.Get<Button>(SearchCriteria.ByText("Commit"));
        public Button PushButton => MainWindow.Get<Button>(SearchCriteria.ByText("Push"));
        public Button PullButton => MainWindow.Get<Button>(SearchCriteria.ByText("Pull"));
        public Button FetchButton => MainWindow.Get<Button>(SearchCriteria.ByText("Fetch"));
        public Button BranchButton => MainWindow.Get<Button>(SearchCriteria.ByText("Branch"));
        public Button MergeButton => MainWindow.Get<Button>(SearchCriteria.ByText("Merge"));
        public Button StashButton => MainWindow.Get<Button>(SearchCriteria.ByText("Stash"));
        public Button DiscardButton => MainWindow.Get<Button>(SearchCriteria.ByText("Discard"));
        public Button TagButton => MainWindow.Get<Button>(SearchCriteria.ByText("Tag"));
        public Button GitFlowButton => MainWindow.Get<Button>(SearchCriteria.ByText("Git Flow"));
        public Button TerminalButton => MainWindow.Get<Button>(SearchCriteria.ByText("Terminal"));
        public Button ExplorerButton => MainWindow.Get<Button>(SearchCriteria.ByText("Explorer"));
        public Button SettingsButton => MainWindow.Get<Button>(SearchCriteria.ByText("Settings"));
        #endregion

        #region Methods
        public GitFlowInitialiseWindow ClickGitFlowButton()
        {
            ClickButton(GitFlowButton);
            return new GitFlowInitialiseWindow(MainWindow);
        }
        #endregion
    }
}
