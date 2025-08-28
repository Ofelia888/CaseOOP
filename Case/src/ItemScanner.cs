using Core.Models;
using PluckList.Printer;

namespace PluckList
{
    public class ItemScanner
    {
        private readonly StatePrinter _printer;

        public ItemScanner(StatePrinter printer)
        {
            _printer = printer;
        }

        public List<Item> ScanItems(Core.Models.PluckList pluckList)
        {
            List<Item> scannedItems = new List<Item>();
            IPrinter printer = new OptionPrinter();
            char readKey = ' ';

            if (pluckList == null)
            {
                throw new NullReferenceException();
            }

            List<Item> scannable = pluckList.Lines.ToList();
            while (scannable.Count() > 0)
            {
                for (int i = 0; i < scannable.Count(); i++)
                {
                    Item item = scannable[i];
                    if (item.Title == null || item.Title.Length == 0)
                    {
                        continue;
                    }
                    printer.Print(item.Title);
                }
                printer.Print("Færdig");

            
                _printer.Print();
                readKey = Console.ReadKey().KeyChar;
                readKey = char.ToUpper(readKey);

                if (readKey == 'F')
                {
                    _printer.Clear();
                    return scannedItems;
                }

                _printer.Clear();
                for (int i = 0; i < scannable.Count(); i++)
                {
                    Item item = scannable[i];
                    if (readKey == item.Title?.First())
                    {
                        scannedItems.Add(item);
                        scannable.Remove(item);
                        
                        _printer.State = PrintState.Status;
                        _printer.Print($"\n{item.Title} scannet");
                        _printer.State = PrintState.Standard;
                    }
                }
            }

            return scannedItems;
        }
    }
}
