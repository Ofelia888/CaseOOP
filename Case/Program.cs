
//Eksempel på funktionel kodning hvor der kun bliver brugt et model lag
namespace PluckList;

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
        FileMover fileMover = new FileMover(files);

        ColorHandle colorHandle = new ColorHandle();

        PluckListPrinter pluckListPrinter;
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

                // Serializes xml contents to plucklist
                using var fileStream = fileReader.ReadSingle(index, files);
                PluckList pluckList = fileReader.SerializeXmlTo<PluckList>(fileStream);

                // Prints properties from plucklist
                pluckListPrinter = new PluckListPrinter(pluckList);

                pluckListPrinter.Print();
                pluckListPrinter.PrintItems();
            }

            //Print options
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

            // Takes input
            readKey = Console.ReadKey().KeyChar;
            readKey = char.ToUpper(readKey);

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
                    break;
            }

            colorHandle.Handle(ColorContext.Standard);

        }
    }
}
