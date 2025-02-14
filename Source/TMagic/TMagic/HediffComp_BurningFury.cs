﻿using RimWorld;
using Verse;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class HediffComp_BurningFury : HediffComp
    {

        private bool initializing = true;
        private int nextAction = 1;
        private int nextSlowAction = 1;
        private bool removeNow = false;

        public string labelCap
        {
            get
            {
                return base.Def.LabelCap;
            }
        }

        public string label
        {
            get
            {
                return base.Def.label;
            }
        }


        private void Initialize()
        {
            bool spawned = base.Pawn.Spawned;
            if (spawned && base.Pawn.Map != null)
            {
                MoteMaker.ThrowLightningGlow(base.Pawn.TrueCenter(), base.Pawn.Map, 3f);
            }
        }

        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            bool flag = base.Pawn != null;
            if (flag)
            {
                if (initializing)
                {
                    initializing = false;
                    this.Initialize();
                }
            }
            if (!this.Pawn.DestroyedOrNull() && this.Pawn.Spawned && !this.Pawn.Downed)
            {
                if (Find.TickManager.TicksGame % 30 == 0)
                {
                    CompAbilityUserMight comp = this.Pawn.GetComp<CompAbilityUserMight>();
                    if (comp != null && comp.Stamina != null)
                    {
                        comp.Stamina.CurLevel -= .02f;
                        if (comp.Stamina.CurLevel <= .001f)
                        {
                            this.removeNow = true;
                        }
                    }
                    else
                    {
                        this.removeNow = true;
                    }
                }
                if (!removeNow && Find.TickManager.TicksGame % nextAction == 0)
                {
                    this.nextAction = Rand.Range(50, 80);
                    TickAction();
                }
                if (!removeNow && Find.TickManager.TicksGame % nextSlowAction == 0)
                {
                    this.nextSlowAction = Rand.Range(200, 500);
                    SlowTickAction();
                }
            }
            else
            {
                this.removeNow = true;
            }
        }

        public void TickAction()
        {
            Pawn victim = TM_Calc.FindNearbyEnemy(this.Pawn, 2);
            if (victim != null)
            {
                TM_Action.DamageEntities(victim, null, Rand.Range(4, 6), DamageDefOf.Burn, this.Pawn);
                TM_MoteMaker.ThrowFlames(victim.DrawPos, victim.Map, Rand.Range(.1f, .4f));
            }

            if(Rand.Chance(.2f))
            {
                TM_Action.DamageEntities(this.Pawn, null, Rand.Range(3, 5), 5f, DamageDefOf.Burn, this.Pawn);
                TM_MoteMaker.ThrowFlames(this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.1f, .2f));
            }

        }

        public void SlowTickAction()
        {
            using (IEnumerator<BodyPartRecord> enumerator = this.Pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BodyPartRecord rec = enumerator.Current;

                    IEnumerable<Hediff_Injury> arg_BB_0 = this.Pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                    Func<Hediff_Injury, bool> arg_BB_1;

                    arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                    foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                    {
                        bool flag5 = current.CanHealNaturally() && !current.IsPermanent() && current.TendableNow();
                        if (flag5)
                        {
                            if (Rand.Chance(.15f))
                            {
                                DamageInfo dinfo;
                                dinfo = new DamageInfo(DamageDefOf.Burn, Mathf.RoundToInt(current.Severity / 2), 0, (float)-1, this.Pawn, rec, null, DamageInfo.SourceCategory.ThingOrUnknown);
                                dinfo.SetAllowDamagePropagation(false);
                                dinfo.SetInstantPermanentInjury(true);
                                current.Heal(100);
                                TM_MoteMaker.ThrowFlames(this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.1f, .2f));
                                this.Pawn.TakeDamage(dinfo);
                            }
                            else
                            {
                                current.Tended(1, 1);
                                TM_MoteMaker.ThrowFlames(this.Pawn.DrawPos, this.Pawn.Map, Rand.Range(.1f, .2f));
                            }
                            goto ExitEnum;
                        }
                    }
                }
            }
            ExitEnum:;
        }

        public override bool CompShouldRemove
        {
            get
            {
                return this.removeNow || !this.Pawn.health.hediffSet.HasHediff(TorannMagicDefOf.TM_BurningFuryHD, false) || base.CompShouldRemove;
            }
        }
    }
}
