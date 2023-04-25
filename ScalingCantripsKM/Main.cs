using Harmony12;
using Kingmaker.Blueprints;
using ScalingCantripsKM.Config;
using ScalingCantripsKM.Utilities;
using System;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

namespace ScalingCantripsKM
{
    static class Main
    {
        internal static HarmonyInstance harmony;
        public static UnityModManager.ModEntry modEntry;
        public static UnityModManager.ModEntry.ModLogger logger;
        public static LibraryScriptableObject library;
        public static Settings settings;
        public static bool isCallOfTheWildEnabled;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try {
                // Setup Main Variables
                Main.modEntry = modEntry;
                Main.logger = modEntry.Logger;
                Main.settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
                SKMLogger.Log("Loading ScalingCantripsKM");

                // Setup UnityModManager GUI Functions
                modEntry.OnGUI = new Action<UnityModManager.ModEntry>(Main.OnGUI);
                modEntry.OnSaveGUI = new Action<UnityModManager.ModEntry>(Main.OnSaveGUI);

                // Load in Blueprint GUIDs so we don't create new ones
                Blueprints.LoadBlueprints();

                // Check if CallOfTheWild is active, if so we will patch for that mod aswell
                UnityModManager.ModEntry callOfTheWild = UnityModManager.FindMod("CallOfTheWild");
                isCallOfTheWildEnabled = (callOfTheWild != null) && (callOfTheWild.Active);
                if (isCallOfTheWildEnabled) SKMLogger.Log("CallOfTheWild detected, including in patching.");

                // Setup Harmony
                harmony = HarmonyInstance.Create(modEntry.Info.Id);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            catch (Exception e)
            {
                logger.LogException("Error loading ScalingCantripsKM", e);
                throw e;
            }
            return true;
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Main.settings.Save(modEntry);
        }

        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            // Layout Options
            GUILayoutOption[] options = new GUILayoutOption[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MaxWidth(1000)
            };

            GUILayout.Label("FOR BEST EFFECT: restart the game after changing these settings.", options);
            GUILayout.Label("Cantrips Caster Levels Required", options);
            GUILayout.Label(Main.settings.casterLevelsReq.ToString(), options);
            Main.settings.casterLevelsReq = (int)GUILayout.HorizontalSlider(Main.settings.casterLevelsReq, 1, 20, options);

            GUILayout.Label("Cantrips Dice Maximum", options);
            GUILayout.Label(Main.settings.maxDice.ToString(), options);
            Main.settings.maxDice = (int)GUILayout.HorizontalSlider(Main.settings.maxDice, 1, 40, options);

            Main.settings.startImmediately = GUILayout.Toggle(Main.settings.startImmediately, "Check this to have caster levels take effect immediately (e.g Wizard 2 gets you 2d3 with default settings)", options);
        }

        [HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary")]
        [HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary", new Type[] { })]
        [HarmonyAfter("CallOfTheWild")]
        static class LibraryScriptableObject_LoadDictionary_Patch
        {
            static bool hasRun = false;

            static void Postfix(LibraryScriptableObject __instance)
            {
                if (hasRun) return;
                hasRun = true;
                library = __instance;
                try
                {
                    logger.Log("Setting up Scaling Cantrips");
                    CantripPatcher.Init();
                    CantripAddRanged.Init();
                    CantripAddMelee.Init();
                    Blueprints.WriteBlueprints();
                }
                catch(Exception e)
                {
                    logger.LogException("Error Setting up ScalingCantripsKM", e);
                    throw e;
                }
                
            }
        }
    }
}
