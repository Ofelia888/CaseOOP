using Newtonsoft.Json;

namespace PluckList.src.io;

public class JSONReader : FileReader, ITypeReader
{
    public JSONReader(string filePath) : base(filePath)
    {
    }

    public T? Read<T>() where T : class
    {
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath));
    }
}
