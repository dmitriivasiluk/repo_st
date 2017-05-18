﻿using System;
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
        public Label WrongPathValidationMessage => MainWindow.Get<Label>(SearchCriteria.ByControlType(ControlType.Text).AndByText("No path / URL supplied"));
        public Label CorrectSourcePathValidationMessage => MainWindow.Get<Label>(SearchCriteria.ByControlType(ControlType.Text).AndByText("This is a Git repository"));
        public Button AdvancedOptions => MainWindow.Get<Button>(SearchCriteria.ByAutomationId("HeaderSite"));
        public TextBox SourceBranch => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(3));
        public TextBox LocalRelativePath => MainWindow.Get<TextBox>(SearchCriteria.ByControlType(ControlType.Edit).AndIndex(2));
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
        #endregion
    }
    public class NotAGitRepository
    {
        private UIItemContainer addSubmoduleWindow;
        private UIItemContainer notAGitRepositoryWindow;
        public NotAGitRepository(Window mainWindow, UIItemContainer addSubmoduleWindow, UIItemContainer notAGitRepositoryWindow)
        {
            this.addSubmoduleWindow = addSubmoduleWindow;
            this.notAGitRepositoryWindow = notAGitRepositoryWindow;
        }

        #region UIItems
        public Button CancelButton => notAGitRepositoryWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
        public TextBox ErrorMessage => notAGitRepositoryWindow.Get<TextBox>(SearchCriteria.ByText("Submodules can only be git repositories."));
        #endregion

        #region Methods
        public UIItemContainer ClickCancelButton()
        {
            CancelButton.Click();
            return addSubmoduleWindow;
        }
        #endregion
    }
}