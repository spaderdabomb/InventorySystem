using System;
using UnityEngine;

namespace InventorySystem
{
    public class InventorySlot
    {
        public InventoryItem inventoryItem = null;
        public ItemType RequiredItemType { get; private set; } = ItemType.None;
        public BaseInventory ParentInventory { get; private set; }

        public InventorySlot(BaseInventory  parentInventory)
        {
            ParentInventory = parentInventory;
        }

        public void SetSlotItem(InventoryItem inventoryItem)
        {
            if (inventoryItem == null || inventoryItem?.quantity == 0)
            {
                ClearSlotItem();
                return;
            }

            this.inventoryItem = inventoryItem;
        }

        private void ClearSlotItem()
        {
            inventoryItem = null;
        }

        public int GetIndexOf()
        {
            return Array.IndexOf(ParentInventory.Slots, this);
        }

        public bool ContainsItem()
        {
            return inventoryItem != null;
        }

        public void SetItemType(ItemType itemType)
        {
            RequiredItemType = itemType;
        }
    }
}
