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
            Directory.CreateDirectory("import");

            var fileName = Path.GetFileName(_files[0]);
            File.Move(_files[index], string.Format(@"import\\{0}", fileName), true);

            Console.WriteLine($"Plukseddel {_files[index]} afsluttet.");
            _files.Remove(_files[index]);
        }
    }
}
