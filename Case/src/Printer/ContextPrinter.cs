namespace PluckList.Printer;

public class ContextPrinter : IPrinter
{
    private readonly IPrinter _printer;
    private readonly IContext _context;
    
    protected IContext Context => _context;
    
    public ContextPrinter(IPrinter printer, IContext context)
    {
        _printer = printer;
        _context = context;
    }
    
    public void Print(string text)
    {
        _context.Handle(_printer, text);
    }
}
