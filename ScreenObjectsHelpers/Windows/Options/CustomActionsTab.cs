using System;
using System.Windows.Automation;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.Options
{
    public class CustomActionsTab : OptionsWindow
    {
        private readonly UIItemContainer customActionsTab;

        public CustomActionsTab(Window mainWindow, UIItemContainer optionsWindow) : base(mainWindow, optionsWindow)
        {
            customActionsTab = optionsWindow;
        }

        public override void ValidateWindow()
        {
            // Need verify opened tab in this method, need implementation! If validation is fail, throw exception!
            Console.WriteLine("WAIT FOR OPENING TAB");
        }

        #region UIItems
        public override UIItem UIElementTab
        {
            get
            {
                return OptionsWindowContainer.Get<UIItem>(SearchCriteria.ByText("Custom Actions"));
            }
        }

        public Button AddCustomActionButton => customActionsTab.Get<Button>(SearchCriteria.ByText("Add"));
        public Button EditCustomActionButton => customActionsTab.Get<Button>(SearchCriteria.ByText("Edit"));
        public Button DeleteCustomActionButton => customActionsTab.Get<Button>(SearchCriteria.ByText("Delete"));
        public ListView AllCustomActions => customActionsTab.Get<ListView>(SearchCriteria.ByClassName("ListView"));
        public ListViewRow FirstCustomAction => customActionsTab.Get<ListViewRow>(SearchCriteria.ByClassName("ListViewItem"));
        #endregion

        #region Methods 
        public EditCustomActionWindow ClickAddCustomActionButton()
        {
            AddCustomActionButton.Click();
            var editCustomActionsWindow = MainWindow.MdiChild(SearchCriteria.ByText("Edit Custom Action"));
            return new EditCustomActionWindow(MainWindow, customActionsTab, editCustomActionsWindow);
        }
        public EditCustomActionWindow ClickEditCustomActionButton()
        {
            FirstCustomAction.Select();
            EditCustomActionButton.Click();
            var editCustomActionsWindow = MainWindow.MdiChild(SearchCriteria.ByText("Edit Custom Action"));
            return new EditCustomActionWindow(MainWindow, customActionsTab, editCustomActionsWindow);
        }
        public void ClickDeleteCustomActionButton()
        {
            FirstCustomAction.Select();
            DeleteCustomActionButton.Click();
        }
        public bool IsMenuCaptionExists(string condition)
        {
            for (int i = 0; i < AllCustomActions.Items.Count; i++)
            {
                if (AllCustomActions.Items[i].Contains(condition))
                {
                    AllCustomActions.Select(i);
                    return true;
                }
            }
            return false;
        }
        public bool IsEditCustomActionButtonEnabled()
        {
            return EditCustomActionButton.Enabled;
        }
        public bool IsDeleteCustomActionButtonEnabled()
        {
            return DeleteCustomActionButton.Enabled;
        }
        public ConfirmDeletionDialogWindow SwitchToConfirmDeletionDialogWindow()
        {
            SearchCriteria searchCriteria = SearchCriteria.ByAutomationId("window");
            var confirmDeletionDialog = this.WaitMdiChildAppears(searchCriteria, 10);
            return new ConfirmDeletionDialogWindow(customActionsTab, confirmDeletionDialog);
        }
        #endregion
    }

    public class ConfirmDeletionDialogWindow
    {
        private readonly UIItemContainer confirmDeletionWindow;
        private readonly UIItemContainer customActionsTab;

        public ConfirmDeletionDialogWindow(UIItemContainer customActionsTab, UIItemContainer confirmDeletionWindow)
        {
            this.customActionsTab = customActionsTab;
            this.confirmDeletionWindow = confirmDeletionWindow;
        }
        #region UIItems
        public Button CancelButton => confirmDeletionWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
        public Button OkButton => confirmDeletionWindow.Get<Button>(SearchCriteria.ByText("OK"));
        #endregion

        #region Methods
        public UIItemContainer ClickCancelButton()
        {
            CancelButton.Click();
            return customActionsTab;
        }

        public UIItemContainer ClickOkButton()
        {
            OkButton.Click();
            return customActionsTab;
        }
        #endregion
    }
}