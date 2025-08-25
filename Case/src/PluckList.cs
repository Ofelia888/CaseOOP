using System.Xml.Serialization;

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
        using var fileStream = File.OpenRead(filePath);
        var xmlSerializer = new XmlSerializer(typeof(PluckList));
        return (PluckList?)xmlSerializer.Deserialize(fileStream);
    }

    //public void AddItem(Item item)
    //{
    //    Lines.Add(item);
    //}
}



