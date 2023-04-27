using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;

namespace ScalingCantripsKM.DataEntries
{
    public class AddCasterStatToDamage : RuleInitiatorLogicComponent<RuleCalculateDamage>
    {
        [ShowIf("UseContextBonus")]
        public ContextValue Value;

        public StatType statType;
        public bool SpellsOnly = false;
        public bool UseContextBonus;
        public bool CantripsOnly = true;

        public override void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            MechanicsContext context = evt.Reason.Context;
            if ((context?.SourceAbility) == null) return;
            if ((context?.MaybeCaster) == null) return;
            if ((!context.SourceAbility.IsSpell && this.SpellsOnly)) return;
            if (!context.SourceAbility.IsCantrip && CantripsOnly) return;
            if (statType == 0 || (int)statType > 6) return;

            ModifiableValueAttributeStat CasterStat = null;
            if (statType == StatType.Intelligence) CasterStat = context.MaybeCaster.Stats.Intelligence;
            if (statType == StatType.Wisdom) CasterStat = context.MaybeCaster.Stats.Wisdom;
            if (statType == StatType.Charisma) CasterStat = context.MaybeCaster.Stats.Charisma;
            if (CasterStat == null) return;


            foreach (BaseDamage baseDamage in evt.DamageBundle)
            {
                int bonus = CasterStat.Bonus;
                if (!baseDamage.Sneak)
                {
                    baseDamage.AddBonus(bonus);
                }
            }
        }

        public override void OnEventDidTrigger(RuleCalculateDamage evt)
        {
            // Nothing
        }
    }
}
