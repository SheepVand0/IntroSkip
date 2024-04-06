using BeatmapSaveDataVersion3;
using HarmonyLib;
using SheepIntroSkip.Config;
using SheepIntroSkip.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SheepIntroSkip.Harmony
{
    [HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Init))]
    internal class OnMapStart
    {
        public static void Postfix(ref PracticeSettings practiceSettings)
        {
            Plugin.Log.Info($"Is in practice : {practiceSettings != null}");
            if (practiceSettings == null)
            {
                ParseBeatmap.s_IsPractice = false;
                ParseBeatmap.s_MapStartTime = 0;
            } else
            {
                ParseBeatmap.s_IsPractice = true;
                ParseBeatmap.s_MapStartTime = practiceSettings.startSongTime;
            }
        }

    }

    [HarmonyPatch(typeof(BeatmapObjectSpawnController), "Start")]
    internal class ParseBeatmap
    {
        public const float DURATION_BETWEEN_SKIP_POINT_AND_NOTES = 0.9f;
        public static float s_Bpm;
        public static float s_MapStartTime;
        public static CustomDifficultyBeatmap s_Beatmap;
        public static bool s_IsPractice;

        public static void Postfix()
        {
            Plugin.Log.Info("Checking");
            if (s_Beatmap == null)
                return;
            Plugin.s_SkipabbleTimes.Clear();
            
            s_Bpm = s_Beatmap.level.beatsPerMinute;
            List<float> l_ElementsTime = new List<float>();
            Dictionary<float, float> l_BpmChanges = new Dictionary<float, float>();
            s_Beatmap.beatmapSaveData.colorNotes.ForEach(p_Index => { if (GetTimeFromBeat(p_Index.beat, s_Bpm) > s_MapStartTime) l_ElementsTime.Add(p_Index.beat); });
            s_Beatmap.beatmapSaveData.bombNotes.ForEach(p_Index => { if (GetTimeFromBeat(p_Index.beat, s_Bpm) > s_MapStartTime && p_Index.layer != 0) l_ElementsTime.Add(p_Index.beat); });
            s_Beatmap.beatmapSaveData.bpmEvents.ForEach(p_Index => l_BpmChanges.Add(p_Index.beat, p_Index.bpm));
            ObjectsGrabber.GrabObjects();

            //StartedFromPractice.s_StartedFromPractice;
            //ObjectsGrabber.GamePlayerData.practiceSettings.startSongTime;

            float l_LastElementBeat = -1f;
            l_ElementsTime.Sort();
            foreach (float l_ElementBeat in l_ElementsTime)
            {

                foreach (var l_BPMChange in l_BpmChanges)
                {
                    if (l_BPMChange.Key >= l_ElementBeat)
                    {
                        s_Bpm = l_BPMChange.Value;
                        break;
                    }
                }

                if (l_LastElementBeat == -1.0f)
                {
                    l_LastElementBeat = l_ElementBeat;
                    if (GetTimeFromBeat(l_LastElementBeat, s_Bpm) - s_MapStartTime >= 3.0f)
                        Plugin.s_SkipabbleTimes.Add(new Plugin.SkippableTime(0.0f, GetTimeFromBeat(l_LastElementBeat, s_Bpm) - 0.9f));
                    continue;
                }

                float l_ElementTime = GetTimeFromBeat(l_ElementBeat, s_Bpm);
                if (l_ElementTime < s_MapStartTime) continue;
                float l_LastElementTime = GetTimeFromBeat(l_LastElementBeat, s_Bpm);
                if (l_ElementTime - l_LastElementTime >= ISConfig.Instance.MinimumDelayToBeSkippable && l_ElementTime != l_LastElementTime)
                    Plugin.s_SkipabbleTimes.Add(new Plugin.SkippableTime(l_LastElementTime, l_ElementTime - ISConfig.Instance.BeforeNoteTime));

                l_LastElementBeat = l_ElementBeat;
            }
            if (s_Beatmap.level.songDuration - GetTimeFromBeat(l_ElementsTime.Last(), s_Bpm) > 3.0)
                Plugin.s_SkipabbleTimes.Add(new Plugin.SkippableTime(GetTimeFromBeat(l_ElementsTime.Last(), s_Bpm), s_Beatmap.level.songDuration - 0.9f));

            if (Plugin.s_SkipabbleTimes.Any())
            {
                Plugin.SkippableTime l_First = Plugin.s_SkipabbleTimes.First();
                if (l_First.Time == 0.0)
                {
                    SongJumpController.Instance.SetCanSkip(l_First.ToTime);
                    Plugin.s_SkipabbleTimes.RemoveAt(0);
                }
            }
            //await Task.Delay(500);
            
        }

        public static float GetTimeFromBeat(float p_Beat, float p_Bpm) => p_Beat * 60f / p_Bpm;

        public static float GetBpm(float p_Time, float p_Bpm) => (float)((double)p_Time * (double)p_Bpm / 60.0 / 4.0);

        public static void CheckPartCanBeSkipped(float p_Time)
        {
            if (!Plugin.s_SkipabbleTimes.Any())
                return;

            Plugin.SkippableTime l_FirstSkippablePart = Plugin.s_SkipabbleTimes.First<Plugin.SkippableTime>();
            if (l_FirstSkippablePart.Time != p_Time)
                return;
            SongJumpController.Instance.SetCanSkip(/*l_FirstSkippablePart.ToTime - l_FirstSkippablePart.Time,*/ l_FirstSkippablePart.ToTime);
            Plugin.s_SkipabbleTimes.RemoveAt(0);
        }
    }
}
