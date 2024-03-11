using HarmonyLib;
using SheepIntroSkip.Core;
using UnityEngine;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(PracticeViewController), "DidDeactivate")]
    internal class OnExit
    {
        public static void Postfix()
        {
            StartedFromPractice.s_StartedFromPractice = false;
            if (!((Object)SongJumpController.Instance != (Object)null))
                return;
            SongJumpController.Instance.m_Waiting = false;
        }
    }
}

