using UnityEngine;

namespace InventorySystem
{
    [RequireComponent(typeof(BaseInventory))]
    public class GearComponent : InteractableComponent
    {

        public override bool CanMoveItem(InventorySlot fromSlot, InventorySlot toSlot)
        {
            bool baseCanMove = base.CanMoveItem(fromSlot, toSlot);
            bool validItemType = fromSlot.inventoryItem.baseItem.itemType == toSlot.RequiredItemType;

            return baseCanMove && validItemType;
        }
    }
}
