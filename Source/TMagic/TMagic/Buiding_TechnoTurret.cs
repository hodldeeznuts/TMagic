﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using AbilityUser;
using Verse;
using Verse.AI;
using UnityEngine;
using Verse.Sound;


namespace TorannMagic
{   
    [StaticConstructorOnStartup]
    public class Building_TechnoTurret : Building_TurretGun
    {
        int mortarMaxRange = 100;
        int mortarMinRange = 40;
        int mortarTicksToFire = 900;
        float mortarManaCost = .1f;

        int rocketMinRange = 5;
        int rocketTicksToFire = 600;
        int rocketCount = 1;
        int nextRocketFireTick = 0;
        float rocketManaCost = .05f;

        private int verVal = 0;
        private int pwrVal = 0;
        private int effVal = 0;

        private bool MannedByColonist => mannableComp != null && mannableComp.ManningPawn != null && mannableComp.ManningPawn.Faction == Faction.OfPlayer;
        private bool MannedByNonColonist => mannableComp != null && mannableComp.ManningPawn != null && mannableComp.ManningPawn.Faction != Faction.OfPlayer;
        private bool PlayerControlled => (base.Faction == Faction.OfPlayer || MannedByColonist) && !MannedByNonColonist;
        private bool Manned => MannedByColonist || MannedByNonColonist;
        private bool holdFire;
        private bool WarmingUp => burstWarmupTicksLeft > 0;
        private bool CanSetForcedTarget => mannableComp != null && PlayerControlled;
        private bool CanToggleHoldFire => PlayerControlled;
        private bool IsMortar => def.building.IsMortar;
        private bool IsMortarOrProjectileFliesOverhead => AttackVerb.ProjectileFliesOverhead() || IsMortar;
        private bool initialized = false;

