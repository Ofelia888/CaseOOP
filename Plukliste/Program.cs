//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
namespace PluckList;

class Program
{
    static void Main()
    {
        char readKey = ' ';
        List<string> files;
        int index = -1;
        ConsoleColor standardColor = Console.ForegroundColor;
        string filePath = "C:\\Users\\HFGF\\Source\\Repos\\CaseOOP\\PlukListe\\export\\";

        Directory.CreateDirectory("import");

        // Find directory
        if (!Directory.Exists(filePath))
        {
            Console.WriteLine("Directory \"export\" not found");
            Console.ReadLine();
            return;
        }

        // Files in directory to list
        files = Directory.EnumerateFiles(filePath).ToList();

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

                // Read file
                FileStream file = File.OpenRead(files[index]);

                // Serializes objects from xml file
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(PluckList));

                // Deserializes xml objects as plucklist properties
                PluckList pluckList = (PluckList?)xmlSerializer.Deserialize(file);

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
                file.Close();
            }

            //Print options
            Console.WriteLine("\n\nOptions:");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Q");
            Console.ForegroundColor = standardColor;
            Console.WriteLine("uit");
            if (index >= 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("A");
                Console.ForegroundColor = standardColor;
                Console.WriteLine("fslut plukseddel");
            }
            if (index > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("F");
                Console.ForegroundColor = standardColor;
                Console.WriteLine("orrige plukseddel");
            }
            if (index < files.Count - 1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("N");
                Console.ForegroundColor = standardColor;
                Console.WriteLine("æste plukseddel");
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("G");
            Console.ForegroundColor = standardColor;
            Console.WriteLine("enindlæs pluksedler");

            // Takes input
            readKey = Console.ReadKey().KeyChar;
            if (readKey >= 'a')
            {
                readKey = char.ToUpper(readKey);
            }
            Console.Clear();

            // Handles input
            Console.ForegroundColor = ConsoleColor.Red; //status in red
            switch (readKey)
            {
                case 'G':
                    // Refresh file contents
                    files = Directory.EnumerateFiles(filePath).ToList();
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
            Console.ForegroundColor = standardColor; //reset color

        }
    }
}
