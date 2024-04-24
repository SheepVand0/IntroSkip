using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(PracticeViewController), "PlayButtonPressed")]
    internal class StartedFromPracticePatch
    {
        public static bool StartedFromPractice;

        private static void Postfix() => Harmony.StartedFromPracticePatch.StartedFromPractice = true;
    }
}
