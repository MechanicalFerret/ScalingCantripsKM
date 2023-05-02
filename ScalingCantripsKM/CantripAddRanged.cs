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
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using ScalingCantripsKM.Config;
using ScalingCantripsKM.Extensions;
using ScalingCantripsKM.Utilities;

namespace ScalingCantripsKM
{
    internal class CantripAddRanged
    {
        public static void Init()
        {
            SKMLogger.Log("Adding New Ranged Cantrips");
            AddFirebolt();
            AddUnholyZap();
            AddDivineZap();
        }

        static void AddFirebolt()
        {
            Cantrip config = Settings.Firebolt;
            SKMLogger.Setting(config.Enabled, "Creating & Adding Firebolt", "Creating Firebolt");

            var IconRef = Blueprints.GetBlueprint<BlueprintAbility>("42a65895ba0cb3a42b6019039dd2bff1");
            var ProjectilRef = Blueprints.GetBlueprint<BlueprintProjectile>("8cc159ce94d29fe46a94b80ce549161f");
            var WeaponRef = Blueprints.GetBlueprint<BlueprintItemWeapon>("f6ef95b1f7bb52b408a5b345a330ffe8");
            var Firebolt = Blueprints.CreateBlueprint<BlueprintAbility>("SKMFirebolt", bp =>
            {
                bp.SetIcon(IconRef.Icon);
                bp.SetName("Firebolt");
                bp.SetDescription("You unleash a bolt of fire via a ranged touch attack. If successful, the target takes {g|Encyclopedia:Dice}1d3{/g} points of fire {g|Encyclopedia:Damage}damage{/g}; for every "
                    + config.CasterLevelsReq + " caster level(s), another dice is added up to a max of " + config.MaxDice +
                    "d3.");
                //bp.m_TargetMapObjects = true;
                bp.Range = AbilityRange.Close;
                bp.SpellResistance = true;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = false;
                bp.LocalizedDuration = Helpers.CreateString("SKM_FB_DR", "");
                bp.LocalizedSavingThrow = Helpers.CreateString("SKM_FB_ST", "");
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Empower | Metamagic.Maximize | Metamagic.Quicken | Metamagic.Reach | Metamagic.Heighten;
                bp.MaterialComponent = new BlueprintAbility.MaterialComponentData();
                bp.MaterialComponent.Count = 1;
                bp.ResourceAssetIds.Add("81e003d5ea5b84b47b67349510f681b3"); // Originally was AddItem
                bp.ResourceAssetIds.Add("2c17c9fd2d8a2314cb1efe869dba4b4a");
                bp.ResourceAssetIds.Add("ee17299746e406d4a9559e2274d18a1b");
                bp.ResourceAssetIds.Add("85a59070f10741745af33c96a5d967f4");
                bp.AddComponent(Helpers.Create<SpellDescriptorComponent>(c =>
                {
                    c.Descriptor = new SpellDescriptorWrapper(SpellDescriptor.Fire);

                }));
                bp.AddComponent(Helpers.Create<AbilityDeliverProjectile>(c =>
                {
                    c.Projectiles = new BlueprintProjectile[] { ProjectilRef };
                    c.Weapon = WeaponRef;
                    c.NeedAttackRoll = true;
                }));
                bp.AddComponent(Helpers.Create<SpellComponent>(c =>
                {
                    c.School = SpellSchool.Evocation;
                }));
                var BaseValueType = ContextRankBaseValueType.CustomProperty;
                var Progression = (config.StartImmediately) ? ContextRankProgression.OnePlusDivStep : ContextRankProgression.StartPlusDivStep;
                var Type = AbilityRankType.Default;
                var StartLevel = (config.StartImmediately) ? 0 : 1;
                var StepLevel = config.CasterLevelsReq;
                var Min = 1;
                var Max = config.MaxDice;
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
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire
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
            });

            // We still need to create Firebolt incase someone already saved their game with it enabled but we wont add it to the spell list.
            // TODO: If we can move when the mod loads to when the game itself loads rather than at the loading screen we could also remove it
            // from an existing spell list. Not sure how to do that just yet.
            if (config.Enabled)
            {
                SpellTools.AddSpell(Firebolt, SpellTools.SpellList.Arcanist);
                SpellTools.AddSpell(Firebolt, SpellTools.SpellList.Inquisitor);
                SpellTools.AddSpell(Firebolt, SpellTools.SpellList.Magus);
                SpellTools.AddSpell(Firebolt, SpellTools.SpellList.Rogue);
                SpellTools.AddSpell(Firebolt, SpellTools.SpellList.Sorcerer);
                SpellTools.AddSpell(Firebolt, SpellTools.SpellList.Summoner);
                SpellTools.AddSpell(Firebolt, SpellTools.SpellList.Wizard);
            }
        }

