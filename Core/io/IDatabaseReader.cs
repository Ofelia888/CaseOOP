namespace Core.io;

public interface IDatabaseReader<T> where T : class
{
    List<T> ReadEntries(Predicate<T>? predicate = null);
    
    T? ReadEntry(Predicate<T> predicate);
}
