using HarmonyLib;
using SheepIntroSkip.Core;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(NoteController), "Init")]
    internal class OnNoteControllerInit
    {
        public static void Prefix() => SongJumpController.Instance.m_Waiting = false;
    }
}