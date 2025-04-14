using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class ItemTooltip : MonoBehaviour
    {
        [SerializeField] private RectTransform rt;
        [SerializeField] private TextMeshProUGUI itemLabel;
        [SerializeField] private TextMeshProUGUI itemDescriptionLabel;
        [SerializeField] private TextMeshProUGUI sellPriceLabel;

        private void Awake()
        {
            Hide();
        }

        public void Update()
        {
            rt.position = Input.mousePosition;
        }

        public void Show(BaseItem baseItem)
        {
            gameObject.SetActive(true);

            itemLabel.text = baseItem.displayName;
            itemDescriptionLabel.text = baseItem.description;
            sellPriceLabel.text = baseItem.sellPrice.ToString();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
