
//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag

using System.Diagnostics;
using PluckList.src.io;
using PluckList.src.printer;
using PluckList.src.Printer;

namespace PluckList.src;

class Program
{
    static void Main()
    {
        FileReader fileReader = new FileReader("export");
        List<string>? files = fileReader.ReadDirectory();
        if (files == null)
        {
            Console.WriteLine("There's no files at the selected directory");
            return;
        }
        ItemScanner itemScanner = new ItemScanner();
        StorageSystem storage = new StorageSystem();

        FileMover fileMover = new FileMover(files);

        ColorHandle colorHandle = new ColorHandle();
        
        char readKey = ' ';
        int index = -1;
        PluckList? pluckList = null;
        List<Item> scannedItems;
        

        // Program loop
        while (readKey != 'Q')
        {
            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");
            }
            else
            {
                if (index == -1)
                {
                    index = 0;
                }

                // Prints file info
                Console.WriteLine($"PlukListe {index + 1} af {files.Count}");
                Console.WriteLine($"\nFil: {files[index]}");

                // Serializes xml contents to plucklist
                using var fileStream = fileReader.ReadSingle(index, files);
                pluckList = fileReader.SerializeXmlTo<PluckList>(fileStream);

                // Prints properties from plucklist

                new PluckListPrinter(pluckList).Print();
                new ItemPrinter(pluckList).Print();
            }

            storage.SetItems(pluckList);

            //Print options
            OptionPrinter optionPrinter = new OptionPrinter();
            Console.WriteLine("\n\nOptions:");
            optionPrinter.Print("Quit");
            if (index >= 0)
            {
                optionPrinter.Print("Afslut plukseddel");
            }
            if (index > 0)
            {
                optionPrinter.Print("Forrige plukseddel");
            }
            if (index < files.Count - 1)
            {
                optionPrinter.Print("Næste plukseddel");
            }
            optionPrinter.Print("Genindlæs plukseddel");
            optionPrinter.Print("Åbn i browser");
            optionPrinter.Print("Scan varer");

            // Takes input
            readKey = Console.ReadKey().KeyChar;
            readKey = char.ToUpper(readKey);
            Console.Clear();

            // Handles input
            colorHandle.Handle(ColorContext.Status);
            switch (readKey)
            {
                case 'G':
                    // Refresh file contents
                    files = fileReader.ReadDirectory();
                    if (files == null)
                    {
                        Console.WriteLine("Der er ingen filer fundet på den valgte Mappe");
                        return;
                    }
                    fileMover = new FileMover(files);
                    index = -1;
                    Console.WriteLine("PlukLister genindlæst");
                    break;
                case 'F':
                    // Go to previous file
                    if (index > 0)
                    {
                        index--;
                    }
                    break;
                case 'N':
                    // Go to next file
                    if (index < files.Count - 1)
                    {
                        index++;
                    }
                    break;
                case 'A':
                    //Move files to import directory
                    fileMover.Move(index);
                    if (index == files.Count)
                    {
                        index--;
                    }
                    storage.RemoveItems(pluckList);
                    new StorageStatusPrinter(storage).Print();
                    break;
                case 'Å':
                    var printItem = pluckList?.Lines.FirstOrDefault(item => item.Type == ItemType.Print);
                    if (pluckList == null || printItem == null) break;
                    var vars = new Dictionary<string, string>();
                    vars.Add("Name", pluckList.Name!);
                    vars.Add("Adresse", pluckList.Address!);
                    vars.Add("Plukliste",
                        string.Join($"<br>{Environment.NewLine}", pluckList.Lines.Select(item => $"{item.Title} (x{item.Amount})")));
                    var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.html");
                    var templatePath = Path.Combine("templates", $"{printItem.ProductID}.html");
                    File.WriteAllText(filePath, HTMLTemplate.Load(templatePath)?.GetContents(vars) ?? string.Empty);
                    var info = new ProcessStartInfo(filePath)
                    {
                        UseShellExecute = true
                    };
                    Process.Start(info);
                    break;
                case 'S':
                    scannedItems = itemScanner.ScanItems(pluckList);
                    new CSVWriter(Path.Combine("export", "varer.csv")).Write(scannedItems);

                    colorHandle.Handle(ColorContext.Status);
                    Console.WriteLine("Varer scannet til CSV fil");
                    break;
            }

            colorHandle.Handle(ColorContext.Standard);

        }
    }
}
