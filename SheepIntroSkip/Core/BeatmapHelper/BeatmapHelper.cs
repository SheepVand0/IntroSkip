using SheepIntroSkip.Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheepIntroSkip.Core.BeatmapHelper
{
    internal class BeatmapHelper
    {
        public static float GetTimeFromBeat(float beat, List<ParseBeatmap.BPMInterval> bpms)
        {
            float l_Time = 0;
            for (int l_i = 0; l_i < bpms.Count;l_i++)
            {
                var l_Item = bpms[l_i];

                if (beat > l_Item.EndBeat)
                {
                    l_Time += GetTimeFromBeatSimple(l_Item.EndBeat - l_Item.StartBeat, l_Item.BPM);
                    continue;
                }

                l_Time += GetTimeFromBeatSimple((beat - l_Item.StartBeat), l_Item.BPM);
            }

            return l_Time;
        }

        public static float GetTimeFromBeatSimple(float beat, float bpm)
        {
            return beat * 60f / bpm;
        }

        public static float GetBpm(float p_Time, float p_Bpm) => (float)((double)p_Time * (double)p_Bpm / 60.0 / 4.0);

        public static float GetBeatCountForDuration(float duration, float bpm)
        {
            return bpm * duration;
        }
    }
}
