using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    [RequireComponent(typeof(SelectableComponent), typeof(InteractableComponent))]
    public class HotbarInventory : BaseInventory
    {
        public void Start()
        {
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
    }
}
