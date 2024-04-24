using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnController), "Start")]
    class BeatmapObjectSpawnControllerPatch
    {
        public static void Postfix()
        {
            ParseBeatmap.SkippableTimes = ParseBeatmap.Parse(ParseBeatmap.CurrentBeatmap);
        }


    }
}
