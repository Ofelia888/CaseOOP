using System;
using System.Linq;

namespace PluckList.Printer
{
    public class OptionPrinter : ConsolePrinter
    {
        public ColorHandle ColorHandle { get; set; }
        public OptionPrinter()
        {
            ColorHandle = new ColorHandle();
        }
        // FIX: parameter text, find different solution to make Print() in IPrint parameterless
        public override void Print(string text)
        {
            char first = text.First();

            ColorHandle.Handle(ColorContext.Option);
            Console.Write(first);
            ColorHandle.Handle(ColorContext.Standard);
            Console.WriteLine(text.Remove(0, 1));
        }
    }
}
