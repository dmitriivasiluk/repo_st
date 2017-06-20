using System;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Options
{
    public class MercurialTab : OptionsWindow
    {
        public MercurialTab(Window mainWindow, UIItemContainer optionsWindow) : base(mainWindow, optionsWindow)
        {
        }


        #region UIElements        
        public override UIItem UIElementTab => OptionsWindowContainer.Get<UIItem>(SearchCriteria.ByText("Mercurial"));
            
        public Button UseEmbeddedMercurialButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Use Embedded Mercurial"));

        public Button UseSystemMercurialButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Use System Mercurial"));

        public Button UpdateEmbeddedMercurialButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Update Embedded Mercurial"));

        public Button OkButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("OK"));

        public Button EnableMercurialSupportButton => OptionsWindowContainer.Get<Button>(SearchCriteria.ByText("Enable Mercurial Support"));

        #endregion

        #region Methods
        public Boolean IsMercurialInstalled()
        {
            if (EnableMercurialSupportButton.Enabled)
            {
                return false;
            }
            return true;
        }

        public void UseEmbeddedMercurial()
        {
            if (UseEmbeddedMercurialButton.Enabled)
            {
                this.ClickButton(UseEmbeddedMercurialButton);
            }
        }

        public void UseSystemMercurial()
        {
            if (UseSystemMercurialButton.Enabled)
            {
                this.ClickButton(UseSystemMercurialButton);
            }
        }        

        /*
        public void UpdateEmbeddedMercurial()
        {
            if (UpdateEmbeddedMercurialButton.Enabled)
            {
                this.ClickButton(UpdateEmbeddedMercurialButton);
            }
        } */

        public DownloadHgWindow UpdateEmbeddedMercurial()
        {
            if (UpdateEmbeddedMercurialButton.Enabled)
            {
                this.ClickButton(UpdateEmbeddedMercurialButton);                
                var downloadHgWindow = MainWindow.MdiChild(SearchCriteria.ByText("Download Embedded HG"));
                return new DownloadHgWindow(MainWindow, downloadHgWindow);
            }
            else
            {
                // TODO discuss how to process this case
                return null;
            }            
        }        

        public bool IsUseEmbeddedMercurialEnabled()
        {
            return UseEmbeddedMercurialButton.Enabled;
        }

        public bool IsUseSystemMercurialEnabled()
        {
            return UseSystemMercurialButton.Enabled;
        }

        public bool IsUpdateEmbeddedMercurialEnabled()
        {
            return UpdateEmbeddedMercurialButton.Enabled;
        }

        public string VersionText()
        {
            return OptionsWindowContainer.HelpText;
        }
        #endregion
    }
}