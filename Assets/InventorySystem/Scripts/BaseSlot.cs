using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class BaseSlot : MonoBehaviour
    {
        public InventoryItem inventoryItem = null;
        public GameObject icon;
        public GameObject quantityLabel;

        private RawImage _iconImage;
        private TextMeshProUGUI _quantityLabelText;

        private void Start()
        {
            _iconImage = icon.GetComponent<RawImage>();
            _iconImage.raycastTarget = false;

            _quantityLabelText = quantityLabel.GetComponent<TextMeshProUGUI>();
            _quantityLabelText.raycastTarget = false;

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
            _iconImage.texture = inventoryItem.baseItem.icon.texture;
            _quantityLabelText.text = inventoryItem.quantity.ToString();
            SetItemUIState(true);
        }

        private void ClearSlotItem()
        {
            inventoryItem = null;
            SetItemUIState(false);
        }

        private void SetItemUIState(bool state)
        {
            icon.SetActive(state);
            quantityLabel.SetActive(state);
        }
    }
}
