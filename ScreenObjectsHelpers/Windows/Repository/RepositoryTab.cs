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
        public UIItem TabTextGit => MainWindow.Get<UIItem>(SearchCriteria.ByText(ConstantsList.testGitRepoBookmarkName));
        public UIItem TabTextHg => MainWindow.Get<UIItem>(SearchCriteria.ByText(ConstantsList.testHgRepoBookmarkName));
        //TO DO: running tests against SourceTree beta
        //public Button CommitButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarCommit"));
        //public Button PushButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarPush"));
        //public Button PullButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarPull"));
        //public Button FetchButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarFetch"));
        //public Button BranchButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarBranch"));
        //public Button MergeButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarMerge"));
        //public Button StashButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarStash"));
        //public Button DiscardButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarDiscard"));
        //public Button TagButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarTag"));
        //public Button GitFlowButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarFlow"));
        //public Button TerminalButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarTerminal"));
        //public Button ExplorerButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarExplorer"));
        //public Button SettingsButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarSettings"));
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
