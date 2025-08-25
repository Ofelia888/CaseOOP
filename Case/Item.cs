using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluckList
{
    public enum ItemType
    {
        Physical, Print
    }
    public class Item
    {
        public string? ProductID;
        public string? Title;
        public ItemType Type;
        public int Amount;
    }
}
