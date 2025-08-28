namespace PluckList.Printer;

public class OptionPrinter() : ContextPrinter(new ConsolePrinter(), new OptionContext())
{
    private class OptionContext : IContext
    {
        public void Handle(IPrinter printer, string text)
        {
            if (text.Length == 0)
            {
                Console.WriteLine();
                return;
            }
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(text[0]);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(text[1..]);
            Console.ForegroundColor = color;
        }
    }
}
