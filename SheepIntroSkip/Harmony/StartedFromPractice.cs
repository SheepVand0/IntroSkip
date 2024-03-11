using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(PracticeViewController), "PlayButtonPressed")]
    internal class StartedFromPractice
    {
        public static bool s_StartedFromPractice;

        private static void Postfix() => StartedFromPractice.s_StartedFromPractice = true;
    }
}
