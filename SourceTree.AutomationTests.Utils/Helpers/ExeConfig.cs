using System.Xml.Linq;
using System.Xml.XPath;

namespace SourceTree.AutomationTests.Utils.Helpers
{
    public class ExeConfig
    {
        private const string APPLICATION_SETTINGS = "applicationSettings";
        private const string USER_SETTINGS = "userSettings";

        private string _location;
        private XDocument _doc;

        public ExeConfig(string location)
        {
            _location = location;
            _doc = XDocument.Load(_location);
        }

        public void SetUserSetting(string name, string value)
        {
            if (!UserSettingExists(name))
            {
                AddUserSetting(name, value);
            }
            else
            {
                UpdateUserSetting(name, value);
            }
        }

        public void SetApplicationSetting(string name, string value)
        {
            if (!ApplicationSettingExists(name))
            {
                AddApplicationSetting(name, value);
            }
            else
            {
                UpdateApplicationSetting(name, value);
            }
        }

        private bool ApplicationSettingExists(string name)
        {
            return SettingExists(APPLICATION_SETTINGS, name);
        }

        private bool UserSettingExists(string name)
        {
            return SettingExists(USER_SETTINGS, name);
        }

        private bool SettingExists(string group, string name)
        {
            return _doc.XPathSelectElement(
                       $"/configuration/{group}/SourceTree.Properties.Settings/setting[@name = '{name}']/value") !=
                   null;
        }

        private void UpdateApplicationSetting(string name, string value)
        {
            UpdateSetting(APPLICATION_SETTINGS, name, value);
        }

        private void UpdateUserSetting(string name, string value)
        {
            UpdateSetting(USER_SETTINGS, name, value);
        }

        private void UpdateSetting(string group, string name, string value)
        {
            _doc.XPathSelectElement($"/configuration/{group}/SourceTree.Properties.Settings/setting[@name = '{name}']/value")
                .SetValue(value);
        }

        private void AddUserSetting(string name, string value)
        {
            AddSetting(USER_SETTINGS, name, value);
        }

        private void AddApplicationSetting(string name, string value)
        {
            AddSetting(APPLICATION_SETTINGS, name, value);
        }

        private void AddSetting(string group, string name, string value)
        {
            XElement setting = new XElement("setting");
            setting.Add(new XAttribute("name", name));
            setting.Add(new XAttribute("serializeAs", "String"));
            setting.Add(new XElement("value", value));
            _doc.XPathSelectElement($"/configuration/{group}/SourceTree.Properties.Settings").Add(setting);
        }
        public void Save()
        {
            if (_doc != null
                && !string.IsNullOrWhiteSpace(_location))
            {
                _doc.Save(_location);
            }
        }
    }
}