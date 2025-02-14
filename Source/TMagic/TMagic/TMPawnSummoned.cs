﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace TorannMagic
{
    public class TMPawnSummoned : Pawn
    {
        private Effecter effecter;
        private bool initialized;
        private bool temporary;
        private int ticksLeft;
        private int ticksToDestroy = 1800;
        public bool validSummoning = false;

        CompAbilityUserMagic compSummoner;
        Pawn spawner;
        Pawn original = null;

        List<float> bodypartDamage = new List<float>();
        List<DamageDef> bodypartDamageType = new List<DamageDef>();

        List<Hediff_Injury> injuries = new List<Hediff_Injury>();

        public Pawn Original
        {
            get => this.original;
            set => original = value;
        }

        public Pawn Spawner
        {
            get => this.spawner;
            set => spawner = value;
        }

        public CompAbilityUserMagic CompSummoner
        {
            get
            {
                return spawner.GetComp<CompAbilityUserMagic>();
            }
        }

        public bool Temporary
        {
            get
            {
                return this.temporary;
            }
            set
            {
                this.temporary = value;
            }
        }

        public int TicksToDestroy
        {
            get
            {
                return this.ticksToDestroy;
            }
            set
            {
                ticksToDestroy = value;
            }
        }

        public int TicksLeft
        {
            get
            {
                return this.ticksLeft;
            }
            set
            {
                this.ticksLeft = value;
            }
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            this.ticksLeft = this.ticksToDestroy;
            base.SpawnSetup(map, respawningAfterLoad);
        }

        public virtual void PostSummonSetup()
        {
            if(!this.validSummoning)
            {
                this.Destroy(DestroyMode.Vanish);
            }
        }

        public void CheckPawnState()
        {
            if (this.def.race.body.ToString() == "Minion")
            {
                try
                {
                    if (this.Downed && !this.Destroyed && this != null && this.Faction == Faction.OfPlayer)
                    {
                        Messages.Message("MinionFled".Translate(), MessageTypeDefOf.NeutralEvent);
                        MoteMaker.ThrowSmoke(this.Position.ToVector3(), base.Map, 1);
                        MoteMaker.ThrowHeatGlow(this.Position, base.Map, 1);
                        if (CompSummoner != null)
                        {
                            CompSummoner.summonedMinions.Remove(this);
                        }
                        this.Destroy(DestroyMode.Vanish);
                    }
                }
                catch
                {
                    Log.Message("TM_ExceptionTick".Translate(
                        this.def.defName
                    ));
                    this.Destroy(DestroyMode.Vanish);
                }
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (Find.TickManager.TicksGame % 10 == 0)
            {
                if (!this.initialized)
                {
                    this.initialized = true;
                    this.PostSummonSetup();
                }
                bool flag2 = this.temporary;
                if (flag2)
                {
                    this.ticksLeft -= 10;
                    bool flag3 = this.ticksLeft <= 0;
                    if (flag3)
                    {
                        this.PreDestroy();
                        this.Destroy(DestroyMode.Vanish);
                    }
                    CheckPawnState();
                    bool spawned = base.Spawned;
                    if (spawned)
                    {
                        bool flag4 = this.effecter == null;
                        if (flag4)
                        {
                            EffecterDef progressBar = EffecterDefOf.ProgressBar;
                            this.effecter = progressBar.Spawn();
                        }
                        else
                        {
                            LocalTargetInfo localTargetInfo = this;
                            bool spawned2 = base.Spawned;
                            if (spawned2)
                            {
                                this.effecter.EffectTick(this, TargetInfo.Invalid);
                            }
                            MoteProgressBar mote = ((SubEffecter_ProgressBar)this.effecter.children[0]).mote;
                            bool flag5 = mote != null;
                            if (flag5)
                            {
                                float value = 1f - (float)(this.TicksToDestroy - this.ticksLeft) / (float)this.TicksToDestroy;
                                mote.progress = Mathf.Clamp01(value);
                                mote.offsetZ = -0.5f;
                            }
                        }
                    }
                }
            }
        }

        public void PreDestroy()
        {
            if (this.def.defName == "TM_MinionR" || this.def.defName == "TM_GreaterMinionR")
            {
                try
                {
                    if (base.Map != null)
                    {
                        MoteMaker.ThrowSmoke(this.Position.ToVector3(), base.Map, 3); 
                    }
                    else
                    {
                        this.holdingOwner.Remove(this);
                    }
                    if (CompSummoner != null)
                    {
                        CompSummoner.summonedMinions.Remove(this);
                    }                    
                }
                catch
                {
                    Log.Message("TM_ExceptionClose".Translate(
                            this.def.defName
                    ));
                }
            }
            if(this.original != null)
            {
                Log.Message("pre destroy");
                CopyDamage(this);
                SpawnOriginal();
                ApplyDamage(original);
            }
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {            
            bool flag = this.effecter != null;
            if (flag)
            {
                this.effecter.Cleanup();
            }
            base.DeSpawn(mode);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref this.temporary, "temporary", false, false);
            Scribe_Values.Look<bool>(ref this.validSummoning, "validSummoning", true, false);
            Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
            Scribe_Values.Look<int>(ref this.ticksToDestroy, "ticksToDestroy", 1800, false);
            Scribe_Values.Look<CompAbilityUserMagic>(ref this.compSummoner, "compSummoner", null, false);
            Scribe_References.Look<Pawn>(ref this.spawner, "spawner", false);
            Scribe_References.Look<Pawn>(ref this.original, "original", false);
        }

        public TMPawnSummoned()
        {

        }

        public void CopyDamage(Pawn pawn)
        {
            using (IEnumerator<BodyPartRecord> enumerator = pawn.health.hediffSet.GetInjuredParts().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BodyPartRecord rec = enumerator.Current;
                    IEnumerable<Hediff_Injury> arg_BB_0 = pawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                    Func<Hediff_Injury, bool> arg_BB_1;
                    arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                    foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                    {
                        bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                        if (flag5)
                        {
                            this.injuries.Add(current);
                            //this.bodypartDamage.Add(current.Severity);
                            //this.bodypartDamageType.Add(current.)
                        }                            
                    }                    
                }
            }
        }

        public void SpawnOriginal()
        {
            GenSpawn.Spawn(this.original, this.Position, this.Map, WipeMode.Vanish);
        }

        public void ApplyDamage(Pawn pawn)
        {
            List<BodyPartRecord> bodyparts = pawn.health.hediffSet.GetNotMissingParts().ToList();
            for(int i =0; i < this.injuries.Count; i++)
            {
                pawn.health.AddHediff(this.injuries[i], bodyparts.RandomElement());
            }
        }
    }
}
