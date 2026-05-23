using System.Xml.Serialization;

namespace Hypah.Utility.Files.Serialization
{
    public class GenericXmlSerializer<T> where T : class, new()
    {
        public GenericXmlSerializer() { }

        public void SerializeXml(T obj, string filename)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, obj);
            }
        }

        public T? DeserializeXml(string filename) => DeserializeXml(filename, typeof(T)) as T;

        public object? DeserializeXml(string filename, Type type)
        {
            var serializer = new XmlSerializer(type);
            using (var reader = new StreamReader(filename))
            {
                return serializer.Deserialize(reader) as T;
            }
        }
    }
}
