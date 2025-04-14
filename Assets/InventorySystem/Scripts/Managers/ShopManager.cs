using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private GameObject slotContainer;
        [SerializeField] private GameObject inventoryPrefab;
        [SerializeField] private GameObject inventoryContainer;

        [SerializeField] public BaseInventoryUI inventoryUI;

        public List<BaseInventory> inventories;

        private void Awake()
        {
            inventories = new List<BaseInventory>();
        }

        public void AddShop()
        {
            GameObject inventoryGO = Instantiate(inventoryPrefab, inventoryContainer.transform);
            BaseInventory inventory = inventoryGO.GetComponent<BaseInventory>();
            inventory.Initialize(inventoryUI);

            inventories.Add(inventoryGO.GetComponent<BaseInventory>());
        }
    }
}
