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
        public ColorHandle ColorHandle { get; set; }
        public abstract void Print(string text);
    }

    public class OptionPrinter : ConsolePrinter
    {
        public OptionPrinter()
        {
            ColorHandle = new ColorHandle();
        }
        public override void Print(string text)
        {
            Console.WriteLine("\n\nOptions:");

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
        public override void Print(string text)
        {
            
        }
    }

    public class FileInfoPrinter : ConsolePrinter
    {
        public FileReader FileReader { get; set; }
        public FileInfoPrinter(FileReader fileReader)
        {
            FileReader = fileReader;
        }
        public override void Print(string text)
        {

        }
    }
}
