using EquinoxsDebuggingTools;
using EquinoxsModUtils;
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
    internal class MachineDefinitionPatch
    {
        public static GameObject prefab;
        private static Dictionary<uint, GameObject> visualsMap = new Dictionary<uint, GameObject>();

        [HarmonyPatch(typeof(MachineDefinition<ChestInstance, ChestDefinition>), nameof(MachineDefinition<ChestInstance, ChestDefinition>.OnBuild))]
        [HarmonyPostfix]
        static void AddToVoidChestsList(MachineInstanceRef<ChestInstance> instRef) {
            if (instRef.Get().myDef.displayName != "Void Chest") return;

            VoidChestsPlugin.voidChestIDs.Add(instRef.GetCommonInfo().instanceId);
            EDT.Log("General", $"Added chest #{instRef.GetCommonInfo().instanceId} to voidChestIDs");

            if (instRef.gridInfo.strata == GameState.instance.GetStrata()) {
                GameObject visuals = GameObject.Instantiate(prefab, instRef.gridInfo.BottomCenter, Quaternion.Euler(0, instRef.gridInfo.yawRot, 0));
                visuals.transform.localScale = new Vector3(1.01f, 1.01f, 1.01f);

                if (visualsMap.ContainsKey(instRef.instanceId)) {
                    GameObject.Destroy(visualsMap[instRef.instanceId]);
                }

                visualsMap[instRef.instanceId] = visuals;
            }
        }
            
        [HarmonyPatch(typeof(MachineDefinition<ChestInstance, ChestDefinition>), "OnDeconstruct")]
        [HarmonyPrefix]
        static void RemoveVisuals(ref InserterInstance erasedInstance) {
            if (erasedInstance.myDef.displayName != "Void Chest") return;
            GameObject.Destroy(visualsMap[erasedInstance.commonInfo.instanceId]);
            visualsMap.Remove(erasedInstance.commonInfo.instanceId);
        }

        public static void RedoVisualsOnStrataChanged() {
            if (!EMU.LoadingStates.hasGameLoaded) return;
            ClearVisuals();
            foreach (uint id in VoidChestsPlugin.voidChestIDs) {
                if (!MachineManager.instance.GetRefFromId(id, out IMachineInstanceRef instRef)) continue;

                if (instRef.gridInfo.strata == GameState.instance.GetStrata()) {
                    GameObject visuals = GameObject.Instantiate(prefab, instRef.gridInfo.BottomCenter, Quaternion.Euler(0, instRef.gridInfo.yawRot, 0));
                    visuals.transform.localScale = new Vector3(1.01f, 1.01f, 1.01f);

                    if (visualsMap.ContainsKey(instRef.instanceId)) {
                        GameObject.Destroy(visualsMap[instRef.instanceId]);
                    }

                    visualsMap[instRef.instanceId] = visuals;
                }
            }
        }

        internal static void OnGameUnloaded() {
            ClearVisuals();
        }

        private static void ClearVisuals() {
            foreach (GameObject visuals in visualsMap.Values) {
                GameObject.Destroy(visuals);
            }

            visualsMap.Clear();
        }
    }
}
