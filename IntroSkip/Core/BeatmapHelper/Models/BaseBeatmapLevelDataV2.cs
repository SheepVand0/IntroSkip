using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions.Must;

namespace SheepIntroSkip.Core.BeatmapHelper.Models
{
    internal class BaseBeatmapLevelDataV2
    {

        internal class NoteData
        {
            public int _type { get; set; }
            public float _time { get; set; }
        }
        internal class CustomData
        {
            internal class CustomBPMChange
            {
                public float _BPM { get; set; }
                public float _time { get; set; }
            }

            public List<CustomBPMChange> _BPMChanges = new List<CustomBPMChange>();
        }

        public BeatmapLevel beatmapLevel;
        public List<BeatmapSaveDataVersion2_6_0AndEarlier.EventData> _events = new List<BeatmapSaveDataVersion2_6_0AndEarlier.EventData>();
        public List<NoteData> _notes { get; set; } = new List<NoteData>();
        public List<NoteData> _bombs { get; set; } = new List<NoteData>();
        public CustomData _customData { get; set; } = new CustomData();
        public string _version { get; set; }

        public BaseBeatmapLevelDataV3 ToV3()
        {
            BaseBeatmapLevelDataV3 l_Result = new BaseBeatmapLevelDataV3();

            _events.OrderBy(x => x.time);

            if (_version[0] == '2' && _version[2] == '6')
                _events.ForEach(x =>
                {
                    if (x.type != BeatmapSaveDataCommon.BeatmapEventType.BpmChange) return;

                    BeatmapSaveDataVersion3.BpmChangeEventData l_Event = new BeatmapSaveDataVersion3.BpmChangeEventData(x.time, x.floatValue);
                    l_Result.bpmEvents.Add(l_Event);
                });
            //_customData._BPMChanges.OrderBy(x => x._time);

            /*float l_Beat = 0;
            float l_LastTime = 0;
            //l_Result.bpmEvents.Add(new BeatmapSaveDataVersion3.BpmChangeEventData(0, beatmapLevel.beatsPerMinute));
            float l_OldBpm = beatmapLevel.beatsPerMinute;
            for (int l_i = 0; l_i < _customData._BPMChanges.Count;l_i++)
            {
                var x = _customData._BPMChanges[l_i];

                l_Beat += ((x._time) - (l_LastTime));

                BeatmapSaveDataVersion3.BpmChangeEventData l_Change = new BeatmapSaveDataVersion3.BpmChangeEventData(l_Beat, x._BPM);
                l_LastTime = x._time;

                l_OldBpm = x._BPM;

                l_Result.bpmEvents.Add(l_Change);
            }*/

            l_Result.TimeInBeats = true;
            _notes.ForEach((x) =>
            {
                if (x._type == 1 || x._type == 0 || x._type == 2) {
                    BaseBeatmapLevelDataV3.NoteData l_Note = new BaseBeatmapLevelDataV3.NoteData();
                    l_Note.b = x._time;
                    l_Result.colorNotes.Add(l_Note);
                } else
                {
                    BaseBeatmapLevelDataV3.BombData l_Bomb = new BaseBeatmapLevelDataV3.BombData();
                    l_Bomb.b = x._time;
                    l_Result.bombsNotes.Add(l_Bomb);
                }
            });

            return l_Result;
        }
    }
}
