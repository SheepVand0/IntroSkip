﻿using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(BombNoteController), "HandleDidPassHalfJump")]
    internal class OnBombPassed
    {
        public static void Postfix(BombNoteController __instance) => ParseBeatmap.CheckPartCanBeSkipped(__instance.noteData.time);
    }
}

