namespace Core.io;

public interface IDatabaseReader<T> where T : class
{
    List<T> ReadEntries(Predicate<KeyValuePair<string, object?>>? predicate = null);
    
    T? ReadEntry(Predicate<KeyValuePair<string, object?>> predicate);
}
