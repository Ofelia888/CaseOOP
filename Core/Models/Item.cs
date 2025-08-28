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
        public int Total = 100;
        public bool IsLeftover()
        {
            return Total - Amount > 0;
        }
    }
}
