using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace LocalizationService
{
    //public class XmlResourceReader : CombinedResourceReader
    //{
    //    private readonly string _xmlFilePath;

    //    public XmlResourceReader(List<IResourceReader> resourceReaders, string xmlFilePath) : base(resourceReaders)
    //    {
    //        _xmlFilePath = xmlFilePath;
    //    }

    //    public override IDictionaryEnumerator GetEnumerator()
    //    {
    //        var xmlReader = XmlReader.Create(_xmlFilePath);

    //        while (xmlReader.Read())
    //        {
    //            if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "entry")
    //            {
    //                string key = xmlReader.GetAttribute("key");
    //                string value = xmlReader.ReadElementContentAsString();

    //                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
    //                {
    //                    ((Dictionary<string, string>)base.GetEnumerator().Current).Add(key, value);
    //                }
    //            }
    //        }
    //        return base.GetEnumerator();
    //    }
    //}

    //public class XmlResourceReader : CombinedResourceReader
    //{
    //    public XmlResourceReader(string filename)
    //    {
    //        var reader = XmlReader.Create(filename);
    //        var xmlResourceReader = new ResXResourceReader(reader);
    //        var dictionary = new Dictionary<string, object>();
    //        foreach (DictionaryEntry entry in xmlResourceReader)
    //        {
    //            dictionary.Add((string)entry.Key, entry.Value);
    //        }
    //        xmlResourceReader.Close();
    //        AddResourceData(dictionary);
    //    }
    //}

    public class XmlResourceReader : CombinedResourceReader
    {
        private readonly string _fileName;

        public XmlResourceReader(string fileName)
        {
            var xmlReader = XmlReader.Create(fileName);
            var resxReader = new ResXResourceReader(xmlReader);
            AddReader(resxReader);
        }

        public override IDictionaryEnumerator GetEnumerator()
        {
            var combinedDictionary = new Dictionary<string, string>();
            foreach (var reader in _readers)
            {
                var resxReader = reader as ResXResourceReader;
                if (resxReader != null)
                {
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        combinedDictionary[entry.Key.ToString()] = entry.Value.ToString();
                    }
                }
                else if (reader is XmlResourceReader)
                {
                    var xmlReader = new XmlDocument();
                    xmlReader.Load(_fileName);
                    var namespaceManager = new XmlNamespaceManager(xmlReader.NameTable);
                    var nodes = xmlReader.SelectNodes("//data", namespaceManager);
                    foreach (XmlNode node in nodes)
                    {
                        var nameAttribute = node.Attributes["name"];
                        if (nameAttribute != null)
                        {
                            var name = nameAttribute.Value;
                            if (!combinedDictionary.ContainsKey(name))
                            {
                                var valueNode = node.SelectSingleNode("value");
                                if (valueNode != null)
                                {
                                    combinedDictionary[name] = valueNode.InnerText;
                                }
                            }
                        }
                    }
                }
            }

            return new DictionaryEnumerator(combinedDictionary);
        }
    }
}
