using System.IO;
using System.Xml.Linq;

namespace AutomationTestsSolution.Helpers
{
    public class OpenTabsXml
    {
        private string _location;
        private XDocument _doc;
        private XElement _root;

        public OpenTabsXml(string location)
        {
            _location = location;
            if (File.Exists(_location))
            {
                _doc = XDocument.Load(_location);
            }
            else
            {
                XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
                XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                _root = new XElement("ArrayOfString");
                _doc = new XDocument(_root);
            }
        }

        public OpenTabsXml SetOpenTab(string path)
        {
            var stringElement = new XElement("string");
            stringElement.SetValue(path);
            _root.Add(stringElement);
            return this;
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