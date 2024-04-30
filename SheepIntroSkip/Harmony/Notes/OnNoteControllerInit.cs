using HarmonyLib;
using SheepIntroSkip.Core;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(NoteController), "Init")]
    internal class OnNoteControllerInit
    {
        public static void Postfix(ref NoteData noteData)
        {
            if (noteData.cutDirection != NoteCutDirection.None)
            {
                SongJumpController.Instance.SetCannotSkip();
            }
        }
    }
}