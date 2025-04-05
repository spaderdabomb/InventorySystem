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
        public void MoveItem(InventorySlot fromSlot, InventorySlot toSlot)
        {
            if (fromSlot.inventoryItem == null)
                return;

            InventoryItem movedItem = fromSlot.inventoryItem;
            InventoryItem targetItem = toSlot.inventoryItem;

            // If the target slot is empty, just move the item
            if (targetItem == null)
            {
                toSlot.SetSlotItem(movedItem);
                fromSlot.SetSlotItem(null);
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
                    toSlot.SetSlotItem(new InventoryItem(movedItem.baseItem, maxStack));
                    fromSlot.SetSlotItem(new InventoryItem(movedItem.baseItem, combinedQuantity - maxStack));
                }
                else
                {
                    toSlot.SetSlotItem(new InventoryItem(movedItem.baseItem, combinedQuantity));
                    fromSlot.SetSlotItem(null);
                }
            }
            else
            {
                // Swap items if they are different
                toSlot.SetSlotItem(movedItem);
                fromSlot.SetSlotItem(targetItem);
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
            slotToSplit.SetSlotItem(new InventoryItem(itemToSplit, remainingQuantity));
            InventoryItem newSplitItem = new InventoryItem(itemToSplit, halfQuantity);
            baseInventory.SetSlotItem(newSplitItem, slot);

            return true;
        }
    }
}
