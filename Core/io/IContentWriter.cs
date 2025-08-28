namespace Core.io;

public interface IContentWriter
{
    void Write<T>(T content, bool append);

    void Write<T>(T content);

    void WriteAll<T>(IEnumerable<T> content, bool append);

    void WriteAll<T>(IEnumerable<T> content);
}
