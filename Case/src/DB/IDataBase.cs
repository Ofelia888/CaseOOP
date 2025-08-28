using Core.io;
using Core.Models;

namespace PluckList.src.DB
{
    public interface IDatabase
    {
        IContentReader IReader { get; }
        IContentWriter IWriter { get; }
        void CreateDatabase();
        List<Item> ReadDatabase();
    }
}
