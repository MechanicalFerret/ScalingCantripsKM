using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System.Linq;
using ScalingCantripsKM.Config;
using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Collections.Generic;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Harmony12;

namespace ScalingCantripsKM.Utilities
{
    static class SpellTools
    {
        public static void AddSpell(BlueprintAbility spell, SpellList spellList, int level = 0)
        {
            if (!spellList.AllowsForSpell(spell)) return;
            SKMLogger.Log($"Adding {spell.Name} to {spellList.GetName()}");
            BlueprintSpellList List = spellList.GetSpellList();
            BlueprintFeature Cantrips = spellList.GetCantripFeature();
            BlueprintFeatureReplaceSpellbook ReplacementSpellBook = spellList.GetReplacedSpellBook();
            if (spellList.isSpecialistSchool())
            {
                List.SpellsByLevel[level].Spells.Add(spell);
            } 
            else
            {
                AddToListIfMissing();
                AddToCantripsIfMissing();
                AddToReplacementSpellBookIfMissing();
            }

            // Iterate through all archetypes, there are conditionals on the spell list that will prevent them from being
            // added where they don't belong but ultimately we assume if a class is being added to we want to handle the archetypes.
            spellList.GetArchetypes().ForEach(spl =>
            {
                AddSpell(spell, spl, level);
            });

            void AddToListIfMissing()
            {
                if (List == null) return;
                if (!spell.GetComponents<SpellListComponent>().Any(c => c.SpellList == List && c.SpellLevel == level))
                {
                    var comp = Helpers.Create<SpellListComponent>();
                    comp.SpellLevel = level;
                    comp.SpellList = List;
                    spell.AddComponent(comp);
                }
                if (!List.SpellsByLevel[level].Spells.Contains(spell))
                {
                    List.SpellsByLevel[level].Spells.Add(spell);
                    List.SpellsByLevel[level].Spells.Sort((x, y) => x.Name.CompareTo(y.Name));
                }
            }
            void AddToCantripsIfMissing()
            {
                if (level != 0) return;
                if (Cantrips == null) return;
                Cantrips.ReapplyOnLevelUp = true;
                Cantrips.GetComponent<AddFacts>().Facts = Cantrips.GetComponent<AddFacts>().Facts.AppendToArrayIfMissing(spell);
                if (Cantrips.GetComponent<LearnSpells>() != null)
                {
                    Cantrips.GetComponent<LearnSpells>().Spells = Cantrips.GetComponent<LearnSpells>().Spells.AppendToArrayIfMissing(spell);
                }
                Cantrips.GetComponent<BindAbilitiesToClass>().Abilites = Cantrips.GetComponent<BindAbilitiesToClass>().Abilites.AppendToArrayIfMissing(spell);
            }
            void AddToReplacementSpellBookIfMissing()
            {
                if (level != 0) return;
                if (ReplacementSpellBook == null) return;
                ReplacementSpellBook.ReapplyOnLevelUp = true;
                ReplacementSpellBook.GetComponent<AddFacts>().Facts = ReplacementSpellBook.GetComponent<AddFacts>().Facts.AppendToArrayIfMissing(spell);
                ReplacementSpellBook.GetComponent<LearnSpells>().Spells = ReplacementSpellBook.GetComponent<LearnSpells>().Spells.AppendToArrayIfMissing(spell);
                ReplacementSpellBook.GetComponent<BindAbilitiesToClass>().Abilites = ReplacementSpellBook.GetComponent<BindAbilitiesToClass>().Abilites.AppendToArrayIfMissing(spell);
            }
        }

