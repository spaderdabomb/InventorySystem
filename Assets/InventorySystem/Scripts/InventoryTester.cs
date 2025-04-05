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
            InventoryManager.Instance.AddChest();

            int chestIndex = chestButtons.Count;
            BaseInventory inventoryRef = InventoryManager.Instance.chestInventories[chestIndex-1];
            label.text = $"Chest {chestIndex}";
            button.onClick.AddListener(() => InventoryManager.Instance.ShowChest(inventoryRef));
        }

        public void AddShop()
        {
            print("Adding shop");
        }
    }
}
