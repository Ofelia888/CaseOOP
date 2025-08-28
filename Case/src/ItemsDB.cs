using System.Collections.Generic;
using System.Linq;
using Core.io;
using Core.Models;

namespace PluckList
{
    public class ItemsDB
    {
        public ItemsDB()
        {
            CreateItemsCSVDataBase();
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
                List<Item> items = new XMLReader(xml).Read<Core.Models.PluckList>().Lines;
                sortedItems.AddRange(items);
            }
            csv.WriteAll(sortedItems.DistinctBy(x => x.ProductID), true, "ProductID", "Title", "Type");
        }
    }
}
