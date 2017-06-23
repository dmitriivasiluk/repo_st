using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Options
{
    public class GitTab : OptionsWindow
    {

        public GitTab(Window mainWindow, UIItemContainer optionsWindow) : base(mainWindow, optionsWindow)
        {
        }

        #region UI Elements
        public override UIItem UIElementTab => OptionsWindowContainer.Get<UIItem>(SearchCriteria.ByText("Git"));

        public Button UseEmbededGitButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Use Embedded Git"));

        public Button UpdateEmbededGitButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Update Embedded Git"));

        private Button SystemGitButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Use System Git"));

        private Button SystemGitButtonSystemGitButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Use System Git"));            

        #endregion

        #region Methods
        public void UseEmbeddedGit()
        {
            if (UseEmbededGitButton.Enabled)
            {
                this.ClickButton(UseEmbededGitButton);
            }
        }
        public void UseSystemGitButton()
        {
            this.ClickButton(SystemGitButtonSystemGitButton);
        }

        public bool IsUseEmbeddedGitEnabled()
        {
            return UseEmbededGitButton.Enabled;
        }

        public bool IsUseSystemGitEnabled()
        {
            return SystemGitButton.Enabled;
        }

        public string VersionText()
        {
            return OptionsWindowContainer.HelpText;
        }

        public void UpdateEmbededGitVersion()
        {
            this.ClickButton(UpdateEmbededGitButton);
        }
        #endregion
    }
}