        public class SpellList
        {
            public static SpellList Arcanist = new SpellList("Arcanist", Spellbook: "ba0401fdeb4062f40a7aa95b6f07fe89", Cantrips: "3a5bd474be40421484c1aeb9d79870c9", isCallOfTheWild: true);
            public static SpellList Bloodrager = new SpellList("Bloodrager", Spellbook: "e93c0b4113f2498f8f206b3fe02f7964", isCallOfTheWild: true);
            public static SpellList Cleric = new SpellList("Cleric", Spellbook: "8443ce803d2d31347897a3d85cc32f53", Cantrips: "e62f392949c24eb4b8fb2bc9db4345e3");
            public static SpellList Druid = new SpellList("Druid", Spellbook: "bad8638d40639d04fa2f80a1cac67d6b", Cantrips: "f2ed91cc202bd344691eef91eb6d5d1a")
                .AddArchetype(new SpellList("Feyspeaker", Spellbook: "640b4c89527334e45b19d884dd82e500", Cantrips: "27b2bc1b3589cc54491b78966e8013e6"));
            public static SpellList Inquisitor = new SpellList("Inquisitor", Spellbook: "57c894665b7895c499b3dce058c284b3", Cantrips: "4f898e6a004b2a84686c1fbd0ffe950e");
            public static SpellList Magus = new SpellList("Magus", Spellbook: "4d72e1e7bd6bc4f4caaea7aa43a14639", Cantrips: "fa5799fb32844e94e88d4cb3430610ff")
                .AddArchetype(new SpellList("EldritchScion", Spellbook: "4d72e1e7bd6bc4f4caaea7aa43a14639", Cantrips: "d093b8dec70b5d144acb593a3029d830"));
            public static SpellList Oracle = new SpellList("Oracle", Spellbook: "f305174b73f64783a8379238a14c3283", Cantrips: "efe37da91f6c4ae5917beb8ec215e02f", isCallOfTheWild: true);
            public static SpellList Rogue = new SpellList("Rogue")
                .AddArchetype(new SpellList("EldritchScoundrel", Spellbook: "ba0401fdeb4062f40a7aa95b6f07fe89", Cantrips: "0e451b208e7b855468986e03fcd4f990"));
            public static SpellList Shaman = new SpellList("Shaman", Spellbook: "7113337f695742559ecdecc8905b132a", Cantrips: "8051f445c28e451baf670036be2b6d8c", isCallOfTheWild: true);
            public static SpellList Sorcerer = new SpellList("Sorcerer", Spellbook: "ba0401fdeb4062f40a7aa95b6f07fe89", Cantrips: "c58b36ec3f759c84089c67611d1bcc21")
                .AddArchetype( new SpellList("Empyreal", Cantrips: "6acb21fbc1bb76c4c9d65ba94c9f15ac"))
                .AddArchetype(new SpellList("Sage", Spellbook: "ba0401fdeb4062f40a7aa95b6f07fe89", Cantrips: "50d700ea98467834c9fc622efa03d598"));
            public static SpellList Warpriest = new SpellList("Warpriest", Spellbook: "9ef48172d50446aca4c80f321402f743", Cantrips: "a5387ec4685944e1bc829acb84ca96a9", isCallOfTheWild: true);
            public static SpellList Witch = new SpellList("Witch", Spellbook: "422490cf62744e16a3e131efd94cf290", Cantrips: "86501dda312a4f548d579632c4a06c0f", isCallOfTheWild: true);
            public static SpellList Wizard = new SpellList("Wizard", Spellbook: "ba0401fdeb4062f40a7aa95b6f07fe89", Cantrips: "44d19b62d00179e4bad7afae7684f2e2")
                .AddArchetype(new SpellList("Abjuration Wizard", Spellbook: "c7a55e475659a944f9229d89c4dc3a8e", OnlySchools: new SpellSchool[] { SpellSchool.Abjuration }))
                .AddArchetype(new SpellList("Conjuration Wizard", Spellbook: "69a6eba12bc77ea4191f573d63c9df12", OnlySchools: new SpellSchool[] { SpellSchool.Conjuration }))
                .AddArchetype(new SpellList("Divination Wizard", Spellbook: "d234e68b3d34d124a9a2550fdc3de9eb", OnlySchools: new SpellSchool[] { SpellSchool.Divination }))
                .AddArchetype(new SpellList("Enchantment Wizard", Spellbook: "c72836bb669f0c04680c01d88d49bb0c", OnlySchools: new SpellSchool[] { SpellSchool.Enchantment }))
                .AddArchetype(new SpellList("Evocation Wizard", Spellbook: "79e731172a2dc1f4d92ba229c6216502", OnlySchools: new SpellSchool[] { SpellSchool.Evocation }))
                .AddArchetype(new SpellList("Illusion Wizard", Spellbook: "d74e55204daa9b14993b2e51ae861501", OnlySchools: new SpellSchool[] { SpellSchool.Illusion }))
                .AddArchetype(new SpellList("Necromancy Wizard", Spellbook: "5fe3acb6f439db9438db7d396f02c75c", OnlySchools: new SpellSchool[] { SpellSchool.Necromancy }))
                .AddArchetype(new SpellList("Transmutation Wizard", Spellbook: "becbcfeca9624b6469319209c2a6b7f1", OnlySchools: new SpellSchool[] { SpellSchool.Transmutation }))
                .AddArchetype(new SpellList("Thas Abjuration Wizard", Spellbook: "280dd5167ccafe449a33fbe93c7a875e", ReplacementSpellbook: "15c681d5a76c1a742abe2760376ddf6d", OpposedSchools: new SpellSchool[] { SpellSchool.Evocation, SpellSchool.Necromancy }))
                .AddArchetype(new SpellList("Thas Conjuration Wizard", Spellbook: "5b154578f228c174bac546b6c29886ce", ReplacementSpellbook: "1a258cd8e93461a4ab011c73a2c43dac", OpposedSchools: new SpellSchool[] { SpellSchool.Evocation, SpellSchool.Illusion }))
                .AddArchetype(new SpellList("Thas Enchantment Wizard", Spellbook: "ac551db78c1baa34eb8edca088be13cb", ReplacementSpellbook: "e1ebc61a71c55054991863a5f6f6d2c2", OpposedSchools: new SpellSchool[] { SpellSchool.Necromancy, SpellSchool.Transmutation }))
                .AddArchetype(new SpellList("Thas Evocation Wizard", Spellbook: "17c0bfe5b7c8ac3449da655cdcaed4e7", ReplacementSpellbook: "5e33543285d1c3d49b55282cf466bef3", OpposedSchools: new SpellSchool[] { SpellSchool.Abjuration, SpellSchool.Conjuration }))
                .AddArchetype(new SpellList("Thas Illusion Wizard", Spellbook: "c311aed33deb7a346ab715baef4a0572", ReplacementSpellbook: "aa271e69902044b47a8e62c4e58a9dcb", OpposedSchools: new SpellSchool[] { SpellSchool.Conjuration, SpellSchool.Transmutation }))
                .AddArchetype(new SpellList("Thas Necromancy Wizard", Spellbook: "5c08349132cb6b04181797f58ccf38ae", ReplacementSpellbook: "fb343ede45ca1a84496c91c190a847ff", OpposedSchools: new SpellSchool[] { SpellSchool.Abjuration, SpellSchool.Enchantment }))
                .AddArchetype(new SpellList("Thas Transmutation Wizard", Spellbook: "f3a8f76b1d030a64084355ba3eea369a", ReplacementSpellbook: "dd163630abbdace4e85284c55d269867", OpposedSchools: new SpellSchool[] { SpellSchool.Enchantment, SpellSchool.Illusion }));


