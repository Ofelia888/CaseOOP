namespace PluckList.Printer
{
    public abstract class ConsolePrinter : IPrint
    {
        public abstract void Print(string text = "default");
    }
}
