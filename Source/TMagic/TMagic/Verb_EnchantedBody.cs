﻿using RimWorld;
using System;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace TorannMagic
{
    public class Verb_EnchantedBody : Verb_UseAbility  
    {
        int pwrVal = 0;
        protected override bool TryCastShot()
        {
            bool result = false;
            bool arg_40_0;

            Pawn pawn = this.CasterPawn;
            Map map = this.CasterPawn.Map;
            CompAbilityUserMagic comp = pawn.GetComp<CompAbilityUserMagic>();
            pwrVal = pawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_EnchantedBody.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_EnchantedBody_pwr").level;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            if (settingsRef.AIHardMode && !pawn.IsColonist)
            {
                pwrVal = 3;
            }
            if (pawn != null && !pawn.Downed)
            {
                if(pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_EnchantedAuraHD))
                {
                    comp.MagicData.MagicPowersStandalone.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedAura).AutoCast = false;
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_EnchantedAuraHD, false));
                    MoteMaker.ThrowHeatGlow(pawn.Position, pawn.Map, 1f);
                }

                if(pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_EnchantedBodyHD))
                {
                    comp.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedBody).AutoCast = false;
                    pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(TorannMagicDefOf.TM_EnchantedBodyHD, false));
                    MoteMaker.ThrowHeatGlow(pawn.Position, pawn.Map, 1f);
                }
                else
                {
                    comp.MagicData.MagicPowersE.FirstOrDefault<MagicPower>((MagicPower x) => x.abilityDef == TorannMagicDefOf.TM_EnchantedBody).AutoCast = true;
                    HealthUtility.AdjustSeverity(pawn, TorannMagicDefOf.TM_EnchantedBodyHD, .5f + pwrVal);
                    for (int i = 0; i < 3; i++)
                    {
                        MoteMaker.ThrowSmoke(pawn.DrawPos, pawn.Map, Rand.Range(.6f, .8f));
                    }
                    MoteMaker.ThrowLightningGlow(pawn.DrawPos, pawn.Map, 1f);
                }
                arg_40_0 = true;
            }
            else
            {
                arg_40_0 = false;
            }
            bool flag = arg_40_0;
            if (flag)
            {
                
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
