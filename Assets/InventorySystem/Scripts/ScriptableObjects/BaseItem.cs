using System;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "baseItem", menuName = "Scriptable Objects/Items/BaseItem")]
    public class BaseItem : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString();
        public string displayName = string.Empty;
        public string description = string.Empty;
        public int maxStack = 50;
        public int sellPrice = 1;
        public ItemType itemType = ItemType.None;

        public Sprite icon;

        private void OnValidate()
        {
            id = Guid.TryParse(id, out _) ? id : Guid.NewGuid().ToString();
        }
    }

    public enum ItemType
    {
        None = 0,
        Feet = 1,
        Hands = 2,
        Head = 3,
        Body = 4,
        Legs = 5,
    }

}