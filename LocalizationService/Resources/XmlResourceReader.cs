using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LocalizationService
{
    public class XmlResourceReader : CombinedResourceReader
    {
        private readonly string _xmlFilePath;

        public XmlResourceReader(List<IResourceReader> resourceReaders, string xmlFilePath) : base(resourceReaders)
        {
            _xmlFilePath = xmlFilePath;
        }

        public override IDictionaryEnumerator GetEnumerator()
        {
            var xmlReader = XmlReader.Create(_xmlFilePath);

            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "entry")
                {
                    string key = xmlReader.GetAttribute("key");
                    string value = xmlReader.ReadElementContentAsString();

                    if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                    {
                        ((Dictionary<string, string>)base.GetEnumerator().Current).Add(key, value);
                    }
                }
            }
            return base.GetEnumerator();
        }
    }
}
