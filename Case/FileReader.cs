using System;
using System.Xml.Serialization;
using System.IO;

namespace PluckList
{
    public class FileReader
    {
        public string Path { get; set; }

        public FileReader(string path)
        {
            Path = path;
        }

        public List<string>? ReadDirectory()
        {
            if (Directory.Exists(Path))
            {
                return Directory.EnumerateFiles(Path).ToList();
            }

            return null;
        }
        public FileStream ReadSingle(int fileIndex, List<string> filesInDirectory)
        {
            return System.IO.File.OpenRead(filesInDirectory[fileIndex]);
        }

        public T SerializeXmlTo<T>(FileStream file)
        {
            // Serializes from xml item to given type object
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T type = (T?)xmlSerializer.Deserialize(file);

            return type;
        }


}
}
