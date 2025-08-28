using Core.io;
using Core.Models;

namespace PluckList.src.DB
{
    public class ItemsDB : IDatabase
    {
        public IContentReader IReader { get; private set; }
        public IContentWriter IWriter { get; private set; }

        public ItemsDB(IContentReader reader, IContentWriter writer)
        {
            IReader = reader;
            IWriter = writer;
        }
        
        private void CreateItemsCSVDataBase()
        {
            FileReader xmlsFileReader = new FileReader("allPluckLists");
            List<string> xmlFiles = xmlsFileReader.ReadList();
            CSVWriter csv = (CSVWriter)IWriter;

            List<Item> sortedItems = new List<Item>();
            foreach (string xml in xmlFiles)
            {
                List<Item>? items = Core.Models.PluckList.Deserialize(xml)?.Lines;
                if (items != null) sortedItems.AddRange(items);
            }
            csv.WriteAll(sortedItems.DistinctBy(x => x.ProductID), true, "ProductID", "Title", "Type");
        }

        public void CreateDatabase()
        {
            if (IWriter is CSVWriter csv)
            {
                if (File.Exists(csv.FilePath)) return;
                CreateItemsCSVDataBase();
            }
            else throw new Exception("Unsupported writer");
        }

        public List<T?> ReadDatabase<T>() where T : class
        {
            if (IReader is CSVReader csv) return csv.ReadList<T>()!;
            throw new Exception("Unsupported reader");
        }

        public int Remove(string productId)
        {
            if (IWriter is CSVWriter csv)
            {
                return csv.Remove(entry => entry.Key == "ProductID" && entry.Value == productId);
            }
            throw new Exception("Unsupported writer");
        }
    }
}
