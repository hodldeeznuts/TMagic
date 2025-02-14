﻿using RimWorld;
using RimWorld.Planet;
using System;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;

namespace TorannMagic
{
    class Verb_ShadowStep : Verb_UseAbility  
    {

        protected override bool TryCastShot()
        {
            bool result = false;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();
            Pawn soulPawn = comp.soulBondPawn;

            if(soulPawn != null && !soulPawn.Dead && !soulPawn.Destroyed)
            {
                Pawn p = this.CasterPawn;
                bool drafted = this.CasterPawn.Drafted;
                Map map = this.CasterPawn.Map;
                Map sMap = soulPawn.Map;
                if (sMap == null)
                {
                    Hediff bondHediff = null;
                    bondHediff = soulPawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_SoulBondPhysicalHD"), false);
                    if (bondHediff != null)
                    {
                        HediffComp_SoulBondHost compS = bondHediff.TryGetComp<HediffComp_SoulBondHost>();
                        if (compS != null && compS.polyHost != null && !compS.polyHost.DestroyedOrNull() && !compS.polyHost.Dead)
                        {
                            soulPawn = compS.polyHost;
                        }
                    }
                    bondHediff = null;

                    bondHediff = soulPawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("TM_SoulBondMentalHD"), false);
                    if (bondHediff != null)
                    {
                        HediffComp_SoulBondHost compS = bondHediff.TryGetComp<HediffComp_SoulBondHost>();
                        if (compS != null && compS.polyHost != null && !compS.polyHost.DestroyedOrNull() && !compS.polyHost.Dead)
                        {
                            soulPawn = compS.polyHost;
                        }
                    }
                    if(soulPawn.ParentHolder != null && soulPawn.ParentHolder is Caravan)
                    {
                        //Log.Message("caravan detected");
                        //p.DeSpawn();
                        Caravan van = soulPawn.ParentHolder as Caravan;
                        van.AddPawn(p, true);
                        Find.WorldPawns.PassToWorld(p);
                        p.Notify_PassedToWorld();
                        Messages.Message("" + p.LabelShort + " has shadow stepped to a caravan with " + soulPawn.LabelShort, MessageTypeDefOf.NeutralEvent);
                        goto fin;
                    }
                }
                IntVec3 casterCell = this.CasterPawn.Position;
                IntVec3 targetCell = soulPawn.Position;
                if (p.Spawned)
                {
                    try
                    {
                        p.DeSpawn();
                        GenSpawn.Spawn(p, targetCell, soulPawn.Map);
                        if (drafted)
                        {
                            p.drafter.Drafted = true;
                        }
                        CameraJumper.TryJumpAndSelect(p);
                    }
                    catch
                    {
                        Log.Message("Exception occured when trying to shadow step to soul bound pawn - recovered caster at original position");
                        GenSpawn.Spawn(p, casterCell, map);

                    }
                }               
                this.Ability.PostAbilityAttempt();
                result = true;
            }
            else
            {
                Log.Warning("No soul bond found to shadow call.");
            }
            
            //this.ability.TicksUntilCasting = (int)base.UseAbilityProps.SecondsToRecharge * 60;
            fin:;
            this.burstShotsLeft = 0;
            return result;
        }
    }
}
