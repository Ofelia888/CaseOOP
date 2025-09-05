using Core.io;
using Core.Models;

namespace PluckList.DB
{
    public class ItemsDB : Database<BaseItem>
    {
        public ItemsDB(Repository<BaseItem> repository) : base(repository)
        {
        }
        
        private void CreateItemsCSVDataBase()
        {
            FileReader xmlsFileReader = new FileReader("allPluckLists");
            List<string> xmlFiles = xmlsFileReader.ReadList();

            List<Item> sortedItems = new List<Item>();
            foreach (string xml in xmlFiles)
            {
                List<Item>? items = Core.Models.Pluklist.Deserialize(xml)?.Lines;
                if (items != null) sortedItems.AddRange(items);
            }
            Repository.AddEntries(sortedItems.DistinctBy(item => item.ProductID).Select(item => new BaseItem
            {
                ProductID = item.ProductID!,
                Title = item.Title!,
                Type = item.Type
            }));
        }

        public override void CreateDatabase()
        {
            if (Repository is CSVRepository<BaseItem> csv)
            {
                if (File.Exists(csv.FilePath)) return;
                CreateItemsCSVDataBase();
            }
            else throw new Exception("Unsupported writer");
        }

        public override int Remove(string id)
        {
            return Repository.Remove(item => item.ProductID.Equals(id));
        }
    }
}
