using Core.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.DB
{
    public class PluckListDB
    {
        public IContentReader IReader { get; private set; }
        public IContentWriter IWriter { get; private set; }
        public PluckListDB(IContentReader reader, IContentWriter writer)
        {
            IReader = reader;
            IWriter = writer;
        }

    }
}
