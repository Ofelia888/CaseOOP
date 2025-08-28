using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.Printer
{
    public class PluckListPrinter : ConsolePrinter
    {
        public PluckList PluckList { get; set; }
        public PluckListPrinter(PluckList pluckList)
        {
            PluckList = pluckList;
        }

        public override void Print(string text = "default")
        {
            if (PluckList != null)
            {
                Console.WriteLine("\n{0, -13}{1}", "Navn", PluckList.Name);
                Console.WriteLine("{0, -13}{1}", "Forsendelse:", PluckList.Shipment);
                Console.WriteLine("{0, -13}{1}", "Adresse:", PluckList.Address);
            }
        }
        public void PrintItems()
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
