using System.Linq;
using UnityEngine;

namespace InventorySystem
{
    public class BaseInventory : MonoBehaviour
    {
        [SerializeField] public int rows;
        [SerializeField] public int columns;

        [SerializeField] public BaseInventoryUI inventoryUI;
        [HideInInspector] public InventorySlot[] Slots { get; private set; } = null;

        public int NumSlots { get; private set; }

        public virtual void Awake()
        {
            NumSlots = rows * columns;
            Slots = new InventorySlot[NumSlots];

            for (int i = 0; i < NumSlots; i++)
            {
                Slots[i] = new InventorySlot(this);
            }

            ShowOnAwake();
        }

        public virtual void Initialize(BaseInventoryUI inventoryUI)
        {
            this.inventoryUI = inventoryUI;
        }

        public virtual void ShowOnAwake()
        {
            if (inventoryUI != null && inventoryUI.gameObject.activeSelf)
                inventoryUI.ShowInventory(this);
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
            inventoryUI.SlotsUI[slot.GetIndexOf()].SetSlotItem();
        }

        public InventorySlot GetFirstEmptySlot()
        {
            return Slots.FirstOrDefault(slot => !slot.ContainsItem());
        }

        public InventorySlot GetFirstFreeSlot(InventoryItem inventoryItem)
        {
            return Slots.FirstOrDefault(slot =>
                !slot.ContainsItem() ||
                (slot.inventoryItem.baseItem.id == inventoryItem.baseItem.id &&
                 slot.inventoryItem.quantity < inventoryItem.baseItem.maxStack)
            );
        }

        public InventorySlot GetFirstSlotWithItem(BaseItem baseItem)
        {
            return Slots.FirstOrDefault(slot => slot.inventoryItem?.baseItem.id == baseItem.id);
        }

        public int GetTotalItemQuantity(BaseItem baseItem)
        {
            return Slots
                .Where(slot => slot.ContainsItem() && slot.inventoryItem.baseItem.id == baseItem.id)
                .Sum(slot => slot.inventoryItem.quantity);
        }
    }

    public abstract class DumbBase<T> : MonoBehaviour
    {
        public T inventoryUI;
    }
}
