using ScalingCantripsKM.Utilities;
using System;
using UnityEngine;

namespace ScalingCantripsKM.Config
{
    public class Menu
    {
        private static bool enabled => Main.modEntry.Enabled;


        public static void OnGUI()
        {
            if (!enabled) return;

            GUIHelper.Header("Existing Cantrips");
            GUIHelper.Toggle(ref Settings.AcidSplash.Enabled, Labels.AcidSplashLabel, Labels.AcidSplashTooltip);
            GUIHelper.Toggle(ref Settings.RayOfFrost.Enabled, Labels.RayOfFrostLabel, Labels.RayOfFrostTooltip);
            GUIHelper.Toggle(ref Settings.Jolt.Enabled, Labels.JoltLabel, Labels.JoltTooltip);
            GUIHelper.Toggle(ref Settings.DisruptUndead.Enabled, Labels.DisruptUndeadLabel, Labels.DisruptUndeadTooltip);
            GUIHelper.Toggle(ref Settings.Virtue.Enabled, Labels.VirtueLabel, Labels.VirtueTooltip);

            GUIHelper.Header("New Cantrips");
            GUIHelper.Toggle(ref Settings.Firebolt.Enabled, Labels.FireBoltLabel, Labels.FireBoltTooltip);
            GUIHelper.Toggle(ref Settings.UnholyZap.Enabled, Labels.UnholyZapLabel, Labels.UnholyZapTooltip);
            GUIHelper.Toggle(ref Settings.DivineZap.Enabled, Labels.DivineZapLabel, Labels.DivineZapTooltip);
            GUIHelper.Toggle(ref Settings.JoltingGrasp.Enabled, Labels.JoltingGraspLabel, Labels.JoltingGraspTooltip);

            GUIHelper.Header("Default Scaling Values");
            GUIHelper.Toggle(ref Settings.settings.startImmediately, Labels.StartImmediatelyLabel, Labels.StartImmediatelyTooltip);
            GUIHelper.ChooseInt(ref Settings.settings.casterLevelsReq, Labels.CasterLevelReqLabel, Labels.CasterLevelReqTooltip, 1, 20);
            GUIHelper.ChooseInt(ref Settings.settings.maxDice, Labels.MaxDiceLabel, Labels.MaxDiceTooltip, 1, 20);

            GUIHelper.Header("Cantrip Override Settings");
            ToggleOverride(Settings.AcidSplash);
            ToggleOverride(Settings.RayOfFrost);
            ToggleOverride(Settings.Jolt);
            ToggleOverride(Settings.DisruptUndead);
            ToggleOverride(Settings.Virtue);
            ToggleOverride(Settings.Firebolt);
            ToggleOverride(Settings.UnholyZap);
            ToggleOverride(Settings.DivineZap);
            ToggleOverride(Settings.JoltingGrasp);
        }

        public static void ToggleOverride(Cantrip cantrip)
        {
            GUILayout.BeginHorizontal();
            GUIHelper.Toggle(ref cantrip.OverrideDefaults, cantrip.Name);
            if (cantrip.OverrideDefaults)
            {
                GUIHelper.Toggle(ref cantrip._startImmediately, Labels.StartImmediatelyLabel, Labels.StartImmediatelyTooltip);
                GUIHelper.ChooseInt(ref cantrip._casterLevelReq, Labels.CasterLevelReqLabel, Labels.CasterLevelReqTooltip, 1, 20);
                GUIHelper.ChooseInt(ref cantrip._maxDice, Labels.MaxDiceLabel, Labels.MaxDiceTooltip, 1, 20);
            }
            GUILayout.EndHorizontal();
        }
    }

    
}
