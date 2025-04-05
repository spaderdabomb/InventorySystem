using UnityEngine;

namespace InventorySystem
{
    [RequireComponent(typeof(BaseInventory))]
    public class SelectableComponent : MonoBehaviour
    {
        private BaseInventory baseInventory;
        public bool canSelectEmptySlots = true;
        public int SelectedIndex { get; private set; } = 0;

        private void Start()
        {
            baseInventory = GetComponent<BaseInventory>();
            SetSelectedSlot(SelectedIndex);
        }

        public void SetSelectedSlot(int index)
        {
            if (!baseInventory.slots[index].ContainsItem() && !canSelectEmptySlots)
                return;

            SelectedIndex = index;

            for (int i = 0; i < baseInventory.slots.Length; i++)
            {
                if (i == SelectedIndex)
                    baseInventory.slots[i].SetSelected();
                else
                    baseInventory.slots[i].ClearSlotSelection();
            }
        }
    }
}
