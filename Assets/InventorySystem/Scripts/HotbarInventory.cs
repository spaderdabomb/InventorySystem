using UnityEngine;

namespace InventorySystem
{
    public class HotbarInventory : BaseInventory<HotbarSlot>
    {
        public override void InitSlots()
        {
            numSlots = rows * columns;
            slots = new HotbarSlot[numSlots];

            print(slots);

            for (int i = 0; i < numSlots; i++)
            {
                GameObject newBaseSlot = Instantiate(baseSlotPrefab, slotContainer.transform);
                slots[i] = newBaseSlot.GetComponent<HotbarSlot>();

                if (i == 0)
                    slots[i].SetSlotSelected();
            }
        }
    }
}
