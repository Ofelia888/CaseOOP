using System.Xml.Serialization;

namespace PluckList.src.io;

public class XMLReader : FileReader, ITypeReader
{
    public XMLReader(string filePath) : base(filePath)
    {
    }

    public T? Read<T>() where T : class
    {
        using var fileStream = File.OpenRead(FilePath);
        var serializer = new XmlSerializer(typeof(T));
        return (T?)serializer.Deserialize(fileStream);
    }
}
