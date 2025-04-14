using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class ShopInventoryUI : BaseInventoryUI
    {
        [SerializeField] private Button buyButton;

        [SerializeField] private BaseInventory toInventory;

        private void Start()
        {
            buyButton.onClick.AddListener(() => InventoryManager.Instance.BuyItem(ActiveInventory, toInventory));
        }
    }
}
