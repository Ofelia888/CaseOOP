using Core.io;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.DB
{
    public class StorageDB : IDatabase
    {
        public IContentReader IReader { get; private set; }
        public IContentWriter IWriter { get; private set; }
        private ItemsDB _database;
        private List<StorageItem> _storageItems;

        public StorageDB(IContentReader reader, IContentWriter writer, ItemsDB database)
        {
            IReader = reader;
            IWriter = writer;
            _database = database;
            _storageItems = new List<StorageItem>();
        }
        private void CreateStorageCSVDataBase()
        {
            foreach (Item item in _database.ReadDatabase<Item>())
            {
                StorageItem storageItem = new StorageItem();
                storageItem.ProductID = item.ProductID;

                _storageItems.Add(storageItem);
            }

            CSVWriter csv = (CSVWriter)IWriter;
            csv.WriteAll(_storageItems, true, "ProductID", "Amount");
        }

        public void CreateDatabase()
        {
            if (IWriter is CSVWriter csv)
            {
                if (File.Exists(csv.FilePath)) return;
                CreateStorageCSVDataBase();
            }
            else throw new Exception("Unsupported writer");
        }

        public List<T?> ReadDatabase<T>() where T : class
        {
            if (IReader is CSVReader csv) return csv.ReadList<T>()!;
            throw new Exception("Unsupported reader");
        }

        public void UpdateDataBase()
        {
            if (IWriter is CSVWriter csv)
            {
                if (File.Exists(csv.FilePath))
                {
                    _storageItems.Clear();

                    foreach (Item item in _database.ReadDatabase<Item>())
                    {
                        StorageItem storageItem = new StorageItem();
                        storageItem.ProductID = item.ProductID;

                        _storageItems.Add(storageItem);
                    }

                    File.Delete(csv.FilePath);
                    csv.WriteAll(_database.ReadDatabase<StorageItem>(), true, "ProductID", "Amount");
                }
                else throw new Exception("File does not exist");
            }
            else throw new Exception("Unsupported writer");
        }
    }
}
