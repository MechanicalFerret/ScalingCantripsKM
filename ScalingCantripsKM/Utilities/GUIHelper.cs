using Harmony12;
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
        public static void Header(string text, string tooltip = null)
        {
            GUILayout.BeginHorizontal();
            var content = tooltip == null ?
                new GUIContent(text) : 
                new GUIContent(text, tooltip);
            GUILayout.Label(content, BoldLabel);
            GUILayout.EndHorizontal();
        }

        public static void Label(string text, string tooltip = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            var content = tooltip == null ?
                new GUIContent(text) :
                new GUIContent(text, tooltip);
            GUILayout.Label(content);
            GUILayout.EndHorizontal();
        }

        public static void Toggle(ref bool value, string text, string tooltip = null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            var content = tooltip == null ?
                new GUIContent(text) :
                new GUIContent(text, tooltip);
            GUILayout.Label(content, GUILayout.Width(300));
            var newValue = GUILayout.Toggle(value, "", GUILayout.ExpandWidth(false));
            value = newValue;
            GUILayout.EndHorizontal();
        }

        public static void ChooseInt(ref int value, string text, string tooltip = null, int min=1, int max=20) {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            var content = tooltip == null ?
                new GUIContent(text) :
                new GUIContent(text, tooltip);
            GUILayout.Label(content, GUILayout.Width(300));
            var result = GUILayout.TextField(value.ToString(), GUILayout.Width(25));
            GUILayout.EndHorizontal();

            int.TryParse(result, out int newValue);
            if (newValue < min) { newValue = min; }
            if (newValue > max) {  newValue = max; }
            value = newValue;
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
