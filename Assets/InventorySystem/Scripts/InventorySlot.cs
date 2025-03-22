using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace InventorySystem
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler
    {
        public InventoryItem inventoryItem = null;

        [SerializeField] private Button button;
        [SerializeField] private RawImage iconImage;
        [SerializeField] private RawImage slotBg;
        [SerializeField] private TextMeshProUGUI quantityLabelText;
        [SerializeField] private TextMeshProUGUI hotkeyLabelText;
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip selectedSound;

        public BaseInventory ParentInventory { get; private set; }

        private void Awake()
        {
            iconImage.raycastTarget = false;
            quantityLabelText.raycastTarget = false;

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
            {
                InventoryManager.Instance.PlayInventorySound(hoverSound);
                InventoryManager.Instance.SetCurrentHoverSlot(this);
            }
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
    }
}
