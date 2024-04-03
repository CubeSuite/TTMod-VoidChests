using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidChests.Patches
{
    internal class ChestDefinitionPatch
    {
        [HarmonyPatch(typeof(ChestDefinition), "InitInstance")]
        [HarmonyPostfix]
        static void setLimit(ChestDefinition __instance, ref ChestInstance newInstance) {
            newInstance.restrictInputToSlots = 56;
        }
    }
}
