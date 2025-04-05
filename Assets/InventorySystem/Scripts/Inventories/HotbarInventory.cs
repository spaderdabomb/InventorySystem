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

            if (inventoryUI.SlotsUI.Length != slotActions.Length)
            {
                Debug.LogError($"Number of slot actions does not match number of {this} slots");
                return;
            }

            for (int i = 0; i < inventoryUI.SlotsUI.Length && i < slotActions.Length; i++)
            {
                string hotkey = slotActions[i].bindings.Count > 0 ? slotActions[i].GetBindingDisplayString() : (i + 1).ToString();
                inventoryUI.SlotsUI[i].SetHotkeyText(hotkey);
            }
        }
    }
}
