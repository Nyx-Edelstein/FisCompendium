using System.IO;
using System.Runtime.Serialization;

namespace FuzzyTranslator
{
    internal static class Serialization
    {
        public static T DeserializeFromFile<T>(string directory, string fileName)
        {
            var path = directory + "/" + fileName;
            var xml = File.ReadAllText(path);

            using (Stream stream = new MemoryStream())
            {
                var data = System.Text.Encoding.UTF8.GetBytes(xml);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(typeof(T));
                return (T)deserializer.ReadObject(stream);
            }
        }

        public static void SerializeToFile<T>(T toSerialize, string directory, string fileName)
        {
            System.IO.Directory.CreateDirectory(directory);
            var path = directory + "/" + fileName;

            using (var memStm = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(memStm, toSerialize);
                memStm.Seek(0, SeekOrigin.Begin);
                using (var streamReader = new StreamReader(memStm))
                {
                    var serialized = streamReader.ReadToEnd();
                    File.WriteAllText(path, serialized);
                }
            }
        }
    }
}