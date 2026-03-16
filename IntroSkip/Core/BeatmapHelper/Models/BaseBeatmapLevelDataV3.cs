using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheepIntroSkip.Core.BeatmapHelper.Models
{
    internal class BaseBeatmapLevelDataV3
    {
        // Not in need of all datas
        
        internal class BPMInfo
        {
            internal class Region
            {
                public float _startBeat = 0;
                public float _endBeat = 0;
            }

            public List<Region> _regions = new List<Region>();

        }

        internal class BPMEventData
        {
            public float b;
            public float c;
        }

        internal class NoteData
        {
            public float b;
        }

        internal class BombData
        {
            public float b;
        }

        public List<BeatmapSaveDataVersion3.BpmChangeEventData> bpmEvents { get; set; } = new List<BeatmapSaveDataVersion3.BpmChangeEventData>();

        public bool TimeInBeats = true;
        public List<NoteData> colorNotes { get; set; } = new List<NoteData>();
        public List<BombData> bombsNotes { get; set; } = new List<BombData>();

        public BPMInfo bpmInfo = new BPMInfo();
    }
}
