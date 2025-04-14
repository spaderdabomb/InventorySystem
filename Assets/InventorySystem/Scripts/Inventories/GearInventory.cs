using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InventorySystem
{
    [RequireComponent(typeof(GearComponent))]
    public class GearInventory : BaseInventory
    {
        public List<InventorySlotType> slotTypes;

        [HideInInspector] public GearComponent gearComponent;

        public override void Awake()
        {
            base.Awake();

            gearComponent = GetComponent<GearComponent>();
        }

        public override void ShowOnAwake()
        {
            print("Showing on awake");
            print(this);

            if (inventoryUI != null && inventoryUI.gameObject.activeSelf)
                inventoryUI.ShowInventory(this);
        }

    }
}
