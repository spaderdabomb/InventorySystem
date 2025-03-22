using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    public class InventoryInput : MonoBehaviour
    {
        public InventoryActions inventoryActions;
        public InputAction[] slotActions;

        private void Awake()
        {
            inventoryActions = new InventoryActions();
            inventoryActions.Enable();
            inventoryActions.HotbarActionMap.Enable();

            slotActions = new InputAction[]
            {
                inventoryActions.HotbarActionMap.SelectSlot1,
                inventoryActions.HotbarActionMap.SelectSlot2,
                inventoryActions.HotbarActionMap.SelectSlot3,
                inventoryActions.HotbarActionMap.SelectSlot4,
                inventoryActions.HotbarActionMap.SelectSlot5,
                inventoryActions.HotbarActionMap.SelectSlot6,
                inventoryActions.HotbarActionMap.SelectSlot7,
                inventoryActions.HotbarActionMap.SelectSlot8
            };
        }

        private void OnEnable()
        {
            for (int i = 0; i < slotActions.Length; i++)
            {
                int index = i;
                slotActions[i].performed += ctx => SetSlotActive(index);
            }

            inventoryActions.HotbarActionMap.InventoryClick.performed += InventoryClick;
            inventoryActions.HotbarActionMap.Split.performed += Split;
        }

        private void OnDisable()
        {
            for (int i = 0; i < slotActions.Length; i++)
            {
                int index = i;
                slotActions[i].performed -= ctx => SetSlotActive(index);
            }

            inventoryActions.Disable();
            inventoryActions.HotbarActionMap.Disable();
        }

        private void SetSlotActive(int index)
        {
            InventoryManager.Instance.hotbarInventory.SetSelectedSlot(index);
        }

        private void InventoryClick(InputAction.CallbackContext context)
        {
            InventoryManager.Instance.InventoryClick();
        }

        private void Split(InputAction.CallbackContext context)
        {
            InventoryManager.Instance.SplitItem();
        }
    }
}
