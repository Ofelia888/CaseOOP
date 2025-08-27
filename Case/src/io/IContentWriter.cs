namespace PluckList.src.io;

public interface IContentWriter
{
    void Write(string content);

    void Write<T>(IEnumerable<T> content);
}
