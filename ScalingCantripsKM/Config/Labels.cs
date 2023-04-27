using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingCantripsKM.Config
{
    internal class Labels
    {
        public static string AcidSplashLabel = "Enable Acid Splash";
        public static string AcidSplashTooltip = "Enable scaling on the Acid Splash cantrip. Turning this off will revert it to normal";
        public static string RayOfFrostLabel = "Enable Ray of Frost";
        public static string RayOfFrostTooltip = "Enable scaling on the Ray of Frost cantrip. Turning this off will revert it to normal";
        public static string JoltLabel = "Enable Jolt";
        public static string JoltTooltip = "Enable scaling on the Jolt cantrip. Turning this off will revert it to normal";
        public static string DisruptUndeadLabel = "Enable Disrupt Undead";
        public static string DisruptUndeadTooltip = "Enable scaling on the Disrupt Undead cantrip. Turning this off will revert it to normal";
        public static string VirtueLabel = "Enable Virtue";
        public static string VirtueTooltip = "Enable scaling on the Virtue cantrip. Turning this off will revert it to normal";
        public static string FireBoltLabel = "Enable Firebolt";
        public static string FireBoltTooltip = "Enable the Firebolt cantrip with scaling. Turning this off will prevent it from being added in the future";
        public static string UnholyZapLabel = "Enable Unholy Zap";
        public static string UnholyZapTooltip = "Enable the Unholy Zap cantrip with scaling. Turning this off will prevent it from being added in the future";
        public static string DivineZapLabel = "Enable Divine Zap";
        public static string DivineZapTooltip = "Enable the Divine Zap cantrip with scaling. Turning this off will prevent it from being added in the future";
        public static string JoltingGraspLabel = "Enable Jolting Grasp";
        public static string JoltingGraspTooltip = "Enable the Jolting Grasp cantrip with scaling. Turning this off will prevent it from being added in the future";

        public static string StartImmediatelyLabel = "Start Progression Immediately";
        public static string StartImmediatelyTooltip = "If true, level progression will start immediately (ex: Wizard 2 gets you 2d3). If false it will start with a 1 level delay. Default: true";
        public static string CasterLevelReqLabel = "Caster Levels Required";
        public static string CasterLevelReqTooltip = "The number of caster levels between increases to the number of dice a cantrip deals in damage. Default: 2";
        public static string MaxDiceLabel = "Maximum Damage Dice";
        public static string MaxDiceTooltip = "The maximum number of damage dice a cantrip will scale to. You can change this to 1 to stop scaling. Default: 6";
    }
}
