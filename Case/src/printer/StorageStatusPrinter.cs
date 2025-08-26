using PluckList.src.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.printer
{
    public class StorageStatusPrinter : ConsolePrinter
    {
        public StorageSystem Storage { get; set; }
        public StorageStatusPrinter(StorageSystem storage)
        {
            Storage = storage;
        }

        public override void Print(string text = "default")
        {
            foreach (Item item in Storage.Items)
            {
                Console.WriteLine($"{item.Title}: {item.Total} på lager");
            }
        }
    }
}
