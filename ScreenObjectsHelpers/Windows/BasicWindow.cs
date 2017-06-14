using ScreenObjectsHelpers.Windows.Repository;
using System;
using System.Threading;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace ScreenObjectsHelpers.Windows
{
    /// <summary>
    /// This is the base page from which all other pages inherit.
    /// It contains base methods that are used in all window like click on button, waits, get text etc.
    /// Also it has an abstract method that enforce all window verify that it is opened.
    /// </summary>
    public abstract class BasicWindow
    {

        public BasicWindow(Window mainWindow)
        {
            this.MainWindow = mainWindow;
        }

        #region UIItems
        public Button OKButton => MainWindow.Get<Button>(SearchCriteria.ByText("OK"));
        public Button CancelButton => MainWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
       
        #endregion

        #region Methods
        public Window MainWindow { get; private set; }

        public void ClickButton(Button button)
        {

            Thread.Sleep(500);
            if (button.Enabled && button.Visible)
            {
                button.Click();
            }
            else
            {
                throw new TimeoutException("Element is not enabled, can not perform action on element.");
            }
        }

        public UIItemContainer WaitMdiChildAppears(SearchCriteria searchCriteria, int secondsForWait)
        {
            int secondsPass = 0;
            UIItemContainer container = MainWindow.MdiChild(searchCriteria);
            while (container == null)
            {
                Thread.Sleep(1000);
                secondsPass++;
                container = MainWindow.MdiChild(searchCriteria);
                if (secondsPass > secondsForWait)
                {
                    throw new TimeoutException();
                }
            }
            return container;
        }

        public void SetComboboxValue(ComboBox combobox, string comboboxValue)
        {
            combobox.Select(comboboxValue);
        }

        public void SetTextboxContent(TextBox textbox, string content)
        {
            textbox.Focus();
            Thread.Sleep(50);
            textbox.SetValue(content);
        }

        public void CheckCheckbox(CheckBox checkbox)
        {
            if (!checkbox.Checked)
            {
                checkbox.Toggle();
            }
        }

        public void UncheckCheckbox(CheckBox checkbox)
        {
            if (checkbox.Checked)
            {
                checkbox.Toggle();
            }
        }

        public static T GetWithWait<T>(Window window, SearchCriteria searchCriteria, int rounds = 5) where T : UIItem
        {
            T result = null;
            for (int i = 0; i < rounds; i++)
            {
                try
                {
                    result = (T) window.Get(searchCriteria);
                    if (result != null && result.Visible)
                    {
                        return result;
                    }
                }
                catch (AutomationException)
                {
                    // empty
                }
            }
            return result;
        }

        public virtual bool IsOkButtonEnabled()
        {
            return OKButton.Enabled;
        }

        public RepositoryTab ClickButtonToGetRepository(Button button)
        {
            ClickButton(button);
            return new RepositoryTab(MainWindow);
        }
        #endregion
    }
}