using UnityEngine;

namespace InventorySystem
{
    public class InventoryItem
    {
        public BaseItem baseItem;
        public int quantity;

        public InventoryItem(BaseItem baseItem, int quantity)
        {
            this.baseItem = baseItem;
            this.quantity = quantity;
        }
    }
}
