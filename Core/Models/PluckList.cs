using Core.io;

namespace Core.Models;
public class PluckList
{
    public string? Name;
    public string? Shipment;
    public string? Address;
    public List<Item> Lines = new List<Item>();

    public Item? GetPrintItem()
    {
        return Lines.FirstOrDefault(item => item.Type == ItemType.Print);
    }

    public static PluckList? Deserialize(string filePath)
    {
        return new XMLReader(filePath).Read<PluckList>();
    }
}
