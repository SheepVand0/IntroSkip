using CP_SDK.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheepIntroSkip.Config
{
    internal class ISConfig : JsonConfig<ISConfig>
    {
        public override string GetRelativePath()
            => "IntroSkip";

        [JsonProperty] internal int PressDuration = 500;
        [JsonProperty] internal float BeforeNoteTime = 2;
        [JsonProperty] internal float MinimumDelayToBeSkippable = 3;
    }
}
