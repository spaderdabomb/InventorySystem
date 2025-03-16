using UnityEngine;
using UnityEditor;

namespace InventorySystem
{
    [CustomEditor(typeof(BaseInventory))]
    public class BaseInventoryEditor : Editor
    {
        private BaseItem selectedItem;
        private int quantity = 1;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // Draws default inspector fields

            BaseInventory inventory = (BaseInventory)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Inventory Controls", EditorStyles.boldLabel);

            selectedItem = (BaseItem)EditorGUILayout.ObjectField("Base Item", selectedItem, typeof(BaseItem), false);
            quantity = EditorGUILayout.IntField("Quantity", quantity);

            if (GUILayout.Button("Add Item"))
            {
                if (selectedItem != null)
                {
                    inventory.AddItem(selectedItem, quantity);
                    EditorUtility.SetDirty(inventory);
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
                    inventory.RemoveItem(selectedItem, quantity);
                    EditorUtility.SetDirty(inventory);
                }
                else
                {
                    Debug.LogWarning("No BaseItem selected.");
                }
            }
        }
    }

}
