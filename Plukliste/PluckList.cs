namespace PluckList;
public class PluckList
{
    public string Name { get; set; }
    public string Shipment { get; set; }
    public string Address { get; set; }
    public List<Item> Lines = new List<Item>();


    //public void AddItem(Item item)
    //{
    //    Lines.Add(item);
    //}
}



