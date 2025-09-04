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

            List<Pluklist> pluckLists = new List<Pluklist>();
            foreach (string xml in xmlFiles)
            {
                if (xml != null) pluckLists.Add(Pluklist.Deserialize(xml)!);
            }
            Repository.AddEntries(pluckLists.Select(pluckList => new BasePluckList
            {
                Id = Guid.NewGuid(),
                Name = pluckList.Name!,
                Shipment = pluckList.Forsendelse!,
                Address = pluckList.Adresse!
            }));
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

        public override int Remove(string id)
        {
            return Repository.Remove(pluckList => pluckList.Name.Equals(id));
        }
    }
}
