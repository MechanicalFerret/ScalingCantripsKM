using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace ScalingCantripsKM.Config
{
    public class Settings : UnityModManager.ModSettings
    {
        static Settings settings => Main.settings;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            UnityModManager.ModSettings.Save<Settings>(this, modEntry);
        }

        public static bool StartImmediately => settings.startImmediately;
        public bool startImmediately = true;

        public static int CasterLevelsReq => settings.casterLevelsReq;
        public int casterLevelsReq = 2;

        public static int MaxDice => settings.maxDice;
        public int maxDice = 6;
    }
}
