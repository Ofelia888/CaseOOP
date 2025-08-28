using Core.io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.DB
{
    public class PluckListsDB
    {
        public IContentReader IReader { get; private set; }
        public IContentWriter IWriter { get; private set; }

        public PluckListsDB(IContentReader reader, IContentWriter writer)
        {
            IReader = reader;
            IWriter = writer;
        }

    }
}
