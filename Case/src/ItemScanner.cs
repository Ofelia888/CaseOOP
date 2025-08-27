using PluckList.src.Printer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PluckList.src
{
    public class ItemScanner
    {
        public List<Item> ScanItems(PluckList pluckList)
        {
            List<Item> scannedItems = new List<Item>();
            OptionPrinter optionPrinter = new OptionPrinter();
            char readKey = ' ';

            if (pluckList == null)
            {
                throw new NullReferenceException();
            }

            foreach (Item item in pluckList.Lines)
            {
                if (item.Title == null || item.Title.Length == 0)
                {
                    continue;
                }
                optionPrinter.Print(item.Title);
            }
            optionPrinter.Print("Færdig");

            while (true)
            {
                Console.WriteLine();
                readKey = Console.ReadKey().KeyChar;
                readKey = char.ToUpper(readKey);

                if (readKey == 'F')
                {
                    Console.Clear();
                    return scannedItems;
                }

                foreach (Item item in pluckList.Lines)
                {
                    if (readKey == item.Title.First())
                    {
                        scannedItems.Add(item);
                        Console.WriteLine($"\n{item.Title} scannet");
                    }
                }
            }
        }
    }
}
