using BepInEx;
using BepInEx.Logging;
using EquinoxsModUtils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VoidChests.Patches;

namespace VoidChests
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class VoidChestsPlugin : BaseUnityPlugin
    {
        internal const string MyGUID = "com.equinox.VoidChests";
        private const string PluginName = "VoidChests";
        private const string VersionString = "2.1.2";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        internal static ManualLogSource Log = new ManualLogSource(PluginName);

        internal static List<uint> voidChestIDs = new List<uint>();

        // Unity Functions

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");

            ApplyPatches();
            BindEvents();
            ContentAdder.AddVoidChest();
            LoadPrefab();

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} has loaded.");
            Log = Logger;
        }

        private void FixedUpdate() {
            foreach (uint id in voidChestIDs) {
                if (MachineManager.instance.GetRefFromId(id, out MachineInstanceRef<ChestInstance> chest)) {
                    ref Inventory inventory = ref chest.GetInventory(0);
                    for (int i = 0; i < inventory.myStacks.Length; i++) {
                        inventory.myStacks[i] = ResourceStack.CreateEmptyStack();
                    }
                }
            }
        }

        // Events

        private void OnGameDefinesLoaded() {
            ChestDefinition voidChestDefinition = (ChestDefinition)EMU.Resources.GetResourceInfoByName("Void Chest");
            voidChestDefinition.inventorySizes = new List<Vector2Int>() { new Vector2Int(1, 1) };
            voidChestDefinition.invSizeOutput = new Vector2Int(1, 1);
        }

        private void OnSaveStateLoaded(object sender, EventArgs e) {
            string worldName = sender.ToString();
        }

        private void OnGameLoaded() {

        }

        private void OnGameSaved(object sender, EventArgs e) {
            string worldName = sender.ToString();
        }

        // Private Functions

        private void ApplyPatches() {
            Harmony.CreateAndPatchAll(typeof(FlowManagerPatch));
            Harmony.CreateAndPatchAll(typeof(MachineDefinitionPatch));
        }

        private void BindEvents() {
            EMU.Events.GameDefinesLoaded += OnGameDefinesLoaded;
            EMU.Events.SaveStateLoaded += OnSaveStateLoaded;
            EMU.Events.GameLoaded += OnGameLoaded;
            EMU.Events.GameSaved += OnGameSaved;
            EMU.Events.GameUnloaded += MachineDefinitionPatch.OnGameUnloaded;
        }

        private void LoadPrefab() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssetBundle bundle = AssetBundle.LoadFromStream(assembly.GetManifestResourceStream("VoidChests.VoidChest"));
            MachineDefinitionPatch.prefab = bundle.LoadAsset<GameObject>("assets/void_box.prefab");
        }
    }
}
