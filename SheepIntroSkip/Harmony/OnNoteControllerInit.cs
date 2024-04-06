using HarmonyLib;
using SheepIntroSkip.Core;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(NoteController), "Init")]
    internal class OnNoteControllerInit
    {
        public static void Postfix(ref NoteData noteData) => SongJumpController.Instance.m_Waiting = noteData.cutDirection == NoteCutDirection.None;
    }
}