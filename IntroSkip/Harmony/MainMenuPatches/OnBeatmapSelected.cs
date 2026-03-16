using BS_Utils.Gameplay;
using HarmonyLib;
using System;
using static PlayerSaveData;

namespace SheepIntroSkip.Harmony
{
    /*[HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), "Init", new Type[20] { 
        typeof(string), 
        typeof(IBeatmapLevelData), 
        typeof(BeatmapKey),
        typeof(BeatmapLevel), 
        typeof(OverrideEnvironmentSettings), 
        typeof(ColorScheme), 
        typeof(ColorScheme), 
        typeof(GameplayModifiers),
        typeof(PlayerSpecificSettings),
        typeof(PracticeSettings),
        typeof(EnvironmentsListModel),
        typeof(AudioClipAsyncLoader),
        typeof(SettingsManager), 
        typeof(BeatmapDataLoader),
        typeof(BeatmapLevelsEntitlementModel),
        typeof(string),
        typeof(bool),
        typeof(bool), 
        typeof(RecordingToolManager.SetupData)
    })]*/
    [HarmonyPatch(typeof(GameScenesManager), nameof(GameScenesManager.PushScenes))]
    internal class OnBeatmapSelected
    {
        public static void Postfix(ref ScenesTransitionSetupDataSO scenesTransitionSetupData) {
            var l_SetupData = (StandardLevelScenesTransitionSetupDataSO)scenesTransitionSetupData;
            //Plugin.Log.Info($"BeatmapVersion : {l_SetupData.beatmapLevel.version}");
            ParseBeatmap.CurrentBeatmap = l_SetupData.beatmapLevel;
            ParseBeatmap.CurrentDifficulty = l_SetupData.beatmapKey;

            if (l_SetupData.practiceSettings == null)
            {
                ParseBeatmap.IsPractice = false;
                ParseBeatmap.MapStartTime = 0;
            } else
            {
                ParseBeatmap.IsPractice = true;
                ParseBeatmap.MapStartTime = l_SetupData.practiceSettings.startSongTime;
            }
        }
    }
} 
