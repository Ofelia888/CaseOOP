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
