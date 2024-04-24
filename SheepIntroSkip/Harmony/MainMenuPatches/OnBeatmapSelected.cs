using HarmonyLib;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "Init")]
    internal class OnBeatmapSelected
    {
        public static void Postfix(string gameMode, IDifficultyBeatmap difficultyBeatmap)
        {
            try
            {
                ParseBeatmap.CurrentBeatmap = (CustomDifficultyBeatmap)difficultyBeatmap;
            }
            catch
            {
                Plugin.Log.Error("Not a custom song");
                ParseBeatmap.CurrentBeatmap = (CustomDifficultyBeatmap)null;
            }
        }
    }
}