        CompAbilityUserMagic comp;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.verVal, "verVal", 0, false);
            Scribe_Values.Look<int>(ref this.pwrVal, "pwrVal", 0, false);
            Scribe_Values.Look<int>(ref this.mortarMaxRange, "mortarMaxRange", 80, false);
            Scribe_Values.Look<int>(ref this.mortarTicksToFire, "mortarTicksToFire", 900, false);
            Scribe_Values.Look<int>(ref this.rocketCount, "rocketCount", 1, false);
            Scribe_Values.Look<int>(ref this.rocketTicksToFire, "rocketTicksToFire", 600, false);
            Scribe_Values.Look<float>(ref this.rocketManaCost, "rocketManaCost", 0.05f, false);
            Scribe_Values.Look<float>(ref this.mortarManaCost, "mortarManaCost", 0.1f, false);
        }

        public override void Tick()
        {
            base.Tick();
            if(!initialized)
            {
                comp = mannableComp.ManningPawn.GetComp<CompAbilityUserMagic>();
                this.verVal = mannableComp.ManningPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoTurret_ver").level;
                this.pwrVal = mannableComp.ManningPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoTurret_pwr").level;
                this.effVal = mannableComp.ManningPawn.GetComp<CompAbilityUserMagic>().MagicData.MagicPowerSkill_TechnoTurret.FirstOrDefault((MagicPowerSkill x) => x.label == "TM_TechnoTurret_eff").level;
                
                if (this.verVal >= 5)
                {
                    this.rocketTicksToFire = 600 - ((verVal - 5) * 20);
                    this.rocketCount = verVal / 5;
                    this.rocketManaCost = .055f - (.001f * this.effVal);                    
                }
                if(this.verVal >= 10)
                {
                    this.mortarTicksToFire = 900 - ((verVal - 10) * 40);
                    this.mortarMaxRange += ((verVal - 10) * 5);
                    this.mortarManaCost = .1f - (.002f * effVal);                    
                }
                
                this.initialized = true;
            }

            if (!mannableComp.ManningPawn.DestroyedOrNull() && !mannableComp.ManningPawn.Dead && !mannableComp.ManningPawn.Downed)
            {
                if (Find.TickManager.TicksGame % 10 == 0 && !Manned)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 rndPos = this.DrawPos;
                        rndPos.x += Rand.Range(-.5f, .5f);
                        rndPos.z += Rand.Range(-.5f, .5f);
                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_SparkFlash"), rndPos, this.Map, Rand.Range(.6f, .8f), .1f, .05f, .05f, 0, 0, 0, Rand.Range(0, 360));
                        MoteMaker.ThrowSmoke(rndPos, this.Map, Rand.Range(.8f, 1.2f));
                        rndPos = this.DrawPos;
                        rndPos.x += Rand.Range(-.5f, .5f);
                        rndPos.z += Rand.Range(-.5f, .5f);
                        TM_MoteMaker.ThrowGenericMote(ThingDef.Named("Mote_ElectricalSpark"), rndPos, this.Map, Rand.Range(.4f, .7f), .2f, .05f, .1f, 0, 0, 0, Rand.Range(0, 360));
                    }
                    GenExplosion.DoExplosion(this.Position, this.Map, 1f, DamageDefOf.EMP, this, 0, 0, SoundDefOf.Crunch, null, null, this, null, 0, 0, false, null, 0, 0, 0, false);
                    this.Destroy(DestroyMode.Vanish);
                }

                if (this.verVal >= 5 && this.nextRocketFireTick < Find.TickManager.TicksGame && this.TargetCurrentlyAimingAt != null && comp.Mana.CurLevel >= this.rocketManaCost)
                {
                    if (this.TargetCurrentlyAimingAt.Cell.IsValid && this.TargetCurrentlyAimingAt.Cell.DistanceToEdge(this.Map) > 5 && (this.TargetCurrentlyAimingAt.Cell - this.Position).LengthHorizontal >= this.rocketMinRange)
                    {
                        bool flag = this.TargetCurrentlyAimingAt.Cell != default(IntVec3);
                        if (flag)
                        {
                            Thing launchedThing = new Thing()
                            {
                                def = ThingDef.Named("FlyingObject_RocketSmall")
                            };
                            FlyingObject_Advanced flyingObject = (FlyingObject_Advanced)GenSpawn.Spawn(ThingDef.Named("FlyingObject_RocketSmall"), this.Position, this.Map);
                            flyingObject.AdvancedLaunch(this, ThingDef.Named("Mote_Smoke"), 1, Rand.Range(5, 45), false, this.DrawPos, this.TargetCurrentlyAimingAt.Cell, launchedThing, Rand.Range(32, 38), true, Rand.Range(16, 22), 2, DamageDefOf.Bomb, null);
                            this.rocketCount--;
                            SoundInfo info = SoundInfo.InMap(new TargetInfo(this.Position, this.Map, false), MaintenanceType.None);
                            info.pitchFactor = 1.3f;
                            info.volumeFactor = 1.5f;
                            TorannMagicDefOf.TM_AirWoosh.PlayOneShot(info);
                        }
                        if (rocketCount <= 0)
                        {
                            this.rocketCount = verVal / 5;
                            this.nextRocketFireTick = Find.TickManager.TicksGame + (600 - ((verVal - 5) * 20));
                            comp.Mana.CurLevel -= this.rocketManaCost;
                            comp.MagicUserXP += Rand.Range(9, 12);
                        }
                        else
                        {
                            this.nextRocketFireTick = Find.TickManager.TicksGame + 20;
                        }
                    }
                }

                if (this.verVal >= 10 && this.mortarTicksToFire < Find.TickManager.TicksGame && this.TargetCurrentlyAimingAt == null && comp.Mana.CurLevel >= this.mortarManaCost)
                {
                    Pawn target = TM_Calc.FindNearbyEnemy(this.Position, this.Map, this.Faction, this.mortarMaxRange, this.mortarMinRange);
                    if (target != null && target.Position.IsValid && target.Position.DistanceToEdge(this.Map) > 5)
                    {
                        bool flag = target.Position != default(IntVec3);
                        if (flag)
                        {
                            IntVec3 rndTarget = target.Position;
                            rndTarget.x += Rand.RangeInclusive(-4, 4);
                            rndTarget.z += Rand.RangeInclusive(-4, 4);
                            Projectile newProjectile = (Projectile)GenSpawn.Spawn(ThingDef.Named("Bullet_Shell_TechnoTurretExplosive"), this.Position, this.Map, WipeMode.Vanish);
                            newProjectile.Launch(this, rndTarget, target, ProjectileHitFlags.All, null);
                        }
                        this.mortarTicksToFire = Find.TickManager.TicksGame + (900 - ((verVal - 10) * 40));
                        SoundInfo info = SoundInfo.InMap(new TargetInfo(this.Position, this.Map, false), MaintenanceType.None);
                        info.pitchFactor = 1.3f;
                        info.volumeFactor = .8f;
                        SoundDef.Named("Mortar_LaunchA").PlayOneShot(info);
                        comp.Mana.CurLevel -= this.mortarManaCost;
                        comp.MagicUserXP += Rand.Range(12, 15);
                    }
                }

                if (CanExtractShell && MannedByColonist)
                {
                    CompChangeableProjectile compChangeableProjectile = gun.TryGetComp<CompChangeableProjectile>();
                    if (!compChangeableProjectile.allowedShellsSettings.AllowedToAccept(compChangeableProjectile.LoadedShell))
                    {
                        ExtractShell();
                    }
                }
                if (forcedTarget.IsValid && !CanSetForcedTarget)
                {
                    ResetForcedTarget();
                }
                if (!CanToggleHoldFire)
                {
                    holdFire = false;
                }
                if (forcedTarget.ThingDestroyed)
                {
                    ResetForcedTarget();
                }
                if ((powerComp == null || powerComp.PowerOn) && (mannableComp == null || mannableComp.MannedNow) && base.Spawned)
                {
                    GunCompEq.verbTracker.VerbsTick();
                    if (!stunner.Stunned && AttackVerb.state != VerbState.Bursting)
                    {
                        if (WarmingUp)
                        {
                            burstWarmupTicksLeft--;
                            if (burstWarmupTicksLeft == 0)
                            {
                                BeginBurst();
                            }
                        }
                        else
                        {
                            if (burstCooldownTicksLeft > 0)
                            {
                                burstCooldownTicksLeft--;
                            }
                            if (burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(10))
                            {
                                TryStartShootSomething(canBeginBurstImmediately: true);
                            }
                        }
                        top.TurretTopTick();
                    }
                }
                else
                {
                    ResetCurrentTarget();
                }
            }
        }

        private void ExtractShell()
        {
            GenPlace.TryPlaceThing(gun.TryGetComp<CompChangeableProjectile>().RemoveShell(), base.Position, base.Map, ThingPlaceMode.Near);
        }

        private void ResetForcedTarget()
        {
            forcedTarget = LocalTargetInfo.Invalid;
            burstWarmupTicksLeft = 0;
            if (burstCooldownTicksLeft <= 0)
            {
                TryStartShootSomething(canBeginBurstImmediately: false);
            }
        }

        private void ResetCurrentTarget()
        {
            currentTargetInt = LocalTargetInfo.Invalid;
            burstWarmupTicksLeft = 0;
        }

        private bool CanExtractShell
        {
            get
            {
                if (!PlayerControlled)
                {
                    return false;
                }
                return gun.TryGetComp<CompChangeableProjectile>()?.Loaded ?? false;
            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            if (this.HitPoints < 1 && mannableComp != null && Manned)
            {
                int rnd = Mathf.RoundToInt(Rand.Range(2, 5) - (.2f * this.effVal));
                for (int i = 0; i < this.effVal; i++)
                {
                    TM_Action.DamageEntities(mannableComp.ManningPawn, null, Rand.Range(1f, 2f), TMDamageDefOf.DamageDefOf.TM_ElectricalBurn, this);
                }
            }
            base.Destroy(mode);            
        }
    }
}
