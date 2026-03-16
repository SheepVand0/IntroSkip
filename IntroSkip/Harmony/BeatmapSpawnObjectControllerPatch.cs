using HarmonyLib;
using SheepIntroSkip.Core;
using System.Threading.Tasks;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(BeatmapObjectSpawnController), "Start")]
    class BeatmapObjectSpawnControllerPatch
    {
        public static async void Postfix()
        {
            ParseBeatmap.SkippableTimes = await ParseBeatmap.Parse(ParseBeatmap.CurrentBeatmap, ParseBeatmap.CurrentDifficulty);

            await Task.Delay(500);

            ParseBeatmap.CheckPartCanBeSkipped(0, ref ParseBeatmap.SkippableTimes);
        }
    }
}
