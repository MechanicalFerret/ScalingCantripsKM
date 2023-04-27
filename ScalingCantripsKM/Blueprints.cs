using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Utility;
using Newtonsoft.Json;
using ScalingCantripsKM.Utilities;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScalingCantripsKM
{
    internal class Blueprints
    {
        // Blueprints Logic
        private static string filename = "blueprints.json";
        private static string newFilename = "new_blueprints.json";

        private static readonly Dictionary<string, BlueprintScriptableObject> ModBlueprints = new Dictionary<string, BlueprintScriptableObject>();
        private static readonly Dictionary<string, string> ModBlueprintNameToGUID = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> NewBlueprintNameToGUID = new Dictionary<string, string>();
        private static LibraryScriptableObject library => Main.library;

        public static void LoadBlueprints()
        {
            string filePath = $"{Main.modEntry.Path}{filename}";
            if (File.Exists(filePath))
            {
                SKMLogger.Log($"Loading {filePath}");
                using (StreamReader file = File.OpenText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    BlueprintJson json = (BlueprintJson)serializer.Deserialize(file, typeof(BlueprintJson));
                    json.nameToGUID.ForEach(entry => ModBlueprintNameToGUID.Add(entry.Key, entry.Value));
                }
            }
        }

        public static void WriteBlueprints()
        {
            string filePath = $"{Main.modEntry.Path}{newFilename}";
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
                        ModBlueprintNameToGUID.ForEach(entry => json.nameToGUID.Add(entry.Key, entry.Value));
                        NewBlueprintNameToGUID.ForEach(entry => json.nameToGUID.Add(entry.Key, entry.Value));
                        serializer.Serialize(jWriter, json);
                    }
                }
            }
            if (File.Exists(filePath))
            {
                Main.modEntry.Enabled = false;
                throw SKMLogger.Error($"A {newFilename} has been generated at {filePath}, must be added to a release.");
            }
        }

        public static string GetGUID(string name)
        {
            if (!ModBlueprintNameToGUID.TryGetValue(name, out var id))
            {
#if DEBUG
                if (!NewBlueprintNameToGUID.TryGetValue(name, out var newId))
                {
                    id = Guid.NewGuid().ToString();
                    NewBlueprintNameToGUID.Add(name, id);
                    SKMLogger.Debug($"Generating new GUID for {name}: {id}");
                }
                else
                {
                    id = newId;
                }
#endif
            }
            if (id == null)
            {
                Main.modEntry.Enabled = false;
                SKMLogger.Error($"GUID for {name} not found, Blueprints.json may be out of date.");
            }
            return id;
        }

        public static T GetModBlueprint<T>(string name) where T : BlueprintScriptableObject
        {
            var assetId = GetGUID(name);
            ModBlueprints.TryGetValue(assetId, out var modBlueprint);
            return modBlueprint as T;
        }

        public static T GetBlueprint<T>(string id) where T : BlueprintScriptableObject
        {
            ModBlueprints.TryGetValue(id, out var value);
            if (value == null) value = library.TryGet<T>(id) as T;
            return value as T;
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
