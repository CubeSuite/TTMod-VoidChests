using FluffyUnderware.DevTools.Extensions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VoidChests.Patches
{
    public class InserterInstancePatch
    {
        [HarmonyPatch(typeof(InserterInstance), "Give")]
        [HarmonyPostfix]
        static void VoidItems(InserterInstance __instance) {
            if (__instance.giveResourceContainer.typeIndex == MachineTypeEnum.Chest) {
                ChestInstance chest = __instance.giveResourceContainer.AsGeneric().Get<ChestInstance>();
                if (chest.restrictInputToSlots != 0) return;

                Inventory inventory = __instance.giveResourceContainer.GetInventory(0);
                foreach(ResourceStack stack in inventory.myStacks) {
                    inventory.RemoveResourcesFromSlot(inventory.myStacks.IndexOf(stack), stack.count);
                }
            }
        }
    }
}
