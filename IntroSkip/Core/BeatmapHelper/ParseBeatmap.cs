using BeatmapSaveDataVersion3;
using HarmonyLib;
using SheepIntroSkip.Config;
using SheepIntroSkip.Core;
using SheepIntroSkip.Core.BeatmapHelper;
using SheepIntroSkip.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static SheepIntroSkip.Core.BeatmapHelper.BeatmapHelper;

namespace SheepIntroSkip.Harmony
{
    /*[HarmonyPatch(typeof(StandardLevelScenesTransitionSetupDataSO), nameof(StandardLevelScenesTransitionSetupDataSO.Init))]
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

    }*/

    internal class ParseBeatmap
    {
        public const float DURATION_BETWEEN_SKIP_POINT_AND_NOTES = 0.9f;
        public static float MapStartTime = 0;
        public static BeatmapLevel CurrentBeatmap;
        public static BeatmapKey CurrentDifficulty;
        public static bool IsPractice;
        public static List<SkippableTime> SkippableTimes;
        public static List<BPMInterval> BPMs;

        public static async Task<List<SkippableTime>> Parse(BeatmapLevel beatmap, BeatmapKey difficulty)
        {
            SkippableTimes = new List<SkippableTime>();
            var l_SkippableTimes = new List<SkippableTime>();

            if (beatmap == null)
                return new List<SkippableTime>();

            /// Bombs and notes beats
            List<float> l_ElementsTimes = new List<float>();

            /// BPM changes list, with the beat of change
            List<BPMInterval> l_BPMS = new List<BPMInterval>();

            /// Annoying walls
            List<(float, float)> l_AnnoyingWalls = new List<(float, float)>();

            var l_SaveData = await BeatmapHelper.GetSimpleLevelSaveDataV3(beatmap, difficulty);

            /// Getting base bpm
            var l_BpmInterval = new BPMInterval()
            {
                StartBeat = 0,
                EndBeat = beatmap.beatsPerMinute * (beatmap.songDuration / 60.0f),
                BPM = beatmap.beatsPerMinute
            };
            l_BPMS.Add(l_BpmInterval);

            /// Parsing bpms
            for (int l_i = 0; l_i < l_SaveData.bpmEvents.Count; l_i++)
            {
                var x = l_SaveData.bpmEvents[l_i];
                BPMInterval l_Interval = new BPMInterval()
                {
                    StartBeat = x.beat,
                    EndBeat = 0,
                    BPM = x.bpm
                };

                var l_LastBPM = l_BPMS[l_i];
                l_LastBPM.EndBeat = x.beat;
                l_BPMS[l_i] = l_LastBPM;

                if (l_i == l_SaveData.bpmEvents.Count - 1)
                {
                    float l_Duration = beatmap.songDuration - GetTimeFromBeat(x.beat, l_BPMS, false);
                    float l_BeatCountForThisInterval = GetBeatCountForDuration(l_Duration, x.bpm);
                    l_Interval.EndBeat = x.beat + l_BeatCountForThisInterval;
                }

                l_BPMS.Add(l_Interval);
            };

            BPMs = l_BPMS;
            /*foreach (var l_BPM in l_BPMS)
            {
                Plugin.Log.Info($"Bpm : {l_BPM.BPM}, {l_BPM.StartBeat}, {l_BPM.EndBeat}");
            }*/

            /// Getting notes
            l_SaveData.colorNotes.OrderBy(x => x.b);
            l_SaveData.colorNotes.ForEach(p_Index => {
                float l_Time = GetTimeFromBeat(p_Index.b, l_BPMS, l_SaveData.TimeInBeats);

                if (l_Time > MapStartTime) 
                    l_ElementsTimes.Add(l_Time); 
                }
            );

            /// Getting bombs
            //l_SaveData.bombsNotes.ForEach(p_Index => { if (GetTimeFromBeat(p_Index.b, l_BPMS) > MapStartTime && p_Index.i != 0) l_ElementsBeats.Add(p_Index.beat); });
            

            ObjectsGrabber.GrabObjects();

            TextViewController.EValueType l_AnyInformationToHelpOurPoorPlayer = TextViewController.EValueType.Nothing;


            /// Parse notes
            float l_LastElementTime = -1f;
            int i = 0;
            foreach (float l_ElementTime in l_ElementsTimes)
            {
                TextViewController.EValueType l_ValueType = TextViewController.EValueType.Nothing;
                /*if (i < 40)
                {
                    i++;
                    Plugin.Log.Info($"notetiem: {l_ElementTime}");
                }*/

                
                /// Check map begin
                if (l_LastElementTime == -1.0f)
                {
                    l_LastElementTime = l_ElementTime;

                    if (l_LastElementTime - MapStartTime >= ISConfig.Instance.MinimumDelayToBeSkippable)
                    {
                        l_SkippableTimes.Add(new SkippableTime(0.0f, l_LastElementTime - ISConfig.Instance.BeforeNoteTime, l_ValueType));
                    }
                    continue;
                }

                /// Ignore elements before the beginning
                if (l_ElementTime < MapStartTime) continue;

                if (((l_ElementTime - l_LastElementTime) >= ISConfig.Instance.MinimumDelayToBeSkippable) && (l_ElementTime != l_LastElementTime))
                {
                    //Plugin.Log.Info($"Skippable Time origin time: {l_LastElementTime}, end note time: {l_ElementTime}");
                    l_SkippableTimes.Add(new SkippableTime(l_LastElementTime, l_ElementTime - ISConfig.Instance.BeforeNoteTime, l_ValueType));
                }

                l_LastElementTime = l_ElementTime;
            }

            /// Checking if the end of the map can be skipped
            //Plugin.Log.Info($"Elements beats: {l_ElementsTimes.Count}");
            //Plugin.Log.Info($"Bpms: {l_BPMS.Count}");
            if (beatmap.songDuration - l_ElementsTimes.Last() >= 3.0)
                l_SkippableTimes.Add(new SkippableTime(l_ElementsTimes.Last(), CurrentBeatmap.songDuration - 0.9f, TextViewController.EValueType.Nothing));

            /*foreach (var l_Item in l_SkippableTimes)
            {
                Plugin.Log.Info($"Skippable Time: start: {l_Item.Time}, end: {l_Item.ToTime}");
            }*/

            return l_SkippableTimes;
        }

        public static void CheckPartCanBeSkipped(float p_Time, ref List<SkippableTime> times)
        {
            if (times.Count == 0)
                return;

            SkippableTime l_FirstSkippablePart = times.First<SkippableTime>();
            if (l_FirstSkippablePart.Time != p_Time)
                return;

            times.RemoveAt(0);

            bool l_IsMapEnd = l_FirstSkippablePart.ToTime == (CurrentBeatmap.songDuration - 0.9f);
            SongJumpController.Instance.SetCanSkip(/*l_FirstSkippablePart.ToTime - l_FirstSkippablePart.Time,*/ l_FirstSkippablePart.ToTime, TextViewController.EValueType.Nothing, l_IsMapEnd);
        }

        public struct SkippableTime
        {
            public SkippableTime(float p_Time, float p_ToTime, TextViewController.EValueType anyInformation)
            {
                Time = p_Time;
                ToTime = p_ToTime;
                AnyInformation = anyInformation;
            }

            public float Time;
            public float ToTime;
            public TextViewController.EValueType AnyInformation;
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
