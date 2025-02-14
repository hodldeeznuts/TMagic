﻿using RimWorld;
using System;
using Verse;
using AbilityUser;
using UnityEngine;

namespace TorannMagic
{
    class Verb_Mimic : Verb_UseAbility  
    {
        private bool validTarg = false;

        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {            
            if ( targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    Pawn targetPawn = targ.Thing as Pawn;
                    if(targ.Thing is Pawn)
                    {
                        if(targetPawn.RaceProps.Humanlike)
                        {
                            ShootLine shootLine;
                            validTarg = this.TryFindShootLineFromTo(root, targ, out shootLine);
                        }
                        else
                        {
                            validTarg = false;
                            //Log.Message("Target was not humanlike or did not have traits");
                        }
                    }
                    else
                    {
                        //target is not a pawn
                        validTarg = false;
                        //Log.Message("Target was not a valid pawn to mimic.");
                    }
                    
                }
                else
                {
                    validTarg = false;
                }
            }
            else
            {
                validTarg = false;
            }
            return validTarg;
        }

        protected override bool TryCastShot()
        {
            bool result = false;

            if (this.currentTarget != null && base.CasterPawn != null && this.currentTarget.Thing is Pawn)
            {
                Pawn targetPawn = this.currentTarget.Thing as Pawn;
                if (targetPawn.RaceProps.Humanlike)
                {
                    CompAbilityUserMagic magicPawn = targetPawn.GetComp<CompAbilityUserMagic>();
                    CompAbilityUserMight mightPawn = targetPawn.GetComp<CompAbilityUserMight>();

                    TMAbilityDef tempAbility = null;
                    CompAbilityUserMight mightComp = this.CasterPawn.GetComp<CompAbilityUserMight>();
                    CompAbilityUserMagic magicComp = this.CasterPawn.GetComp<CompAbilityUserMagic>();

                    if (magicPawn.IsMagicUser)
                    {
                        if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 3);
                                if (rnd == 0 && magicPawn.MagicData.MagicPowersA[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersA[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Shadow;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Shadow_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Shadow_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Shadow_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 1 && magicPawn.MagicData.MagicPowersA[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    int level = magicPawn.MagicData.MagicPowersA[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_MagicMissile;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_MagicMissile_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_MagicMissile_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_MagicMissile_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersA[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersA[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Blink;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Blink_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Blink_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Blink_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersA[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersA[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Summon;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Summon_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Summon_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Summon_III;
                                            break;
                                    }
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 3);
                                if (rnd == 0 && magicPawn.MagicData.MagicPowersSB[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersSB[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_AMP;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_AMP_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_AMP_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_AMP_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 1 && magicPawn.MagicData.MagicPowersSB[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_LightningBolt;
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersSB[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_LightningCloud;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersSB[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_LightningStorm;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 3);
                                if (rnd == 0 && magicPawn.MagicData.MagicPowersIF[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersIF[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_RayofHope;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_RayofHope_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_RayofHope_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_RayofHope_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 1 && magicPawn.MagicData.MagicPowersIF[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Firebolt;
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersIF[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Fireclaw;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersIF[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Fireball;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 4);
                                if (rnd == 0 && magicPawn.MagicData.MagicPowersHoF[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersHoF[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Soothe;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Soothe_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Soothe_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Soothe_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 1 && magicPawn.MagicData.MagicPowersHoF[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_Rainmaker;
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersHoF[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Icebolt;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersHoF[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    int level = magicPawn.MagicData.MagicPowersHoF[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_FrostRay;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_FrostRay_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_FrostRay_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_FrostRay_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 4 && magicPawn.MagicData.MagicPowersHoF[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Snowball;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Druid))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 3);
                                if (rnd == 0 && magicPawn.MagicData.MagicPowersD[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Poison;
                                    i = 5;
                                }
                                else if (rnd == 1 && magicPawn.MagicData.MagicPowersD[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersD[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_SootheAnimal;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_SootheAnimal_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_SootheAnimal_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_SootheAnimal_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersD[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_Regenerate;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersD[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_CureDisease;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(1, 3);
                                if (rnd == 1 && magicPawn.MagicData.MagicPowersN[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    int level = magicPawn.MagicData.MagicPowersN[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_DeathMark;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_DeathMark_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_DeathMark_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_DeathMark_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersN[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_FogOfTorment;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersN[rnd + 1].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    int level = magicPawn.MagicData.MagicPowersN[rnd + 1].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_CorpseExplosion;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_CorpseExplosion_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_CorpseExplosion_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_CorpseExplosion_III;
                                            break;
                                    }
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Paladin))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 3);
                                if (rnd == 1 && magicPawn.MagicData.MagicPowersP[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersP[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Shield;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Shield_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Shield_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Shield_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 0 && magicPawn.MagicData.MagicPowersP[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_Heal;
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersP[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_ValiantCharge;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersP[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Overwhelm;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Priest))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 3);
                                if (rnd == 3 && magicPawn.MagicData.MagicPowersPR[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersPR[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_BestowMight;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_BestowMight_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_BestowMight_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_BestowMight_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersPR[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersPR[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_HealingCircle;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_HealingCircle_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_HealingCircle_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_HealingCircle_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 1 && magicPawn.MagicData.MagicPowersPR[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_Purify;
                                    i = 5;
                                }
                                else if (rnd == 0 && magicPawn.MagicData.MagicPowersPR[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_AdvancedHeal;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Summoner))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(1, 3);
                                if (rnd == 1 && magicPawn.MagicData.MagicPowersS[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_SummonPylon;
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersS[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_SummonExplosive;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersS[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_SummonElemental;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
                        {
                            int level = magicPawn.MagicData.MagicPowersB[3].level;
                            switch (level)
                            {
                                case 0:
                                    tempAbility = TorannMagicDefOf.TM_Lullaby;
                                    break;
                                case 1:
                                    tempAbility = TorannMagicDefOf.TM_Lullaby_I;
                                    break;
                                case 2:
                                    tempAbility = TorannMagicDefOf.TM_Lullaby_II;
                                    break;
                                case 3:
                                    tempAbility = TorannMagicDefOf.TM_Lullaby_III;
                                    break;
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Warlock))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(1, 3);
                                if (rnd == 1 && magicPawn.MagicData.MagicPowersWD[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    int level = magicPawn.MagicData.MagicPowersWD[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersWD[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Dominate;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersWD[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersWD[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Repulsion;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Repulsion_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Repulsion_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Repulsion_III;
                                            break;
                                    }
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Succubus))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(1, 3);
                                if (rnd == 1 && magicPawn.MagicData.MagicPowersSD[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    int level = magicPawn.MagicData.MagicPowersSD[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_ShadowBolt_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 2 && magicPawn.MagicData.MagicPowersSD[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_Dominate;
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersSD[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersSD[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Attraction;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Attraction_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Attraction_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Attraction_III;
                                            break;
                                    }
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(0, 2);
                                if (rnd == 2)
                                {
                                    rnd = 3;
                                }
                                if (rnd == 0 && magicPawn.MagicData.MagicPowersG[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_Stoneskin;
                                    i = 5;
                                }
                                else if (rnd == 1 && magicPawn.MagicData.MagicPowersG[rnd].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersG[rnd].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Encase;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Encase_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Encase_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Encase_III;
                                            break;
                                    }
                                    i = 5;
                                }
                                else if (rnd == 3 && magicPawn.MagicData.MagicPowersG[rnd].learned && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                                {
                                    tempAbility = TorannMagicDefOf.TM_EarthernHammer;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                int rnd = Rand.RangeInclusive(3, 5);
                                if (rnd == 3 && magicPawn.MagicData.MagicPowersT[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_TechnoShield;
                                    i = 5;
                                }
                                else if (rnd == 4 && magicPawn.MagicData.MagicPowersT[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_Sabotage;
                                    i = 5;
                                }
                                else if (rnd == 5 && magicPawn.MagicData.MagicPowersT[rnd].learned)
                                {
                                    tempAbility = TorannMagicDefOf.TM_Overdrive;
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Enchanter))
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                if (magicPawn.MagicData.MagicPowersE[4].learned)
                                {
                                    int level = magicPawn.MagicData.MagicPowersE[4].level;
                                    switch (level)
                                    {
                                        case 0:
                                            tempAbility = TorannMagicDefOf.TM_Polymorph;
                                            break;
                                        case 1:
                                            tempAbility = TorannMagicDefOf.TM_Polymorph_I;
                                            break;
                                        case 2:
                                            tempAbility = TorannMagicDefOf.TM_Polymorph_II;
                                            break;
                                        case 3:
                                            tempAbility = TorannMagicDefOf.TM_Polymorph_III;
                                            break;
                                    }
                                    i = 5;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.BloodMage))
                        {
                            Messages.Message("TM_CannotMimicBloodMage".Translate(
                                    this.CasterPawn.LabelShort
                                ), MessageTypeDefOf.RejectInput);
                        }


                        if (tempAbility != null)
                        {
                            if (mightComp.mimicAbility != null)
                            {
                                mightComp.RemovePawnAbility(mightComp.mimicAbility);
                            }
                            if (magicComp.mimicAbility != null)
                            {
                                magicComp.RemovePawnAbility(magicComp.mimicAbility);
                            }
                            mightComp.mimicAbility = tempAbility;
                            mightComp.AddPawnAbility(tempAbility);
                        }
                        else
                        {
                            //invalid target
                            Messages.Message("TM_MimicFailed".Translate(
                                    this.CasterPawn.LabelShort
                                ), MessageTypeDefOf.RejectInput);

                        }
                    }
                    else if (mightPawn.IsMightUser)
                    {
                        if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                        {
                            int rnd = Rand.RangeInclusive(0, 1);
                            if (rnd == 0)
                            {
                                int level = mightPawn.MightData.MightPowersG[2].level;
                                switch (level)
                                {
                                    case 0:
                                        tempAbility = TorannMagicDefOf.TM_Grapple;
                                        break;
                                    case 1:
                                        tempAbility = TorannMagicDefOf.TM_Grapple_I;
                                        break;
                                    case 2:
                                        tempAbility = TorannMagicDefOf.TM_Grapple_II;
                                        break;
                                    case 3:
                                        tempAbility = TorannMagicDefOf.TM_Grapple_III;
                                        break;
                                }
                            }
                            else
                            {
                                tempAbility = TorannMagicDefOf.TM_Whirlwind;
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                        {
                            int rnd = Rand.RangeInclusive(0, 2);
                            if(rnd==0)
                            {
                                tempAbility = TorannMagicDefOf.TM_AntiArmor;
                            }
                            else if (rnd == 1)
                            {
                                int level = mightPawn.MightData.MightPowersS[2].level;
                                switch (level)
                                {
                                    case 0:
                                        tempAbility = TorannMagicDefOf.TM_DisablingShot;
                                        break;
                                    case 1:
                                        tempAbility = TorannMagicDefOf.TM_DisablingShot_I;
                                        break;
                                    case 2:
                                        tempAbility = TorannMagicDefOf.TM_DisablingShot_II;
                                        break;
                                    case 3:
                                        tempAbility = TorannMagicDefOf.TM_DisablingShot_III;
                                        break;
                                }
                            }
                            else
                            {
                                tempAbility = TorannMagicDefOf.TM_Headshot;
                            }
                            
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                        {
                            int rnd = Rand.RangeInclusive(0, 2);
                            if (rnd == 0)
                            {
                                tempAbility = TorannMagicDefOf.TM_SeismicSlash;
                            }
                            else if (rnd == 1)
                            {
                                int level = mightPawn.MightData.MightPowersB[4].level;
                                switch (level)
                                {
                                    case 0:
                                        tempAbility = TorannMagicDefOf.TM_PhaseStrike;
                                        break;
                                    case 1:
                                        tempAbility = TorannMagicDefOf.TM_PhaseStrike_I;
                                        break;
                                    case 2:
                                        tempAbility = TorannMagicDefOf.TM_PhaseStrike_II;
                                        break;
                                    case 3:
                                        tempAbility = TorannMagicDefOf.TM_PhaseStrike_III;
                                        break;
                                }
                            }
                            else
                            {
                                tempAbility = TorannMagicDefOf.TM_BladeSpin;
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                        {
                            int rnd = Rand.RangeInclusive(0, 1);
                            if (rnd == 0)
                            {
                                int level = mightPawn.MightData.MightPowersB[4].level;
                                switch (level)
                                {
                                    case 0:
                                        tempAbility = TorannMagicDefOf.TM_ArrowStorm;
                                        break;
                                    case 1:
                                        tempAbility = TorannMagicDefOf.TM_ArrowStorm_I;
                                        break;
                                    case 2:
                                        tempAbility = TorannMagicDefOf.TM_ArrowStorm_II;
                                        break;
                                    case 3:
                                        tempAbility = TorannMagicDefOf.TM_ArrowStorm_III;
                                        break;
                                }
                            }
                            else
                            {
                                tempAbility = TorannMagicDefOf.TM_PoisonTrap;
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic))
                        {
                            int rnd = Rand.RangeInclusive(0, 3);
                            if ((rnd == 0 || rnd == 3) && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                            {
                                int level = mightPawn.MightData.MightPowersP[1].level;
                                switch (level)
                                {
                                    case 0:
                                        tempAbility = TorannMagicDefOf.TM_PsionicBlast;
                                        break;
                                    case 1:
                                        tempAbility = TorannMagicDefOf.TM_PsionicBlast_I;
                                        break;
                                    case 2:
                                        tempAbility = TorannMagicDefOf.TM_PsionicBlast_II;
                                        break;
                                    case 3:
                                        tempAbility = TorannMagicDefOf.TM_PsionicBlast_III;
                                        break;
                                }
                            }
                            else if (rnd == 1 && !magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                            {
                                tempAbility = TorannMagicDefOf.TM_PsionicDash;
                            }
                            else 
                            {
                                int level = mightPawn.MightData.MightPowersP[3].level;
                                switch (level)
                                {
                                    case 0:
                                        tempAbility = TorannMagicDefOf.TM_PsionicBarrier;
                                        break;
                                    case 1:
                                        tempAbility = TorannMagicDefOf.TM_PsionicBarrier_Projected;
                                        break;                                    
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.DeathKnight))
                        {
                            int rnd = Rand.RangeInclusive(1, 2);
                            if (rnd == 1 || magicPawn.Pawn.story.WorkTagIsDisabled(WorkTags.Violent))
                            {
                                tempAbility = TorannMagicDefOf.TM_WaveOfFear;                                
                            }
                            else
                            {
                                int level = mightPawn.MightData.MightPowersDK[4].level;
                                switch (level)
                                {
                                    case 0:
                                        tempAbility = TorannMagicDefOf.TM_GraveBlade;
                                        break;
                                    case 1:
                                        tempAbility = TorannMagicDefOf.TM_GraveBlade_I;
                                        break;
                                    case 2:
                                        tempAbility = TorannMagicDefOf.TM_GraveBlade_II;
                                        break;
                                    case 3:
                                        tempAbility = TorannMagicDefOf.TM_GraveBlade_III;
                                        break;
                                }
                            }
                        }
                        else if (targetPawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                        {

                        }

                        if (tempAbility != null)
                        {
                            if (mightComp.mimicAbility != null)
                            {
                                mightComp.RemovePawnAbility(mightComp.mimicAbility);
                            }
                            if(magicComp.mimicAbility != null)
                            {
                                magicComp.RemovePawnAbility(magicComp.mimicAbility);
                            }
                            mightComp.mimicAbility = tempAbility;
                            mightComp.AddPawnAbility(tempAbility);
                        }
                        else
                        {
                            //invalid target
                            Messages.Message("TM_MimicFailed".Translate(
                                    this.CasterPawn.LabelShort
                                ), MessageTypeDefOf.RejectInput);
                        }
                    }
                }
            }
            else
            {
                Log.Warning("failed to TryCastShot");
            }
            this.burstShotsLeft = 0;
            return result;
        }
    }
}
