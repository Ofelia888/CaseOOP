using System;
using System.Xml.Serialization;
using System.IO;

namespace PluckList
{
    public class FileReader
    {
        public string Path { get; set; }
        public List<string> FilesInDirectory { get; set; }
        public FileStream File { get; set; }

        public FileReader(string path)
        {
            Path = path;
        }

        public List<string> ReadDirectory()
        {
            if (Directory.Exists(Path))
            {
                return FilesInDirectory = Directory.EnumerateFiles(Path).ToList();
            }

            return null;
        }
        public FileStream ReadSingle(int fileIndex)
        {
            return System.IO.File.OpenRead(FilesInDirectory[fileIndex]);
        }

        public T SerializeXmlTo<T>(FileStream file)
        {
            // Serializes from xml item to given type object
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T type = (T?)xmlSerializer.Deserialize(File);

            return type;
        }


}
}
