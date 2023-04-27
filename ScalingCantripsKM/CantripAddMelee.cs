using Harmony12;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using ScalingCantripsKM.Config;
using ScalingCantripsKM.Utilities;

namespace ScalingCantripsKM
{
    internal class CantripAddMelee
    {
        static LibraryScriptableObject library => Main.library;

        public static void Init()
        {
            SKMLogger.Log("Adding New Melee Cantrips");
            AddJoltingGrasp();
        }

        static void AddJoltingGrasp()
        {
            SKMLogger.Log("Adding Jolting Grasp");
            var ShockingGrasp = Blueprints.GetBlueprint<BlueprintAbility>("17451c1327c571641a1345bd31155209");
            var TouchReference = Blueprints.GetBlueprint<BlueprintItemWeapon>("bb337517547de1a4189518d404ec49d4");
            var JoltingGraspEffect = Blueprints.CreateBlueprint<BlueprintAbility>("SKMJoltingGraspEffect", bp =>
            {

                bp.SetIcon(ShockingGrasp.Icon);
                bp.SetName("Jolting Grasp");
                bp.SetDescription("Your successful melee {g|Encyclopedia:TouchAttack}touch attack{/g} deals {g|Encyclopedia:Dice}1d3{/g} points of {g|Encyclopedia:Energy_Damage}electricity damage{/g} per "
                    + Settings.CasterLevelsReq + " {g|Encyclopedia:Caster_Level}caster level(s){/g} (maximum " + Settings.MaxDice +
                    "d6)" + " When delivering the jolt, you gain a +3 {g|Encyclopedia:Bonus}bonus{/g} on {g|Encyclopedia:Attack}attack rolls{/g} if the opponent is wearing metal armor (or is carrying a metal weapon or is made of metal).");
                bp.SpellResistance = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = false;
                bp.LocalizedDuration = ShockingGrasp.LocalizedDuration;
                bp.LocalizedSavingThrow = ShockingGrasp.LocalizedSavingThrow;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
                bp.AnimationStyle = Kingmaker.View.Animation.CastAnimationStyle.CastActionTouch;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Empower | Metamagic.Maximize | Metamagic.Quicken | Metamagic.Reach | Metamagic.Heighten;
                bp.MaterialComponent = new BlueprintAbility.MaterialComponentData();
                bp.MaterialComponent.Count = 1;
                bp.ResourceAssetIds.Add("3ab291fca61cf3b4da311da82340ee9e");
                bp.AddComponent(Helpers.Create<AbilitySpawnFx>(c =>
                {
                    c.Anchor = AbilitySpawnFxAnchor.SelectedTarget;
                    c.PrefabLink = new Kingmaker.ResourceLinks.PrefabLink();
                    c.PrefabLink.AssetId = "3ab291fca61cf3b4da311da82340ee9e";
                }));
                bp.AddComponent(Helpers.Create<AbilityDeliverTouch>(c =>
                {
                    c.TouchWeapon = TouchReference;
                }));
                bp.AddComponent(Helpers.Create<SpellComponent>(c =>
                {
                    c.School = SpellSchool.Evocation;
                }));
                bp.AddComponent(Helpers.Create<SpellDescriptorComponent>(c =>
                {
                    c.Descriptor = new SpellDescriptorWrapper(SpellDescriptor.Electricity);
                }));

                // TODO: Check here again for Settings of Jolting Grasp
                var BaseValueType = ContextRankBaseValueType.CustomProperty;
                var Progression = (Settings.StartImmediately) ? ContextRankProgression.OnePlusDivStep : ContextRankProgression.StartPlusDivStep;
                var Type = AbilityRankType.Default;
                var StartLevel = (Settings.StartImmediately) ? 0 : 1;
                var StepLevel = Settings.CasterLevelsReq;
                var Min = 1;
                var Max = Settings.MaxDice;
                var ExceptClasses = false;
                var Stat = StatType.Unknown;
                var CustomProperty = CantripPatcher.CreateHighestCasterLevel();
                var RankConfig = Helpers.CreateContextRankConfig(BaseValueType, Progression, Type, Min, Max, StartLevel, StepLevel, ExceptClasses, Stat, CustomProperty);
                bp.AddComponent(RankConfig);
                bp.AddComponent(Helpers.Create<CantripComponent>());
                ContextActionDealDamage dmgContext = Helpers.Create<ContextActionDealDamage>();
                dmgContext.DamageType = new DamageTypeDescription()
                {
                    Type = DamageType.Energy,
                    Common = new DamageTypeDescription.CommomData(),
                    Physical = new DamageTypeDescription.PhysicalData(),
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Electricity
                };
                dmgContext.Duration = new ContextDurationValue()
                {
                    DiceCountValue = new ContextValue(),
                    BonusValue = new ContextValue()
                };
                dmgContext.Value = new ContextDiceValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.D3,
                    DiceCountValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Rank
                    },
                    BonusValue = new ContextValue()
                };
                bp.AddComponent(Helpers.Create<AbilityEffectRunAction>(c =>
                {
                    c.Actions = new Kingmaker.ElementsSystem.ActionList()
                    {
                        Actions = new Kingmaker.ElementsSystem.GameAction[]
                        {
                            dmgContext
                        }
                    };
                }));
            }
            );
            var JoltingGrasp = Blueprints.CreateBlueprint<BlueprintAbility>("SKMJoltingGrasp", bp =>
            {
                bp.SetIcon(ShockingGrasp.Icon);
                bp.SetName("Jolting Grasp");
                bp.SetDescription("Your successful melee {g|Encyclopedia:TouchAttack}touch attack{/g} deals {g|Encyclopedia:Dice}1d3{/g} points of {g|Encyclopedia:Energy_Damage}electricity damage{/g} per " + Settings.CasterLevelsReq +
                    " {g|Encyclopedia:Caster_Level}caster level(s){/g} (maximum " + Settings.MaxDice + "d3)" +
                    " When delivering the jolt, you gain a +3 {g|Encyclopedia:Bonus}bonus{/g} on {g|Encyclopedia:Attack}attack rolls{/g} if the opponent is wearing metal armor (or is carrying a metal weapon or is made of metal).");
                bp.SpellResistance = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = false;
                bp.ActionBarAutoFillIgnored = false;
                bp.LocalizedDuration = ShockingGrasp.LocalizedDuration;
                bp.LocalizedSavingThrow = ShockingGrasp.LocalizedSavingThrow;
                bp.Type = AbilityType.Spell;
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch;
                bp.AnimationStyle = Kingmaker.View.Animation.CastAnimationStyle.CastActionTouch;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Quicken | Metamagic.Reach | Metamagic.Heighten;
                bp.MaterialComponent = new BlueprintAbility.MaterialComponentData();
                bp.MaterialComponent.Count = 1;
                bp.ResourceAssetIds.Add("3ab291fca61cf3b4da311da82340ee9e");
                bp.AddComponent(Helpers.Create<SpellComponent>(c =>
                {

                    c.School = SpellSchool.Evocation;
                }));
                bp.AddComponent(Helpers.Create<SpellDescriptorComponent>(c =>
                {
                    c.Descriptor = SpellDescriptor.Electricity;
                }));
                bp.AddComponent(Helpers.Create<CantripComponent>());
                bp.AddComponent(Helpers.Create<AbilityEffectStickyTouch>(c =>
                {
                    c.TouchDeliveryAbility = JoltingGraspEffect;
                }));
            });

            SpellTools.AddSpell(JoltingGrasp, SpellTools.SpellList.Arcanist);
            SpellTools.AddSpell(JoltingGrasp, SpellTools.SpellList.Magus);
            SpellTools.AddSpell(JoltingGrasp, SpellTools.SpellList.Rogue);
            SpellTools.AddSpell(JoltingGrasp, SpellTools.SpellList.Sorcerer);
            SpellTools.AddSpell(JoltingGrasp, SpellTools.SpellList.Wizard);
        }
    }
}
