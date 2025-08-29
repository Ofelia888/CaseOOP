using Core.io;
using Core.Models;

namespace PluckList.DB
{
    public class StorageDB : Database<StorageItem>
    {
        private ItemsDB _database;
        private List<StorageItem> _storageItems;

        public StorageDB(Repository<StorageItem> repository, ItemsDB database) : base(repository)
        {
            IdField = "ProductID";
            _database = database;
            _storageItems = new List<StorageItem>();
        }
        private void CreateStorageCSVDataBase()
        {
            foreach (BaseItem item in _database.GetEntries())
            {
                StorageItem storageItem = new StorageItem
                {
                    ProductID = item.ProductID
                };

                _storageItems.Add(storageItem);
            }
            Repository.AddEntries(_storageItems);
        }

        public override void CreateDatabase()
        {
            if (Repository is CSVRepository<StorageItem> csv)
            {
                if (File.Exists(csv.FilePath)) return;
                CreateStorageCSVDataBase();
            }
            else throw new Exception("Unsupported writer");
        }

        public void UpdateDataBase()
        {
            if (Repository is CSVRepository<StorageItem> csv)
            {
                if (File.Exists(csv.FilePath))
                {
                    _storageItems.Clear();

                    foreach (BaseItem item in _database.GetEntries())
                    {
                        StorageItem storageItem = new StorageItem
                        {
                            ProductID = item.ProductID
                        };

                        _storageItems.Add(storageItem);
                    }
                    File.Create(csv.FilePath);
                    Repository.AddEntries(_storageItems);
                }
                else throw new Exception("File does not exist");
            }
            else throw new Exception("Unsupported writer");
        }
    }
}
