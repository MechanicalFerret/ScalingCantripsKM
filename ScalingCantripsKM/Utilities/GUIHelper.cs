using Harmony12;
using Pathfinding.RVO;
using ScalingCantripsKM.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityModManagerNet;

namespace ScalingCantripsKM.Utilities
{
    public static class GUIHelper
    {
        private static float columnWidth = 200f;

        public static void Header(string text, string tooltip = null)
        {
            GUILayout.BeginHorizontal();
            var content = tooltip == null ?
                new GUIContent(text) : 
                new GUIContent(text, tooltip);
            GUILayout.Label(content, BoldLabel);
            GUILayout.EndHorizontal();
        }

        public static void Label(string text, string tooltip = null, bool showWhen = true, float indent= 10f)
        {
            if (!showWhen) return;
            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUILayout.Label((tooltip == null) ? new GUIContent(text) : new GUIContent(text, tooltip));
            GUILayout.EndHorizontal();
        }

        public static void Toggle(ref bool value, string text, string tooltip = null, bool showWhen = true, int indent = 10)
        {
            if (!showWhen) return;
            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUILayout.Label((tooltip == null) ? new GUIContent(text) : new GUIContent(text, tooltip), GUILayout.Width(columnWidth - indent));
            value = GUILayout.Toggle(value, "", GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();
        }

        public static void ChooseInt(ref int value, string text, string tooltip = null, int min=1, int max=20, bool showWhen = true, float indent=10f) {
            if (!showWhen) return;
            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUILayout.Label((tooltip == null) ? new GUIContent(text) : new GUIContent(text, tooltip), GUILayout.Width(columnWidth - indent));
            var result = (int) GUILayout.HorizontalSlider(value, min, max, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(220));
            GUILayout.Label(result.ToString());
            GUILayout.EndHorizontal();

            if (result < min) { result = min; }
            if (result > max) { result = max; }
            value = result;
        }

        private static GUIStyle m_BoldLabel;
        internal static GUIStyle BoldLabel
        {
            get
            {
                if (m_BoldLabel == null)
                {
                    m_BoldLabel = new GUIStyle(GUI.skin.label)
                    {
                        fontStyle = FontStyle.Bold,
                    };
                }
                return m_BoldLabel;
            }
        }
    }
}
