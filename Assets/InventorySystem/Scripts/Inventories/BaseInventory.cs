using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InventorySystem
{
    public class BaseInventory : MonoBehaviour
    {
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] GameObject slotContainer;
        [SerializeField] GameObject slotPrefab;

        private int numSlots;
        private protected InventorySlot[] slots;

        public virtual void Start()
        {
            numSlots = rows * columns;
            slots = new InventorySlot[numSlots];

            for (int i = 0; i < numSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab, slotContainer.transform);
                slots[i] = newSlot.GetComponent<InventorySlot>();
            }

            RectTransform slotContainerRT = slotContainer.GetComponent<RectTransform>();
            GridLayoutGroup layoutGroup = slotContainer.GetComponent<GridLayoutGroup>();
            float width = (layoutGroup.cellSize.x + layoutGroup.spacing.x) * columns + 2 * layoutGroup.spacing.x;
            float height = (layoutGroup.cellSize.y + layoutGroup.spacing.y) * rows + 2 * layoutGroup.spacing.y;
            slotContainerRT.sizeDelta = new Vector2(width, height);
        }

        public int AddItem(BaseItem baseItem, int quantity)
        {
            if (baseItem == null)
            {
                Debug.LogWarning("Trying to add null item to inventory");
                return 0;
            }

            int quantityRemaining = quantity;
            while (quantityRemaining > 0)
            {
                InventoryItem inventoryItem = new InventoryItem(baseItem, quantityRemaining);
                int? freeSlotIdx = GetFirstFreeSlot(inventoryItem);

                // Handle full inventory
                if (freeSlotIdx == null)
                {
                    Debug.Log("Inventory is full");
                    break;
                }

                quantityRemaining = AddItemToSlot(inventoryItem, (int)freeSlotIdx);
            }

            return quantityRemaining;
        }

        private int AddItemToSlot(InventoryItem inventoryItem, int index)
        {
            InventorySlot slot = slots[index];
            InventoryItem existingItem = slot.inventoryItem ?? new InventoryItem(inventoryItem.baseItem, 0);

            int totalQuantity = existingItem.quantity + inventoryItem.quantity;
            int slotQuantity = totalQuantity >= existingItem.baseItem.maxStack ? existingItem.baseItem.maxStack : totalQuantity;
            int quantityRemaining = totalQuantity - slotQuantity;

            InventoryItem newItem = new InventoryItem(inventoryItem.baseItem, slotQuantity);
            SetSlotItem(newItem, index);

            return quantityRemaining;
        }

        public bool RemoveItem(BaseItem baseItem, int quantity)
        {
            if (baseItem == null)
            {
                Debug.LogWarning("Trying to remove a null item from inventory.");
                return false;
            }

            if (GetTotalItemQuantity(baseItem) < quantity)
                return false;

            int quantityRemaining = quantity;
            int i = 0;
            while (quantityRemaining > 0)
            {
                int slotIndex = (int)GetFirstSlotWithItem(baseItem);
                InventoryItem newItem = new InventoryItem(baseItem, quantityRemaining);
                quantityRemaining = RemoveItemFromSlot(newItem, slotIndex);

                i++;
                if (i > 100)
                    break;
            }

            return true;
        }

        private int RemoveItemFromSlot(InventoryItem inventoryItem, int index)
        {
            InventorySlot slot = slots[index];
            int quantityInSlot = Mathf.Max(slot.inventoryItem.quantity - inventoryItem.quantity, 0);
            int quantityRemaining = Mathf.Max(inventoryItem.quantity - slot.inventoryItem.quantity, 0);

            InventoryItem newItem = new InventoryItem(inventoryItem.baseItem, quantityInSlot);
            SetSlotItem(newItem, index);

            return quantityRemaining;
        }

        private void SetSlotItem(InventoryItem inventoryItem, int index)
        {
            slots[index].SetSlotItem(inventoryItem);
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
            {
                Debug.LogWarning("Cannot split: slot is empty or has only 1 item");
                return false;
            }

            BaseItem itemToSplit = slotToSplit.inventoryItem.baseItem;
            int totalQuantity = slotToSplit.inventoryItem.quantity;
            int halfQuantity = totalQuantity / 2; // Integer division rounds down automatically
            int remainingQuantity = totalQuantity - halfQuantity;

            // Create new inventory item with half the quantity
            InventoryItem newSplitItem = new InventoryItem(itemToSplit, halfQuantity);

            // Find the first available slot
            int? freeSlotIdx = GetFirstFreeSlot(newSplitItem);
            if (freeSlotIdx == null)
            {
                Debug.LogWarning("Cannot split: no free slot available");
                return false;
            }

            // Update the original slot with remaining quantity
            slotToSplit.SetSlotItem(new InventoryItem(itemToSplit, remainingQuantity));

            // Add the split item to the free slot
            SetSlotItem(newSplitItem, (int)freeSlotIdx);

            return true;
        }

        private int? GetFirstFreeSlot(InventoryItem inventoryItem)
        {
            int? freeSlotIdx = null;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].inventoryItem != null && 
                    (slots[i].inventoryItem.baseItem.id != inventoryItem.baseItem.id ||
                    slots[i].inventoryItem.quantity >= inventoryItem.baseItem.maxStack))
                {
                    continue;
                }

                freeSlotIdx = i;
                break;
            }

            return freeSlotIdx;
        }

        private int? GetFirstSlotWithItem(BaseItem baseItem)
        {
            int? firstItemIdx = null;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].inventoryItem?.baseItem.id != baseItem.id) 
                    continue;

                firstItemIdx = i;
                break;
            }

            return firstItemIdx;
        }

        public int GetTotalItemQuantity(BaseItem baseItem)
        {
            int totalQuantity = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].inventoryItem == null ||
                    slots[i].inventoryItem.baseItem.id != baseItem.id)
                {
                    continue;
                }

                totalQuantity += slots[i].inventoryItem.quantity;
            }

            return totalQuantity;
        }
    }
}
