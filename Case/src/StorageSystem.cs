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
    }
}
