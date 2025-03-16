using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class BaseInventory : MonoBehaviour
{
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] GameObject slotContainer;
    [SerializeField] GameObject baseSlotPrefab;

    private int numSlots;
    private BaseSlot[] slots;

    private void Start()
    {
        numSlots = rows * columns;
        slots = new BaseSlot[numSlots];

        for (int i = 0; i < numSlots; i++)
        {
            GameObject newBaseSlot = Instantiate(baseSlotPrefab, slotContainer.transform);
            slots[i] = newBaseSlot.GetComponent<BaseSlot>();
        }

        RectTransform slotContainerRT = slotContainer.GetComponent<RectTransform>();
        GridLayoutGroup layoutGroup = slotContainer.GetComponent<GridLayoutGroup>();
        float width = (layoutGroup.cellSize.x + layoutGroup.spacing.x) * columns + 2 * layoutGroup.spacing.x;
        float height = (layoutGroup.cellSize.y + layoutGroup.spacing.y) * rows + 2 * layoutGroup.spacing.y;
        slotContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    public int AddItem(BaseItem baseItem, int quantity)
    {
        if (baseItem == null)
        {
            Debug.LogWarning("Trying to add null item to inventory");
            return 0;
        }

        int quantityRemaining = quantity;
        InventoryItem inventoryItem = new InventoryItem(baseItem, quantity);

        int i = 0;
        while (quantityRemaining > 0)
        {
            int? freeSlotIdx = GetFirstFreeSlot(inventoryItem);

            // Handle full inventory
            if (freeSlotIdx == null)
            {
                Debug.Log("Inventory is full");
                break;
            }

            quantityRemaining = AddItemToSlot(inventoryItem, (int)freeSlotIdx);

            i++;
            if (i > 100)
                break;
        }

        return quantityRemaining;
    }

    private int AddItemToSlot(InventoryItem inventoryItem, int index)
    {
        BaseSlot baseSlot = slots[index];
        int totalQuantity = baseSlot.inventoryItem.quantity + inventoryItem.quantity;
        int quantityRemaining = baseSlot.inventoryItem.baseItem.maxStack - totalQuantity;
        int slotQuantity = quantityRemaining > 0 ? baseSlot.inventoryItem.baseItem.maxStack : totalQuantity;

        InventoryItem newItem = new InventoryItem(inventoryItem.baseItem, slotQuantity);
        SetSlotItem(newItem, index);

        return slotQuantity;
    }

    public bool RemoveItem(BaseItem baseItem, int quantity)
    {
        if (baseItem == null)
            Debug.LogWarning("Trying to remove null item from inventory");

        // Check if there are enough items in inventory to remove
        int totalQuantity = GetTotalItemQuantity(baseItem);
        if (totalQuantity < quantity)
            return false;

        int quantityRemaining = quantity;
        int i = 0;
        while (quantityRemaining > 0)
        {
            int firstItemSlotIdx = (int)GetFirstSlotWithItem(baseItem);
            BaseSlot baseSlot = slots[firstItemSlotIdx];
            if (baseSlot.inventoryItem.quantity < quantity)
                return false;

            InventoryItem newItem = new InventoryItem(baseItem, quantity);
            RemoveItemFromSlot(newItem, firstItemSlotIdx);

            i++;
            if (i > 100)
                break;
        }

        return true;

    }

    private int RemoveItemFromSlot(InventoryItem inventoryItem, int index)
    {
        return 1;
    }

    public void MoveItem()
    {

    }

    private void SetSlotItem(InventoryItem inventoryItem, int index)
    {
        slots[index].SetSlotItem(inventoryItem);
    }

    private int? GetFirstFreeSlot(InventoryItem inventoryItem)
    {
        int? freeSlotIdx = null;
        for (int i = 0; i < slots.Length;i++)
        {
            if (slots[i] != null && slots[i].inventoryItem.baseItem.id != inventoryItem.baseItem.id) continue;

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
            if (slots[i].inventoryItem.baseItem.id != baseItem.id) continue;

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
            if (slots[i].inventoryItem.baseItem.id != baseItem.id) continue;

            totalQuantity += slots[i].inventoryItem.quantity;
        }

        return totalQuantity;
    }
}

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
