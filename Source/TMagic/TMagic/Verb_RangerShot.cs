﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using AbilityUser;


namespace TorannMagic
{
    public class Verb_RangerShot : Verb_UseAbility
    {
        protected override bool TryCastShot()
        {
            if ( this.CasterPawn.equipment.Primary !=null && this.CasterPawn.equipment.Primary.def.IsRangedWeapon)
            {
                Thing wpn = this.CasterPawn.equipment.Primary;

                if (wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName == "Arrow" || wpn.def.defName.Contains("Bow") || wpn.def.defName.Contains("bow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("Arrow") || wpn.def.Verbs.FirstOrDefault<VerbProperties>().defaultProjectile.projectile.damageDef.defName.Contains("arrow"))
                {
                    base.TryCastShot();
                    return true;
                }
                else
                {                   
                    if (this.CasterPawn.IsColonist)
                    {
                        Messages.Message("MustHaveBow".Translate(
                        this.CasterPawn.LabelShort,
                        wpn.LabelShort
                        ), MessageTypeDefOf.NegativeEvent);
                    }
                    return false;
                }                
            }
            else
            {
                Messages.Message("MustHaveRangedWeapon".Translate(
                    this.CasterPawn.LabelCap
                ), MessageTypeDefOf.RejectInput);
                return false;
            }
        }
    }
}
