using PluckList.src.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.printer
{
    public class ItemPrinter : ConsolePrinter
    {
        public PluckList PluckList { get; set; }
        public ItemPrinter(PluckList pluckList)
        {
            PluckList = pluckList;
        }
        public override void Print(string text = "default")
        {
            if (PluckList.Lines != null)
            {
                Console.WriteLine("\n{0,-7}{1,-9}{2,-9}{3,-20}{4}", "Antal", "Rest", "Type", "Produktnr.", "Navn");
                foreach (Item item in PluckList.Lines)
                {
                    Console.WriteLine("{0,-7}{1,-9}{2,-9}{3,-20}{4}", item.Amount, item.IsLeftover().ToString(), item.Type, item.ProductID, item.Title);
                }
            }
        }
    }
}
