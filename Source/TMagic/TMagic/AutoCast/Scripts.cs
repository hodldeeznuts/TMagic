﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;
using Verse;
using Verse.AI;
using RimWorld;
using AbilityUser;

namespace TorannMagic.AutoCast
{

    public static class Phase
    {
        public static void Evaluate(CompAbilityUserMight casterComp, TMAbilityDef abilitydef, PawnAbility ability, MightPower power, float minDistance, out bool success)
        {
            success = false;
            Pawn caster = casterComp.Pawn;
            LocalTargetInfo jobTarget = caster.CurJob.targetA;
            Thing carriedThing = null;
            if (caster.CurJob.targetA.Thing != null) //&& caster.CurJob.def.defName != "Sow")
            {
                if (caster.CurJob.targetA.Thing.Map != caster.Map) //carrying TargetA to TargetB
                {
                    jobTarget = caster.CurJob.targetB;
                    carriedThing = caster.CurJob.targetA.Thing;
                }
                else if (caster.CurJob.targetB != null && caster.CurJob.targetB.Thing != null && caster.CurJob.def != JobDefOf.Rescue) //targetA using targetB for job
                {
                    if (caster.CurJob.targetB.Thing.Map != caster.Map) //carrying targetB to targetA
                    {
                        jobTarget = caster.CurJob.targetA;
                        carriedThing = caster.CurJob.targetB.Thing;
                    }
                    else if(caster.CurJob.def == JobDefOf.TendPatient)
                    {
                        jobTarget = caster.CurJob.targetB;
                    }
                    else //Getting targetA to carry to TargetB
                    {
                        jobTarget = caster.CurJob.targetA;
                    }
                }
                else
                {
                    jobTarget = caster.CurJob.targetA;
                }
            }
            float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
            Vector3 directionToTarget = TM_Calc.GetVector(caster.Position, jobTarget.Cell);
            //Log.Message("" + caster.LabelShort + " job def is " + caster.CurJob.def.defName + " targetA " + caster.CurJob.targetA + " targetB " + caster.CurJob.targetB + " jobTarget " + jobTarget + " at distance " + distanceToTarget + " min distance " + minDistance + " at vector " + directionToTarget);
            if (casterComp.Stamina.CurLevel >= casterComp.ActualStaminaCost(abilitydef) && ability.CooldownTicksLeft <= 0 && distanceToTarget < 200)
            {
                if (distanceToTarget > minDistance && caster.CurJob.locomotionUrgency >= LocomotionUrgency.Jog && caster.CurJob.bill == null)
                {
                    if (distanceToTarget <= abilitydef.MainVerb.range && jobTarget.Cell != default(IntVec3))
                    {
                        //Log.Message("doing blink to thing");
                        DoPhase(caster, jobTarget.Cell, ability, carriedThing);
                        success = true;
                    }
                    else
                    {
                        IntVec3 phaseToCell = caster.Position + (directionToTarget * abilitydef.MainVerb.range).ToIntVec3();
                        //Log.Message("doing partial blink to cell " + blinkToCell);
                        //MoteMaker.ThrowHeatGlow(blinkToCell, caster.Map, 1f);
                        if (phaseToCell.IsValid && phaseToCell.InBounds(caster.Map) && phaseToCell.Walkable(caster.Map) && !phaseToCell.Fogged(caster.Map) && ((phaseToCell - caster.Position).LengthHorizontal < distanceToTarget))
                        {
                            DoPhase(caster, phaseToCell, ability, carriedThing);
                            success = true;
                        }
                    }
                }
            }
        }

