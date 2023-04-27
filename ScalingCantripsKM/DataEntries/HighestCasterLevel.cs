using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScalingCantripsKM.DataEntries
{
    public class HighestCasterLevel : PropertyValueGetter
    {

        public override int GetInt(UnitEntityData unit)
        {
            // this won't be perfect but it will at least be a fix
            // we will go through every non-mythic spellbook and just send back the highest out
            int HighestCasterLevel = 0;
            foreach (Spellbook spellbook in unit.Descriptor.Spellbooks)
            {
                if (HighestCasterLevel < spellbook.CasterLevel)
                {
                    HighestCasterLevel = spellbook.CasterLevel;
                }
            }
            return HighestCasterLevel;
        }

    }
}
