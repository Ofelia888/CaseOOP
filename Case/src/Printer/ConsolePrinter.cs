using System.Diagnostics.CodeAnalysis;

namespace PluckList.Printer;

public class ConsolePrinter : IPrinter
{
    public virtual void Print(string text)
    {
        Console.WriteLine(text);
    }

    public virtual void Print([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[]? args)
    {
        Console.WriteLine(format, args);
    }

    public void Print()
    {
        Console.WriteLine();
    }

    public void Clear()
    {
        Console.Clear();
    }
}
