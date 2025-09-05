namespace PluckList.Printer;

public class StatePrinter : ConsolePrinter
{
    private readonly Dictionary<PrintState, ConsoleColor> _colors = new()
    {
        { PrintState.Standard, ConsoleColor.Gray },
        { PrintState.Status, ConsoleColor.Red },
    };
    
    public PrintState State = PrintState.Standard;

    private void Color(Action action)
    {
        var color = Console.ForegroundColor;
        Console.ForegroundColor = _colors.GetValueOrDefault(State, color);
        action.Invoke();
        Console.ForegroundColor = color;
    }

    public override void Print(string text)
    {
        Color(() => base.Print(text));
    }

    public override void Print(string format, params object?[]? args)
    {
        Color(() => base.Print(format, args));
    }
}
