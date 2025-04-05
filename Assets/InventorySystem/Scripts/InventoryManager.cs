using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        public BaseItem defaultItem;

        public BaseInventory playerInventory;
        public HotbarInventory hotbarInventory;
        public List<BaseInventory> chestInventories;
        public List<BaseInventory> shopInventories;

        public GameObject chestInventoryPrefab;
        public GameObject shopInventoryPrefab;
        public GameObject chestContainer;
        public GameObject shopContainer;

        public AudioSource audioSource;
        public InventoryInput inventoryInput;

        public InventorySlot CurrentHoverSlot { get; private set; } = null;
        public InventorySlot CurrentClickedSlot { get; private set; } = null;

        private void Awake()
        {
            Instance = this;

            chestInventories = new List<BaseInventory>();
            shopInventories = new List<BaseInventory>();
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
                CurrentClickedSlot.ParentInventory.TryGetComponent(out SelectableComponent selectable))
            {
                selectable.SetSelectedSlot(CurrentClickedSlot.GetIndexOf());
            }

            CurrentClickedSlot = null;
        }

        public void PlayInventorySound(AudioClip audioClip)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(audioClip);
        }

        public void SetCurrentHoverSlot(InventorySlot inventorySlot)
        {
            CurrentHoverSlot = inventorySlot;
        }

        public void SplitItem()
        {
            if (CurrentHoverSlot == null ||
                !CurrentHoverSlot.ParentInventory.TryGetComponent(out InteractableComponent interactable))
            {
                return;
            }

            interactable.SplitItem(CurrentHoverSlot);
        }

        public void AddChest()
        {
            GameObject chestGO = Instantiate(chestInventoryPrefab, chestContainer.transform);
            chestInventories.Add(chestGO.GetComponent<BaseInventory>());
        }

        public void ShowChest(BaseInventory baseInventory)
        {
            print(baseInventory.name);
        }

        public void ShowShop(BaseInventory baseInventory)
        {

        }
    }
}
