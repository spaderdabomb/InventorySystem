using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public InventoryItem inventoryItem = null;

        [SerializeField] private Button button;
        [SerializeField] private RawImage iconImage;
        [SerializeField] private RawImage slotBg;
        [SerializeField] private TextMeshProUGUI quantityLabelText;
        [SerializeField] private TextMeshProUGUI hotkeyLabelText;
        [SerializeField] private float hoverScale = 1.2f;
        [SerializeField] private float dragTint = 0.7f;

        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip selectedSound;

        private Vector2 _iconStartSize;
        private Color _iconStartColor;
        private Color _slotStartColor;
        private RectTransform _iconRectTransform;

        public BaseInventory ParentInventory { get; private set; }

        private void Awake()
        {
            iconImage.raycastTarget = false;
            quantityLabelText.raycastTarget = false;

            _iconRectTransform = iconImage.gameObject.GetComponent<RectTransform>();
            _iconStartSize = _iconRectTransform.sizeDelta;
            _iconStartColor = iconImage.color;
            _slotStartColor = button.colors.normalColor;

            if (hotkeyLabelText != null)
                hotkeyLabelText.raycastTarget = false;

            if (slotBg != null)
            {
                slotBg.raycastTarget = false;
                slotBg.gameObject.SetActive(false);
            }

            UpdateSlotUI(false);
            DisableSelection();
        }

        private void Start()
        {
            ParentInventory = GetComponentInParent<BaseInventory>();
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
            UpdateSlotUI(true);
        }

        private void ClearSlotItem()
        {
            inventoryItem = null;
            UpdateSlotUI(false);
        }

        private void DisableSelection()
        {
            Navigation nav = button.navigation;
            nav.mode = Navigation.Mode.None;
            button.navigation = nav;
        }

        private void UpdateSlotUI(bool state)
        {
            iconImage.gameObject.SetActive(state);
            quantityLabelText.gameObject.SetActive(state);

            if (hotkeyLabelText != null)
                hotkeyLabelText.gameObject.SetActive(true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hoverSound != null)
                InventoryManager.Instance.PlayInventorySound(hoverSound);

            InventoryManager.Instance.SetCurrentHoverSlot(this);
            _iconRectTransform.sizeDelta = _iconStartSize * hoverScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _iconRectTransform.sizeDelta = _iconStartSize;
            InventoryManager.Instance.SetCurrentHoverSlot(null);
        }

        public void SetSelected()
        {
            if (slotBg != null)
            {
                slotBg.gameObject.SetActive(true);
                InventoryManager.Instance.PlayInventorySound(selectedSound);
            }
        }

        public void ClearSlotSelection()
        {
            if (slotBg != null)
                slotBg.gameObject.SetActive(false);
        }

        public void SetHotkeyText(string hotkey)
        {
            if (hotkeyLabelText != null)
                hotkeyLabelText.text = hotkey;
        }

        public int GetIndexOf()
        {
            return Array.IndexOf(ParentInventory.slots, this);
        }

        public bool ContainsItem()
        {
            return inventoryItem != null;
        }

        public void UpdateDragBeginUI()
        {
            iconImage.color = _iconStartColor * dragTint;

            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = _iconStartColor * dragTint;
            button.colors = colorBlock;
        }

        public void UpdateDragEndUI()
        {
            iconImage.color = _iconStartColor;

            ColorBlock colorBlock = button.colors;
            colorBlock.normalColor = _slotStartColor;
            button.colors = colorBlock;
        }
    }
}
