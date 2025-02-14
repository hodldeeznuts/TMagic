﻿using Verse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace TorannMagic
{
    class PlaceWorker_ShowDMPRadius : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
        {
            Map visibleMap = Find.CurrentMap;
            GenDraw.DrawFieldEdges(Building_TM_DMP.PortableCellsAround(center, visibleMap));
        }
    }
}
