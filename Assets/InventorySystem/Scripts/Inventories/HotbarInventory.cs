using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    public class HotbarInventory : BaseInventory
    {
        [HideInInspector] public int selectedIndex = 0;

        public override void Start()
        {
            base.Start();
            SetSelectedSlot(selectedIndex);
            InitHotkeyLabels();
        }

        private void InitHotkeyLabels()
        {
            InputAction[] slotActions = InventoryManager.Instance.inventoryInput.slotActions;
            for (int i = 0; i < slots.Length && i < slotActions.Length; i++)
            {
                string hotkey = slotActions[i].bindings.Count > 0 ? slotActions[i].GetBindingDisplayString() : (i + 1).ToString();
                slots[i].SetHotkeyText(hotkey);
            }
        }

        public void SetSelectedSlot(int index)
        {
            selectedIndex = index;

            for (int i = 0; i < slots.Length; i++)
            {
                if (i == selectedIndex)
                    slots[i].SetSelected();
                else
                    slots[i].ClearSlotSelection();
            }
        }
    }
}
