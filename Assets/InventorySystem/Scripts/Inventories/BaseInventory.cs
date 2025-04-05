using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace InventorySystem
{
    public class BaseInventory : MonoBehaviour
    {
        [SerializeField] private int rows;
        [SerializeField] private int columns;

        [SerializeField] private GameObject slotContainer;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject inventoryUI;

        [HideInInspector] public InventorySlot[] slots;
        private int numSlots;

        public virtual void Awake()
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
                InventorySlot slot = GetFirstFreeSlot(inventoryItem);

                if (slot == null)
                {
                    Debug.Log("Inventory is full");
                    break;
                }

                quantityRemaining = AddItemToSlot(inventoryItem, slot);
            }

            return quantityRemaining;
        }

        private int AddItemToSlot(InventoryItem inventoryItem, InventorySlot slot)
        {
            InventoryItem existingItem = slot.inventoryItem ?? new InventoryItem(inventoryItem.baseItem, 0);

            int totalQuantity = existingItem.quantity + inventoryItem.quantity;
            int slotQuantity = totalQuantity >= existingItem.baseItem.maxStack ? existingItem.baseItem.maxStack : totalQuantity;
            int quantityRemaining = totalQuantity - slotQuantity;

            InventoryItem newItem = new InventoryItem(inventoryItem.baseItem, slotQuantity);
            SetSlotItem(newItem, slot);

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
            while (quantityRemaining > 0)
            {
                InventorySlot slot = GetFirstSlotWithItem(baseItem);
                InventoryItem newItem = new InventoryItem(baseItem, quantityRemaining);
                quantityRemaining = RemoveItemFromSlot(newItem, slot);
            }

            return true;
        }

        private int RemoveItemFromSlot(InventoryItem inventoryItem, InventorySlot slot)
        {
            int quantityInSlot = Mathf.Max(slot.inventoryItem.quantity - inventoryItem.quantity, 0);
            int quantityRemaining = Mathf.Max(inventoryItem.quantity - slot.inventoryItem.quantity, 0);

            InventoryItem newItem = new InventoryItem(inventoryItem.baseItem, quantityInSlot);
            SetSlotItem(newItem, slot);

            return quantityRemaining;
        }

        public void SetSlotItem(InventoryItem inventoryItem, InventorySlot slot)
        {
            slot.SetSlotItem(inventoryItem);
        }

        public InventorySlot GetFirstEmptySlot()
        {
            return slots.FirstOrDefault(slot => !slot.ContainsItem());
        }

        public InventorySlot GetFirstFreeSlot(InventoryItem inventoryItem)
        {
            return slots.FirstOrDefault(slot =>
                !slot.ContainsItem() ||
                (slot.inventoryItem.baseItem.id == inventoryItem.baseItem.id &&
                 slot.inventoryItem.quantity < inventoryItem.baseItem.maxStack)
            );
        }

        public InventorySlot GetFirstSlotWithItem(BaseItem baseItem)
        {
            return slots.FirstOrDefault(slot => slot.inventoryItem?.baseItem.id == baseItem.id);
        }

        public int GetTotalItemQuantity(BaseItem baseItem)
        {
            return slots
                .Where(slot => slot.ContainsItem() && slot.inventoryItem.baseItem.id == baseItem.id)
                .Sum(slot => slot.inventoryItem.quantity);
        }

        public void ShowInventory()
        {
            if (inventoryUI == null)
            {
                Debug.LogWarning($"Inventory UI for {this} not set - set in inspector");
                return;
            }

            inventoryUI.SetActive(true);
        }

        public void HideInventory()
        {
            if (inventoryUI == null)
            {
                Debug.LogWarning($"Inventory UI for {this} not set - set in inspector");
                return;
            }

            inventoryUI.SetActive(false);
        }
    }
}
