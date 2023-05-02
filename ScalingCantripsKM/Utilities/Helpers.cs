using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/**
 * This code was taken from CallOfTheWild and trimed down to just what the mod needs to function.
 */
namespace ScalingCantripsKM.Utilities
{
    static class Helpers
    {
        public static T Create<T>(Action<T> init = null) where T : ScriptableObject
        {
            var result = ScriptableObject.CreateInstance<T>();
            if (init != null) init(result);
            return result;
        }

        public static ContextRankConfig CreateContextRankConfig(
            ContextRankBaseValueType baseValueType = ContextRankBaseValueType.CasterLevel,
            ContextRankProgression progression = ContextRankProgression.AsIs,
            AbilityRankType type = AbilityRankType.Default,
            int? min = null, int? max = null, int startLevel = 0, int stepLevel = 0,
            bool exceptClasses = false, StatType stat = StatType.Unknown,
            BlueprintUnitProperty customProperty = null,
            BlueprintCharacterClass[] classes = null, BlueprintArchetype archetype = null,
            BlueprintFeature feature = null, BlueprintFeature[] featureList = null,
            (int, int)[] customProgression = null)
        {
            var config = Create<ContextRankConfig>();
            setType(config, type);
            setBaseValueType(config, baseValueType);
            setProgression(config, progression);
            setUseMin(config, min.HasValue);
            setMin(config, min.GetValueOrDefault());
            setUseMax(config, max.HasValue);
            setMax(config, max.GetValueOrDefault());
            setStartLevel(config, startLevel);
            setStepLevel(config, stepLevel);
            setFeature(config, feature);
            setExceptClasses(config, exceptClasses);
            setCustomProperty(config, customProperty);
            setStat(config, stat);
            setClass(config, classes ?? Array.Empty<BlueprintCharacterClass>());
            setArchetype(config, archetype);
            setFeatureList(config, featureList ?? Array.Empty<BlueprintFeature>());

            if (customProgression != null)
            {
                var items = Array.CreateInstance(customProgressionItemType, customProgression.Length);
                for (int i = 0; i < items.Length; i++)
                {
                    var item = Activator.CreateInstance(customProgressionItemType);
                    var p = customProgression[i];
                    SetField(item, "BaseValue", p.Item1);
                    SetField(item, "ProgressionValue", p.Item2);
                    items.SetValue(item, i);
                }
                setCustomProgression(config, items);
            }

            return config;
        }

        static readonly FastSetter setType = Helpers.CreateFieldSetter<ContextRankConfig>("m_Type");
        static readonly FastSetter setBaseValueType = Helpers.CreateFieldSetter<ContextRankConfig>("m_BaseValueType");
        static readonly FastSetter setProgression = Helpers.CreateFieldSetter<ContextRankConfig>("m_Progression");
        static readonly FastSetter setUseMin = Helpers.CreateFieldSetter<ContextRankConfig>("m_UseMin");
        static readonly FastSetter setMin = Helpers.CreateFieldSetter<ContextRankConfig>("m_Min");
        static readonly FastSetter setUseMax = Helpers.CreateFieldSetter<ContextRankConfig>("m_UseMax");
        static readonly FastSetter setMax = Helpers.CreateFieldSetter<ContextRankConfig>("m_Max");
        static readonly FastSetter setStartLevel = Helpers.CreateFieldSetter<ContextRankConfig>("m_StartLevel");
        static readonly FastSetter setStepLevel = Helpers.CreateFieldSetter<ContextRankConfig>("m_StepLevel");
        static readonly FastSetter setFeature = Helpers.CreateFieldSetter<ContextRankConfig>("m_Feature");
        static readonly FastSetter setExceptClasses = Helpers.CreateFieldSetter<ContextRankConfig>("m_ExceptClasses");
        static readonly FastSetter setCustomProperty = Helpers.CreateFieldSetter<ContextRankConfig>("m_CustomProperty");
        static readonly FastSetter setStat = Helpers.CreateFieldSetter<ContextRankConfig>("m_Stat");
        static readonly FastSetter setClass = Helpers.CreateFieldSetter<ContextRankConfig>("m_Class");
        static readonly FastSetter setArchetype = Helpers.CreateFieldSetter<ContextRankConfig>("Archetype");
        static readonly FastSetter setFeatureList = Helpers.CreateFieldSetter<ContextRankConfig>("m_FeatureList");
        static readonly FastSetter setCustomProgression = Helpers.CreateFieldSetter<ContextRankConfig>("m_CustomProgression");
        static readonly Type customProgressionItemType = Harmony12.AccessTools.Inner(typeof(ContextRankConfig), "CustomProgressionItem");


        public static void SetField(object obj, string name, object value)
        {
            Harmony12.AccessTools.Field(obj.GetType(), name).SetValue(obj, value);
        }

        public static void SetLocalizedStringField(BlueprintScriptableObject obj, string name, string value)
        {
            Harmony12.AccessTools.Field(obj.GetType(), name).SetValue(obj, Helpers.CreateString($"{obj.name}.{name}", value));
        }

        public static object GetField(object obj, string name)
        {
            return Harmony12.AccessTools.Field(obj.GetType(), name).GetValue(obj);
        }

        public static object GetField(Type type, object obj, string name)
        {
            return Harmony12.AccessTools.Field(type, name).GetValue(obj);
        }

        public static T GetField<T>(object obj, string name)
        {
            return (T)Harmony12.AccessTools.Field(obj.GetType(), name).GetValue(obj);
        }

        public static FastGetter CreateGetter<T>(string name) => CreateGetter(typeof(T), name);

