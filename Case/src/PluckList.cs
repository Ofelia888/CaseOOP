using PluckList.src.io;

namespace PluckList.src;
public class PluckList
{
    public string? Name { get; set; }
    public string? Shipment { get; set; }
    public string? Address { get; set; }
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
