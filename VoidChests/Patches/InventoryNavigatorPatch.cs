using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquinoxsModUtils;
using UnityEngine.UI;
using System.Reflection;

namespace VoidChests.Patches
{
    internal class InventoryNavigatorPatch
    {
        [HarmonyPatch(typeof(InventoryNavigator), "UpdateLimitedSlots")]
        [HarmonyPostfix]
        static void allowZeroSlots(InventoryNavigator __instance, int limit) {
            ModUtils.SetPrivateField("curLimit", __instance, limit);
            
            InventoryUI ui = (InventoryUI)ModUtils.GetPrivateField("otherInventoryUI", __instance);
            for (int j = 0; j < ui.activeSlots.Count; j++) {
                ui.activeSlots[j].SetSlotLimitMask(j >= limit);
            }

            if (!(bool)ModUtils.GetPrivateField("canUseSlotLimits", __instance)) {
                ((Button)ModUtils.GetPrivateField("storageLimitClearButton", __instance)).gameObject.ToggleActive(false);
                ((Button)ModUtils.GetPrivateField("storageLimitSetButtonHilit", __instance)).gameObject.ToggleActive(false);
                ((Button)ModUtils.GetPrivateField("storageLimitSetButton", __instance)).gameObject.ToggleActive(false);
                ((Button)ModUtils.GetPrivateField("storageLimitCancelButton", __instance)).gameObject.ToggleActive(false);
                ((ButtonPromptUI)ModUtils.GetPrivateField("limitStorageBP", __instance)).gameObject.ToggleActive(false);
                ((ButtonPromptUI)ModUtils.GetPrivateField("clearLimitBP", __instance)).gameObject.ToggleActive(false);
                return;
            }

            if((bool)ModUtils.GetPrivateField("settingLimit", __instance)) {
                ((Button)ModUtils.GetPrivateField("storageLimitClearButton", __instance)).gameObject.ToggleActive(false);
                ((Button)ModUtils.GetPrivateField("storageLimitSetButtonHilit", __instance)).gameObject.ToggleActive(true);
                ((Button)ModUtils.GetPrivateField("storageLimitSetButton", __instance)).gameObject.ToggleActive(false);
                ((Button)ModUtils.GetPrivateField("storageLimitCancelButton", __instance)).gameObject.ToggleActive(true);
                ui.Set(-1, false);
                ((ButtonPromptUI)ModUtils.GetPrivateField("limitStorageBP", __instance)).gameObject.ToggleActive(true);
                ((ButtonPromptUI)ModUtils.GetPrivateField("clearLimitBP", __instance)).gameObject.ToggleActive(true);
                return;
            }

            ((Button)ModUtils.GetPrivateField("storageLimitClearButton", __instance)).gameObject.ToggleActive(true);
            ((Button)ModUtils.GetPrivateField("storageLimitClearButton", __instance)).interactable = true;
            ((Button)ModUtils.GetPrivateField("storageLimitSetButton", __instance)).gameObject.ToggleActive(true);
            ((Button)ModUtils.GetPrivateField("storageLimitSetButtonHilit", __instance)).gameObject.ToggleActive(false);
            ((Button)ModUtils.GetPrivateField("storageLimitCancelButton", __instance)).gameObject.ToggleActive(false);
            ((ButtonPromptUI)ModUtils.GetPrivateField("limitStorageBP", __instance)).gameObject.ToggleActive(true);
            ((ButtonPromptUI)ModUtils.GetPrivateField("clearLimitBP", __instance)).gameObject.ToggleActive(true);
        }

        [HarmonyPatch(typeof(InventoryNavigator), "StorageLimitClearCommand")]
        [HarmonyPrefix]
        static bool clearLimit(InventoryNavigator __instance) {
            MethodInfo updateSlotsInfo = typeof(InventoryNavigator).GetMethod("UpdateLimitedSlots", BindingFlags.NonPublic | BindingFlags.Instance);
            updateSlotsInfo.Invoke(__instance, new object[] { 56 });

            MethodInfo stopSettingLimitInfo = typeof(InventoryNavigator).GetMethod("StopSettingLimit", BindingFlags.NonPublic | BindingFlags.Instance);
            stopSettingLimitInfo.Invoke(__instance, new object[] { true });

            return false;
        }
    }
}
