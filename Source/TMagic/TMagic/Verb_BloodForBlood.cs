﻿using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using AbilityUser;
using Verse;
using Verse.AI;


namespace TorannMagic
{
    public class Verb_BloodForBlood : Verb_UseAbility
    {

        private int verVal;
        private int pwrVal;

        private float arcaneDmg;

        protected override bool TryCastShot()
        {
            bool flag = false;
            MagicPowerSkill bpwr = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_BloodGift.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodGift_pwr");
            MagicPowerSkill pwr = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodForBlood_pwr");
            MagicPowerSkill ver = base.CasterPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_BloodForBlood.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_BloodForBlood_ver");
            verVal = ver.level;
            pwrVal = pwr.level;
            if (base.CasterPawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
            {
                MightPowerSkill mpwr = base.CasterPawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_pwr");
                MightPowerSkill mver = base.CasterPawn.GetComp<CompAbilityUserMight>().MightData.MightPowerSkill_Mimic.FirstOrDefault((MightPowerSkill x) => x.label == "TM_Mimic_ver");
                pwrVal = mpwr.level;
                verVal = mver.level;
            }
            this.arcaneDmg = base.CasterPawn.GetComp<CompAbilityUserMagic>().arcaneDmg;
            this.arcaneDmg *= (1 + (.1f * bpwr.level));
            if(this.currentTarget.Thing != null && this.currentTarget.Thing is Pawn)
            {
                Pawn victim = this.currentTarget.Thing as Pawn;
                if (victim.RaceProps.BloodDef != null && victim != this.CasterPawn)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        TM_MoteMaker.ThrowBloodSquirt(victim.DrawPos, victim.Map, Rand.Range(.6f, .9f));
                    }

                    HealthUtility.AdjustSeverity(victim, TorannMagicDefOf.TM_BloodForBloodHD, (.5f + (.1f * pwrVal)) * this.arcaneDmg);
                    HediffComp_BloodForBlood comp = victim.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_BloodForBloodHD, false).TryGetComp<HediffComp_BloodForBlood>();
                    if (comp != null)
                    {
                        comp.linkedPawn = this.CasterPawn;
                    }
                    else
                    {
                        Messages.Message("TM_InvalidTarget".Translate(this.CasterPawn.LabelShort, TorannMagicDefOf.TM_BloodForBlood.label), MessageTypeDefOf.RejectInput);
                    }
                }
                else
                {
                    Messages.Message("TM_InvalidTarget".Translate(this.CasterPawn.LabelShort, TorannMagicDefOf.TM_BloodForBlood.label), MessageTypeDefOf.RejectInput);
                }
            }

            this.PostCastShot(flag, out flag);
            return flag;
        }
        
    }
}
