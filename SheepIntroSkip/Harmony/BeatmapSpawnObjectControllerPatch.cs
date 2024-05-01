using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnController), "Start")]
    class BeatmapObjectSpawnControllerPatch
    {
        public static void Postfix()
        {
            ParseBeatmap.SkippableTimes = ParseBeatmap.Parse(ParseBeatmap.CurrentBeatmap);

            ParseBeatmap.CheckPartCanBeSkipped(0, ref ParseBeatmap.SkippableTimes);
        }
    }
}
