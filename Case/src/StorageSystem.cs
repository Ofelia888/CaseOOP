using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src
{
    public class StorageSystem
    {
        public List<Item> Items { get; }
        public List<Item> reservedItems = new List<Item>();
        public StorageSystem()
        {
            Items = new List<Item>();
        }
        public void SetItems(PluckList pluckList)
        {
            foreach (Item pluckItem in pluckList.Lines)
            {
                bool notInList = true;

                foreach (Item storageItem in Items)
                {
                    if (pluckItem.ProductID == storageItem.ProductID)
                    {
                        notInList = false;
                        break;
                    }
                }

                if (notInList)
                {
                    Items.Add(pluckItem);
                }
            }
        }
        public void RemoveItems(PluckList pluckList)
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
        public List<string> ReserveOnCreate(PluckList pluckList)
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