        static void AddUnholyZap()
        {
            Cantrip config = Settings.UnholyZap;
            SKMLogger.Setting(config.Enabled, "Creating & Adding Unholy Zap", "Creating Unholy Zap");

            var IconRef = Blueprints.GetBlueprint<BlueprintAbility>("fa3078b9976a5b24caf92e20ee9c0f54");
            var ProjectileRef = Blueprints.GetBlueprint<BlueprintProjectile>("fe47a7660448bc54289823b07547bbe8");
            var WeaponRef = Blueprints.GetBlueprint<BlueprintItemWeapon>("f6ef95b1f7bb52b408a5b345a330ffe8");
            var UnholyZap = Blueprints.CreateBlueprint<BlueprintAbility>("SKMUnholyZapEffect", bp =>
            {

                bp.SetIcon(IconRef.Icon);
                bp.SetName("Unholy Zap");
                bp.SetDescription("You unleash your unholy powers against a single target. The target takes {g|Encyclopedia:Dice}1d3{/g} points of negative {g|Encyclopedia:Damage}damage{/g}; for every "
                    + config.CasterLevelsReq + " caster level(s), another dice is added up to a max of " + config.MaxDice +
                    "d3. Fortitude saves if successful, halves damage.");
                //bp.m_TargetMapObjects = true;
                bp.Range = AbilityRange.Close;
                bp.SpellResistance = false;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = false;
                bp.LocalizedDuration = Helpers.CreateString("SKM_UZ_DR", "");
                bp.LocalizedSavingThrow = Helpers.CreateString("SKM_UZ_ST", "Fortitude half");
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Empower | Metamagic.Maximize | Metamagic.Quicken | Metamagic.Reach | Metamagic.Heighten;
                bp.MaterialComponent = new BlueprintAbility.MaterialComponentData();
                bp.MaterialComponent.Count = 1;
                bp.ResourceAssetIds.Add("10bd7db8c04041c47bfbff8a3c5b592d");
                bp.ResourceAssetIds.Add("ee09444ddcf95a144a3f1474ee985465");
                bp.ResourceAssetIds.Add("f5e984268f37b9f4cb44e40a6c8461ce");
                bp.ResourceAssetIds.Add("53279164584a8df4f9b2d6c40d65673d");
                bp.ResourceAssetIds.Add("76b5b5a45eef7e94ca4006486e245b68");
                bp.ResourceAssetIds.Add("e197ea880ace2ca4a9f96598ca96f81e");
                bp.AddComponent(Helpers.Create<AbilityDeliverProjectile>(c =>
                {
                    c.Projectiles = new BlueprintProjectile[] { ProjectileRef };
                    c.Weapon = WeaponRef;
                    c.NeedAttackRoll = false;
                }));
                bp.AddComponent(Helpers.Create<SpellComponent>(c =>
                {
                    c.School = SpellSchool.Necromancy;
                }));
                var BaseValueType = ContextRankBaseValueType.CustomProperty;
                var Progression = (config.StartImmediately) ? ContextRankProgression.OnePlusDivStep : ContextRankProgression.StartPlusDivStep;
                var Type = AbilityRankType.Default;
                var StartLevel = (config.StartImmediately) ? 0 : 1;
                var StepLevel = config.CasterLevelsReq;
                var Min = 1;
                var Max = config.MaxDice;
                var ExceptClasses = false;
                var Stat = StatType.Unknown;
                var CustomProperty = CantripPatcher.CreateHighestCasterLevel();
                var RankConfig = Helpers.CreateContextRankConfig(BaseValueType, Progression, Type, Min, Max, StartLevel, StepLevel, ExceptClasses, Stat, CustomProperty);
                bp.AddComponent(RankConfig);
                bp.AddComponent(Helpers.Create<CantripComponent>());
                bp.AddComponent(Helpers.Create<CantripComponent>());
                ContextActionDealDamage dmgContext = Helpers.Create<ContextActionDealDamage>();
                dmgContext.DamageType = new DamageTypeDescription()
                {
                    Type = DamageType.Energy,
                    Common = new DamageTypeDescription.CommomData(),
                    Physical = new DamageTypeDescription.PhysicalData(),
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.NegativeEnergy
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
                dmgContext.HalfIfSaved = true;
                bp.AddComponent(Helpers.Create<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude;
                    c.Actions = new Kingmaker.ElementsSystem.ActionList()
                    {
                        Actions = new Kingmaker.ElementsSystem.GameAction[]
                        {
                            dmgContext
                        }
                    };
                }));
            });

            if (config.Enabled)
            {
                SpellTools.AddSpell(UnholyZap, SpellTools.SpellList.Cleric);
                SpellTools.AddSpell(UnholyZap, SpellTools.SpellList.Druid);
                SpellTools.AddSpell(UnholyZap, SpellTools.SpellList.Inquisitor);
                SpellTools.AddSpell(UnholyZap, SpellTools.SpellList.Oracle);
                SpellTools.AddSpell(UnholyZap, SpellTools.SpellList.Shaman);
                SpellTools.AddSpell(UnholyZap, SpellTools.SpellList.Warpriest);
                SpellTools.AddSpell(UnholyZap, SpellTools.SpellList.Witch);
            }
        }

