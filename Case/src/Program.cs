
//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag

using System.Diagnostics;
using Core.io;
using Core.Models;
using PluckList.Printer;
using PluckList.src;

namespace PluckList;

class Program
{
    static void Main()
    {
        FileReader fileReader = new FileReader("pending");
        List<string>? files = fileReader.ReadList();
        
        ItemScanner itemScanner = new ItemScanner();
        ItemsDB database = new ItemsDB(new CSVReader("items.csv"), new CSVWriter("items.csv"));
        StorageSystem storage = new StorageSystem(database);

        FileMover fileMover = new FileMover(files);

        ColorHandle colorHandle = new ColorHandle();
        
        char readKey = ' ';
        int index = -1;
        Core.Models.PluckList? pluckList = null;
        List<Item> scannedItems;

        database.CreateDatabase();
        storage.LoadItems();

        // Program loop
        while (readKey != 'Q')
        {
            if (files.Count == 0)
            {
                Console.WriteLine("No files found.");
                break;
            }
            if (index == -1) index = 0;

            // Prints file info
            Console.WriteLine($"PlukListe {index + 1} af {files.Count}");
            Console.WriteLine($"\nFil: {files[index]}");

            // Serializes xml contents to plucklist
            pluckList = Core.Models.PluckList.Deserialize(files[index]);
            if (pluckList == null) break;

            // Prints properties from plucklist

            new PluckListPrinter(pluckList).Print();
            new ItemPrinter(pluckList).Print();

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
                    files = fileReader.ReadList();
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
                    //Move files to handled directory
                    if (index == -1)
                    {
                        break;
                    }
                    fileMover.Move(index);
                    if (index == files.Count)
                    {
                        index--;
                    }
                    storage.RemoveItems(pluckList);
                    foreach (string status in storage.StorageStatus())
                    {
                        Console.WriteLine(status);
                    }
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
                    new CSVWriter(Path.Combine("pending", "varer.csv")).WriteAll(scannedItems);

                    colorHandle.Handle(ColorContext.Status);
                    Console.WriteLine("Varer scannet til CSV fil");
                    break;
            }

            colorHandle.Handle(ColorContext.Standard);

        }
    }
}
