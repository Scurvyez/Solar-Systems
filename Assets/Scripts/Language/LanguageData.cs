using System.Collections.Generic;
using System.Xml.Serialization;

namespace Language
{
    [XmlRoot("language")]
    public class LanguageData
    {
        [XmlElement("string")] public List<LocalizedString> Strings { get; set; }
    }

    public class LocalizedString
    {
        [XmlAttribute("key")] public string Key { get; set; }
        [XmlText] public string Value { get; set; }
    }
}