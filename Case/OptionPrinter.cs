using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList
{
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
}
