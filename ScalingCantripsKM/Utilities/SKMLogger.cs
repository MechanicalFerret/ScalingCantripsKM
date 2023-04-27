using JetBrains.Annotations;
using Kingmaker.Blueprints;
using System;
using UnityModManagerNet;

namespace ScalingCantripsKM.Utilities
{
    internal class SKMLogger
    {
        private static UnityModManager.ModEntry.ModLogger logger;

        public static void InitializeLogger(UnityModManager.ModEntry modEntry)
        {
            logger = modEntry.Logger;
        }

        public static void Log(string msg)
        {
            logger.Log(msg);
        }

        public static void Setting(bool setting, string ConfiguredOn, string ConfiguredOff)
        {
            var msg = (setting) ? ConfiguredOn : ConfiguredOff;
            Log(msg);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Debug(string msg)
        {
            Log($"DEBUG: {msg}");
        }

        public static void Warning(string msg)
        {
            Log($"WARNING: {msg}");
        }

        public static Exception Error(string msg = null, Exception ex = null)
        {
            if (msg != null) Log($"ERROR: {msg}");
            if (ex != null)
            {
                logger.LogException(ex);
                return ex;
            }
            return new InvalidOperationException(msg);
        }

        public static void Patch(string action, [NotNull] BlueprintScriptableObject bp)
        {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }
    }
}