            // SpellList Entry
            private string name;
            private string spellListAssetId;
            private string cantripFeatureAssetId;
            private string replacedSpellbookAssetId;
            private SpellSchool[] onlySchools;
            private SpellSchool[] opposedSchools;
            private Dictionary<string, SpellList> archetypes;
            private bool isSpecialist;
            private bool isCallOfTheWild;

            public SpellList(string Name, string Spellbook = null, string Cantrips = null, string ReplacementSpellbook = null, bool isSpecialistSchool = false, bool isCallOfTheWild = false, SpellSchool[] OnlySchools = null, SpellSchool[] OpposedSchools = null)
            {
                this.name = Name;
                this.spellListAssetId = Spellbook;
                this.cantripFeatureAssetId = Cantrips;
                this.replacedSpellbookAssetId = ReplacementSpellbook;
                this.onlySchools = OnlySchools;
                this.opposedSchools = OpposedSchools;
                this.archetypes = new Dictionary<string, SpellList>();
                this.isSpecialist = isSpecialistSchool;
                this.isCallOfTheWild = isCallOfTheWild;
            }

            public SpellList AddArchetype(SpellList archetype)
            {
                this.archetypes.Add(archetype.name, archetype);
                return this;
            }

            public SpellList GetArchetype(string archetypeName)
            {
                archetypes.TryGetValue(archetypeName, out var value);
                return value;
            }

            public SpellList[] GetArchetypes()
            {
                return archetypes.Values.ToArray();
            }

            public string GetName()
            {
                return name;
            }

            public BlueprintSpellList GetSpellList()
            {
                if (spellListAssetId == null || spellListAssetId.Length == 0) return null;
                if (isCallOfTheWild && !Main.isCallOfTheWildEnabled) return null;
                return Blueprints.GetBlueprint<BlueprintSpellList>(spellListAssetId);
            }

            public BlueprintFeature GetCantripFeature()
            {
                if (cantripFeatureAssetId == null || cantripFeatureAssetId.Length == 0) return null;
                if (isCallOfTheWild && !Main.isCallOfTheWildEnabled) return null;
                return Blueprints.GetBlueprint<BlueprintFeature>(cantripFeatureAssetId);
            }

            public BlueprintFeatureReplaceSpellbook GetReplacedSpellBook()
            {
                if (replacedSpellbookAssetId == null || replacedSpellbookAssetId.Length == 0) return null;
                if (isCallOfTheWild && !Main.isCallOfTheWildEnabled) return null;
                return Blueprints.GetBlueprint<BlueprintFeatureReplaceSpellbook>(replacedSpellbookAssetId);
            }

            public bool AllowsForSpell(BlueprintAbility spell)
            {
                if (onlySchools != null && !onlySchools.Contains(spell.School)) return false;
                if (opposedSchools != null && opposedSchools.Contains(spell.School)) { return false; }
                return true;
            }

            public bool isSpecialistSchool()
            {
                return isSpecialist;
            }
        }
    }
}