        private static void DoPhase(Pawn caster, IntVec3 targetCell, PawnAbility ability, Thing carriedThing)
        {
            JobDef retainJobDef = caster.CurJobDef;
            LocalTargetInfo retainTargetA = caster.CurJob.targetA;
            int retainCount = 1;
            if (retainTargetA.Thing != null && retainTargetA.Thing.stackCount != 1)
            {
                 retainCount = retainTargetA.Thing.stackCount;
            }
            LocalTargetInfo retainTargetB = caster.CurJob.targetB;
            LocalTargetInfo retainTargetC = caster.CurJob.targetC;
            Pawn p = caster;
            Thing cT = carriedThing;
            if (cT != null && cT.stackCount != 1)
            {
                retainCount = cT.stackCount;
            }
            Map map = caster.Map;
            IntVec3 casterCell = caster.Position;
            bool selectCaster = false;
            if (Find.Selector.FirstSelectedObject == caster)
            {
                selectCaster = true;
            }
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, caster.DrawPos, caster.Map, Rand.Range(.6f, 1f), .4f, .1f, Rand.Range(.8f, 1.2f), 0, Rand.Range(2, 3), Rand.Range(-30, 30), 0);
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Enchanting, caster.DrawPos, caster.Map, Rand.Range(1.4f, 2f), .2f, .05f, Rand.Range(.4f, .6f), Rand.Range(-200, 200), 0, 0, 0);
                }

                caster.DeSpawn();
                GenSpawn.Spawn(p, targetCell, map);
                if (carriedThing != null)
                {
                    carriedThing.DeSpawn();
                    GenPlace.TryPlaceThing(cT, targetCell, map, ThingPlaceMode.Near);
                    //GenSpawn.Spawn(cT, targetCell, map);
                }

                caster.GetComp<CompAbilityUserMight>().MightUserXP -= 50;
                ability.PostAbilityAttempt();
                if (selectCaster)
                {
                    Find.Selector.Select(caster, false, true);
                }
                for (int i = 0; i < 3; i++)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, caster.DrawPos, caster.Map, Rand.Range(.6f, 1f), .4f, .1f, Rand.Range(.8f, 1.2f), 0, Rand.Range(2, 3), Rand.Range(-30, 30), 0);
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Enchanting, caster.DrawPos, caster.Map, Rand.Range(1.4f, 2f), .2f, .05f, Rand.Range(.4f, .6f), Rand.Range(-200, 200), 0, 0, 0);
                }

                Job job = new Job(retainJobDef, retainTargetA, retainTargetB, retainTargetC)
                {
                    count = retainCount,
                    playerForced = false                    
                };
                //caster.jobs.TryTakeOrderedJob();
                
                caster.jobs.StartJob(job);
            }
            catch
            {
                if (!caster.Spawned)
                {
                    GenSpawn.Spawn(p, casterCell, map);
                }
            }
        }
    }

    public static class MeleeCombat_OnTarget
    {
        public static void TryExecute(CompAbilityUserMight casterComp, TMAbilityDef abilitydef, PawnAbility ability, MightPower power, LocalTargetInfo target, out bool success)
        {
            success = false;
            if (casterComp.Stamina.CurLevel >= abilitydef.staminaCost && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = target;
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget < (abilitydef.MainVerb.range ) && jobTarget != null && jobTarget.Thing != null)
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }
            }
        }
    }
    
    public static class CombatAbility_OnTarget
    {
        public static void TryExecute(CompAbilityUserMight casterComp, TMAbilityDef abilitydef, PawnAbility ability, MightPower power, LocalTargetInfo target, int minRange, out bool success)
        {
            success = false;
            if (casterComp.Stamina.CurLevel >= abilitydef.staminaCost && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = target;
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget > minRange && distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null && TM_Calc.HasLoSFromTo(caster.Position, jobTarget, caster, 0, abilitydef.MainVerb.range))
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    job.endIfCantShootTargetFromCurPos = true;
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }
            }
        }
    }

    public static class AoECombat
    {
        public static void Evaluate(CompAbilityUserMight mightComp, TMAbilityDef abilitydef, PawnAbility ability, MightPower power, int minTargetCount, int radiusAround, IntVec3 evaluatedCenter, bool hostile, out bool success)
        {
            success = false;
            if (mightComp.Stamina.CurLevel >= abilitydef.staminaCost && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = mightComp.Pawn;
                List<Pawn> targetList = TM_Calc.FindPawnsNearTarget(caster, radiusAround, evaluatedCenter, hostile);
                if (targetList != null)
                {
                    LocalTargetInfo jobTarget = null;
                    if (targetList.Count >= minTargetCount && (abilitydef == TorannMagicDefOf.TM_BladeSpin))
                    {
                        jobTarget = caster;
                    }
                    if (jobTarget != null && jobTarget.Thing != caster)
                    {
                        float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                    }
                    if (jobTarget != null && jobTarget.Thing != null)
                    {
                        Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                        caster.jobs.TryTakeOrderedJob(job);
                        success = true;
                    }
                }
            }
        }
    }

    public static class CombatAbility
    {
        public static void Evaluate(CompAbilityUserMight casterComp, TMAbilityDef abilitydef, PawnAbility ability, MightPower power, out bool success)
        {
            success = false;
            EvaluateMinRange(casterComp, abilitydef, ability, power, 3f, out success);
        }

        public static void EvaluateMinRange(CompAbilityUserMight casterComp, TMAbilityDef abilitydef, PawnAbility ability, MightPower power, float minRange, out bool success)
        {
            success = false;
            if (casterComp.Stamina.CurLevel >= abilitydef.staminaCost && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyEnemy(caster, (int)(abilitydef.MainVerb.range * .9f));
                if(jobTarget != null && jobTarget.Thing != null && abilitydef == TorannMagicDefOf.TM_AntiArmor)
                {
                    Pawn targetPawn = jobTarget.Thing as Pawn;
                    if(targetPawn.RaceProps.IsFlesh)
                    {
                        jobTarget = null;
                    }
                }
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if(jobTarget != null && jobTarget.Thing != null && (distanceToTarget > minRange && distanceToTarget < (abilitydef.MainVerb.range * .9f)) && TM_Calc.HasLoSFromTo(caster.Position, jobTarget, caster, 0, abilitydef.MainVerb.range))
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    job.endIfCantShootTargetFromCurPos = true;
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }
            }
        }
    }

    public static class CureSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, List<string> validAfflictionNames, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyAfflictedPawn(caster, (int)(abilitydef.MainVerb.range * .9f), validAfflictionNames);
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null)
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                    TM_Action.TM_Toils.GotoAndWait(jobTarget.Thing as Pawn, caster, Mathf.RoundToInt(ability.Def.MainVerb.warmupTime * 60));
                }
            }
        }
    }

    public static class CureAddictionSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, List<string> validAddictionNames, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyAddictedPawn(caster, (int)(abilitydef.MainVerb.range * .9f), validAddictionNames);
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null)
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                    TM_Action.TM_Toils.GotoAndWait(jobTarget.Thing as Pawn, caster, Mathf.RoundToInt(ability.Def.MainVerb.warmupTime * 60));
                }
            }
        }
    }

    public static class HealSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, out bool success)
        {
            success = false;
            EvaluateMinSeverity(casterComp, abilitydef, ability, power, 0, out success);
        }

        public static void EvaluateMinSeverity(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, float minSeverity, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyInjuredPawn(caster, (int)(abilitydef.MainVerb.range * .9f), minSeverity);
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;                
                if (distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null)
                {
                    if (abilitydef == TorannMagicDefOf.TM_CauterizeWound && jobTarget.Thing is Pawn)
                    {
                        Pawn targetPawn = jobTarget.Thing as Pawn;
                        if (targetPawn.health.HasHediffsNeedingTend(false))
                        {
                            Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                            job.endIfCantShootTargetFromCurPos = true;
                            caster.jobs.TryTakeOrderedJob(job);
                            success = true;
                        }
                    }
                    else
                    {
                        Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                        caster.jobs.TryTakeOrderedJob(job);
                        success = true;
                    }                    
                }
            }
        }
    }

    public static class HediffHealSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, HediffDef hediffDef, out bool success)
        {
            success = false;
            EvaluateMinSeverity(casterComp, abilitydef, ability, power, hediffDef, 0, out success);
        }

        public static void EvaluateMinSeverity(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, HediffDef hediffDef, float minSeverity, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyInjuredPawn(caster, (int)(abilitydef.MainVerb.range * .9f), minSeverity);
                if (jobTarget != null)
                {
                    float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                    if (distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget.Thing != null && jobTarget.Thing is Pawn)
                    {
                        Pawn targetPawn = jobTarget.Thing as Pawn;
                        if (targetPawn.health != null && targetPawn.health.hediffSet != null && !targetPawn.health.hediffSet.HasHediff(hediffDef, false))
                        {
                            Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                            caster.jobs.TryTakeOrderedJob(job);
                            success = true;
                            TM_Action.TM_Toils.GotoAndWait(jobTarget.Thing as Pawn, caster, Mathf.RoundToInt(ability.Def.MainVerb.warmupTime * 60));
                        }
                    }
                }
            }
        }
    }

    public static class HealPermanentSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, out bool success)
        {
            success = false;
            EvaluateMinSeverity(casterComp, abilitydef, ability, power, 0, out success);
        }

        public static void EvaluateMinSeverity(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, float minSeverity, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyPermanentlyInjuredPawn(caster, (int)(abilitydef.MainVerb.range * .9f), minSeverity);
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null)
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                    TM_Action.TM_Toils.GotoAndWait(jobTarget.Thing as Pawn, caster, Mathf.RoundToInt(ability.Def.MainVerb.warmupTime * 60));
                }
            }
        }
    }

    public static class HediffSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, HediffDef hediffDef, out bool success)
        {
            success = false;
            EvaluateMinRange(casterComp, abilitydef, ability, power, hediffDef, 6f, out success);
        }

        public static void EvaluateMinRange(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, HediffDef hediffDef, float minRange, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyEnemy(caster, (int)(abilitydef.MainVerb.range * .9f));
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget > minRange && distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null && jobTarget.Thing is Pawn && TM_Calc.HasLoSFromTo(caster.Position, jobTarget, caster, 0, abilitydef.MainVerb.range))
                {
                    Pawn targetPawn = jobTarget.Thing as Pawn;
                    if (!targetPawn.health.hediffSet.HasHediff(hediffDef, false))
                    {
                        Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                        caster.jobs.TryTakeOrderedJob(job);
                        success = true;
                    }
                }
            }
        }
    }

    public static class DamageSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, out bool success)
        {
            success = false;
            EvaluateMinRange(casterComp, abilitydef, ability, power, 6f, out success);
        }

        public static void EvaluateMinRange(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, float minRange, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyEnemy(caster, (int)(abilitydef.MainVerb.range * .9f));
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget > minRange && distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null && TM_Calc.HasLoSFromTo(caster.Position, jobTarget, caster, 0, abilitydef.MainVerb.range))
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    job.endIfCantShootTargetFromCurPos = true;
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }
            }
        }
    }

    public static class TransferManaSpell
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, bool inCombat, bool reverse, out bool success) //reverse == true transfers mana to caster
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyMage(caster, (int)(abilitydef.MainVerb.range * .9f), inCombat);
                if (!inCombat && jobTarget != null && jobTarget.Thing != null)
                {
                    Pawn transferPawn = jobTarget.Thing as Pawn;
                    CompAbilityUserMagic tComp = transferPawn.GetComp<CompAbilityUserMagic>();
                    if (reverse)
                    {                        
                        if(casterComp.Mana.CurLevel >= .3f || tComp.Mana.CurLevel <= .9f)
                        {
                            jobTarget = null;
                        }
                    }
                    else
                    {
                        if(casterComp.Mana.CurLevel <= .9f || tComp.Mana.CurLevel >= .9f)
                        {
                            jobTarget = null;
                        }
                    }
                }
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null)
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }
            }
        }
    }

    public static class Summon
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, float minDistance, out bool success)
        {
            success = false;
            Pawn caster = casterComp.Pawn;
            LocalTargetInfo jobTarget = caster.CurJob.targetA;
            float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
            Vector3 directionToTarget = TM_Calc.GetVector(caster.Position, jobTarget.Cell);

            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0 && jobTarget.Thing != null && jobTarget.Thing.def.EverHaulable)
            {
                //Log.Message("summon: " + caster.LabelShort + " job def is " + caster.CurJob.def.defName + " targetA " + caster.CurJob.targetA + " targetB " + caster.CurJob.targetB + " jobTarget " + jobTarget + " at distance " + distanceToTarget + " min distance " + minDistance + " at vector " + directionToTarget);
                if (distanceToTarget > minDistance && distanceToTarget < abilitydef.MainVerb.range && caster.CurJob.locomotionUrgency >= LocomotionUrgency.Jog && caster.CurJob.bill == null && distanceToTarget < 200)
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }
            }
        }
    }

    public static class Shield
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, out bool success)
        {
            success = false;
            Pawn caster = casterComp.Pawn;
            LocalTargetInfo jobTarget = caster;

            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0 && jobTarget.Thing != null)
            {
                float injurySeverity = 0;
                using (IEnumerator<BodyPartRecord> enumerator = caster.health.hediffSet.GetInjuredParts().GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        BodyPartRecord rec = enumerator.Current;
                        IEnumerable<Hediff_Injury> arg_BB_0 = caster.health.hediffSet.GetHediffs<Hediff_Injury>();
                        Func<Hediff_Injury, bool> arg_BB_1;
                        arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                        foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                        {
                            bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                            if (flag5)
                            {
                                injurySeverity += current.Severity;                                
                            }
                        }
                    }
                }
                if (injurySeverity != 0 && !(caster.health.hediffSet.HasHediff(HediffDef.Named("TM_HediffShield"))))
                {
                    Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                    caster.jobs.TryTakeOrderedJob(job);
                    success = true;
                }            
            }
        }
    }

    public static class CastOnSelf
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, out bool success)
        {
            success = false;
            Pawn caster = casterComp.Pawn;
            LocalTargetInfo jobTarget = caster.Position;

            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0 && jobTarget != null)
            {                
                Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                caster.jobs.TryTakeOrderedJob(job);
                success = true;                
            }
        }
    }

    public static class SpellMending
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, HediffDef hediffDef, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyPawn(caster, (int)(abilitydef.MainVerb.range * .9f));
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;                
                if (distanceToTarget < (abilitydef.MainVerb.range * .9f) && jobTarget != null && jobTarget.Thing != null && jobTarget.Thing is Pawn)
                {
                    Pawn targetPawn = jobTarget.Thing as Pawn;
                    if (targetPawn.RaceProps.Humanlike && targetPawn.IsColonist)
                    {
                        bool tatteredApparel = false;
                        //List<Thought_Memory> targetPawnThoughts = null;
                        //targetPawn.needs.mood.thoughts.GetDistinctMoodThoughtGroups(targetPawnThoughts);
                        //Log.Message("target pawn is " + targetPawn.LabelShort);
                        //List<Thought_Memory> targetPawnThoughts = targetPawn.needs.mood.thoughts.memories.Memories;
                        //for (int i = 0; i < targetPawnThoughts.Count; i++)
                        //{
                        
                        //    if (targetPawnThoughts[i].def == ThoughtDefOf.ApparelDamaged)
                        //    {
                        //        tatteredApparel = true;
                        //    }
                        //}
                        List<Apparel> apparel = targetPawn.apparel.WornApparel;
                        for (int i = 0; i < apparel.Count; i++)
                        {
                            //Log.Message("evaluating equipment " + apparel[i].def.defName + " with hitpoint % of " + (float)(apparel[i].HitPoints/ apparel[i].MaxHitPoints) + " or " + (float)(apparel[i].HitPoints) / (float)(apparel[i].MaxHitPoints));
                            if (((float)(apparel[i].HitPoints) / (float)(apparel[i].MaxHitPoints)) < .5f)
                            {
                                tatteredApparel = true;
                            }
                        }
                        if (targetPawn.equipment.Primary != null)
                        {
                           if((float)(targetPawn.equipment.Primary.HitPoints) / (float)(targetPawn.equipment.Primary.MaxHitPoints) < .5f)
                            {
                                tatteredApparel = true;
                            }
                        }
                        if (!targetPawn.health.hediffSet.HasHediff(hediffDef, false) && tatteredApparel)
                        {
                            Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                            caster.jobs.TryTakeOrderedJob(job);
                            success = true;
                        }
                    }
                }
            }
        }
    }

    public static class Teach
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, out bool success)
        {
            success = false;
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyMage(caster, (int)(abilitydef.MainVerb.range * 1.5f), false);
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget < (abilitydef.MainVerb.range * 1.5f) && jobTarget != null && jobTarget.Thing != null && jobTarget.Thing is Pawn)
                {
                    Pawn targetPawn = jobTarget.Thing as Pawn;
                    CompAbilityUserMagic targetPawnComp = targetPawn.GetComp<CompAbilityUserMagic>();
                    if (targetPawn.CurJobDef.joyKind != null || targetPawn.CurJobDef == JobDefOf.Wait_Wander || targetPawn.CurJobDef == JobDefOf.GotoWander)
                    {
                        if (targetPawn.IsColonist && targetPawnComp.MagicUserLevel < casterComp.MagicUserLevel && caster.relations.OpinionOf(targetPawn) >= 0)
                        {
                            Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                            caster.jobs.TryTakeOrderedJob(job);
                            success = true;
                        }
                    }
                }
            }
        }
    }

    public static class TeachMight
    {
        public static void Evaluate(CompAbilityUserMight casterComp, TMAbilityDef abilitydef, PawnAbility ability, MightPower power, out bool success)
        {
            success = false;
            if (casterComp.Stamina.CurLevel >= abilitydef.staminaCost && ability.CooldownTicksLeft <= 0)
            {
                Pawn caster = casterComp.Pawn;
                LocalTargetInfo jobTarget = TM_Calc.FindNearbyFighter(caster, (int)(abilitydef.MainVerb.range * 1.5f), false);
                float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
                if (distanceToTarget < (abilitydef.MainVerb.range * 1.5f) && jobTarget != null && jobTarget.Thing != null && jobTarget.Thing is Pawn)
                {
                    Pawn targetPawn = jobTarget.Thing as Pawn;
                    CompAbilityUserMight targetPawnComp = targetPawn.GetComp<CompAbilityUserMight>();
                    if (targetPawn.CurJobDef.joyKind != null || targetPawn.CurJobDef == JobDefOf.Wait_Wander || targetPawn.CurJobDef == JobDefOf.GotoWander)
                    {
                        if (targetPawn.IsColonist && targetPawnComp.MightUserLevel < casterComp.MightUserLevel && caster.relations.OpinionOf(targetPawn) >= 0)
                        {
                            Job job = ability.GetJob(AbilityContext.AI, jobTarget);
                            caster.jobs.TryTakeOrderedJob(job);
                            success = true;
                        }
                    }
                }
            }
        }
    }

    public static class Blink
    {
        public static void Evaluate(CompAbilityUserMagic casterComp, TMAbilityDef abilitydef, PawnAbility ability, MagicPower power, float minDistance, out bool success)
        {
            success = false;
            Pawn caster = casterComp.Pawn;
            LocalTargetInfo jobTarget = caster.CurJob.targetA;
            Thing carriedThing = null;
            if (caster.CurJob.targetA.Thing != null ) //&& caster.CurJob.def.defName != "Sow")
            {
                if(caster.CurJob.targetA.Thing.Map != caster.Map) //carrying thing
                {
                    jobTarget = caster.CurJob.targetB;
                    carriedThing = caster.CurJob.targetA.Thing;
                }
                else if(caster.CurJob.targetB != null && caster.CurJob.targetB.Thing != null && caster.CurJob.def != JobDefOf.Rescue) //targetA using targetB for job
                {
                    if(caster.CurJob.targetB.Thing.Map != caster.Map) //carrying targetB to targetA
                    {
                        jobTarget = caster.CurJob.targetA;
                        carriedThing = caster.CurJob.targetB.Thing;
                    }
                    else if(caster.CurJob.def == JobDefOf.TendPatient)
                    {
                        jobTarget = caster.CurJob.targetB;
                    }
                    else //Getting targetA to carry to TargetB
                    {
                        jobTarget = caster.CurJob.targetA;
                    }
                }
                else
                {
                    jobTarget = caster.CurJob.targetA;
                }
            }
            float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
            Vector3 directionToTarget = TM_Calc.GetVector(caster.Position, jobTarget.Cell);
            //Log.Message("" + caster.LabelShort + " job def is " + caster.CurJob.def.defName + " targetA " + caster.CurJob.targetA + " targetB " + caster.CurJob.targetB + " jobTarget " + jobTarget + " at distance " + distanceToTarget + " min distance " + minDistance + " at vector " + directionToTarget);
            if (casterComp.Mana.CurLevel >= casterComp.ActualManaCost(abilitydef) && ability.CooldownTicksLeft <= 0 && distanceToTarget < 200)
            {
                if (distanceToTarget > minDistance && caster.CurJob.locomotionUrgency >= LocomotionUrgency.Jog && caster.CurJob.bill == null)
                {
                    if (distanceToTarget <= abilitydef.MainVerb.range && jobTarget.Cell != default(IntVec3))
                    {
                        //Log.Message("doing blink to thing");
                        DoBlink(caster, jobTarget.Cell, ability, carriedThing);
                        success = true;
                    }
                    else
                    {
                        IntVec3 blinkToCell = caster.Position + (directionToTarget * abilitydef.MainVerb.range).ToIntVec3();
                        //Log.Message("doing partial blink to cell " + blinkToCell);
                        //MoteMaker.ThrowHeatGlow(blinkToCell, caster.Map, 1f);
                        if (blinkToCell.IsValid && blinkToCell.InBounds(caster.Map) && blinkToCell.Walkable(caster.Map) && !blinkToCell.Fogged(caster.Map) && ((blinkToCell - caster.Position).LengthHorizontal < distanceToTarget))
                        {
                            DoBlink(caster, blinkToCell, ability, carriedThing);
                            success = true;
                        }
                    }
                }
            }
        }

        private static void DoBlink(Pawn caster, IntVec3 targetCell, PawnAbility ability, Thing carriedThing)
        {
            JobDef retainJobDef = caster.CurJobDef;
            int retainCount = 1;
            LocalTargetInfo retainTargetA = caster.CurJob.targetA;
            if(retainTargetA.Thing != null && retainTargetA.Thing.stackCount != 1)
            {
                retainCount = retainTargetA.Thing.stackCount;
            }
            LocalTargetInfo retainTargetB = caster.CurJob.targetB;
            LocalTargetInfo retainTargetC = caster.CurJob.targetC;
            Pawn p = caster;
            Thing cT = carriedThing;
            if (cT != null && cT.stackCount != 1)
            {
                retainCount = cT.stackCount;
            }
            Map map = caster.Map;
            IntVec3 casterCell = caster.Position;
            bool selectCaster = false;
            if (Find.Selector.FirstSelectedObject == caster)
            {
                selectCaster = true;
            }
            try
            {
                for (int i = 0; i < 3; i++)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, caster.DrawPos, caster.Map, Rand.Range(.6f, 1f), .4f, .1f, Rand.Range(.8f, 1.2f), 0, Rand.Range(2, 3), Rand.Range(-30, 30), 0);
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Casting, caster.DrawPos, caster.Map, Rand.Range(1.4f, 2f), .2f, .05f, Rand.Range(.4f, .6f), Rand.Range(-200, 200), 0, 0, 0);
                }
                
                caster.DeSpawn();                
                GenSpawn.Spawn(p, targetCell, map);
                if(carriedThing != null)
                {
                    carriedThing.DeSpawn();
                    GenPlace.TryPlaceThing(cT, targetCell, map, ThingPlaceMode.Near);
                    //GenSpawn.Spawn(cT, targetCell, map);
                }
                if (caster.kindDef != PawnKindDef.Named("TM_Dire_Wolf"))
                {
                    caster.GetComp<CompAbilityUserMagic>().MagicUserXP -= 30;
                    ability.PostAbilityAttempt();
                }
                if(selectCaster)
                {
                    Find.Selector.Select(caster, false, true);
                }
                for (int i = 0; i < 3; i++)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, caster.DrawPos, caster.Map, Rand.Range(.6f, 1f), .4f, .1f, Rand.Range(.8f, 1.2f), 0, Rand.Range(2, 3), Rand.Range(-30, 30), 0);
                    TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Casting, caster.DrawPos, caster.Map, Rand.Range(1.4f, 2f), .2f, .05f, Rand.Range(.4f, .6f), Rand.Range(-200, 200), 0, 0, 0);
                }

                Job job = new Job(retainJobDef, retainTargetA, retainTargetB, retainTargetC)
                {
                    count = retainCount
                };
                //caster.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                caster.jobs.StartJob(job);
            }
            catch
            {
                if (!caster.Spawned)
                {
                    GenSpawn.Spawn(p, casterCell, map);
                }
            }
        }
    }

    public static class AnimalBlink
    {
        public static void Evaluate(Pawn casterComp, float minDistance, float maxDistance, out bool success)
        {
            success = false;
            Pawn caster = casterComp;
            LocalTargetInfo jobTarget = caster.CurJob.targetA;
            Thing carriedThing = null;
            if (caster.CurJob.targetA.Thing != null) //&& caster.CurJob.def.defName != "Sow")
            {
                if (caster.CurJob.targetA.Thing.Map != caster.Map) //carrying thing
                {
                    jobTarget = caster.CurJob.targetB;
                    carriedThing = caster.CurJob.targetA.Thing;
                }
                else if (caster.CurJob.targetB != null && caster.CurJob.targetB.Thing != null && caster.CurJob.def.defName != "Rescue") //targetA using targetB for job
                {
                    if (caster.CurJob.targetB.Thing.Map != caster.Map) //carrying targetB to targetA
                    {
                        jobTarget = caster.CurJob.targetA;
                        carriedThing = caster.CurJob.targetB.Thing;
                    }
                    else //Getting targetA to carry to TargetB
                    {
                        jobTarget = caster.CurJob.targetA;
                    }
                }
                else
                {
                    jobTarget = caster.CurJob.targetA;
                }
            }
            float distanceToTarget = (jobTarget.Cell - caster.Position).LengthHorizontal;
            Vector3 directionToTarget = TM_Calc.GetVector(caster.Position, jobTarget.Cell);
            //Log.Message("" + caster.LabelShort + " job def is " + caster.CurJob.def.defName + " targetA " + caster.CurJob.targetA + " targetB " + caster.CurJob.targetB + " jobTarget " + jobTarget + " at distance " + distanceToTarget + " min distance " + minDistance + " at vector " + directionToTarget);
            if (distanceToTarget < 200)
            {
                if (distanceToTarget > minDistance && caster.CurJob.locomotionUrgency >= LocomotionUrgency.Jog && caster.CurJob.bill == null)
                {
                    if (distanceToTarget <= maxDistance && jobTarget.Cell != default(IntVec3))
                    {
                        //Log.Message("doing blink to thing");
                        DoBlink(caster, jobTarget.Cell, carriedThing);
                        success = true;
                    }
                    else
                    {
                        IntVec3 blinkToCell = caster.Position + (directionToTarget * maxDistance).ToIntVec3();
                        //Log.Message("doing partial blink to cell " + blinkToCell);
                        //MoteMaker.ThrowHeatGlow(blinkToCell, caster.Map, 1f);
                        if (blinkToCell.IsValid && blinkToCell.InBounds(caster.Map) && blinkToCell.Walkable(caster.Map) && !blinkToCell.Fogged(caster.Map) && ((blinkToCell - caster.Position).LengthHorizontal < distanceToTarget))
                        {
                            DoBlink(caster, blinkToCell, carriedThing);
                            success = true;
                        }
                    }
                }
            }
        }

        private static void DoBlink(Pawn caster, IntVec3 targetCell, Thing carriedThing)
        {
            JobDef retainJobDef = caster.CurJobDef;
            int retainCount = 1;
            LocalTargetInfo retainTargetA = caster.CurJob.targetA;
            if (retainTargetA.Thing != null && retainTargetA.Thing.stackCount != 1)
            {
                retainCount = retainTargetA.Thing.stackCount;
            }
            LocalTargetInfo retainTargetB = caster.CurJob.targetB;
            LocalTargetInfo retainTargetC = caster.CurJob.targetC;
            Pawn p = caster;
            Thing cT = carriedThing;
            if (cT != null && cT.stackCount != 1)
            {
                retainCount = cT.stackCount;
            }
            Map map = caster.Map;
            IntVec3 casterCell = caster.Position;
            bool selectCaster = false;
            if (Find.Selector.FirstSelectedObject == caster)
            {
                selectCaster = true;
            }
            try
            {
                ThingDef moteThrown = null;
                Vector3 moteVector = TM_Calc.GetVector(casterCell, targetCell);
                float angle = moteVector.ToAngleFlat();
                if (angle >= -135 && angle < -45) //north
                {
                    moteThrown = ThingDef.Named("Mote_DWPhase_North");
                }
                else if (angle >= 45 && angle < 135) //south
                {
                    moteThrown = ThingDef.Named("Mote_DWPhase_South");
                }
                else if (angle >= -45 && angle < 45) //east
                {
                    moteThrown = ThingDef.Named("Mote_DWPhase_East");
                }
                else //west
                {
                    moteThrown = ThingDef.Named("Mote_DWPhase_West");
                }
                for (int i = 0; i < 3; i++)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, caster.DrawPos, caster.Map, Rand.Range(.6f, 1f), .2f, .1f, .5f, 0, Rand.Range(2, 3), Rand.Range(-30, 30), 0);                    
                }
                TM_MoteMaker.ThrowGenericMote(moteThrown, caster.DrawPos, caster.Map, 1.4f, .1f, 0f, .4f, 0, 5f, (Quaternion.AngleAxis(90, Vector3.up) * moteVector).ToAngleFlat(), 0);
                bool drafted = caster.drafter.Drafted;
                caster.DeSpawn();
                GenSpawn.Spawn(p, targetCell, map);
                if (carriedThing != null)
                {
                    carriedThing.DeSpawn();
                    GenPlace.TryPlaceThing(cT, targetCell, map, ThingPlaceMode.Near);
                    //GenSpawn.Spawn(cT, targetCell, map);
                }
                if (selectCaster)
                {
                    Find.Selector.Select(caster, false, true);
                }
                for (int i = 0; i < 3; i++)
                {
                    TM_MoteMaker.ThrowGenericMote(ThingDefOf.Mote_Smoke, caster.DrawPos, caster.Map, Rand.Range(.6f, 1f), .4f, .1f, Rand.Range(.8f, 1.2f), 0, Rand.Range(2, 3), Rand.Range(-30, 30), 0);
                    //TM_MoteMaker.ThrowGenericMote(TorannMagicDefOf.Mote_Casting, caster.DrawPos, caster.Map, Rand.Range(1.4f, 2f), .2f, .05f, Rand.Range(.4f, .6f), Rand.Range(-200, 200), 0, 0, 0);
                }
                Vector3 drawPos = caster.DrawPos + (-2 * moteVector);
                TM_MoteMaker.ThrowGenericMote(moteThrown, drawPos, caster.Map, 1.4f, .1f, .3f, 0f, 0, 8f, (Quaternion.AngleAxis(90, Vector3.up) * moteVector).ToAngleFlat(), 0);
                if (caster.drafter == null)
                {
                    caster.drafter = new Pawn_DraftController(caster);
                }                

                if (drafted)
                {
                    caster.drafter.Drafted = true;
                }

                Job job = new Job(retainJobDef, retainTargetA, retainTargetB, retainTargetC)
                {
                    count = retainCount
                };
                //caster.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                caster.jobs.StartJob(job);
            }
            catch
            {
                if (!caster.Spawned)
                {
                    GenSpawn.Spawn(p, casterCell, map);
                }
            }
        }
    }
}
