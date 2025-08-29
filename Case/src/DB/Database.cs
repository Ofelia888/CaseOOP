using Core.io;

namespace PluckList.DB;

public abstract class Database<T> : IDatabase<T> where T: class
{
    protected readonly Repository<T> Repository;
    protected string IdField = "Id";

    public Database(Repository<T> repository)
    {
        Repository = repository;
    }

    public abstract void CreateDatabase();

    public List<T> GetEntries()
    {
        return Repository.ReadEntries();
    }

    public int Remove(string id)
    {
        return Repository.Remove(entry => entry.Key == IdField && entry.Value == id);
    }
}
