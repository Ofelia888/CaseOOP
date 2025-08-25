using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.Printer
{
    public abstract class ConsolePrinter : IPrint
    {
        public abstract void Print(string text);
    }
}