        public static FastGetter CreateGetter(Type type, string name)
        {
            return new FastGetter(Harmony12.FastAccess.CreateGetterHandler(Harmony12.AccessTools.Property(type, name)));
        }

        public static FastGetter CreateFieldGetter<T>(string name) => CreateFieldGetter(typeof(T), name);

        public static FastGetter CreateFieldGetter(Type type, string name)
        {
            return new FastGetter(Harmony12.FastAccess.CreateGetterHandler(Harmony12.AccessTools.Field(type, name)));
        }

        public static FastSetter CreateSetter<T>(string name) => CreateSetter(typeof(T), name);

        public static FastSetter CreateSetter(Type type, string name)
        {
            return new FastSetter(Harmony12.FastAccess.CreateSetterHandler(Harmony12.AccessTools.Property(type, name)));
        }

        public static FastSetter CreateFieldSetter<T>(string name) => CreateFieldSetter(typeof(T), name);

        public static FastSetter CreateFieldSetter(Type type, string name)
        {
            return new FastSetter(Harmony12.FastAccess.CreateSetterHandler(Harmony12.AccessTools.Field(type, name)));
        }

        public static FastInvoke CreateInvoker<T>(String name) => CreateInvoker(typeof(T), name);

        public static FastInvoke CreateInvoker(Type type, String name)
        {
            return new FastInvoke(Harmony12.MethodInvoker.GetHandler(Harmony12.AccessTools.Method(type, name)));
        }

        public static FastInvoke CreateInvoker<T>(String name, Type[] args, Type[] typeArgs = null) => CreateInvoker(typeof(T), name, args, typeArgs);

        public static FastInvoke CreateInvoker(Type type, String name, Type[] args, Type[] typeArgs = null)
        {
            return new FastInvoke(Harmony12.MethodInvoker.GetHandler(Harmony12.AccessTools.Method(type, name, args, typeArgs)));
        }

        public static LocalizedString CreateString(string key, string value)
        {
            // See if we used the text previously.
            // (It's common for many features to use the same localized text.
            // In that case, we reuse the old entry instead of making a new one.)
            LocalizedString localized;
            /*if (textToLocalizedString.TryGetValue(value, out localized))
            {
                return localized;
            }*/
            var strings = LocalizationManager.CurrentPack.Strings;
            String oldValue;
            if (strings.TryGetValue(key, out oldValue) && value != oldValue)
            {
#if DEBUG
                //Log.Write($"Info: duplicate localized string `{key}`, different text.");
#endif
            }
            strings[key] = value;
            localized = new LocalizedString();
            localizedString_m_Key(localized, key);
            textToLocalizedString[value] = localized;
            return localized;
        }

        // All localized strings created in this mod, mapped to their localized key. Populated by CreateString.
        static Dictionary<String, LocalizedString> textToLocalizedString = new Dictionary<string, LocalizedString>();
        static FastSetter localizedString_m_Key = Helpers.CreateFieldSetter<LocalizedString>("m_Key");

        public static class GuidStorage
        {
            static Dictionary<string, string> guids_in_use = new Dictionary<string, string>();
            static bool allow_guid_generation;

            static public void load(string file_content)
            {
                load(file_content, false);
            }

            static public void load(string file_content, bool debug_mode)
            {
                allow_guid_generation = debug_mode;
                guids_in_use = new Dictionary<string, string>();
                using (System.IO.StringReader reader = new System.IO.StringReader(file_content))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] items = line.Split('\t');
                        guids_in_use.Add(items[0], items[1]);
                    }
                }
            }

            static public void dump(string guid_file_name)
            {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(guid_file_name))
                {
                    foreach (var pair in guids_in_use)
                    {
                        BlueprintScriptableObject existing;
                        Main.library.BlueprintsByAssetId.TryGetValue(pair.Value, out existing);
                        if (existing != null)
                        {
                            sw.WriteLine(pair.Key + '\t' + pair.Value + '\t' + existing.GetType().FullName);
                        }
                    }
                }
            }

            static public void addEntry(string name, string guid)
            {
                string original_guid;
                if (guids_in_use.TryGetValue(name, out original_guid))
                {
                    if (original_guid != guid)
                    {
                        throw SKMLogger.Error($"Asset: {name}, is already registered for object with another guid: {guid}");
                    }
                }
                else
                {
                    guids_in_use.Add(name, guid);
                }
            }


            static public bool hasStoredGuid(string blueprint_name)
            {
                string stored_guid = "";
                return guids_in_use.TryGetValue(blueprint_name, out stored_guid);
            }


            static public string getGuid(string name)
            {
                string original_guid;
                if (guids_in_use.TryGetValue(name, out original_guid))
                {
                    return original_guid;
                }
                else if (allow_guid_generation)
                {
                    return Guid.NewGuid().ToString("N");
                }
                else
                {
                    throw SKMLogger.Error($"Missing AssetId for: {name}"); //ensure that no guids generated in release mode
                }
            }


            static public string maybeGetGuid(string name, string new_guid = "")
            {
                string original_guid;
                if (guids_in_use.TryGetValue(name, out original_guid))
                {
                    return original_guid;
                }
                else
                {
                    return new_guid;
                }
            }


            static public string maybeGetGuid(string name)
            {
                string original_guid;
                if (guids_in_use.TryGetValue(name, out original_guid))
                {
                    return original_guid;
                }
                else
                {
                    return getGuidFromString(name);
                }
            }


            static public string getGuidFromString(string str)
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                    Guid new_guid = new Guid(hash);
                    return new_guid.ToString();
                }
            }

        }
    }

    public delegate void FastSetter(object source, object value);
    public delegate object FastGetter(object source);
    public delegate object FastInvoke(object target, params object[] paramters);
}
