using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(GameNoteController), "HandleCut")]
    internal class OnNoteCut
    {
        public static void Postfix(GameNoteController __instance) => ParseBeatmap.CheckPartCanBeSkipped(__instance.noteData.time, ref ParseBeatmap.SkippableTimes);
    }
}

