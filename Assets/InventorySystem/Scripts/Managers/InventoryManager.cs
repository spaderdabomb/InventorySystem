using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        public BaseItem defaultItem;

        public BaseInventory playerInventory;
        public HotbarInventory hotbarInventory;

        public ChestManager chestManager;
        public ShopManager shopManager;

        public ItemTooltip itemTooltip;

        public AudioSource audioSource;
        public InventoryInput inventoryInput;

        public InventorySlotUI CurrentHoverSlot { get; private set; } = null;
        public InventorySlotUI CurrentClickedSlot { get; private set; } = null;

        private void Awake()
        {
            Instance = this;
        }

        public void InventoryMouseDown()
        {
            if (CurrentHoverSlot == null)
                return;

            CurrentClickedSlot = CurrentHoverSlot;
        }

        public void InventoryMouseUp()
        {
            if (CurrentClickedSlot == null || CurrentHoverSlot == null)
            {
                CurrentClickedSlot = null;
                return;
            }

            if (CurrentHoverSlot == CurrentClickedSlot &&
                CurrentClickedSlot.InventorySlot.ParentInventory.TryGetComponent(out SelectableComponent selectable))
            {
                selectable.SetSelectedSlot(CurrentClickedSlot.InventorySlot.GetIndexOf());
            }

            CurrentClickedSlot = null;
        }

        public void PlayInventorySound(AudioClip audioClip)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(audioClip);
        }

        public void SetCurrentHoverSlot(InventorySlotUI inventorySlot)
        {
            CurrentHoverSlot = inventorySlot;
        }

        public void SplitItem()
        {
            if (CurrentHoverSlot == null ||
                !CurrentHoverSlot.InventorySlot.ParentInventory.TryGetComponent(out InteractableComponent interactable))
            {
                return;
            }

            interactable.SplitItem(CurrentHoverSlot.InventorySlot);
        }

        public void BuyItem(BaseInventory fromInventory, BaseInventory toInventory)
        {
            if (fromInventory == null ||
                toInventory == null ||
                !fromInventory.TryGetComponent(out SelectableComponent selectable))
            {
                Debug.LogWarning($"{fromInventory} does not have a selectable component - cannot buy item");
                return;
            }

            BaseItem boughtItem = fromInventory.Slots[selectable.SelectedIndex].inventoryItem.baseItem;
            fromInventory.RemoveItem(boughtItem, 1);
            toInventory.AddItem(boughtItem, 1);
        }

        public void SellItem(BaseInventory fromInventory, BaseInventory toInventory)
        {
            if (fromInventory == null ||
                toInventory == null ||
                !toInventory.TryGetComponent(out SelectableComponent selectable))
            {
                Debug.LogWarning($"{toInventory} does not have a selectable component - cannot sell item");
                return;
            }

            BaseItem soldItem = toInventory.Slots[selectable.SelectedIndex].inventoryItem.baseItem;
            toInventory.RemoveItem(soldItem, 1);
        }
    }
}
