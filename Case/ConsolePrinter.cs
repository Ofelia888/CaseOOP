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
