using UnityEngine;

namespace InventorySystem
{
    [RequireComponent(typeof(BaseInventory))]
    public class InteractableComponent : MonoBehaviour
    {
        private BaseInventory baseInventory;

        private void Start()
        {
            baseInventory = GetComponent<BaseInventory>();
        }

        public virtual bool CanMoveItem(InventorySlot fromSlot, InventorySlot toSlot)
        {
            return fromSlot.inventoryItem != null;
        }

        public void MoveItem(InventorySlot fromSlot, InventorySlot toSlot)
        {
            if (!CanMoveItem(fromSlot, toSlot))
                return;

            InventoryItem movedItem = fromSlot.inventoryItem;
            InventoryItem targetItem = toSlot.inventoryItem;

            // If the target slot is empty, just move the item
            if (targetItem == null)
            {
                toSlot.ParentInventory.SetSlotItem(movedItem, toSlot);
                fromSlot.ParentInventory.SetSlotItem(null, fromSlot);
                return;
            }

            // If the items are the same type, try to merge them
            if (targetItem.baseItem.id == movedItem.baseItem.id)
            {
                int combinedQuantity = targetItem.quantity + movedItem.quantity;
                int maxStack = movedItem.baseItem.maxStack;

                // If combined quantity exceeds max stack, leave the excess in the fromSlot
                if (combinedQuantity > maxStack)
                {
                    toSlot.ParentInventory.SetSlotItem(new InventoryItem(movedItem.baseItem, maxStack), toSlot);
                    fromSlot.ParentInventory.SetSlotItem(new InventoryItem(movedItem.baseItem, combinedQuantity - maxStack), fromSlot);
                }
                else
                {
                    toSlot.ParentInventory.SetSlotItem(new InventoryItem(movedItem.baseItem, combinedQuantity), toSlot);
                    fromSlot.ParentInventory.SetSlotItem(null, fromSlot);
                }
            }
            else
            {
                // Swap items if they are different
                toSlot.ParentInventory.SetSlotItem(movedItem, toSlot);
                fromSlot.ParentInventory.SetSlotItem(targetItem, fromSlot);
            }
        }

        public bool SplitItem(InventorySlot slotToSplit)
        {
            if (slotToSplit == null || slotToSplit.inventoryItem == null || slotToSplit.inventoryItem.quantity <= 1)
                return false;

            BaseItem itemToSplit = slotToSplit.inventoryItem.baseItem;
            int totalQuantity = slotToSplit.inventoryItem.quantity;
            int halfQuantity = totalQuantity / 2; // Integer division rounds down automatically
            int remainingQuantity = totalQuantity - halfQuantity;

            InventorySlot slot = baseInventory.GetFirstEmptySlot();
            if (slot == null)
            {
                Debug.LogWarning("Cannot split: no free slot available");
                return false;
            }

            // Update the original and target slots with new items
            slotToSplit.ParentInventory.SetSlotItem(new InventoryItem(itemToSplit, remainingQuantity), slotToSplit);
            baseInventory.SetSlotItem(new InventoryItem(itemToSplit, halfQuantity), slot);

            return true;
        }
    }
}
