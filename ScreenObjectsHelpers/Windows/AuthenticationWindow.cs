using ScreenObjectsHelpers.Helpers;
using System.Threading;
using TestStack.White.InputDevices;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;
using static TestStack.White.WindowsAPI.KeyboardInput;

namespace ScreenObjectsHelpers.Windows
{
    public class AuthenticationWindow : BasicWindow
    {
        private readonly Window authorizationWindow;

        public AuthenticationWindow(Window mainWindow, Window authorizationWindow) : base(mainWindow)
        {
            this.authorizationWindow = authorizationWindow;
        }

        #region UIElements
        public Button LogInByGoogle => authorizationWindow.Get<Button>(SearchCriteria.ByText("Log in with Google"));
        public TextBox EmailTextBox => authorizationWindow.Get<TextBox>(SearchCriteria.ByText("Email"));
        public TextBox PasswordTextBox => authorizationWindow.Get<TextBox>(SearchCriteria.ByText("Password"));
        public Button NextButton => authorizationWindow.Get<Button>(SearchCriteria.ByText("NextLog inLoading"));
        #endregion

        #region Methods
        public InstallationWindow SignIn(string loginEmail, string password)
        {
            // It is workaround method, because I can't catch TextEdit fields from authorization window. Need time to resolve this issue
            // Need to reimplement in future. 
            Thread.Sleep(2000);
            Keyboard.Instance.Enter(loginEmail);
            ClickButton(NextButton);
            Thread.Sleep(2000);
            Keyboard.Instance.Enter(password);
            ClickButton(NextButton);
            Thread.Sleep(1000);
            Keyboard.Instance.PressSpecialKey(SpecialKeys.RETURN);
            Thread.Sleep(8000);
            return new InstallationWindow(MainWindow);
        }
        #endregion

    }
}
