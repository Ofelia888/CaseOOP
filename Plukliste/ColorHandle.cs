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
        public virtual ConsoleColor Handle(ColorContext context)
        {
            switch (context)
            {
                case ColorContext.Standard:
                    return ConsoleColor.Black;
                case ColorContext.Option:
                    return ConsoleColor.Green;
                case ColorContext.Status:
                    return ConsoleColor.Red;
            }

            return ConsoleColor.Black;
        }
    }
}
