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
    public class GitFlowInitialiseWindow : GeneralWindow
    {
        public GitFlowInitialiseWindow(Window mainWindow) : base(mainWindow)
        {
        }
        public override void ValidateWindow()
        {
            // Need verify opened tab in this method, need implementation! If validation is fail, throw exception!
            Console.WriteLine("WAIT FOR OPENING TAB");
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
        public void ClickOkButton()
        {
            OKButton.Click();
        }
        public RepositoryTab ClickCancelButton()
        {
            CancelButton.Click();
            return new RepositoryTab(MainWindow);
        }
        public void ClickUseDefaultsButton()
        {
            UseDefaultsButton.Click();
        }
        public bool TextboxDefaultContent(TextBox textbox, string expectedContent)
        {
            return textbox.Text.Equals(expectedContent);
        }
        public bool IsVersionTagEmpty()
        {
            return VersionTagTextbox.Text.Equals("");
        }
        public void SetTextboxContent(TextBox textbox, string content)
        {
            textbox.Focus();
            Keyboard.Instance.Enter(content);
        }
        public bool IsDefaultBranchNameCorrect(TextBox textbox, string condition)
        {
            if (textbox.Text == condition)
                return true;
            else
                return false;
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
