using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(BombNoteController), "HandleWasCutBySaber")]
    internal class OnBombCut
    {
        public static void Postfix(BombNoteController __instance) => ParseBeatmap.CheckPartCanBeSkipped(__instance.noteData.time, ref ParseBeatmap.SkippableTimes);
    }
}

