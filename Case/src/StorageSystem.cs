using Core.Models;
using PluckList.src.DB;

namespace PluckList
{
    public class StorageSystem
    {
        public List<StorageItem> Items { get; private set; }
        public List<StorageItem> ReservedItems = new List<StorageItem>();

        private StorageDB _database;

        public StorageSystem(StorageDB database)
        {
            _database = database;
        }

        public void LoadItems()
        {
            Items = _database.ReadDatabase<StorageItem>();
        }

        public void RemoveItems(Core.Models.PluckList pluckList)
        {
            foreach (Item pluckItem in pluckList.Lines)
            {
                foreach (StorageItem storageItem in Items)
                {
                    if (pluckItem.ProductID == storageItem.ProductID)
                    {
                        storageItem.Amount -= pluckItem.Amount;
                        ReservedItems.Remove(storageItem);
                    }
                }
            }
        }
        public List<string> StorageStatus()
        {
            List<string> statuses = new List<string>();
            foreach (StorageItem item in Items)
            {
                statuses.Add($"{item.ProductID}: {item.Amount} på lager");
            }
            return statuses;
        }

        // TODO: Rename the right list name when creating a plucklist from web
        public List<string> ReserveOnCreate(Core.Models.PluckList pluckList)
        {
            List<string> statuses = new List<string>();
            foreach (Item pluckItem in pluckList.Lines)
            {
                foreach (StorageItem storageItem in Items)
                {
                    if (pluckItem.ProductID == storageItem.ProductID)
                    {
                        if (storageItem.Amount - pluckItem.Amount < 0)
                        {
                            statuses.Add($"Advarsel: {storageItem.ProductID} har ikke nok på lager til at reservere {pluckItem.Amount}. Der er kun {storageItem.Amount} på lager.");
                        }
                        else
                        {
                            statuses.Add($"{storageItem.ProductID}: {storageItem.Amount} på lager efter reservation af {pluckItem.Amount}");
                            ReservedItems.Add(storageItem);
                        }
                    }
                }
            }
            return statuses;
        }
        public bool IsLeftover(Item item)
        {
            return Items.Single(x => x.ProductID == item.ProductID).Amount >= item.Amount;
        }
    }
}