        // Divine Zap
        // IconRef Searing Light: bf0accce250381a44b857d4af6c8e10d
        // Projectile Scorching Ray: 8cc159ce94d29fe46a94b80ce549161f
        // Projectile Arrow of Law: d543d55f7fdb60340af40ea8fc5e686d
        static void AddDivineZap()
        {
            Cantrip config = Settings.DivineZap;
            SKMLogger.Setting(config.Enabled, "Creating & Adding Divine Zap", "Creating Divine Zap");

            var IconRef = Blueprints.GetBlueprint<BlueprintAbility>("bf0accce250381a44b857d4af6c8e10d");
            var ProjectileRef = Blueprints.GetBlueprint<BlueprintProjectile>("8cc159ce94d29fe46a94b80ce549161f");
            var WeaponRef = Blueprints.GetBlueprint<BlueprintItemWeapon>("f6ef95b1f7bb52b408a5b345a330ffe8");
            var DivineZap = Blueprints.CreateBlueprint<BlueprintAbility>("SKMDivineZapEffect", bp =>
            {

                bp.SetIcon(IconRef.Icon);
                bp.SetName("Divine Zap");
                bp.SetDescription("You unleash your divine powers against a single target. The target takes {g|Encyclopedia:Dice}1d3{/g} points of positive {g|Encyclopedia:Damage}damage{/g}; for every "
                    + config.CasterLevelsReq + " caster level(s), another dice is added up to a max of " + config.MaxDice +
                    "d3. Fortitude saves if successful, halves damage.");
                //bp.m_TargetMapObjects = true;
                bp.Range = AbilityRange.Close;
                bp.SpellResistance = false;
                bp.CanTargetEnemies = true;
                bp.CanTargetSelf = false;
                bp.LocalizedDuration = Helpers.CreateString("SKM_DZ_DR", "");
                bp.LocalizedSavingThrow = Helpers.CreateString("SKM_DZ_ST", "Fortitude half");
                bp.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                bp.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional;
                bp.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                bp.AvailableMetamagic = Metamagic.Empower | Metamagic.Maximize | Metamagic.Quicken | Metamagic.Reach | Metamagic.Heighten;
                bp.MaterialComponent = new BlueprintAbility.MaterialComponentData();
                bp.MaterialComponent.Count = 1;
                bp.ResourceAssetIds.Add("10bd7db8c04041c47bfbff8a3c5b592d");
                bp.ResourceAssetIds.Add("ee09444ddcf95a144a3f1474ee985465");
                bp.ResourceAssetIds.Add("f5e984268f37b9f4cb44e40a6c8461ce");
                bp.ResourceAssetIds.Add("53279164584a8df4f9b2d6c40d65673d");
                bp.ResourceAssetIds.Add("76b5b5a45eef7e94ca4006486e245b68");
                bp.ResourceAssetIds.Add("e197ea880ace2ca4a9f96598ca96f81e");
                bp.AddComponent(Helpers.Create<AbilityDeliverProjectile>(c =>
                {
                    c.Projectiles = new BlueprintProjectile[] { ProjectileRef };
                    c.Weapon = WeaponRef;
                    c.NeedAttackRoll = false;
                }));
                bp.AddComponent(Helpers.Create<SpellComponent>(c =>
                {
                    c.School = SpellSchool.Divination;
                }));
                var BaseValueType = ContextRankBaseValueType.CustomProperty;
                var Progression = (config.StartImmediately) ? ContextRankProgression.OnePlusDivStep : ContextRankProgression.StartPlusDivStep;
                var Type = AbilityRankType.Default;
                var StartLevel = (config.StartImmediately) ? 0 : 1;
                var StepLevel = config.CasterLevelsReq;
                var Min = 1;
                var Max = config.MaxDice;
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
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.PositiveEnergy
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
                dmgContext.HalfIfSaved = true;
                bp.AddComponent(Helpers.Create<AbilityEffectRunAction>(c =>
                {
                    c.SavingThrowType = Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude;
                    c.Actions = new Kingmaker.ElementsSystem.ActionList()
                    {
                        Actions = new Kingmaker.ElementsSystem.GameAction[]
                        {
                            dmgContext
                        }
                    };
                }));
            });

            if (config.Enabled)
            {
                SpellTools.AddSpell(DivineZap, SpellTools.SpellList.Cleric);
                SpellTools.AddSpell(DivineZap, SpellTools.SpellList.Druid);
                SpellTools.AddSpell(DivineZap, SpellTools.SpellList.Inquisitor);
                SpellTools.AddSpell(DivineZap, SpellTools.SpellList.Oracle);
                SpellTools.AddSpell(DivineZap, SpellTools.SpellList.Shaman);
                SpellTools.AddSpell(DivineZap, SpellTools.SpellList.Warpriest);
                SpellTools.AddSpell(DivineZap, SpellTools.SpellList.Witch);
            }
        }
    }
}
