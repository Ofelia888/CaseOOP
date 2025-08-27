namespace PluckList.src
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
            // HTML templates
            Directory.CreateDirectory("print");
            
            var pluckList = PluckList.Deserialize(_files[index]);
            var printItem = pluckList?.GetPrintItem();
            if (pluckList == null || printItem == null) return;
            
            var htmlPath = Path.Combine("print", $"{Directory.GetFiles("print").Length + 1}.html");
            var templatePath = Path.Combine("templates", $"{printItem.ProductID}.html");
            HTMLTemplate.Load(templatePath)?.Write(htmlPath, pluckList);
            
            // Import
            Directory.CreateDirectory("import");

            var fileName = Path.GetFileName(_files[index]);
            File.Move(_files[index], string.Format(@"import\\{0}", fileName), true);

            Console.WriteLine($"Plukseddel {_files[index]} afsluttet.");
            _files.Remove(_files[index]);
        }
    }
}
