using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(NoteCutSoundEffectManager), "HandleNoteWasCut")]
    public class NoteCutSoundEffectManagerFix
    {
        public static bool m_Disabled = true;

        private static bool Prefix() => NoteCutSoundEffectManagerFix.m_Disabled;
    }
}
