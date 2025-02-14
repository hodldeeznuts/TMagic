﻿using RimWorld;
using Verse;
using AbilityUser;
using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace TorannMagic
{
    [StaticConstructorOnStartup]
    class Verb_BriarPatch : Verb_UseAbility  
    {
        bool validTarg;

        public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
        {
            if (targ.IsValid && targ.CenterVector3.InBounds(base.CasterPawn.Map) && !targ.Cell.Fogged(base.CasterPawn.Map) && targ.Cell.Walkable(base.CasterPawn.Map))
            {
                if ((root - targ.Cell).LengthHorizontal < this.verbProps.range)
                {
                    //out of range
                    validTarg = true;
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
            Pawn p = this.CasterPawn;
            Pawn hitPawn = this.currentTarget.Thing as Pawn;
            CompAbilityUserMagic comp = this.CasterPawn.GetComp<CompAbilityUserMagic>();

            if (this.currentTarget != null && p != null)
            {                
                Map map = this.CasterPawn.Map;
                List<IntVec3> cells = GenRadial.RadialCellsAround(this.currentTarget.Cell, this.verbProps.defaultProjectile.projectile.explosionRadius, true).ToList();
                for(int i = 0; i < cells.Count; i++)
                {
                    Thing briar = ThingMaker.MakeThing(TorannMagicDefOf.TM_Plant_Briar, null);
                    GenPlace.TryPlaceThing(briar, cells[i], this.CasterPawn.Map, ThingPlaceMode.Near);
                }
                result = true;
            }

            this.burstShotsLeft = 0;
            return result;
        }        
    }
}
