using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ChestManager : MonoBehaviour
    {
        [SerializeField] private GameObject slotContainer;
        [SerializeField] private GameObject chestInventoryPrefab;
        [SerializeField] private GameObject chestContainer;

        [SerializeField] private BaseInventoryUI chestUI;

        public List<BaseInventory> chestInventories;

        private void Awake()
        {
            chestInventories = new List<BaseInventory>();
        }

        public void AddChest()
        {
            GameObject chestGO = Instantiate(chestInventoryPrefab, chestContainer.transform);
            BaseInventory chestInventory = chestGO.GetComponent<BaseInventory>();
            chestInventory.Initialize(chestUI);

            chestInventories.Add(chestGO.GetComponent<BaseInventory>());
        }
    }
}
