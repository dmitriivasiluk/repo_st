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
        public override void ValidateWindow()
        {
            // Need verify opened tab in this method, need implementation! If validation is fail, throw exception!
            Console.WriteLine("AddLinkSubtreeWindow opened");
        }

        #region UIItems
        //Automation IDs required
        public TextBox SourcePathTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(0));
        public Button AdvancedOptionsButton => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("HeaderSite"));
        public TextBox BranchCommitTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(1));
        public TextBox LocalRelativePathTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(2));
        public CheckBox SquashCommitsCheckbox => MainWindow.Get<CheckBox>(SearchCriteria.ByText("Squash commits?"));
        public Button OKButton => MainWindow.Get<Button>(SearchCriteria.ByText("OK"));
        public Button CancelButton => MainWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
        #endregion

        #region Methods
        public RepositoryTab ClickOkButton()
        {
            ClickButton(OKButton);
            return new RepositoryTab(MainWindow);
        }

        public RepositoryTab ClickCancelButton()
        {
            ClickButton(CancelButton);
            return new RepositoryTab(MainWindow);
        }

        public bool IsOkButtonEnabled()
        {
            return OKButton.Enabled;
        }

        public void SetSourcePath(string value)
        {
            SourcePathTextbox.Text = value;
        }

        public void SetLocalRelativePath(string value)
        {
            LocalRelativePathTextbox.Text = value;
        }

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