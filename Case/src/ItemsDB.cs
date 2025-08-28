using Core.io;
using Core.Models;

namespace PluckList
{
    public class ItemsDB
    {
        private readonly IContentReader _reader;
        private readonly IContentWriter _writer;
        
        public ItemsDB(IContentReader reader, IContentWriter writer)
        {
            _reader = reader;
            _writer = writer;
        }
        
        private void CreateItemsCSVDataBase()
        {
            List<Item> scannedItems;

            FileReader xmlsFileReader = new FileReader("allPluckLists");
            List<string> xmlFiles = xmlsFileReader.ReadList();
            CSVWriter csv = new CSVWriter("items.csv");

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
            if (_writer is CSVWriter) CreateItemsCSVDataBase();
            else throw new Exception("Unsupported writer");
        }

        public List<Item> ReadDatabase()
        {
            if (_reader is CSVReader csv) return csv.ReadList<Item>()!;
            throw new Exception("Unsupported reader");
        }
    }
}
