using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSlot : MonoBehaviour
{
    public InventoryItem inventoryItem = null;

    public void SetSlotItem(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
    }
}
