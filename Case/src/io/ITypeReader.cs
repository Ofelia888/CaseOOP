namespace PluckList.src.io;

public interface ITypeReader
{
    List<T?> ReadList<T>() where T : class;
}
