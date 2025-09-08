namespace Core.io;

public interface IDatabaseWriter<T> where T : class
{
    void AddEntry(T entry, DatabaseWriteOptions? options = null);
    
    void AddEntries(IEnumerable<T> entries, DatabaseWriteOptions? options = null);

    int Update(Predicate<T> predicate, Func<T, T> mapper);

    int Remove(Predicate<T> predicate);

    int Remove(T entry);
}
