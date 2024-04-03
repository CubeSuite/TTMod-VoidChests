using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using VoidChests.Patches;

namespace VoidChests
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class VoidChestsPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.equinox.VoidChests";
        private const string PluginName = "VoidChests";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            Harmony.CreateAndPatchAll(typeof(InserterInstancePatch));
            Harmony.CreateAndPatchAll(typeof(InventoryNavigatorPatch));
            Harmony.CreateAndPatchAll(typeof(ChestDefinitionPatch));

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        private void Update() {
            // ToDo: Delete If Not Needed
        }
    }
}
