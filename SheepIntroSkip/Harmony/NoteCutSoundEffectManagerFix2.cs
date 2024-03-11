using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(NoteCutSoundEffectManager), "HandleNoteWasSpawned")]
    internal class NoteCutSoundEffectManagerFix2
    {
        private static bool Prefix() => NoteCutSoundEffectManagerFix.m_Disabled;
    }
}

