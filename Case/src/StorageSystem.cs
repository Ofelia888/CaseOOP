using Core.Models;

namespace PluckList
{
    public class StorageSystem
    {
        public List<Item> Items { get; private set; }
        public List<Item> reservedItems = new List<Item>();

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
                        storageItem.Total -= pluckItem.Amount;
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
                statuses.Add($"{item.Title}: {item.Total} på lager");
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
                        if (storageItem.Total - pluckItem.Amount < 0)
                        {
                            statuses.Add($"Advarsel: {storageItem.Title} har ikke nok på lager til at reservere {pluckItem.Amount}. Der er kun {storageItem.Total} på lager.");
                        }
                        else
                        {
                            statuses.Add($"{storageItem.Title}: {storageItem.Total} på lager efter reservation af {pluckItem.Amount}");
                            reservedItems.Add(pluckItem);
                        }
                    }
                }
            }
            return statuses;
        }
    }
}
