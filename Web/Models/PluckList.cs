namespace Web.Models;

public class PluckList
{
    public Guid Id;
    public string Name;
    public string Shipment;
    public string Address;
    public List<PluckListItem> Items;
}
