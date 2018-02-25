﻿using System;
using Verse;
using RimWorld;
using AbilityUser;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TorannMagic.Enchantment
{
    public class CompProperties_EnchantedItem : CompProperties, IExposable
    {
        public List<TMAbilityDef> MagicAbilities = new List<TMAbilityDef>();

        public Type AbilityUserClass;

        private bool hasEnchantment = false;

        public EnchantmentTier maxMPTier;
        public EnchantmentTier mpRegenRateTier;
        public EnchantmentTier coolDownTier;
        public EnchantmentTier mpCostTier;
        public EnchantmentTier xpGainTier;

        //Magic Stats (%)
        public float maxMP = 0;
        public float mpRegenRate = 0;
        public float coolDown = 0;
        public float mpCost = 0;
        public float xpGain = 0;

        //Might Stats (%)

        //Common Stats (%)

        public float fireRes = 0;
        public float iceRes = 0;
        public float lightningRes = 0;
        public float arcaneRes = 0;

        public float healthRegenRate = 0;

        public void ExposeData()
        {
            Scribe_Values.Look<float>(ref this.maxMP, "maxMP", 0, false);
            Scribe_Values.Look<float>(ref this.mpRegenRate, "mpRegenRateP", 0, false);
            Scribe_Values.Look<float>(ref this.coolDown, "coolDown", 0, false);
            Scribe_Values.Look<float>(ref this.mpCost, "mpCost", 0, false);
            Scribe_Values.Look<float>(ref this.xpGain, "xpGain", 0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.maxMPTier, "maxMPTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.mpRegenRateTier, "mpRegenRateTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.coolDownTier, "coolDownTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.mpCostTier, "mpCostTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<EnchantmentTier>(ref this.xpGainTier, "xpGainTier", (EnchantmentTier)0, false);
            Scribe_Values.Look<bool>(ref this.hasEnchantment, "hasEnchantment", false, false);
        }

        public string MaxMPLabel
        {
            get
            {
                return "TM_MaxMPLabel".Translate(new object[]
                {
                    this.maxMP * 100
                });
            }
        }

        public string MPRegenRateLabel
        {
            get
            {
                return "TM_MPRegenRateLabel".Translate(new object[]
                {
                    this.mpRegenRate * 100
                });
            }
        }

        public string CoolDownLabel
        {
            get
            {
                return "TM_CoolDownLabel".Translate(new object[]
                {
                    this.coolDown * 100
                });
            }
        }

        public string MPCostLabel
        {
            get
            {
                return "TM_MPCostLabel".Translate(new object[]
                {
                    this.mpCost * 100
                });
            }
        }

        public string XPGainLabel
        {
            get
            {
                return "TM_XPGainLabel".Translate(new object[]
                {
                    this.xpGain * 100
                });
            }
        }

        public bool HasMagic
        {
            get
            {
                return MagicAbilities.Count > 0;
            }
        }

        public bool HasEnchantment
        {
            get
            {
                return hasEnchantment;
            }
            set
            {
                hasEnchantment = value;
            }
        }

        public CompProperties_EnchantedItem()
        {
            this.compClass = typeof(CompEnchantedItem);
            this.AbilityUserClass = typeof(GenericCompAbilityUser);
        }
    }
}
