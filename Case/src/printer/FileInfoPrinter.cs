using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.Printer
{
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
