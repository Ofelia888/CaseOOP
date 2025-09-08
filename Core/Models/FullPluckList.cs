namespace Core.Models;

public class FullPluckList
{
    public Guid Id;
    public string Name;
    public string Shipment;
    public string Address;
    public List<Item> Items;
    public bool Archived = false;
}
