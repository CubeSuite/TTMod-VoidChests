using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoidChests.Patches
{
    internal class FlowManagerPatch
    {
        [HarmonyPatch(typeof(FlowManager), nameof(FlowManager.SwitchStrataTrampoline))]
        [HarmonyPostfix]
        static void RedoVisuals() {
            MachineDefinitionPatch.RedoVisualsOnStrataChanged();
        }
    }
}
