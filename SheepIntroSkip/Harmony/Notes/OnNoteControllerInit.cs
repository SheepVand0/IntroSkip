using HarmonyLib;
using SheepIntroSkip.Core;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(NoteController), "Init")]
    internal class OnNoteControllerInit
    {
        public static void Postfix(ref NoteData noteData) => SongJumpController.Instance.Waiting = noteData.cutDirection == NoteCutDirection.None;
    }
}