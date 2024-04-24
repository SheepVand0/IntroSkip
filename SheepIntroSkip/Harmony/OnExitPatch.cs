using HarmonyLib;
using SheepIntroSkip.Core;
using UnityEngine;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(PracticeViewController), "DidDeactivate")]
    internal class OnExitPatch
    {
        public static void Postfix()
        {
            StartedFromPracticePatch.StartedFromPractice = false;
            if (!(SongJumpController.Instance != null))
                return;

            SongJumpController.Instance.Waiting = false;
        }
    }
}

