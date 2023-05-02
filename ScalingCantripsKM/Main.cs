using Harmony12;
using Kingmaker.Blueprints;
using ScalingCantripsKM.Config;
using ScalingCantripsKM.Utilities;
using System;
using System.Reflection;
using UnityModManagerNet;

namespace ScalingCantripsKM
{
    static class Main
    {
        internal static HarmonyInstance harmony;
        public static UnityModManager.ModEntry modEntry { get; private set; }
        public static LibraryScriptableObject library { get; private set; }
        public static bool isCallOfTheWildEnabled { get; private set; }

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                // Setup Main Variables
                Main.modEntry = modEntry;
                SKMLogger.InitializeLogger(modEntry);
                Settings.LoadAllSettings(modEntry);
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
                throw SKMLogger.Error("Unable to Load ScalingCantripsKM", e);
            }
            return true;
        }


        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            try
            {
                Menu.OnGUI();
            }
            catch (Exception e)
            {
                throw SKMLogger.Error("UI has encountered an error", e);
            }
        }

        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Settings.SaveAllSettings(modEntry);
        }

        [HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary")]
        [HarmonyPatch(typeof(LibraryScriptableObject), "LoadDictionary", new Type[] { })]
        [HarmonyAfter("CallOfTheWild")]
        static class LibraryScriptableObject_LoadDictionary_Patch
        {
            public static bool hasRun = false;

            static void Postfix(LibraryScriptableObject __instance)
            {
                if (hasRun) return;
                hasRun = true;
                library = __instance;
                try
                {
                    SKMLogger.Log("Setting up Scaling Cantrips");
                    CantripPatcher.Init();
                    CantripAddRanged.Init();
                    CantripAddMelee.Init();
                    CantripFeatures.Init();
                    Blueprints.WriteBlueprints();
                }
                catch (Exception e)
                {
                    Main.modEntry.Enabled = false;
                    throw SKMLogger.Error("Unable to Patch ScalingCantripsKM", e);
                }

            }
        }
    }
}
