using System;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "baseItem", menuName = "Items/BaseItem")]
    public class BaseItem : ScriptableObject
    {
        public string id = Guid.NewGuid().ToString();
        public string displayName = string.Empty;
        public string description = string.Empty;
        public int maxStack = 50;

        public Sprite icon;

        private void OnValidate()
        {
            id = Guid.TryParse(id, out _) ? id : Guid.NewGuid().ToString();
        }
    }

}