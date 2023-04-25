using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using Newtonsoft.Json;
using ScalingCantripsKM.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityModManagerNet;

namespace ScalingCantripsKM.Config
{
    internal class Blueprints
    {
        // Blueprints Logic
        private static string filename = "Blueprints.json";
        private static string newFilename = "NewBlueprints.json";

        private static readonly Dictionary<string, BlueprintScriptableObject> ModBlueprints = new Dictionary<string, BlueprintScriptableObject>();
        private static readonly Dictionary<string, string> ModBlueprintNameToGUID = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> NewBlueprintNameToGUID = new Dictionary<string, string>();
        private static LibraryScriptableObject library => Main.library;

        public static void LoadBlueprints()
        {
            string filePath = $"{Main.modEntry.Path}Config{Path.DirectorySeparatorChar}{filename}";
            if (File.Exists(filePath))
            {
                SKMLogger.Log($"Loading {filePath}");
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    BlueprintJson json = (BlueprintJson)serializer.Deserialize(file, typeof(BlueprintJson));
                    json.nameToGUID.ForEach(entry =>
                    {
                        ModBlueprintNameToGUID.Add(entry.Key, entry.Value);
                    });
                }
            }
        }

        public static void WriteBlueprints()
        {
            string filePath = $"{Main.modEntry.Path}Config{Path.DirectorySeparatorChar}{newFilename}";
            if (NewBlueprintNameToGUID.Count > 0)
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    NullValueHandling = NullValueHandling.Include,
                    Formatting = Formatting.Indented,
                };
                using (StreamWriter sWriter = new StreamWriter(filePath))
                {
                    using (JsonWriter jWriter = new JsonTextWriter(sWriter))
                    {
                        BlueprintJson json = new BlueprintJson();
                        NewBlueprintNameToGUID.ForEach(entry =>
                        {
                            json.nameToGUID.Add(entry.Key, entry.Value);
                        });
                        serializer.Serialize(jWriter, json);
                    }
                }
            }
            if (File.Exists(filePath))
            {
                Main.modEntry.Enabled = false;
                throw SKMLogger.Error($"New Blueprints generated at {filePath}, must be added to a release. Are you on a Development Build?");
            }
        }

        public static T CreateBlueprint<T>([NotNull] string name, Action<T> init = null) where T : BlueprintScriptableObject
        {
            var result = Helpers.Create<T>();
            result.name = name;
            result.AssetGuid = GetGUID(name);
            AddBlueprint(result);
            init?.Invoke(result);
            return result;
        }

        public static string GetGUID(string name)
        {
            ModBlueprintNameToGUID.TryGetValue(name, out var id);
            if (id == null) // Should only occur in development builds.
            {
                id = Guid.NewGuid().ToString();
                NewBlueprintNameToGUID[name] = id;
            }
            return id;
        }

        public static T GetBlueprint<T>(string id) where T : BlueprintScriptableObject
        {
            ModBlueprints.TryGetValue(id, out var value);
            if (value == null) value = library.TryGet<T>(id) as T;
            return value as T;
        }

        public static void AddBlueprint(BlueprintScriptableObject blueprint, string id = null)
        {
            if (id == null) id = blueprint.AssetGuid;
            var loadedBlueprint = library.TryGet<BlueprintScriptableObject>(id);
            if (loadedBlueprint == null)
            {
                ModBlueprints[id] = blueprint;
                library.AddAsset(blueprint, id);
                blueprint.OnEnable();
                SKMLogger.Patch("Added", blueprint);
            }
            else
            {
                SKMLogger.Error($"Failed to Add: {blueprint.name}");
                SKMLogger.Error($"Asset ID: {id} already in use by: {loadedBlueprint.name}");
            }
        }

        public class BlueprintJson
        {
            [JsonProperty]
            public Dictionary<string, string> nameToGUID = new Dictionary<string, string>();
        }
    }
}
