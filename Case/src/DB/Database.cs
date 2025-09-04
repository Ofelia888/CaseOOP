using Core.io;

namespace PluckList.DB;

public abstract class Database<T> : IDatabase<T> where T: class, new()
{
    protected readonly Repository<T> Repository;

    public Database(Repository<T> repository)
    {
        Repository = repository;
    }

    public abstract void CreateDatabase();
    public abstract int Remove(string id);

    public List<T> GetEntries()
    {
        return Repository.ReadEntries();
    }
}
