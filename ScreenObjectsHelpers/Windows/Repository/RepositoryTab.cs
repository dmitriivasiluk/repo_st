using System;
using TestStack.White.UIItems.TabItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;
using System.Threading;

namespace ScreenObjectsHelpers.Windows.Repository
{
    public class RepositoryTab : GeneralWindow
    {
        public RepositoryTab(Window mainWindow) : base(mainWindow)
        {
        }
        
        #region UIItems
        public Thumb TabThumb => MainWindow.Get<Thumb>(SearchCriteria.ByAutomationId("PART_Thumb"));
        //Any element contains specified text pattern
        public UIItem TabTextGit => MainWindow.Get<UIItem>(SearchCriteria.ByText(ConstantsList.testGitRepoBookmarkName));
        public UIItem TabTextHg => MainWindow.Get<UIItem>(SearchCriteria.ByText(ConstantsList.testHgRepoBookmarkName));
        public Button CommitButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarCommit"));
        public Button PushButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarPush"));
        public Button PullButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarPull"));
        public Button FetchButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarFetch"));
        public Button BranchButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarBranch"));
        public Button MergeButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarMerge"));
        public Button StashButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarStash"));
        public Button DiscardButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarDiscard"));
        public Button TagButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarTag"));
        public Button GitFlowButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarFlow"));
        public Button TerminalButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarTerminal"));
        public Button ExplorerButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarExplorer"));
        public Button SettingsButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("ToolbarSettings"));
        #endregion

        #region Methods
        public GitFlowInitialiseWindow ClickGitFlowButton()
        {
            Thread.Sleep(3000);
            ClickButton(GitFlowButton);
            return new GitFlowInitialiseWindow(MainWindow);
        }

        public StashShelveWindow ClickStashButton()
        {
            Thread.Sleep(3000);
            ClickButton(StashButton);
            return new StashShelveWindow(MainWindow);
        }

        public FetchWindow ClickFetchButton()
        {
            Thread.Sleep(3000);
            ClickButton(FetchButton);
            return new FetchWindow(MainWindow);
        }

        public bool IsRepoTabTitledWithText(string text)
        {
            if (GetWithWait<WPFLabel>(MainWindow, SearchCriteria.ByText(text)) == null)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
