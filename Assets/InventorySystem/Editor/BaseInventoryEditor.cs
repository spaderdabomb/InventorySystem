using UnityEditor;
using UnityEngine;

namespace InventorySystem
{
    [CustomEditor(typeof(MonoBehaviour), true)] // Applies to all MonoBehaviour subclasses
    public class BaseInventoryEditor : Editor
    {
        private BaseItem selectedItem;
        private int quantity = 1;
        private object inventory; // Holds any `BaseInventory<T>`

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // Draws default inspector fields

            // Find the correct inventory type dynamically
            inventory = target;
            var inventoryType = inventory.GetType();
            if (!IsSubclassOfRawGeneric(typeof(BaseInventory<>), inventoryType)) return;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Inventory Controls", EditorStyles.boldLabel);

            selectedItem = (BaseItem)EditorGUILayout.ObjectField("Base Item", selectedItem, typeof(BaseItem), false);
            quantity = EditorGUILayout.IntField("Quantity", quantity);

            if (GUILayout.Button("Add Item"))
            {
                if (selectedItem != null)
                {
                    var addItemMethod = inventoryType.GetMethod("AddItem");
                    addItemMethod?.Invoke(inventory, new object[] { selectedItem, quantity });
                    EditorUtility.SetDirty((Object)inventory);
                }
                else
                {
                    Debug.LogWarning("No BaseItem selected.");
                }
            }

            if (GUILayout.Button("Remove Item"))
            {
                if (selectedItem != null)
                {
                    var removeItemMethod = inventoryType.GetMethod("RemoveItem");
                    removeItemMethod?.Invoke(inventory, new object[] { selectedItem, quantity });
                    EditorUtility.SetDirty((Object)inventory);
                }
                else
                {
                    Debug.LogWarning("No BaseItem selected.");
                }
            }
        }

        private bool IsSubclassOfRawGeneric(System.Type generic, System.Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}