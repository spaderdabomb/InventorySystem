using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "InventorySlotType", menuName = "Scriptable Objects/Inventory/InventorySlotType")]
    public class InventorySlotType : ScriptableObject
    {
        public ItemType itemType;
        public Sprite icon;
    }
}
