using System;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem
{
    public class InventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public InventorySlot InventorySlot { get; set; } = null;

        [SerializeField] private Button button;
        [SerializeField] private RawImage iconImage;
        [SerializeField] private RawImage slotBg;
        [SerializeField] private RawImage backingIcon;
        [SerializeField] private TextMeshProUGUI quantityLabelText;
        [SerializeField] private TextMeshProUGUI hotkeyLabelText;
        [SerializeField] private float hoverScale = 1.2f;
        [SerializeField] private float dragTint = 0.7f;
        [SerializeField] private float tooltipHoverTime = 0.5f;

        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip selectedSound;

        private Vector2 _iconStartSize;
        private Color _iconStartColor;
        private Color _slotStartColor;
        private RectTransform _iconRectTransform;
        private float _timeHoveringSlot = 0f;
        private bool _isHovering = false;

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

        private void Update()
        {
            if (!_isHovering)
                return;

            _timeHoveringSlot += Time.deltaTime;
            if (_timeHoveringSlot > tooltipHoverTime && InventorySlot.ContainsItem())
                InventoryManager.Instance.itemTooltip.Show(InventorySlot.inventoryItem.baseItem);
        }

        private void Start()
        {
            ParentInventory = GetComponentInParent<BaseInventory>();
        }

        public void SetSlotItem()
        {
            if (InventorySlot.inventoryItem == null || InventorySlot.inventoryItem?.quantity == 0)
            {
                ClearSlotItem();
                return;
            }

            iconImage.texture = InventorySlot.inventoryItem.baseItem.icon.texture;
            quantityLabelText.text = InventorySlot.inventoryItem.quantity.ToString();
            UpdateSlotUI(true);
        }

        private void ClearSlotItem()
        {
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
            _isHovering = true;
            _timeHoveringSlot = 0f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _iconRectTransform.sizeDelta = _iconStartSize;
            InventoryManager.Instance.SetCurrentHoverSlot(null);
            InventoryManager.Instance.itemTooltip.Hide();
            _isHovering = false;
            _timeHoveringSlot = 0f;
        }

        public void SetSelected()
        {
            if (slotBg == null)
            {
                Debug.LogWarning("Attemping to set slot selected without background element - choose a different slot prefab");
                return;
            }

            slotBg.gameObject.SetActive(true);
            InventoryManager.Instance.PlayInventorySound(selectedSound);
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

        public void SetBackingIcon(Texture iconTexture)
        {
            if (backingIcon == null)
                return;

            backingIcon.texture = iconTexture;
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
