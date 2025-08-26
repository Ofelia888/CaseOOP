namespace PluckList.src.io;

public interface ITypeListReader
{
    List<T?> ReadList<T>() where T : class;
}
