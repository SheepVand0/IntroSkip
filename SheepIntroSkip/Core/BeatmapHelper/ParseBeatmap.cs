using BeatmapSaveDataVersion3;
using HarmonyLib;
using SheepIntroSkip.Config;
using SheepIntroSkip.Core;
using SheepIntroSkip.Core.BeatmapHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SheepIntroSkip.Core.BeatmapHelper.BeatmapHelper;

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
                ParseBeatmap.IsPractice = false;
                ParseBeatmap.MapStartTime = 0;
            } else
            {
                ParseBeatmap.IsPractice = true;
                ParseBeatmap.MapStartTime = practiceSettings.startSongTime;
            }
        }

    }

    internal class ParseBeatmap
    {
        public const float DURATION_BETWEEN_SKIP_POINT_AND_NOTES = 0.9f;
        public static float MapStartTime;
        public static CustomDifficultyBeatmap CurrentBeatmap;
        public static bool IsPractice;
        public static List<SkippableTime> SkippableTimes;

        public static List<SkippableTime> Parse(CustomDifficultyBeatmap beatmap)
        {
            var l_SkippableTimes = new List<SkippableTime>();

            if (beatmap == null)
                return new List<SkippableTime>();

            /// Bombs and notes beats
            List<float> l_ElementsBeats = new List<float>();

            /// BPM changes list, with the beat of change
            List<BPMInterval> l_BPMS = new List<BPMInterval>();

            /// Annoying walls
            List<(float, float)> l_AnnoyingWalls = new List<(float, float)>();

            /// Getting base bpm
            BPMInterval l_FirstInterval = new BPMInterval()
            {
                StartBeat = 0,
                EndBeat = GetBeatCountForDuration(beatmap.level.songDuration, beatmap.beatsPerMinute),
                BPM = beatmap.beatsPerMinute
            };

            l_BPMS.Add(l_FirstInterval);

            /// Parsing bpms
            for (int l_i = 0; l_i < beatmap.beatmapSaveData.bpmEvents.Count; l_i++)
            {
                var x = beatmap.beatmapSaveData.bpmEvents[l_i];
                BPMInterval l_Interval = new BPMInterval();
                l_Interval.StartBeat = x.beat;
                l_Interval.BPM = x.bpm;
                var l_OldChange = l_BPMS[l_i];
                l_OldChange.EndBeat = x.beat;
                l_BPMS[l_i] = l_OldChange;

                if (l_i == beatmap.beatmapSaveData.bpmEvents.Count - 1)
                {
                    float l_Duration = beatmap.level.songDuration - GetTimeFromBeat(x.beat, l_BPMS);
                    float l_BeatCountForThisInterval = GetBeatCountForDuration(l_Duration, x.bpm);
                    l_Interval.EndBeat = x.beat + l_BeatCountForThisInterval;
                }

                l_BPMS.Add(l_Interval);
            };

            /// Getting notes
            beatmap.beatmapSaveData.colorNotes.ForEach(p_Index => {
                if (GetTimeFromBeat(p_Index.beat, l_BPMS) > MapStartTime) 
                    l_ElementsBeats.Add(p_Index.beat); }
            );

            /// Getting Walls
            beatmap.beatmapSaveData.bombNotes.ForEach(p_Index => { if (GetTimeFromBeat(p_Index.beat, l_BPMS) > MapStartTime && p_Index.layer != 0) l_ElementsBeats.Add(p_Index.beat); });
            

            ObjectsGrabber.GrabObjects();

            TextViewController.EValueType l_AnyInformationToHelpOurPoorPlayer = TextViewController.EValueType.Nothing;



            float l_LastElementBeat = -1f;
            foreach (float l_ElementBeat in l_ElementsBeats)
            {
                if (l_LastElementBeat == -1.0f)
                {
                    l_LastElementBeat = l_ElementBeat;
                    float l_Time = GetTimeFromBeat(l_LastElementBeat, l_BPMS);
                    if (l_Time - MapStartTime >= 3.0f)
                    {
                        l_SkippableTimes.Add(new SkippableTime(0.0f, l_Time - 0.9f));
                    }
                    continue;
                }

                float l_ElementTime = GetTimeFromBeat(l_ElementBeat, l_BPMS);

                if (l_ElementTime < MapStartTime) continue;

                float l_LastElementTime = GetTimeFromBeat(l_LastElementBeat, l_BPMS);
                if (l_ElementTime - l_LastElementTime >= ISConfig.Instance.MinimumDelayToBeSkippable && l_ElementTime != l_LastElementTime)
                    l_SkippableTimes.Add(new SkippableTime(l_LastElementTime, l_ElementTime - ISConfig.Instance.BeforeNoteTime));

                l_LastElementBeat = l_ElementBeat;
            }
            if (beatmap.level.songDuration - GetTimeFromBeat(l_ElementsBeats.Last(), l_BPMS) > 3.0)
                l_SkippableTimes.Add(new SkippableTime(GetTimeFromBeat(l_ElementsBeats.Last(), l_BPMS), CurrentBeatmap.level.songDuration - 0.9f));

            if (l_SkippableTimes.Any())
            {
                SkippableTime l_First = l_SkippableTimes.First();
                if (l_First.Time == 0.0)
                {
                    SongJumpController.Instance.SetCanSkip(l_First.ToTime, TextViewController.EValueType.Nothing);
                    l_SkippableTimes.RemoveAt(0);
                }
            }

            return l_SkippableTimes;
        }

        public static void CheckPartCanBeSkipped(float p_Time, ref List<SkippableTime> times)
        {
            if (!times.Any())
                return;

            SkippableTime l_FirstSkippablePart = times.First<SkippableTime>();
            if (l_FirstSkippablePart.Time != p_Time)
                return;
            SongJumpController.Instance.SetCanSkip(/*l_FirstSkippablePart.ToTime - l_FirstSkippablePart.Time,*/ l_FirstSkippablePart.ToTime, TextViewController.EValueType.Nothing);
            times.RemoveAt(0);
        }

        public struct SkippableTime
        {
            public SkippableTime(float p_Time, float p_ToTime)
            {
                Time = p_Time;
                ToTime = p_ToTime;
            }

            public float Time;
            public float ToTime;
        }

        public struct BPMInterval
        {
            public float StartBeat;
            public float EndBeat;
            public float BPM;
        }

        public struct AnnoyingWallTime
        {
            public float StartTime;
            public float EndTime;
        }
    }
}
