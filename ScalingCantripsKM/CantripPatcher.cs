using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using ScalingCantripsKM.Config;
using ScalingCantripsKM.DataEntries;
using ScalingCantripsKM.Utilities;
using System.Linq;
using static Kingmaker.RuleSystem.DiceType;

namespace ScalingCantripsKM
{
    internal class CantripPatcher
    {
        static LibraryScriptableObject library => Main.library;

        public static void Init()
        {
            SKMLogger.Log("Patching Existing Cantrips");
            BlueprintAbility AcidSplash = Blueprints.GetBlueprint<BlueprintAbility>("0c852a2405dd9f14a8bbcfaf245ff823");
            BlueprintAbility RayOfFrost = Blueprints.GetBlueprint<BlueprintAbility>("9af2ab69df6538f4793b2f9c3cc85603");
            BlueprintAbility Jolt = Blueprints.GetBlueprint<BlueprintAbility>("16e23c7a8ae53cc42a93066d19766404");
            BlueprintAbility DisruptUndead = Blueprints.GetBlueprint<BlueprintAbility>("652739779aa05504a9ad5db1db6d02ae");
            BlueprintBuff VirtueBuff = Blueprints.GetBlueprint<BlueprintBuff>("a13ad2502d9e4904082868eb71efb0c5");
            BlueprintAbility Virtue = Blueprints.GetBlueprint<BlueprintAbility>("d3a852385ba4cd740992d1970170301a");
            EditAndAddAbility(AcidSplash, D3);
            EditAndAddAbility(RayOfFrost, D3);
            EditAndAddAbility(Jolt, D3);
            EditAndAddAbility(DisruptUndead, D6);
            EditVirtue(VirtueBuff, Virtue);
        }

        private static void EditAndAddAbility(BlueprintAbility cantrip, Kingmaker.RuleSystem.DiceType diceType)
        {
            SKMLogger.Log($"Patching: {cantrip.name}");

            // Rank Config Creation
            var BaseValueType = ContextRankBaseValueType.CustomProperty;
            var Progression = (Settings.StartImmediately) ? ContextRankProgression.OnePlusDivStep : ContextRankProgression.StartPlusDivStep;
            var Type = AbilityRankType.Default;
            var StartLevel = (Settings.StartImmediately) ? 0 : 1;
            var StepLevel = Settings.CasterLevelsReq;
            var Min = 1;
            var Max = Settings.MaxDice;
            var ExceptClasses = false;
            var Stat = StatType.Unknown;
            var CustomProperty = CreateHighestCasterLevel();
            var RankConfig = Helpers.CreateContextRankConfig(BaseValueType, Progression, Type, Min, Max, StartLevel, StepLevel, ExceptClasses, Stat, CustomProperty);

            // Cantrip Modification
            var DamageAction = cantrip.GetComponent<AbilityEffectRunAction>().Actions.Actions.OfType<ContextActionDealDamage>().First().Value;
            var oldDiceType = DamageAction.DiceType;
            DamageAction.DiceType = diceType;
            DamageAction.DiceCountValue.Value = 0;
            DamageAction.DiceCountValue.ValueType = ContextValueType.Rank;
            if (cantrip.GetComponent<ContextRankConfig>() == null) cantrip.AddComponents(RankConfig);
            else cantrip.ReplaceComponent<ContextRankConfig>(RankConfig);

            // Unfortuantely mods that change the damage type of cantrips (like CallOfTheWild) will have their damage dice show up incorrectly
            // in the description. Not sure if this should be accounted for but its a known issue.
            var description = cantrip.Description.Replace($"d{(int)oldDiceType}", $"d{(int) diceType}");
            var newString = $"{description} Damage dice is increased by 1 every {Settings.CasterLevelsReq} caster level(s), up to a maximum of {Settings.MaxDice}d{(int) diceType}.";
            cantrip.SetDescription(newString);
        }

        private static void EditVirtue(BlueprintBuff buff, BlueprintAbility cantrip)
        {
            // Rank Config
            var BaseValueType = ContextRankBaseValueType.CustomProperty;
            var Progression = (Settings.StartImmediately) ? ContextRankProgression.OnePlusDivStep : ContextRankProgression.StartPlusDivStep;
            var Type = AbilityRankType.Default;
            var StartLevel = (Settings.StartImmediately) ? 0 : 1;
            var StepLevel = Settings.CasterLevelsReq;
            var Min = 1;
            var Max = Settings.MaxDice;
            var ExceptClasses = false;
            var Stat = StatType.Unknown;
            var CustomProperty = CreateHighestCasterLevel();
            var RankConfig = Helpers.CreateContextRankConfig(BaseValueType, Progression, Type, Min, Max, StartLevel, StepLevel, ExceptClasses, Stat, CustomProperty);

            // Cantrip Modification
            buff.GetComponent<TemporaryHitPointsFromAbilityValue>().Value.ValueType = ContextValueType.Rank;
            buff.GetComponent<TemporaryHitPointsFromAbilityValue>().Value.Value = 0;
            if (buff.GetComponent<ContextRankConfig>() == null)
            {
                buff.AddComponent(RankConfig);
            }
            else
            {
                buff.ReplaceComponent<ContextRankConfig>(RankConfig);

            }

            var newString = buff.Description;
            newString += "For every " + Settings.CasterLevelsReq + " caster level(s) the caster has, Virtue will grant another point of temporary HP, up to "
                + Settings.MaxDice + " points total.";
            buff.SetDescription(newString);
            cantrip.SetDescription(newString);
        }

        public static BlueprintUnitProperty CreateHighestCasterLevel()
        {
            var customProperty = Blueprints.GetModBlueprint<BlueprintUnitProperty>("SKMHighestCasterLevelCantrips");

            if (customProperty == null)
            {
                customProperty = Blueprints.CreateBlueprint<BlueprintUnitProperty>("SKMHighestCasterLevelCantrips", bp =>
                {
                    bp.AddComponent(Helpers.Create<HighestCasterLevel>());
                });
            }
            return customProperty;
        }
    }
}
