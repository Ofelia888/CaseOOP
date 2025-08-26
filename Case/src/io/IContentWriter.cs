namespace PluckList.src.io;

public interface IContentWriter
{
    void Write<T>(T content);

    void Write<T>(IEnumerable<T> content);
}
