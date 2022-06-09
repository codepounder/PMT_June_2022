using System.IO;
using System.Xml.Serialization;

namespace Ferrara.Compass.Abstractions.Helper
{
    public class XmlSerializationHelper
    {
        public static void Serialize<T>(string fileName, T obj)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var wr = new StreamWriter(fileName))
            {
                xs.Serialize(wr, obj);
            }
        }

        public static T Deserialize<T>(string fileName)
        {
            var xs = new XmlSerializer(typeof(T));
            using (var rd = new StreamReader(fileName))
            {
                return (T)xs.Deserialize(rd);
            }
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            var xs = new XmlSerializer(typeof(T));
            var buffer = System.Text.Encoding.UTF8.GetBytes(xml);
            var rd = new MemoryStream(buffer);
            using (rd)
            {
                return (T)xs.Deserialize(rd);
            }
        }
    }
}
