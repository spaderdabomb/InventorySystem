using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class BaseInventory<TSlot> : MonoBehaviour where TSlot : BaseSlot
    {
        [SerializeField] private protected int rows;
        [SerializeField] private protected int columns;
        [SerializeField] private protected GameObject slotContainer;
        [SerializeField] private protected GameObject baseSlotPrefab;

        private protected int numSlots;
        private protected TSlot[] slots;

        private void Start()
        {
            InitSlots();
            ResizeSlotContainer();
        }

        public virtual void InitSlots()
        {
            numSlots = rows * columns;
            slots = new TSlot[numSlots];

            for (int i = 0; i < numSlots; i++)
            {
                GameObject newBaseSlot = Instantiate(baseSlotPrefab, slotContainer.transform);
                slots[i] = newBaseSlot.GetComponent<TSlot>();
            }

            print("new slots");

        }

        private void ResizeSlotContainer()
        {
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
            TSlot baseSlot = slots[index];
            InventoryItem existingItem = baseSlot.inventoryItem ?? new InventoryItem(inventoryItem.baseItem, 0);

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
            TSlot baseSlot = slots[index];
            int quantityInSlot = Mathf.Max(baseSlot.inventoryItem.quantity - inventoryItem.quantity, 0);
            int quantityRemaining = Mathf.Max(inventoryItem.quantity - baseSlot.inventoryItem.quantity, 0);

            InventoryItem newItem = new InventoryItem(inventoryItem.baseItem, quantityInSlot);
            SetSlotItem(newItem, index);

            return quantityRemaining;
        }

        private void SetSlotItem(InventoryItem inventoryItem, int index)
        {
            slots[index].SetSlotItem(inventoryItem);
        }

        public void MoveItem(TSlot fromSlot, TSlot toSlot)
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
