using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using PluckList.Printer;

namespace PluckList.src
{
    public class ItemScanner
    {
        public List<Item> ScanItems(Core.Models.PluckList pluckList)
        {
            List<Item> scannedItems = new List<Item>();
            OptionPrinter optionPrinter = new OptionPrinter();
            ColorHandle color = new ColorHandle();
            char readKey = ' ';

            if (pluckList == null)
            {
                throw new NullReferenceException();
            }

            List<Item> scannable = pluckList.Lines.ToList();
            while (scannable.Count() > 0)
            {
                for (int i = 0; i < scannable.Count(); i++)
                {
                    Item item = scannable[i];
                    if (item.Title == null || item.Title.Length == 0)
                    {
                        continue;
                    }
                    optionPrinter.Print(item.Title);
                }
                optionPrinter.Print("Færdig");

            
                Console.WriteLine();
                readKey = Console.ReadKey().KeyChar;
                readKey = char.ToUpper(readKey);

                if (readKey == 'F')
                {
                    Console.Clear();
                    return scannedItems;
                }

                Console.Clear();
                for (int i = 0; i < scannable.Count(); i++)
                {
                    Item item = scannable[i];
                    if (readKey == item.Title?.First())
                    {
                        scannedItems.Add(item);
                        scannable.Remove(item);
                        
                        color.Handle(ColorContext.Status);
                        Console.WriteLine($"\n{item.Title} scannet");
                        color.Handle(ColorContext.Standard);
                    }
                }
            }

            return scannedItems;
        }
    }
}
