namespace PluckList.src.io;

public interface ITypeReader
{
    T? Read<T>() where T : class;
}
