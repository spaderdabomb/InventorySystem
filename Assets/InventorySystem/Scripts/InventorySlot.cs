using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour
    {
        public InventoryItem inventoryItem = null;

        [SerializeField] private RawImage iconImage;
        [SerializeField] private TextMeshProUGUI quantityLabelText;
        [SerializeField] private TextMeshProUGUI hotkeyLabelText;

        private void Start()
        {
            iconImage.raycastTarget = false;
            quantityLabelText.raycastTarget = false;
            hotkeyLabelText.raycastTarget = false;

            SetItemUIState(false);
        }

        public void SetSlotItem(InventoryItem inventoryItem)
        {
            if (inventoryItem == null || inventoryItem?.quantity == 0)
            {
                ClearSlotItem();
                return;
            }

            this.inventoryItem = inventoryItem;
            iconImage.texture = inventoryItem.baseItem.icon.texture;
            quantityLabelText.text = inventoryItem.quantity.ToString();
            SetItemUIState(true);
        }

        private void ClearSlotItem()
        {
            inventoryItem = null;
            SetItemUIState(false);
        }

        private void SetItemUIState(bool state)
        {
            iconImage.gameObject.SetActive(state);
            quantityLabelText.gameObject.SetActive(state);

            if (hotkeyLabelText != null)
                hotkeyLabelText.gameObject.SetActive(true);
        }
    }
}
