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

        public static void Log(String msg)
        {
            if (logger != null)
            {
                logger.Log(msg);
            }
        }

        [System.Diagnostics.Conditional("DEBUG")]
        public static void Debug(String msg)
        {
            if (logger != null)
            {
                logger.Log(msg);
            }
        }

        public static Exception Error(String msg)
        {
            return Error(msg, null);
        }

        public static Exception Error(String msg, Exception ex = null)
        {
            if (ex != null)
            {
                logger.Log(msg + "\n" + ex.ToString() + "\n" + ex.StackTrace);
                return new InvalidOperationException(msg, ex);
            }
            else
            {
                logger.Log(msg);
                return new InvalidOperationException(msg);
            }
        }

        public static void Patch(string action, [NotNull] BlueprintScriptableObject bp)
        {
            Log($"{action}: {bp.AssetGuid} - {bp.name}");
        }
    }
}
