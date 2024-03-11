using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(GameNoteController), "NoteDidPassMissedMarker")]
    internal class OnNoteMissed
    {
        public static void Postfix(GameNoteController __instance) => ParseBeatmap.CheckPartCanBeSkipped(__instance.noteData.time);
    }
}
