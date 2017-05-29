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
    public class AddSubmoduleWindow : GeneralWindow
    {
        public AddSubmoduleWindow(Window mainWindow) : base(mainWindow)
        {
        }
        public override void ValidateWindow()
        {
            // Need verify opened tab in this method, need implementation! If validation is fail, throw exception!
            Console.WriteLine("AddSubmoduleWindow opened");
        }

        #region UIItems
        //Automation IDs required
        public TextBox SourcePathTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByAutomationId("SourceTextBox"));
        public Button OKButton => MainWindow.Get<Button>(SearchCriteria.ByText("OK"));
        public Button CancelButton => MainWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
        public Button AdvancedOptions => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("HeaderSite"));
        public TextBox SourceBranch => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(3));
        public TextBox LocalRelativePath => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(2));
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

        public void LocalRelativePathTextboxFocus()
        {
            LocalRelativePath.Focus();
        }

        public bool IsOkButtonEnabled()
        {
            return OKButton.Enabled;
        }

        public void SetSourcePath(string value)
        {
            SourcePathTextbox.Text = value;
        }

        public bool GetValidationMessage(string text)
        {
            LocalRelativePath.Focus();

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
            public static string checkingSource = "Checking source...";
        }

        public NotAGitRepository SwitchToNotAGitRepositoryWindow()
        {
            SearchCriteria searchCriteria = SearchCriteria.ByText("Not a Git repository");
            var notAGitRepoWindow = this.WaitMdiChildAppears(searchCriteria, 10);
            return new NotAGitRepository(MainWindow, this, notAGitRepoWindow);
        }
        #endregion
    }
    public class NotAGitRepository
    {
        private AddSubmoduleWindow addSubmoduleWindow;
        private UIItemContainer notAGitRepositoryWindow;

        public NotAGitRepository(Window mainWindow, AddSubmoduleWindow addSubmoduleWindow, UIItemContainer notAGitRepositoryWindow)
        {
            this.addSubmoduleWindow = addSubmoduleWindow;
            this.notAGitRepositoryWindow = notAGitRepositoryWindow;
        }

        #region UIItems

        public Button CancelButton => notAGitRepositoryWindow.Get<Button>(SearchCriteria.ByText("Cancel"));

        public Label ErrorMessage
        {
            get
            {
                var controlElement = notAGitRepositoryWindow.GetElement(SearchCriteria.ByText("Submodules can only be git repositories.").AndControlType(ControlType.Text));
                return controlElement != null ? new Label(controlElement, notAGitRepositoryWindow.ActionListener) : null;
            }
        }
        #endregion

        #region Methods
        public AddSubmoduleWindow ClickCancelButton()
        {
            CancelButton.Click();
            return addSubmoduleWindow;
        }
        #endregion
    }
}