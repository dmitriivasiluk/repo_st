using System.Windows.Automation;
using ScreenObjectsHelpers.Windows;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Menu.Repository
{
    public class FetchWindow : GeneralWindow
    {
        public FetchWindow(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIItems
        //AutomationID_required
        public CheckBox FetchFromAllRemotes => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Fetch from all remotes"));
        public CheckBox PruneTrackingBranches => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Prune tracking branches no longer present on remote(s)"));
        public CheckBox FetchAllTags => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Fetch all tags"));
        public ComboBox RemotePicker => MainWindow.Get<ComboBox>(SearchCriteria.ByControlType(ControlType.ComboBox).AndIndex(0));
        #endregion
    }

}
