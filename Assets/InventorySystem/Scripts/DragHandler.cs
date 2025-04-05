using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject ghostIconPrefab;
        [SerializeField] private float ghostIconScale = 0.9f;

        private static GameObject ghostIcon;
        private static InventorySlotUI originSlotUI;
        private Transform canvasTransform;

        public bool IsDragging = false;

        private void Awake()
        {
            canvasTransform = GetComponentInParent<Canvas>().transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originSlotUI = GetComponent<InventorySlotUI>();
            originSlotUI.InventorySlot.ParentInventory.TryGetComponent(out InteractableComponent interactable);
            if (!originSlotUI.InventorySlot.ContainsItem() || ghostIconPrefab == null || interactable == null)
                return;

            originSlotUI.UpdateDragBeginUI();

            ghostIcon = Instantiate(ghostIconPrefab, canvasTransform);
            Image ghostImage = ghostIcon.GetComponent<Image>();

            ghostImage.sprite = originSlotUI.InventorySlot.inventoryItem.baseItem.icon;
            ghostImage.raycastTarget = false;

            Vector2 slotSize = originSlotUI.GetComponent<RectTransform>().sizeDelta;
            ghostIcon.GetComponent<RectTransform>().sizeDelta = slotSize * ghostIconScale;

            IsDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ghostIcon)
                ghostIcon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;
            originSlotUI.UpdateDragEndUI();

            if (ghostIcon)
                Destroy(ghostIcon);

            GameObject hoveredObject = eventData.pointerEnter;
            if (hoveredObject == null)
                return;

            InventorySlotUI targetSlotUI = hoveredObject.GetComponent<InventorySlotUI>();
            if (targetSlotUI != null && targetSlotUI != originSlotUI)
            {
                originSlotUI.InventorySlot.ParentInventory.TryGetComponent(out InteractableComponent originInteractable);
                targetSlotUI.InventorySlot.ParentInventory.TryGetComponent(out InteractableComponent targetInteractable);
                if (originInteractable == null || targetInteractable == null)
                    return;

                originInteractable.MoveItem(originSlotUI.InventorySlot, targetSlotUI.InventorySlot);
            }
        }
    }
}