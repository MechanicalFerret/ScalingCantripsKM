using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Localization;
using ScalingCantripsKM.Utilities;
using System;
using UnityEngine;

namespace ScalingCantripsKM.Extensions
{
    static class BlueprintUnitFactExtensions
    {
        // FastSetters
        static readonly FastSetter blueprintUnitFact_set_Description = Helpers.CreateFieldSetter<BlueprintUnitFact>("m_Description");
        static readonly FastSetter blueprintItem_set_Description = Helpers.CreateFieldSetter<BlueprintItem>("m_DescriptionText");
        static readonly FastSetter blueprintUnitFact_set_Icon = Helpers.CreateFieldSetter<BlueprintUnitFact>("m_Icon");
        static readonly FastSetter blueprintUnitFact_set_DisplayName = Helpers.CreateFieldSetter<BlueprintUnitFact>("m_DisplayName");
        static readonly FastGetter blueprintUnitFact_get_Description = Helpers.CreateFieldGetter<BlueprintUnitFact>("m_Description");
        static readonly FastGetter blueprintUnitFact_get_DisplayName = Helpers.CreateFieldGetter<BlueprintUnitFact>("m_DisplayName");

        // Getters
        public static LocalizedString GetName(this BlueprintUnitFact fact) => (LocalizedString)blueprintUnitFact_get_DisplayName(fact);
        public static LocalizedString GetDescription(this BlueprintUnitFact fact) => (LocalizedString)blueprintUnitFact_get_Description(fact);

        // Setters
        public static void SetNameDescriptionIcon(this BlueprintUnitFact feature, String displayName, String description, Sprite icon)
        {
            SetNameDescription(feature, displayName, description);
            feature.SetIcon(icon);
        }

        public static void SetNameDescriptionIcon(this BlueprintUnitFact feature, BlueprintUnitFact other)
        {
            SetNameDescription(feature, other);
            feature.SetIcon(other.Icon);
        }

        public static void SetNameDescription(this BlueprintUnitFact feature, String displayName, String description)
        {
            feature.SetName(Helpers.CreateString(feature.name + ".Name", displayName));
            feature.SetDescription(description);
        }

        public static void SetNameDescription(this BlueprintUnitFact feature, BlueprintUnitFact other)
        {
            blueprintUnitFact_set_DisplayName(feature, other.GetName());
            blueprintUnitFact_set_Description(feature, other.GetDescription());
        }

        public static void SetIcon(this BlueprintUnitFact feature, Sprite icon)
        {
            blueprintUnitFact_set_Icon(feature, icon);
        }

        public static void SetName(this BlueprintUnitFact feature, LocalizedString name)
        {
            blueprintUnitFact_set_DisplayName(feature, name);
        }

        public static void SetName(this BlueprintUnitFact feature, String name)
        {
            blueprintUnitFact_set_DisplayName(feature, Helpers.CreateString(feature.name + ".Name", name));
        }

        public static void SetDescription(this BlueprintUnitFact feature, String description)
        {
            blueprintUnitFact_set_Description(feature, Helpers.CreateString(feature.name + ".Description", description));
        }

        public static void SetDescription(this BlueprintItem item, String description)
        {
            blueprintItem_set_Description(item, Helpers.CreateString(item.name + ".Description", description));
        }

        public static void SetDescription(this BlueprintUnitFact feature, LocalizedString description)
        {
            blueprintUnitFact_set_Description(feature, description);
        }
    }
}
