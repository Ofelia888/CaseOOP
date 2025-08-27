namespace PluckList.src.io;

public interface IContentWriter
{
    void Write<T>(T content);

    void WriteAll<T>(IEnumerable<T> content);
}
