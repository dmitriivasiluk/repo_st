using System.IO;
using System.Xml.Linq;

namespace AutomationTestsSolution.Helpers
{
    public class OpenTabsXml
    {
        private string _location;
        private XDocument _doc;

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
                _doc = new XDocument(
                    new XElement("ArrayOfString"));
            }
        }

        public OpenTabsXml SetOpenTab(string path)
        {
            var stringElement = new XElement("string");
            stringElement.SetValue(path);
            _doc.Add(stringElement);
            return this;
        }
    }
}