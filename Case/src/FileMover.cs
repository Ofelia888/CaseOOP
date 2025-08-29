using PluckList.Printer;

namespace PluckList
{
    public class FileMover
    {
        private readonly IPrinter _printer;
        private readonly List<string> _files;
        
        public FileMover(IPrinter printer, List<string> files)
        {
            _printer = printer;
            _files = files;
        }
        
        public void Move(int index)
        {
            if (index < 0 || index >= _files.Count) return;
            
            // HTML templates
            Directory.CreateDirectory("print");
            
            var pluckList = Core.Models.Pluklist.Deserialize(_files[index]);
            var printItem = pluckList?.GetPrintItem();
            if (pluckList == null || printItem == null) return;
            
            var htmlPath = Path.Combine("print", $"{Directory.GetFiles("print").Length + 1}.html");
            var templatePath = Path.Combine("templates", $"{printItem.ProductID}.html");
            HTMLTemplate.Load(templatePath)?.Write(htmlPath, pluckList);
            
            // handled
            Directory.CreateDirectory("handled");

            var fileName = Path.GetFileName(_files[index]);
            File.Move(_files[index], string.Format(@"handled\\{0}", fileName), true);

            _printer.Print($"Plukseddel {_files[index]} afsluttet.");
            _files.Remove(_files[index]);
        }
    }
}
