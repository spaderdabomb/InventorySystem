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
        private static InventorySlot originSlot;
        private Transform canvasTransform;

        private void Awake()
        {
            canvasTransform = GetComponentInParent<Canvas>().transform;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originSlot = GetComponent<InventorySlot>();
            if (originSlot.inventoryItem == null || ghostIconPrefab == null)
                return;

            ghostIcon = Instantiate(ghostIconPrefab, canvasTransform);
            Image ghostImage = ghostIcon.GetComponent<Image>();

            ghostImage.sprite = originSlot.inventoryItem.baseItem.icon;
            ghostImage.raycastTarget = false;

            Vector2 slotSize = originSlot.GetComponent<RectTransform>().sizeDelta;
            ghostIcon.GetComponent<RectTransform>().sizeDelta = slotSize * ghostIconScale;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (ghostIcon)
                ghostIcon.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (ghostIcon)
                Destroy(ghostIcon);

            GameObject hoveredObject = eventData.pointerEnter;
            if (hoveredObject == null)
                return;

            InventorySlot targetSlot = hoveredObject.GetComponent<InventorySlot>();
            if (targetSlot != null && targetSlot != originSlot)
            {
                BaseInventory inventory = originSlot.GetComponentInParent<BaseInventory>();
                inventory.MoveItem(originSlot, targetSlot);
            }
        }
    }
}