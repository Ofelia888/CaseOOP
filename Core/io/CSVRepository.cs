namespace Core.io;

public class CSVRepository<T>(string filePath, char separator = ',') : Repository<T>(new CSVReader<T>(filePath, separator), new CSVWriter<T>(filePath, separator)) where T : class, new()
{
    public string FilePath { get; } = filePath;
}
