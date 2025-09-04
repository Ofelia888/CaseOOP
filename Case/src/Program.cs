
//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag

using System.Diagnostics;
using Core.io;
using Core.Models;
using PluckList.DB;
using PluckList.Printer;

namespace PluckList;

class Program
{
    private static readonly StatePrinter Printer = new StatePrinter();
    
    private static void PrintPluckList(Core.Models.Pluklist pluckList, StorageSystem storageSystem)
    {
        Printer.Print("\n{0, -13}{1}", "Navn", pluckList.Name);
        Printer.Print("{0, -13}{1}", "Forsendelse:", pluckList.Forsendelse);
        Printer.Print("{0, -13}{1}", "Adresse:", pluckList.Adresse);
        if (pluckList.Lines.Count == 0) return;
        Printer.Print("\n{0,-7}{1,-9}{2,-9}{3,-20}{4}", "Antal", "Rest", "Type", "Produktnr.", "Navn");
        foreach (var item in pluckList.Lines)
        {
            Printer.Print("{0,-7}{1,-9}{2,-9}{3,-20}{4}", item.Amount, storageSystem.IsLeftover(item).ToString(), item.Type, item.ProductID, item.Title);
        }
    }
    
    static void Main()
    {
        FileReader fileReader = new FileReader("pending");
        List<string> files = fileReader.ReadList();

        ItemScanner itemScanner = new ItemScanner(Printer);

        ItemsDB itemsDB = new ItemsDB(new CSVRepository<BaseItem>("items.csv"));
        StorageDB storageDB = new StorageDB(new CSVRepository<StorageItem>("storage.csv"), itemsDB);
        PluckListDB pluckListDB = new PluckListDB(new CSVRepository<BasePluckList>("plucklists.csv", ';'));

        StorageSystem storage = new StorageSystem(storageDB);
        FileMover fileMover = new FileMover(Printer, files);

        char readKey = ' ';
        int index = -1;
        Core.Models.Pluklist? pluckList = null;
        List<Item> scannedItems;

        itemsDB.CreateDatabase();
        storageDB.CreateDatabase();
        pluckListDB.CreateDatabase();

        storage.LoadItems();

        // Program loop
        while (readKey != 'Q')
        {
            if (files.Count == 0)
            {
                Printer.Print("No files found.");
                break;
            }
            if (index == -1) index = 0;

            // Prints file info
            Printer.Print($"PlukListe {index + 1} af {files.Count}");
            Printer.Print($"\nFil: {files[index]}");

            // Serializes xml contents to plucklist
            pluckList = Core.Models.Pluklist.Deserialize(files[index]);
            if (pluckList == null) break;

            // Prints properties from plucklist
            PrintPluckList(pluckList, storage);

            //Print options
            var optionPrinter = new OptionPrinter();
            Printer.Print("\n\nOptions:");
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
            Printer.Clear();

            // Handles input
            Printer.State = PrintState.Status;
            switch (readKey)
            {
                case 'G':
                    // Refresh file contents
                    files = fileReader.ReadList();
                    fileMover = new FileMover(Printer, files);
                    index = -1;
                    Printer.Print("PlukLister genindlæst");
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
                        Printer.Print(status);
                    }
                    break;
                case 'Å':
                    var printItem = pluckList?.Lines.FirstOrDefault(item => item.Type == ItemType.Print);
                    if (pluckList == null || printItem == null) break;
                    var vars = new Dictionary<string, string>();
                    vars.Add("Name", pluckList.Name!);
                    vars.Add("Adresse", pluckList.Adresse!);
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
                    new CSVRepository<Item>(Path.Combine("pending", "varer.csv")).AddEntries(scannedItems, new DatabaseWriteOptions() { Append = false });

                    Printer.State = PrintState.Status;
                    Printer.Print("Varer scannet til CSV fil");
                    break;
            }

            Printer.State = PrintState.Standard;
        }
    }
}
