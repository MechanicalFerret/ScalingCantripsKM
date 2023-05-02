using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace ScalingCantripsKM.Config
{
    public class Settings : UnityModManager.ModSettings
    {
        // Static References
        internal static Settings settings;
        public static Cantrip AcidSplash { get; private set; }
        public static Cantrip RayOfFrost { get; private set; }
        public static Cantrip Jolt { get; private set; }
        public static Cantrip DisruptUndead { get; private set; }
        public static Cantrip Virtue { get; private set; }
        public static Cantrip Firebolt { get; private set; }
        public static Cantrip UnholyZap { get; private set; }
        public static Cantrip DivineZap { get; private set; }
        public static Cantrip JoltingGrasp { get; private set; }
        public static bool StartImmediately => settings.startImmediately;
        public static int CasterLevelsReq => settings.casterLevelsReq;
        public static int MaxDice => settings.maxDice;

        // Settings.xml Values
        public bool startImmediately = true;
        public int casterLevelsReq = 2;
        public int maxDice = 6;

        public static void LoadAllSettings(ModEntry modEntry)
        {
            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            AcidSplash = Cantrip.Load("AcidSplash");
            RayOfFrost = Cantrip.Load("RayOfFrost");
            Jolt = Cantrip.Load("Jolt");
            DisruptUndead = Cantrip.Load("DisruptUndead");
            Virtue = Cantrip.Load("Virtue");
            Firebolt = Cantrip.Load("Firebolt");
            UnholyZap = Cantrip.Load("UnholyZap");
            DivineZap = Cantrip.Load("DivineZap");
            JoltingGrasp = Cantrip.Load("JoltingGrasp");
        }

        public static void SaveAllSettings(ModEntry modEntry)
        {
            settings.Save(modEntry);
            AcidSplash.Save();
            RayOfFrost.Save();
            Jolt.Save();
            DisruptUndead.Save();
            Virtue.Save();
            Firebolt.Save();
            UnholyZap.Save();
            DivineZap.Save();
            JoltingGrasp.Save();
        }

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }
    }
}
