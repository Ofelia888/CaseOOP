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
            var htmlName = Path.Combine("print", $"{Directory.GetFiles("print").Length + 1}.html");
            var html = Path.Combine("templates", "PRINT-WELCOME.html");
            
            var fileStream = File.OpenRead(_files[index]);
            var xmlSerializer = new XmlSerializer(typeof(PluckList));
            var pluckList = (PluckList?)xmlSerializer.Deserialize(fileStream);
            fileStream.Close();

            var vars = new Dictionary<string, string>();
            vars.Add("Name", pluckList.Name);
            vars.Add("Adresse", pluckList.Address);
            vars.Add("Plukliste",
                string.Join($"<br>{Environment.NewLine}", pluckList.Lines.Select(item => $"{item.Title} (x{item.Amount})")));
            File.WriteAllText(htmlName, HTMLTemplate.Load(html)?.GetContents(vars) ?? string.Empty);
            
            Directory.CreateDirectory("import");

            var fileName = Path.GetFileName(_files[index]);
            File.Move(_files[index], string.Format(@"import\\{0}", fileName), true);

            Console.WriteLine($"Plukseddel {_files[index]} afsluttet.");
            _files.Remove(_files[index]);
        }
    }
}
