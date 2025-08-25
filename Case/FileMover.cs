using System.Xml.Serialization;

namespace PluckList
{
    public class FileMover
    {
        private readonly List<string> _files;
        
        public FileMover(List<string> files)
        {
            _files = files;
        }
        
        public void Move(int index)
        {
            Directory.CreateDirectory("print");
            var fileStream = File.OpenRead(_files[index]);
            var xmlSerializer = new XmlSerializer(typeof(PluckList));
            var pluckList = (PluckList?)xmlSerializer.Deserialize(fileStream);
            fileStream.Close();

            var printItem = pluckList?.Lines.FirstOrDefault(item => item.Type == ItemType.Print);
            if (pluckList == null || printItem == null) return;
            
            var htmlPath = Path.Combine("print", $"{Directory.GetFiles("print").Length + 1}.html");
            var templatePath = Path.Combine("templates", $"{printItem.ProductID}.html");

            var vars = new Dictionary<string, string>();
            vars.Add("Name", pluckList.Name!);
            vars.Add("Adresse", pluckList.Address!);
            vars.Add("Plukliste",
                string.Join($"<br>{Environment.NewLine}", pluckList.Lines.Select(item => $"{item.Title} (x{item.Amount})")));
            File.WriteAllText(htmlPath, HTMLTemplate.Load(templatePath)?.GetContents(vars) ?? string.Empty);
            
            Directory.CreateDirectory("import");

            var fileName = Path.GetFileName(_files[index]);
            File.Move(_files[index], string.Format(@"import\\{0}", fileName), true);

            Console.WriteLine($"Plukseddel {_files[index]} afsluttet.");
            _files.Remove(_files[index]);
        }
    }
}
