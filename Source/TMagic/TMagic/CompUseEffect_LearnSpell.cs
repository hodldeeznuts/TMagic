﻿using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace TorannMagic
{
    public class CompUseEffect_LearnSpell : CompUseEffect
    {
        public override void DoEffect(Pawn user)
        {
            CompAbilityUserMagic comp = user.GetComp<CompAbilityUserMagic>();
            MagicPower magicPower;

            if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Enchanter) || user.story.traits.HasTrait(TorannMagicDefOf.BloodMage) || user.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || user.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || user.story.traits.HasTrait(TorannMagicDefOf.Warlock) || user.story.traits.HasTrait(TorannMagicDefOf.Succubus) || user.story.traits.HasTrait(TorannMagicDefOf.TM_Bard) || user.story.traits.HasTrait(TorannMagicDefOf.Priest) || (user.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || user.story.traits.HasTrait(TorannMagicDefOf.Lich)) || user.story.traits.HasTrait(TorannMagicDefOf.Druid) || parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Summoner) || user.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost) || user.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || user.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || user.story.traits.HasTrait(TorannMagicDefOf.Paladin))))
            {
                if (parent.def.defName == "SpellOf_Rain" && comp.spell_Rain == false)
                {
                    comp.spell_Rain = true;
                    magicPower = comp.MagicData.MagicPowersHoF.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Rainmaker);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Rainmaker);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Blink" && comp.spell_Blink == false)
                {
                    comp.spell_Blink = true;
                    magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Blink);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Blink);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Teleport" && comp.spell_Teleport == false)
                {
                    comp.spell_Teleport = true;
                    magicPower = comp.MagicData.MagicPowersA.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Teleport);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Teleport);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Heal" && comp.spell_Heal == false)
                {
                    comp.spell_Heal = true;
                    magicPower = comp.MagicData.MagicPowersP.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Heal);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Heal);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Heater" && comp.spell_Heater == false)
                {
                    comp.spell_Heater = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Cooler" && comp.spell_Cooler == false)
                {
                    comp.spell_Cooler = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_PowerNode" && comp.spell_PowerNode == false)
                {
                    comp.spell_PowerNode = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Sunlight" && comp.spell_Sunlight == false)
                {
                    comp.spell_Sunlight = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_DryGround" && comp.spell_DryGround == false && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                {
                    comp.spell_DryGround = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Firestorm" && comp.spell_Firestorm == false && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                {
                    comp.spell_Firestorm = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_WetGround" && comp.spell_WetGround == false && user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                {
                    comp.spell_WetGround = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Blizzard" && comp.spell_Blizzard == false && user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                {
                    comp.spell_Blizzard = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_ChargeBattery" && comp.spell_ChargeBattery == false && user.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
                {
                    comp.spell_ChargeBattery = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_SmokeCloud" && comp.spell_SmokeCloud == false)
                {
                    comp.spell_SmokeCloud = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Extinguish" && comp.spell_Extinguish == false)
                {
                    comp.spell_Extinguish = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_EMP" && comp.spell_EMP == false)
                {
                    comp.spell_EMP = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_SummonMinion" && comp.spell_SummonMinion == false)
                {
                    comp.spell_SummonMinion = true;
                    magicPower = comp.MagicData.MagicPowersS.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_SummonMinion);
                    comp.AddPawnAbility(TorannMagicDefOf.TM_SummonMinion);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_TransferMana" && comp.spell_TransferMana == false)
                {
                    comp.spell_TransferMana = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_SiphonMana" && comp.spell_SiphonMana == false)
                {
                    comp.spell_SiphonMana = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_RegrowLimb" && comp.spell_RegrowLimb == false && user.story.traits.HasTrait(TorannMagicDefOf.Druid))
                {
                    comp.spell_RegrowLimb = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_EyeOfTheStorm" && comp.spell_EyeOfTheStorm == false && user.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
                {
                    comp.spell_EyeOfTheStorm = true;
                    magicPower = comp.MagicData.MagicPowersSB.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EyeOfTheStorm);
                    magicPower.learned = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_ManaShield" && comp.spell_ManaShield == false)
                {
                    comp.spell_ManaShield = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_FoldReality" && comp.spell_FoldReality == false && user.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
                {
                    comp.spell_FoldReality = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Resurrection" && comp.spell_Resurrection == false && user.story.traits.HasTrait(TorannMagicDefOf.Priest))
                {
                    comp.spell_Resurrection = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_BattleHymn" && comp.spell_BattleHymn == false && user.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
                {
                    comp.spell_BattleHymn = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_HolyWrath" && comp.spell_HolyWrath == false && user.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                {
                    comp.spell_HolyWrath = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_LichForm" && comp.spell_LichForm == false && user.story.traits.HasTrait(TorannMagicDefOf.Necromancer))
                {
                    comp.spell_LichForm = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_SummonPoppi" && comp.spell_SummonPoppi == false && user.story.traits.HasTrait(TorannMagicDefOf.Summoner))
                {
                    comp.spell_SummonPoppi = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Scorn" && comp.spell_Scorn == false && user.story.traits.HasTrait(TorannMagicDefOf.Succubus))
                {
                    comp.spell_Scorn = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_PsychicShock" && comp.spell_PsychicShock == false && user.story.traits.HasTrait(TorannMagicDefOf.Warlock))
                {
                    comp.spell_PsychicShock = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Meteor" && comp.spell_Meteor == false && user.story.traits.HasTrait(TorannMagicDefOf.Geomancer))
                {
                    comp.spell_Meteor = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_OrbitalStrike" && comp.spell_OrbitalStrike == false && user.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                {
                    comp.spell_OrbitalStrike = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_CauterizeWound" && comp.spell_CauterizeWound == false && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                {
                        comp.spell_CauterizeWound = true;
                        comp.InitializeSpell();
                        this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_FertileLands" && comp.spell_FertileLands == false && user.story.traits.HasTrait(TorannMagicDefOf.Druid))
                {
                    comp.spell_FertileLands = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);                    
                }
                else if (parent.def.defName == "SpellOf_SpellMending" && comp.spell_SpellMending == false)
                {
                        comp.spell_SpellMending = true;
                        comp.InitializeSpell();
                        this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_TechnoShield" && comp.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoShield).learned == false && user.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                {
                    comp.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_TechnoShield).learned = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_TechnoShield);
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Sabotage" && comp.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sabotage).learned == false && user.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                {
                    comp.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Sabotage).learned = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Sabotage);
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_Overdrive" && comp.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overdrive).learned == false && user.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                {
                    comp.MagicData.MagicPowersT.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_Overdrive).learned = true;
                    comp.AddPawnAbility(TorannMagicDefOf.TM_Overdrive);
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def.defName == "SpellOf_BloodMoon" && comp.spell_BloodMoon == false && user.story.traits.HasTrait(TorannMagicDefOf.BloodMage))
                {
                    comp.spell_BloodMoon = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_Shapeshift && comp.spell_Shapeshift == false && user.story.traits.HasTrait(TorannMagicDefOf.Enchanter))
                {
                    comp.spell_Shapeshift = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_Blur && comp.spell_Blur == false)
                {
                    comp.spell_Blur = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_BlankMind && comp.spell_BlankMind == false && user.story.traits.HasTrait(TorannMagicDefOf.Enchanter))
                {
                    comp.spell_BlankMind = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_DirtDevil && comp.spell_DirtDevil == false)
                {
                    comp.spell_DirtDevil = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_MechaniteReprogramming && comp.spell_MechaniteReprogramming == false && user.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                {
                    comp.spell_MechaniteReprogramming = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_ArcaneBolt && comp.spell_ArcaneBolt == false && !comp.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                {
                    comp.spell_ArcaneBolt = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_LightningTrap && comp.spell_LightningTrap == false && !comp.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                {
                    comp.spell_LightningTrap = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_Invisibility && comp.spell_Invisibility == false)
                {
                    comp.spell_Invisibility = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else if (parent.def == TorannMagicDefOf.SpellOf_BriarPatch && comp.spell_BriarPatch == false && user.story.traits.HasTrait(TorannMagicDefOf.Druid))
                {
                    comp.spell_BriarPatch = true;
                    comp.InitializeSpell();
                    this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
                }
                else
                {
                    Messages.Message("CannotLearnSpell".Translate(), MessageTypeDefOf.RejectInput);
                }
            }
            else
            {
                Messages.Message("NotMageToLearnSpell".Translate(), MessageTypeDefOf.RejectInput);
            }
        }
    }
}
