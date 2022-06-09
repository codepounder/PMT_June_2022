using System.Xml.Serialization;

namespace Ferrara.Compass.Abstractions.Models
{
    public class GlobalLookupField
    {
        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string LookupFieldValue { get; set; }

        [XmlAttribute]
        public bool Active { get; set; }
    }
}
