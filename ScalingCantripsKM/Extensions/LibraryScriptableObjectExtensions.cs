using Kingmaker.Blueprints;
using ScalingCantripsKM.Utilities;
using System;

namespace ScalingCantripsKM.Extensions
{
    static class LibraryScriptableObjectExtensions
    {
        static readonly FastSetter blueprintScriptableObject_set_AssetId = Helpers.CreateFieldSetter<BlueprintScriptableObject>("m_AssetGuid");
        public static void AddAsset(this LibraryScriptableObject library, BlueprintScriptableObject blueprint, String guid)
        {
            if (guid == "")
            {
                guid = Helpers.GuidStorage.getGuid(blueprint.name);
            }
            else if (guid[0] == '$')
            {
                guid = Helpers.GuidStorage.maybeGetGuid(blueprint.name, guid.Remove(0));
            }
            blueprintScriptableObject_set_AssetId(blueprint, guid);
            // Sanity check that we don't stop on our own GUIDs or someone else's.
            BlueprintScriptableObject existing;
            if (library.BlueprintsByAssetId.TryGetValue(guid, out existing))
            {
                throw SKMLogger.Error($"Duplicate AssetId for {blueprint.name}, existing entry ID: {guid}, name: {existing.name}, type: {existing.GetType().Name}");
            }
            else if (guid == "")
            {
                throw SKMLogger.Error($"Missing AssetId: {guid}, name: {existing.name}, type: {existing.GetType().Name}");
            }

            library.GetAllBlueprints().Add(blueprint);
            library.BlueprintsByAssetId[guid] = blueprint;
            Helpers.GuidStorage.addEntry(blueprint.name, guid);
        }

        public static T TryGet<T>(this LibraryScriptableObject library, String assetId) where T : BlueprintScriptableObject
        {
            BlueprintScriptableObject result;
            if (library.BlueprintsByAssetId.TryGetValue(assetId, out result))
            {
                return (T)result;
            }
            return null;
        }
    }
}
