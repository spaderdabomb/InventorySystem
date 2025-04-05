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
            if (!baseInventory.Slots[index].ContainsItem() && !canSelectEmptySlots)
                return;

            SelectedIndex = index;

            for (int i = 0; i < baseInventory.Slots.Length; i++)
            {
                if (i == SelectedIndex)
                    baseInventory.inventoryUI.SlotsUI[i].SetSelected();
                else
                    baseInventory.inventoryUI.SlotsUI[i].ClearSlotSelection();
            }
        }
    }
}
