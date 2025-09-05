using Core.io;

namespace Core.Models;
public class Pluklist
{
    public string? Name;
    public string? Forsendelse;
    public string? Adresse;
    public List<Item> Lines = new List<Item>();

    public Item? GetPrintItem()
    {
        return Lines.FirstOrDefault(item => item.Type == ItemType.Print);
    }

    public static Pluklist? Deserialize(string filePath)
    {
        return new XMLReader(filePath).Read<Pluklist>();
    }
}
