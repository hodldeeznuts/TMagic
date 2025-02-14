﻿using RimWorld;
using Verse;

namespace TorannMagic.SihvRMagicScrollScribe
{
    public class CompUseEffect_WriteFullScript : CompUseEffect
    {

        public override void DoEffect(Pawn user)
        {
            ThingDef tempPod = null;
            IntVec3 currentPos = parent.PositionHeld;
            Map map = parent.Map;
            if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.InnerFire))
            {
                tempPod = ThingDef.Named("BookOfInnerFire");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost))
            {
                tempPod = ThingDef.Named("BookOfHeartOfFrost");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.StormBorn))
            {
                tempPod = ThingDef.Named("BookOfStormBorn");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Arcanist))
            {
                tempPod = ThingDef.Named("BookOfArcanist");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Paladin))
            {
                tempPod = ThingDef.Named("BookOfValiant");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Summoner))
            {
                tempPod = ThingDef.Named("BookOfSummoner");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Druid))
            {
                tempPod = ThingDef.Named("BookOfDruid");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || user.story.traits.HasTrait(TorannMagicDefOf.Lich)))
            {
                tempPod = ThingDef.Named("BookOfNecromancer");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Priest))
            {
                tempPod = ThingDef.Named("BookOfPriest");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.TM_Bard))
            {
                tempPod = ThingDef.Named("BookOfBard");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Succubus) || user.story.traits.HasTrait(TorannMagicDefOf.Warlock)))
            {
                tempPod = ThingDef.Named("BookOfDemons");
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Geomancer)))
            {
                tempPod = TorannMagicDefOf.BookOfEarth;
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Technomancer)))
            {
                tempPod = TorannMagicDefOf.BookOfMagitech;
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.BloodMage)))
            {
                tempPod = TorannMagicDefOf.BookOfHemomancy;
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && (user.story.traits.HasTrait(TorannMagicDefOf.Enchanter)))
            {
                tempPod = TorannMagicDefOf.BookOfEnchanter;
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else if (parent.def != null && user.story.traits.HasTrait(TorannMagicDefOf.Gifted))
            {
                tempPod = TorannMagicDefOf.BookOfQuestion;
                this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
            }
            else
            {
                Messages.Message("NotGiftedPawn".Translate(
                        user.LabelShort
                    ), MessageTypeDefOf.RejectInput);
            }
            if (tempPod != null)
            {
                SihvSpawnThings.SpawnThingDefOfCountAt(tempPod, 1, new TargetInfo(currentPos, map));
            }
        }
    }
}
