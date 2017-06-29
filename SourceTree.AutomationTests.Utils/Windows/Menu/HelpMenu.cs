using ScreenObjectsHelpers.Windows;
using SourceTree.AutomationTests.Utils.Windows.MenuFolder;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace SourceTree.AutomationTests.Utils.Windows.Menu
{
    public class HelpMenu : MenuBar
    {
        private const string About = "About SourceTree";
		
        public HelpMenu(Window mainWindow) : base(mainWindow)
        {
        }
        public override TestStack.White.UIItems.MenuItems.Menu UIElementMenu { get { return MainWindow.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByText("Help")); } }
        //TODO: running tests against SourceTree beta
        //public override Menu UIElementMenu { get {  return MainWindow.Get<Menu>(SearchCriteria.ByAutomationId("MenuHelp")); } }

        #region Methods        
        public void ClickOperations(OperationsHelp operation)
        {
            UIElementMenu.SubMenu(operation.Value).Click();
        }
		
        public AboutWindow OpenAbout()
        {
            UIElementMenu.SubMenu(About).Click();
            var aboutWindow = MainWindow.ModalWindow(SearchCriteria.ByText("About"));
            
            return new AboutWindow(MainWindow, aboutWindow);
        }
        #endregion        
    }

    public struct OperationsHelp
    {
        private OperationsHelp(string value) { Value = value; }
        public string Value { get; set; }        
        public static OperationsHelp GetStarted => new OperationsHelp("Get Started With SourceTree"); 
        public static OperationsHelp SupportWebsite => new OperationsHelp("SourceTree Support Website");        
        public static OperationsHelp SourceTreeWebsite => new OperationsHelp("SourceTree Website"); 
        public static OperationsHelp ReleaseNotes => new OperationsHelp("Release Notes"); 
        public static OperationsHelp GetGitRight => new OperationsHelp("Get Git Right"); 
        
    }
}
