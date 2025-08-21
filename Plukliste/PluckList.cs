namespace Plukliste;

// The pluklist class containing a list, identifier properties and a method for adding items to the list
public class PluckList
{
    public string Name { get; set; }
    public string Shipment { get; set; }
    public string Address { get; set; }
    public List<Item> Lines { get; set; }
    public PluckList(string name, string shipment, string address)
    {
        Name = name;
        Shipment = shipment;
        Address = address;
        Lines = new List<Item>();
    }
    public void AddItem(Item item)
    {
        Lines.Add(item);
    }
}

// The item class containing identifier properties
public class Item
{
    public string ProductID { get; set; }
    public string Title { get; set; }
    public ItemType Type { get; set; }
    public int Amount { get; set; }
    public Item(string productID, string title, ItemType type, int amount)
    {
        ProductID = productID;
        Title = title;
        Type = type;
        Amount = amount;
    }
}

public enum ItemType
{
    Fysisk,
    Print
}



