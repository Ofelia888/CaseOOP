using Core.io;
using Core.Models;

namespace PluckList.DB
{
    public class PluckListDB : Database<BasePluckList>
    {
        public PluckListDB(Repository<BasePluckList> repository) : base(repository)
        {
        }

        private void CreatePluckListsCSVDataBase()
        {
            FileReader xmlsFileReader = new FileReader("allPluckLists");
            List<string> xmlFiles = xmlsFileReader.ReadList();

            List<Core.Models.PluckList> pluckLists = new List<Core.Models.PluckList>();
            foreach (string xml in xmlFiles)
            {
                if (xml != null) pluckLists.Add(Core.Models.PluckList.Deserialize(xml)!);
            }
            Repository.AddEntries(pluckLists.Select(pluckList => new BasePluckList
            {
                Name = pluckList.Name!,
                Shipment = pluckList.Shipment!,
                Address = pluckList.Address!
            }), new DatabaseWriteOptions() { GenerateId = true });
        }

        public override void CreateDatabase()
        {
            if (Repository is CSVRepository<BasePluckList> csv)
            {
                if (File.Exists(csv.FilePath)) return;
                CreatePluckListsCSVDataBase();
            }
            else throw new Exception("Unsupported writer");
        }
    }
}
