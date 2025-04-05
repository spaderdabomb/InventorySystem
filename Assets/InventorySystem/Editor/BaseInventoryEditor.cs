using UnityEngine;
using UnityEditor;

namespace InventorySystem
{
    [CustomEditor(typeof(HotbarInventory))]
    public class HotbarInventoryEditor : BaseInventoryEditor<HotbarInventory> { }

    [CustomEditor(typeof(BaseInventory))]
    public class InventoryEditor : BaseInventoryEditor<BaseInventory> { }

    public class BaseInventoryEditor<T> : Editor where T : BaseInventory
    {
        private BaseItem selectedItem;
        private int quantity = 1;

        private void OnEnable()
        {
            selectedItem = InventoryManager.Instance.defaultItem;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            T inventory = (T)target;

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
