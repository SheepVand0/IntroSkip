using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SheepIntroSkip.Core
{
    public class ObjectsGrabber
    {
        public static BeatmapObjectSpawnController ObjectSpawnController;
        public static BasicBeatmapObjectManager ObjectManager;
        public static BeatmapCallbacksController CallbacksController;
        public static BeatmapObjectSpawnMovementData ObjectsSpawnMovementData;
        public static GameSongController GameSongControllerObj;
        public static AudioTimeSyncController AudioTimeSyncControlleObj;
        public static AudioSource GameAudioSource;
        public static PlayerData GamePlayerData;
        public static Saber RightSaber;
        public static Saber LeftSaber;

        public static void GrabObjects()
        {
            try
            {
                ObjectSpawnController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().FirstOrDefault();
                CallbacksController = ObjectSpawnController.GetField<BeatmapCallbacksController, BeatmapObjectSpawnController>("_beatmapCallbacksController");
                GameSongControllerObj = Resources.FindObjectsOfTypeAll<GameSongController>().FirstOrDefault();
                AudioTimeSyncControlleObj = Resources.FindObjectsOfTypeAll<AudioTimeSyncController>().FirstOrDefault();
                GamePlayerData = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First().playerData;
                ObjectsSpawnMovementData = ObjectSpawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData");
                GameAudioSource = AudioTimeSyncControlleObj.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
                foreach (Saber saber in Resources.FindObjectsOfTypeAll<Saber>())
                {
                    if (saber.saberType == SaberType.SaberA)
                        LeftSaber = saber;
                    else
                        RightSaber = saber;
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("[SHEEP_COMMAND_ERROR] : " + ex.Message);
            }
        }
    }
}
