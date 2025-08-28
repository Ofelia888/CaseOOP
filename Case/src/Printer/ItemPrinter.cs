using System;
using Core.Models;

namespace PluckList.Printer
{
    public class ItemPrinter : ConsolePrinter
    {
        public Core.Models.PluckList PluckList { get; set; }
        public ItemPrinter(Core.Models.PluckList pluckList)
        {
            PluckList = pluckList;
        }
        public void Print(StorageSystem storageSystem,string text = "default") // StorageSystem storagesystem og fjern override
        {
            if (PluckList.Lines != null)
            {
                Console.WriteLine("\n{0,-7}{1,-9}{2,-9}{3,-20}{4}", "Antal", "Rest", "Type", "Produktnr.", "Navn");
                foreach (Item item in PluckList.Lines)
                {
                    Console.WriteLine("{0,-7}{1,-9}{2,-9}{3,-20}{4}", item.Amount, storageSystem.IsLeftover(item).ToString(), item.Type, item.ProductID, item.Title);
                }
            }
        }
        public override void Print(string text = "default")
        {
            throw new NotImplementedException();
        }
    }
}
