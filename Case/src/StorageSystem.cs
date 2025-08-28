using Core.Models;
using PluckList.src.DB;

namespace PluckList
{
    public class StorageSystem
    {
        public List<Item> Items { get; private set; }
        public List<Item> reservedItems = new List<Item>();
        private Dictionary<string,int> ItemCount = new Dictionary<string,int>();

        private ItemsDB _database;

        public StorageSystem(ItemsDB database)
        {
            _database = database;
        }

        public void LoadItems()
        {
            Items = _database.ReadDatabase();
        }

        public void RemoveItems(Core.Models.PluckList pluckList)
        {
            foreach (Item pluckItem in pluckList.Lines)
            {
                foreach (Item storageItem in Items)
                {
                    if (pluckItem.ProductID == storageItem.ProductID)
                    {
                        ItemCount[storageItem.ProductID!] = GetCount(storageItem) - pluckItem.Amount;
                        reservedItems.Remove(pluckItem);
                    }
                }
            }
        }
        public List<string> StorageStatus()
        {
            List<string> statuses = new List<string>();
            foreach (Item item in Items)
            {
                statuses.Add($"{item.Title}: {GetCount(item)} på lager");
            }
            return statuses;
        }

        // TODO: Rename the right list name when creating a plucklist from web
        public List<string> ReserveOnCreate(Core.Models.PluckList pluckList)
        {
            List<string> statuses = new List<string>();
            foreach (Item pluckItem in pluckList.Lines)
            {
                foreach (Item storageItem in Items)
                {
                    if (pluckItem.ProductID == storageItem.ProductID)
                    {
                        if (GetCount(storageItem) - pluckItem.Amount < 0)
                        {
                            statuses.Add($"Advarsel: {storageItem.Title} har ikke nok på lager til at reservere {pluckItem.Amount}. Der er kun {GetCount(storageItem)} på lager.");
                        }
                        else
                        {
                            statuses.Add($"{storageItem.Title}: {GetCount(storageItem)} på lager efter reservation af {pluckItem.Amount}");
                            reservedItems.Add(pluckItem);
                        }
                    }
                }
            }
            return statuses;
        }
        public bool IsLeftover(Item item)
        {
            return GetCount(item) >= item.Amount;
        }

        private int GetCount(Item item)
        {
            return ItemCount.GetValueOrDefault(item.ProductID!, 0);
        }
    }
}
