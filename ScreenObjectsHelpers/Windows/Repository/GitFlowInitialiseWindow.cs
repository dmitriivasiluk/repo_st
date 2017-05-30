using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using System.Windows.Automation;
using ScreenObjectsHelpers.Helpers;

namespace ScreenObjectsHelpers.Windows.Repository
{
    public class GitFlowInitialiseWindow : GeneralWindow
    {
        public GitFlowInitialiseWindow(Window mainWindow) : base(mainWindow)
        {
        }

        #region UIItems
        public TextBox ProductionBranchTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(1));
        public TextBox DevelopmentBranchTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(2));
        public TextBox FeatureBranchTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(3));
        public TextBox ReleaseBranchTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(4));
        public TextBox HotfixBranchTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(5));
        public TextBox VersionTagTextbox => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(6));
        public Button OKButton => MainWindow.Get<Button>(SearchCriteria.ByText("OK"));
        public Button CancelButton => MainWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
        public Button UseDefaultsButton => MainWindow.Get<Button>(SearchCriteria.ByText("Use Defaults"));
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

        public void ClickUseDefaultsButton()
        {
            ClickButton(UseDefaultsButton);
        }

        public bool IsVersionTagEmpty()
        {
            return VersionTagTextbox.Text.Equals("");
        }

        public void SetTextboxContent(TextBox textbox, string content)
        {
            textbox.Focus();
            Utils.ThreadWait(50);
            textbox.SetValue(content);            
        }

        public void SetAllTextboxes(string testString)
        {
            SetTextboxContent(ProductionBranchTextbox, testString);
            SetTextboxContent(DevelopmentBranchTextbox, testString);
            SetTextboxContent(FeatureBranchTextbox, testString);
            SetTextboxContent(ReleaseBranchTextbox, testString);
            SetTextboxContent(HotfixBranchTextbox, testString);
            SetTextboxContent(VersionTagTextbox, testString);
        }
        #endregion
    }
}
