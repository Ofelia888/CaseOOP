namespace Core.io;

public class CSVRepository<T>(string filePath) : Repository<T>(new CSVReader<T>(filePath), new CSVWriter<T>(filePath)) where T : class
{
    public string FilePath { get; } = filePath;
}
