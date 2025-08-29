namespace Core.io;

public class Repository<T>(IDatabaseReader<T> reader, IDatabaseWriter<T> writer)
    : IDatabaseReader<T>, IDatabaseWriter<T>
    where T : class
{
    public List<T> ReadEntries(Predicate<KeyValuePair<string, object?>>? predicate = null)
    {
        return reader.ReadEntries(predicate);
    }

    public T? ReadEntry(Predicate<KeyValuePair<string, object?>> predicate)
    {
        return reader.ReadEntry(predicate);
    }

    public void AddEntry(T entry, DatabaseWriteOptions? options = null)
    {
        writer.AddEntry(entry, options);
    }

    public void AddEntries(IEnumerable<T> entries, DatabaseWriteOptions? options = null)
    {
        writer.AddEntries(entries, options);
    }

    public int Remove(Predicate<KeyValuePair<string, string>> predicate)
    {
        return writer.Remove(predicate);
    }
}
