using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class BaseInventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject slotContainer;

        public BaseInventory ActiveInventory { get; private set; } = null;
        public InventorySlotUI[] SlotsUI { get; private set; } = null;

        public virtual void ShowInventory(BaseInventory baseInventory)
        {
            if (ActiveInventory == baseInventory)
                return;

            gameObject.SetActive(true);
            ClearSlots();

            ActiveInventory = baseInventory;
            InitInventoryPanel(baseInventory.rows, baseInventory.columns);

            SlotsUI = new InventorySlotUI[baseInventory.NumSlots];
            for (int i = 0; i < baseInventory.NumSlots; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab, slotContainer.transform);
                SlotsUI[i] = newSlot.GetComponent<InventorySlotUI>();
                SlotsUI[i].InventorySlot = baseInventory.Slots[i];
                SlotsUI[i].SetSlotItem();
            }
        }

        public void HideInventory()
        {
            ClearSlots();
            ActiveInventory = null;
            gameObject.SetActive(false);
        }

        public void ToggleInventory(BaseInventory baseInventory)
        {
            if (ActiveInventory == baseInventory)
                HideInventory();
            else
                ShowInventory(baseInventory);
        }

        private void ClearSlots()
        {
            foreach (Transform child in slotContainer.transform)
            {
                Destroy(child.gameObject);
            }

            SlotsUI = null;
        }

        private void InitInventoryPanel(int rows, int columns)
        {
            RectTransform slotContainerRT = slotContainer.GetComponent<RectTransform>();
            GridLayoutGroup layoutGroup = slotContainer.GetComponent<GridLayoutGroup>();
            float width = (layoutGroup.cellSize.x + layoutGroup.spacing.x) * columns + 2 * layoutGroup.spacing.x;
            float height = (layoutGroup.cellSize.y + layoutGroup.spacing.y) * rows + 2 * layoutGroup.spacing.y;
            slotContainerRT.sizeDelta = new Vector2(width, height);
        }
    }
}
