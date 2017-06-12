using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Repository
{
    public class StashShelveWindow : GeneralWindow
    {
        public StashShelveWindow(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIItems
        //AutomationID_required
        public TextBox FetchFromAllRemotesTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("TextField"));
        public CheckBox PruneTrackingBranchesCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Keep staged changes"));

        #endregion
    }

}
