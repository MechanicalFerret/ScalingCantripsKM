using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Recommendations;
using ScalingCantripsKM.Config;
using ScalingCantripsKM.DataEntries;
using ScalingCantripsKM.Utilities;

namespace ScalingCantripsKM
{
    public static class CantripFeatures
    {

        public static void Init()
        {
            SKMLogger.Log("Adding Cantrip Feats");
            AddCantripFeatures();
            AddCantripPrereqs();
        }

        private static void AddCantripPrereqs()
        {
            // Intelligence
            var AddIntToCantripDamage = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddIntStatToDamage");
            AddIntToCantripDamage.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                c.Feature = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddWisStatToDamage");
            }));
            AddIntToCantripDamage.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                c.Feature = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddChaStatToDamage");
            }));

            // Wisdom
            var AddWisToCantripDamage = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddWisStatToDamage");
            AddWisToCantripDamage.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                c.Feature = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddIntStatToDamage");
            }));
            AddWisToCantripDamage.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                c.Feature = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddChaStatToDamage");
            }));

            // Dump Stat. I mean Charisma
            var AddChaToCantripDamage = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddChaStatToDamage");
            AddChaToCantripDamage.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                c.Feature = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddWisStatToDamage");
            }));
            AddChaToCantripDamage.AddComponent(Helpers.Create<PrerequisiteNoFeature>(c => {
                c.Feature = Blueprints.GetModBlueprint<BlueprintFeature>("SKMAddIntStatToDamage");
            }));
        }

        private static void AddCantripFeatures()
        {
            var AddWisToCantripDamage = Blueprints.CreateBlueprint<BlueprintFeature>("SKMAddWisStatToDamage", bp =>
            {
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                    FeatureGroup.WizardFeat, FeatureGroup.Feat};
                bp.Ranks = 1;
                bp.SetName("Cantrip Expert (Wisdom)");
                bp.SetDescription("Cantrips you can cast now have a damage bonus equal to your Wisdom stat bonus.");
                bp.AddComponent(Helpers.Create<AddCasterStatToDamage>(c =>
                {
                    c.statType = Kingmaker.EntitySystem.Stats.StatType.Wisdom;

                }));
                bp.AddComponent(Helpers.Create<RecommendationRequiresSpellbook>());

            });

            var AddIntToCantripDamage = Blueprints.CreateBlueprint<BlueprintFeature>("SKMAddIntStatToDamage", bp =>
            {
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                    FeatureGroup.WizardFeat, FeatureGroup.Feat};
                bp.Ranks = 1;
                bp.SetName("Cantrip Expert (Intelligence)");
                bp.SetDescription("Cantrips you can cast now have a damage bonus equal to your Intelligence stat bonus.");
                bp.AddComponent(Helpers.Create<AddCasterStatToDamage>(c =>
                {
                    c.statType = Kingmaker.EntitySystem.Stats.StatType.Intelligence;

                }));
                bp.AddComponent(Helpers.Create<RecommendationRequiresSpellbook>());

            });

            var AddChaToCantripDamage = Blueprints.CreateBlueprint<BlueprintFeature>("SKMAddChaStatToDamage", bp =>
            {
                bp.IsClassFeature = true;
                bp.Groups = new FeatureGroup[] {
                    FeatureGroup.WizardFeat, FeatureGroup.Feat};
                bp.Ranks = 1;
                bp.SetName("Cantrip Expert (Charisma)");
                bp.SetDescription("Cantrips you can cast now have a damage bonus equal to your Charisma stat bonus.");
                bp.AddComponent(Helpers.Create<AddCasterStatToDamage>(c =>
                {
                    c.statType = Kingmaker.EntitySystem.Stats.StatType.Charisma;

                }));
                bp.AddComponent(Helpers.Create<RecommendationRequiresSpellbook>());

            });

            AddFeaturetoSelection(AddWisToCantripDamage);
            AddFeaturetoSelection(AddIntToCantripDamage);
            AddFeaturetoSelection(AddChaToCantripDamage);
        }

        static void AddFeaturetoSelection(BlueprintFeature feat)
        {
            var BasicFeat = Blueprints.GetBlueprint<BlueprintFeatureSelection>("247a4068296e8be42890143f451b4b45");
            var ArcaneBloodline = Blueprints.GetBlueprint<BlueprintFeatureSelection>("ff4fd877b4c801342ab8e880b734a6b9");
            var InfernalBloodline = Blueprints.GetBlueprint<BlueprintFeatureSelection>("f19d9bfcbc1e3ea42bda754a03c40843");
            var MagusFeat = Blueprints.GetBlueprint<BlueprintFeatureSelection>("66befe7b24c42dd458952e3c47c93563");
            var WizardFeat = Blueprints.GetBlueprint<BlueprintFeatureSelection>("8c3102c2ff3b69444b139a98521a4899");


            AddtoSelection(feat, BasicFeat);
            AddtoSelection(feat, ArcaneBloodline);
            AddtoSelection(feat, InfernalBloodline);
            AddtoSelection(feat, MagusFeat);
            AddtoSelection(feat, WizardFeat);

        }
        static void AddtoSelection(BlueprintFeature feat, BlueprintFeatureSelection selection)
        {
            selection.Features = selection.AllFeatures.AppendToArray(feat);
            selection.AllFeatures = selection.AllFeatures.AppendToArray(feat);

        }
    }
}
