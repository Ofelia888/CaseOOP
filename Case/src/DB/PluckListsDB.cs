using Core.io;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList.src.DB
{
    public class PluckListDB : IDatabase
    {
        public IContentReader IReader { get; private set; }
        public IContentWriter IWriter { get; private set; }

        public PluckListDB(IContentReader reader, IContentWriter writer)
        {
            IReader = reader;
            IWriter = writer;
        }

        private void CreatePluckListsCSVDataBase()
        {
            FileReader xmlsFileReader = new FileReader("allPluckLists");
            List<string> xmlFiles = xmlsFileReader.ReadList();
            CSVWriter csv = (CSVWriter)IWriter;

            List<Core.Models.PluckList> pluckLists = new List<Core.Models.PluckList>();
            foreach (string xml in xmlFiles)
            {
                if (xml != null) pluckLists.Add(Core.Models.PluckList.Deserialize(xml)!);
            }
            csv.WriteAll(pluckLists, true, true, "Name", "Shipment", "Address");
        }

        public void CreateDatabase()
        {
            if (IWriter is CSVWriter csv)
            {
                if (File.Exists(csv.FilePath)) return;
                CreatePluckListsCSVDataBase();
            }
            else throw new Exception("Unsupported writer");
        }

        public List<T?> ReadDatabase<T>() where T : class
        {
            if (IReader is CSVReader csv) return csv.ReadList<T>()!;
            throw new Exception("Unsupported reader");
        }

        public int Remove(string pluckListID)
        {
            if (IWriter is CSVWriter csv)
            {
                return csv.Remove(entry => entry.Key == "Id" && entry.Value == pluckListID);
            }
            throw new Exception("Unsupported writer");
        }
    }
}
