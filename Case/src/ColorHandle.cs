using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList
{
    public enum ColorContext
    {
        Standard,
        Option,
        Status,

    }
    public class ColorHandle
    {
        public virtual void Handle(ColorContext context)
        {
            switch (context)
            {
                case ColorContext.Standard:
                     Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case ColorContext.Option:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ColorContext.Status:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
        }
    }
}
