using System;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using ScreenObjectsHelpers.Helpers;
using System.Windows.Automation;
using TestStack.White.InputDevices;

namespace ScreenObjectsHelpers.Windows.Repository
{
    public class AddLinkSubtreeWindow : GeneralWindow
    {
        public AddLinkSubtreeWindow(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIItems
        //Automation IDs required
        public TextBox SourcePathTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(1));
        public Button AdvancedOptionsButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("HeaderSite"));
        public TextBox BranchCommitTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(2));
        public TextBox LocalRelativePathTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(3));
        public CheckBox SquashCommitsCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Squash commits?"));
        
        #endregion

        #region Methods
        public bool GetValidationMessage(string text)
        {
            LocalRelativePathTextbox.Focus();

            if (GetWithWait<Label>(MainWindow, SearchCriteria.ByText(text)) == null)
            {
                return false;
            }
            return true;
        }

        public struct LinkValidationMessage
        {
            public static string noPathSupplied = "No path / URL supplied";
            public static string notValidPath = "This is not a valid source path / URL";
            public static string gitRepoType = "This is a Git repository";
            public static string mercurialRepoType = "This is a Mercurial repository";
            public static string checkingSource = "Checking source...";
        }

        #endregion
    }
}