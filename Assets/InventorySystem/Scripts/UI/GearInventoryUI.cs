using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class GearInventoryUI : BaseInventoryUI
    {

        /*        public override void ShowInventory(GearInventory baseInventory)
                {
                    base.ShowInventory(baseInventory);

                    for (int i = 0; i < backingIcons.Count && i < SlotsUI.Length; i++)
                    {
                        SlotsUI[i].SetBackingIcon(backingIcons[i].texture);
                    }
                }*/

        public void ShowInventory(GearInventory gearInventory)
        {
            print("Showing gear inventory");

            base.ShowInventory(gearInventory);

            for (int i = 0; i < gearInventory.slotTypes.Count && i < SlotsUI.Length; i++)
            {
                SlotsUI[i].SetBackingIcon(gearInventory.slotTypes[i].icon.texture);
            }
        }
    }
}
