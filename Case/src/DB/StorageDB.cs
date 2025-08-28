using Core.io;
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
        private StorageSystem Storage;

        public StorageDB(IContentReader reader, IContentWriter writer, StorageSystem storage)
        {
            IReader = reader;
            IWriter = writer;
            Storage = storage;
        }
        private void CreateStorageCSVDataBase()
        {
            CSVWriter csv = (CSVWriter)IWriter;
            csv.WriteAll(Storage.Items, true, "ProductID", "Amount");
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
                    csv.WriteAll(Storage.Items, false, "ProductID", "Amount");
                }
                else throw new Exception("File does not exist");
            }
            else throw new Exception("Unsupported writer");
        }
    }
}
