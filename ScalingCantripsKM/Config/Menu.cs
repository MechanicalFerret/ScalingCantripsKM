using ScalingCantripsKM.Utilities;
using UnityEngine;

namespace ScalingCantripsKM.Config
{
    public class Menu
    {
        private static bool enabled => Main.modEntry.Enabled;


        public static void OnGUI()
        {
            if (!enabled) return;

            GUIHelper.Header("Default Scaling Values");
            GUIHelper.Toggle(ref Settings.settings.startImmediately, Labels.StartImmediatelyLabel, Labels.StartImmediatelyTooltip);
            GUIHelper.ChooseInt(ref Settings.settings.casterLevelsReq, Labels.CasterLevelReqLabel, Labels.CasterLevelReqTooltip, 1, 20);
            GUIHelper.ChooseInt(ref Settings.settings.maxDice, Labels.MaxDiceLabel, Labels.MaxDiceTooltip, 1, 20);

            GUIHelper.Header("Cantrip Config");
            ToggleOverride(Settings.AcidSplash);
            ToggleOverride(Settings.RayOfFrost);
            ToggleOverride(Settings.Jolt);
            ToggleOverride(Settings.DisruptUndead);
            ToggleOverride(Settings.Virtue);
            ToggleOverride(Settings.Firebolt);
            ToggleOverride(Settings.UnholyZap);
            ToggleOverride(Settings.DivineZap);
            ToggleOverride(Settings.JoltingGrasp);

            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUIHelper.Label("Restart your game for changes to take effect.");
            GUILayout.EndVertical();
        }

        public static void ToggleOverride(Cantrip cantrip)
        {
            GUILayout.BeginHorizontal();
            GUIHelper.Toggle(ref cantrip.Enabled, $"{cantrip.Name}: ");
            GUIHelper.Toggle(ref cantrip.OverrideDefaults, "Override: ", showWhen: cantrip.Enabled);
            GUILayout.EndHorizontal();

            GUILayout.BeginVertical();
            GUIHelper.Toggle(ref cantrip._startImmediately, Labels.StartImmediatelyLabel, Labels.StartImmediatelyTooltip, showWhen: cantrip.OverrideDefaults, indent: 20);
            GUIHelper.ChooseInt(ref cantrip._casterLevelReq, Labels.CasterLevelReqLabel, Labels.CasterLevelReqTooltip, 1, 20, showWhen: cantrip.OverrideDefaults, indent: 20);
            GUIHelper.ChooseInt(ref cantrip._maxDice, Labels.MaxDiceLabel, Labels.MaxDiceTooltip, 1, 20, showWhen: cantrip.OverrideDefaults, indent: 20);
            GUILayout.EndVertical();
        }
    }


}
