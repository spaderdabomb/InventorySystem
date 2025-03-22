using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance;

        public BaseInventory playerInventory;
        public HotbarInventory hotbarInventory;

        public AudioSource audioSource;
        public InventoryInput inventoryInput;

        public InventorySlot CurrentHoverSlot { get; private set; } = null;

        private void Awake()
        {
            Instance = this;
        }

        public void InventoryClick()
        {

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
            if (CurrentHoverSlot == null)
                return;

            CurrentHoverSlot.ParentInventory.SplitItem(CurrentHoverSlot);
        }
    }
}
