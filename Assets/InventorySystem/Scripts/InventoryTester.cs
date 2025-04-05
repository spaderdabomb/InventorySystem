using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventoryTester : MonoBehaviour
    {
        [SerializeField] private Button addChestButton;
        [SerializeField] private Button addShopButton;

        [SerializeField] private GridLayoutGroup chestButtonLayout;
        [SerializeField] private GridLayoutGroup shopButtonLayout;

        [SerializeField] private GameObject openInterfaceButtonPrefab;

        private List<Button> chestButtons = new List<Button>();
        private List<Button> shopButtons = new List<Button>();

        public void AddChest()
        {
            GameObject buttonGO = Instantiate(openInterfaceButtonPrefab, chestButtonLayout.transform);
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI label = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            chestButtons.Add(button);
            InventoryManager.Instance.chestManager.AddChest();

            int chestIndex = chestButtons.Count;
            BaseInventory inventoryRef = InventoryManager.Instance.chestManager.chestInventories[chestIndex-1];
            label.text = $"Chest {chestIndex}";
            button.onClick.AddListener(() => inventoryRef.inventoryUI.ToggleInventory(inventoryRef));
        }

        public void AddShop()
        {
            GameObject buttonGO = Instantiate(openInterfaceButtonPrefab, shopButtonLayout.transform);
            Button button = buttonGO.GetComponent<Button>();
            TextMeshProUGUI label = buttonGO.GetComponentInChildren<TextMeshProUGUI>();

            shopButtons.Add(button);
            InventoryManager.Instance.shopManager.AddShop();

            int shopIndex = shopButtons.Count;
            BaseInventory inventoryRef = InventoryManager.Instance.shopManager.inventories[shopIndex - 1];
            label.text = $"Shop {shopIndex}";
            button.onClick.AddListener(() => inventoryRef.inventoryUI.ToggleInventory(inventoryRef));
        }

        public void TogglePlayerInventory()
        {
            InventoryManager.Instance.playerInventory.inventoryUI.ToggleInventory(InventoryManager.Instance.playerInventory);
        }

        public void ToggleHotbar()
        {
            InventoryManager.Instance.hotbarInventory.inventoryUI.ToggleInventory(InventoryManager.Instance.hotbarInventory);
        }
    }
}
