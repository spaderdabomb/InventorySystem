using UnityEngine;

namespace InventorySystem
{
    [RequireComponent(typeof(BaseInventory))]
    public class ItemTestComponent : MonoBehaviour
    {
        [SerializeField] private BaseItem defaultItem;
        private BaseInventory inventory;
        private BaseItem selectedItem;
        private int quantity = 1;

        private void Awake()
        {
            inventory = GetComponent<BaseInventory>();
            selectedItem = defaultItem; // Set default at start
        }

        private void OnValidate()
        {
            if (selectedItem == null)
                selectedItem = defaultItem; // Ensure default item is assigned in the inspector
        }

        public void SetSelectedItem(BaseItem item)
        {
            selectedItem = item;
        }

        public void SetQuantity(int amount)
        {
            quantity = Mathf.Max(1, amount);
        }

        public void AddItem()
        {
            if (selectedItem != null)
            {
                inventory.AddItem(selectedItem, quantity);
            }
            else
            {
                Debug.LogWarning("No BaseItem selected.");
            }
        }

        public void RemoveItem()
        {
            if (selectedItem != null)
            {
                inventory.RemoveItem(selectedItem, quantity);
            }
            else
            {
                Debug.LogWarning("No BaseItem selected.");
            }
        }
    }
}
