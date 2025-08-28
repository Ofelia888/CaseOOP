namespace Core.Models
{
    public enum ItemType
    {
        Fysisk, Print
    }
    public class Item
    {
        public string? ProductID;
        public string? Title;
        public ItemType Type;
        public int Amount;
    }
}
