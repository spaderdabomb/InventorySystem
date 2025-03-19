using UnityEngine;

namespace InventorySystem
{
    public class HotbarSlot : BaseSlot
    {
        public bool IsSlotSelected { get; private set; } = false;

        public void SetSlotSelected()
        {
            IsSlotSelected = true;
        }

        public void SetSlotDeselected()
        {
            IsSlotSelected = false;
        }
    }
}
