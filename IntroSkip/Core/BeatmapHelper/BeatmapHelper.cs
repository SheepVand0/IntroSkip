using BeatmapDataLoaderVersion4;
using BeatmapLevelSaveDataVersion4;
using ModestTree;
using Newtonsoft.Json;
using SheepIntroSkip.Core.BeatmapHelper.Models;
using SheepIntroSkip.Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheepIntroSkip.Core.BeatmapHelper
{
    internal class BeatmapHelper
    {
        public static Dictionary<BeatmapKey, BaseBeatmapLevelDataV3> BeatmapCacheV3 = new Dictionary<BeatmapKey, BaseBeatmapLevelDataV3>();

        internal enum EBeatmapVersion
        {
            V2,
            V3,
            V4
        }


        public static async Task<BaseBeatmapLevelDataV3> GetSimpleLevelSaveDataV3(BeatmapLevel level, BeatmapKey beatmapKey)
        {
            BaseBeatmapLevelDataV3 l_Result = new BaseBeatmapLevelDataV3();
            var l_BeatmapLevelData = SongCore.Loader.CustomLevelLoader.LoadBeatmapLevelData(level);
            Plugin.Log.Info($"Level ID: {level.levelID}");

            //BeatmapLevel
            //SongCore.Loader.GetLevelById()

            var l_BeatmapString = await l_BeatmapLevelData.GetBeatmapStringAsync(beatmapKey);
            l_BeatmapString = l_BeatmapString.Replace(" ", "");
            l_BeatmapString = l_BeatmapString.Replace("\n", "");

            int l_IndexToCheck = l_BeatmapString.IndexOf("version");
            string l_VersionString = string.Empty;
            if (l_BeatmapString[l_IndexToCheck - 1] == '_')
            {
                l_VersionString = "{" + l_BeatmapString.Substring(l_IndexToCheck - 2, 18) + "}";
            } else
            {
                l_VersionString = "{" + l_BeatmapString.Substring(l_IndexToCheck - 1, 17) + "}";
            }


            Plugin.Log.Info("Version String : " + l_VersionString);

            EBeatmapVersion l_BeatmapVersion = EBeatmapVersion.V2;

            var l_VersionDeserialized = JsonConvert.DeserializeObject<BeatmapVersionModel>(l_VersionString);
            string l_UsedVer = string.Empty;
            if (l_VersionDeserialized.version == string.Empty)
            {
                l_UsedVer = l_VersionDeserialized._version;
            } else
            {
                l_UsedVer = l_VersionDeserialized.version;
            }

            if (l_UsedVer.StartsWith("2"))
            {
                l_BeatmapVersion = EBeatmapVersion.V2;
                var l_V2Beatmap = JsonConvert.DeserializeObject<BaseBeatmapLevelDataV2>(l_BeatmapString);
                l_V2Beatmap.beatmapLevel = level;
                l_Result = l_V2Beatmap.ToV3();
            } else
            {
                l_BeatmapVersion = EBeatmapVersion.V3;
                l_Result = JsonConvert.DeserializeObject<BaseBeatmapLevelDataV3>(l_BeatmapString);
/*
                /// Find the level path in order to find this annoying bpminfo file
                List<string> l_Keys = new List<string>(SongCore.Loader.CustomLevels.Keys);
                string l_Path = string.Empty;
                foreach (var l_Item in l_Keys)
                {
                    BeatmapLevel l_FoundLevel;
                    SongCore.Loader.CustomLevels.TryGetValue(l_Item, out l_FoundLevel);

                    if (l_FoundLevel.levelID == level.levelID)
                    {
                        l_Path = l_Item;
                        break;
                    }
                }

                if (File.Exists(l_Path + "/BPMInfo.dat"))
                {
                    BaseBeatmapLevelDataV3.BPMInfo l_BPMInfo 
                        = JsonConvert.DeserializeObject<BaseBeatmapLevelDataV3.BPMInfo>(File.ReadAllText(l_Path + "/BPMInfo.dat"));

                    for (int l_i = 0; l_i < l_BPMInfo._regions.Count;l_i++)
                    {
                        *//*l_Result.bpmEvents[l_i]. = l_BPMInfo._regions[l_i]._startBeat;*//*
                    }
                }*/
            }


            Plugin.Log.Info($"BeatmapElementsCount : {l_Result.colorNotes.Count}");


            l_Result.colorNotes.OrderBy(x => x.b);
            l_Result.bombsNotes.OrderBy(x => x.b);

            return l_Result;
        }

        /*public static Dictionary<BeatmapKey, BeatmapSaveDataVersion4.BeatmapSaveData> BeatmapsCacheV4 = new Dictionary<BeatmapKey, BeatmapSaveDataVersion4.BeatmapSaveData>();
        public static Dictionary<BeatmapKey, BeatmapSaveDataVersion3.BeatmapSaveData> BeatmapsCacheV3 = new Dictionary<BeatmapKey, BeatmapSaveDataVersion3.BeatmapSaveData>();
        public static Dictionary<BeatmapKey, BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData> BeatmapsCacheV2 = new Dictionary<BeatmapKey, BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData>();

        public static async Task<BeatmapSaveDataVersion4.BeatmapSaveData> GetBeatmapSaveData(BeatmapLevel level, BeatmapDifficulty diff, BeatmapCharacteristicSO mode)
        {
            BeatmapSaveDataVersion4.BeatmapSaveData l_Result = new BeatmapSaveDataVersion4.BeatmapSaveData();

            l_Result = await GetBeatmapSaveDataV4(level, diff, mode);
            if (l_Result.colorNotes.Length != 0) return l_Result;

            var l_V3 = await GetBeatmapSaveDataV3(level, diff, mode);
            if (l_V3.colorNotes.LongCount() != 0)
            {
                
                return l_Result;
            }

            return l_Result;
        }

        public static async Task<BeatmapSaveDataVersion4.BeatmapSaveData> GetBeatmapSaveDataV4(BeatmapLevel level, BeatmapDifficulty diff, BeatmapCharacteristicSO characteristics)
        {
            var l_BeatmapLevelData = SongCore.Loader.CustomLevelLoader.LoadBeatmapLevelData(level);
            BeatmapKey l_Key = new BeatmapKey(level.levelID, characteristics, diff);
            if (BeatmapsCacheV4.ContainsKey(l_Key))
            {
                BeatmapsCacheV4.TryGetValue(l_Key, out var l_Value);
                return l_Value;
            }

            var l_BeatmapString = await l_BeatmapLevelData.GetBeatmapStringAsync(l_Key);
            Plugin.Log.Info(l_BeatmapString);
            if (l_BeatmapString == null)
            {
                throw new Exception("Difficulty Beatmap not found");
            }
            BeatmapSaveDataVersion4.BeatmapSaveData l_BeatmapSaveData = null;
            try
            {
                l_BeatmapSaveData = JsonConvert.DeserializeObject<BeatmapSaveDataVersion4.BeatmapSaveData>(l_BeatmapString);
            }
            catch  (Exception ex)
            {
                Plugin.Log.Error("Failed beatmap deserialization");
                Plugin.Log.Error(ex);
                return null;
            }
            BeatmapsCacheV4.Add(l_Key, l_BeatmapSaveData);

            return l_BeatmapSaveData;
        }

        public static async Task<BeatmapSaveDataVersion3.BeatmapSaveData> GetBeatmapSaveDataV3(BeatmapLevel level, BeatmapDifficulty diff, BeatmapCharacteristicSO characteristics)
        {
            var l_BeatmapLevelData = SongCore.Loader.CustomLevelLoader.LoadBeatmapLevelData(level);
            BeatmapKey l_Key = new BeatmapKey(level.levelID, characteristics, diff);
            if (BeatmapsCacheV3.ContainsKey(l_Key))
            {
                BeatmapsCacheV3.TryGetValue(l_Key, out var l_Value);
                return l_Value;
            }

            var l_BeatmapString = await l_BeatmapLevelData.GetBeatmapStringAsync(l_Key);
            Plugin.Log.Info(l_BeatmapString);
            if (l_BeatmapString == null)
            {
                throw new Exception("Difficulty Beatmap not found");
            }
            BeatmapSaveDataVersion3.BeatmapSaveData l_BeatmapSaveData = null;
            try
            {
                l_BeatmapSaveData = JsonConvert.DeserializeObject<BeatmapSaveDataVersion3.BeatmapSaveData>(l_BeatmapString);
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Failed beatmap deserialization");
                Plugin.Log.Error(ex);
                return null;
            }
            BeatmapsCacheV3.Add(l_Key, l_BeatmapSaveData);

            return l_BeatmapSaveData;
        }

        public static async Task<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData> GetBeatmapSaveDataV2(BeatmapLevel level, BeatmapDifficulty diff, BeatmapCharacteristicSO characteristics)
        {
            var l_BeatmapLevelData = SongCore.Loader.CustomLevelLoader.LoadBeatmapLevelData(level);
            BeatmapKey l_Key = new BeatmapKey(level.levelID, characteristics, diff);
            if (BeatmapsCacheV2.ContainsKey(l_Key))
            {
                BeatmapsCacheV2.TryGetValue(l_Key, out var l_Value);
                return l_Value;
            }

            var l_BeatmapString = await l_BeatmapLevelData.GetBeatmapStringAsync(l_Key);
            Plugin.Log.Info(l_BeatmapString);
            if (l_BeatmapString == null)
            {
                throw new Exception("Difficulty Beatmap not found");
            }
            BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData l_BeatmapSaveData = null;
            try
            {
                l_BeatmapSaveData = JsonConvert.DeserializeObject<BeatmapSaveDataVersion2_6_0AndEarlier.BeatmapSaveData>(l_BeatmapString);
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Failed beatmap deserialization");
                Plugin.Log.Error(ex);
                return null;
            }
            BeatmapsCacheV2.Add(l_Key, l_BeatmapSaveData);

            return l_BeatmapSaveData;
        }*/

        public static float GetTimeFromBeat(float beat, List<ParseBeatmap.BPMInterval> bpms, bool timeInBeats)
        {
            if (!timeInBeats) return beat;

            float l_Time = 0;
            for (int l_i = 0; l_i < bpms.Count;l_i++)
            {
                var l_Item = bpms[l_i];

                if (beat > l_Item.EndBeat)
                {
                    l_Time += GetTimeFromBeatSimple((l_Item.EndBeat - l_Item.StartBeat), l_Item.BPM);
                    continue;
                } else
                {
                    l_Time += GetTimeFromBeatSimple(beat - l_Item.StartBeat, l_Item.BPM);
                    break;
                }
                
            }

            return l_Time;
        }

        public static float GetTimeFromBeatSimple(float beat, float bpm)
        {
            return (60f / bpm) * beat;
        }

        //public static float GetBpm(float p_Time, float p_Bpm) => (float)((double)p_Time * (double)p_Bpm / 60.0 / 4.0);

        public static float GetBeatCountForDuration(float duration, float bpm)
        {
            return bpm * (duration / 60);
        }
    }
}
