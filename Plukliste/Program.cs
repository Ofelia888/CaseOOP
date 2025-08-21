using System.Xml.Serialization;
//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
namespace PluckList;

class Program
{
    static void Main()
    {
        FileReader fileReader = new FileReader("C:\\Users\\HFGF\\Source\\Repos\\CaseOOP\\PlukListe\\export\\");
        List<string> files = fileReader.ReadDirectory();

        ColorHandle colorHandle = new ColorHandle();

        OptionPrinter optionPrinter = new OptionPrinter();
        
        char readKey = ' ';
        int index = -1;

        

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

                PluckList pluckList = fileReader.SerializeXmlTo<PluckList>(fileReader.ReadSingle(index));

                // Prints properties from plucklist
                if (pluckList != null && pluckList.Lines != null)
                {
                    Console.WriteLine("\n{0, -13}{1}", "Navn:", pluckList.Name);
                    Console.WriteLine("{0, -13}{1}", "Forsendelse:", pluckList.Shipment);
                    #warning TODO: Add address to screen print

                    Console.WriteLine("\n{0,-7}{1,-9}{2,-20}{3}", "Antal", "Type", "Produktnr.", "Navn");
                    foreach (Item item in pluckList.Lines)
                    {
                        Console.WriteLine("{0,-7}{1,-9}{2,-20}{3}", item.Amount, item.Type, item.ProductID, item.Title);
                    }
                }
            }

            //Print options
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

            // Takes input
            readKey = Console.ReadKey().KeyChar;
            if (readKey >= 'a')
            {
                readKey = char.ToUpper(readKey);
            }
            Console.Clear();

            // Handles input
            colorHandle.Handle(ColorContext.Status);
            switch (readKey)
            {
                case 'G':
                    // Refresh file contents
                    files = fileReader.ReadDirectory();
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
                    Directory.CreateDirectory("import");

                    string fileWithoutPath = files[index].Substring(files[index].LastIndexOf('\\'));
                    File.Move(files[index], string.Format(@"import\\{0}", fileWithoutPath));

                    Console.WriteLine($"Plukseddel {files[index]} afsluttet.");
                    files.Remove(files[index]);
                    
                    if (index == files.Count)
                    {
                        index--;
                    }

                    break;
            }

            colorHandle.Handle(ColorContext.Standard);

        }
    }
}
