namespace Core.io;

public interface ITypeReader
{
    T? Read<T>() where T : class;
}
