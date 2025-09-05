namespace PluckList.DB
{
    public interface IDatabase<T> where T : class
    {
        void CreateDatabase();
        
        List<T> GetEntries();

        int Remove(string id);
    }
}
