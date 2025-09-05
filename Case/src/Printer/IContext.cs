namespace PluckList.Printer;

public interface IContext
{
    void Handle(IPrinter printer, string text);
}
