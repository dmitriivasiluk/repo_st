using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows.MenuFolder.ActionMenu
{
    public class ResolveConflict : ActionsMenu
    {
        public ResolveConflict(Window mainWindow) : base(mainWindow)
        {
        }

        public Menu UIElementSubMenu { get { return UIElementMenu.SubMenu("Resolve Conflicts"); } }

        public void ClickOperations(ResolveConflictOperations operation)
        {
            UIElementSubMenu.SubMenu(operation.Value).Click();
            UIItemContainer confirmDialog = MainWindow.MdiChild(SearchCriteria.ByText(operation.ConfirmDialogName));
            UIItemContainer OkButtonInConfirmation = MainWindow.MdiChild(SearchCriteria.ByText("OK"));
            OkButtonInConfirmation.Click();
        }
    }

    public struct ResolveConflictOperations
    {
        private ResolveConflictOperations(string value, string confirmDialogName) { Value = value; ConfirmDialogName = confirmDialogName; }
        public string Value { get; set; }
        public string ConfirmDialogName { get; set; } // Need to change when automationId would be implemented
        public static ResolveConflictOperations ResolveUsingMine { get { return new ResolveConflictOperations("Resolve Using 'Mine'", "Resolve conflict using one side?"); } }
        public static ResolveConflictOperations ResolveUsingTheirs { get { return new ResolveConflictOperations("Resolve Using 'Theirs'", "Resolve conflict using one side?"); } }
        public static ResolveConflictOperations RestartMerge { get { return new ResolveConflictOperations("Restart Merge", "Confirm Restart Merge"); } }
        public static ResolveConflictOperations MarkResolved { get { return new ResolveConflictOperations("Mark Resolved", "Confirm Mark Resolved"); } }
        public static ResolveConflictOperations MarkUnresolved { get { return new ResolveConflictOperations("Mark Unresolved", "Confirm Mark Unresolved"); } }
    }
}
