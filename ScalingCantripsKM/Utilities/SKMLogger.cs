using JetBrains.Annotations;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace ScalingCantripsKM.Utilities
{
    internal class SKMLogger
    {
        static UnityModManager.ModEntry.ModLogger logger => Main.logger;

        public static void Log(string msg)
        {
            logger.Log(msg);
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

        public static Exception Error(string msg)
        {
            return Error(msg, null);
        }

        public static Exception Error(string msg, Exception ex = null)
        {
            if (ex != null)
            {
                Log("ERROR: " + msg + "\n" + ex.ToString() + "\n" + ex.StackTrace);
                return new InvalidOperationException(msg, ex);
            }
            else
            {
                Log("ERROR: " + msg);
                return new InvalidOperationException(msg);
            }
        }

        public static void Patch(string action, [NotNull] BlueprintScriptableObject bp)
        {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }
    }
}
