using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluckList
{
    public abstract class ConsolePrinter : IPrint
    {
        public abstract void Print(string text);
    }

    public class OptionPrinter : ConsolePrinter
    {
        public ColorHandle ColorHandle { get; set; }
        public OptionPrinter()
        {
            ColorHandle = new ColorHandle();
        }
        public override void Print(string text)
        {
            ColorHandle.Handle(ColorContext.Option);
            Console.Write(text.First());
            ColorHandle.Handle(ColorContext.Standard);
            Console.WriteLine(text.Remove(0, 1));
        }
    }

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
                Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Antal", "Type", "Produktnr.", "Navn");
                foreach (Item item in PluckList.Lines)
                {
                    Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);
                }
            }
        }
    }

    //public class FileInfoPrinter : ConsolePrinter
    //{
    //    public FileReader FileReader { get; set; }
    //    public FileInfoPrinter(FileReader fileReader)
    //    {
    //        FileReader = fileReader;
    //    }
    //    public override void Print(string text)
    //    {

    //    }
    //}
}
