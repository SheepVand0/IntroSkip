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
                ObjectsGrabber.ObjectSpawnController = ((IEnumerable<BeatmapObjectSpawnController>)Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>()).FirstOrDefault<BeatmapObjectSpawnController>();
                ObjectsGrabber.CallbacksController = ObjectsGrabber.ObjectSpawnController.GetField<BeatmapCallbacksController, BeatmapObjectSpawnController>("_beatmapCallbacksController");
                ObjectsGrabber.GameSongControllerObj = ((IEnumerable<GameSongController>)Resources.FindObjectsOfTypeAll<GameSongController>()).FirstOrDefault<GameSongController>();
                ObjectsGrabber.AudioTimeSyncControlleObj = ((IEnumerable<AudioTimeSyncController>)Resources.FindObjectsOfTypeAll<AudioTimeSyncController>()).FirstOrDefault<AudioTimeSyncController>();
                ObjectsGrabber.GamePlayerData = ((IEnumerable<PlayerDataModel>)Resources.FindObjectsOfTypeAll<PlayerDataModel>()).First<PlayerDataModel>().playerData;
                ObjectsGrabber.ObjectsSpawnMovementData = ObjectsGrabber.ObjectSpawnController.GetField<BeatmapObjectSpawnMovementData, BeatmapObjectSpawnController>("_beatmapObjectSpawnMovementData");
                ObjectsGrabber.GameAudioSource = ObjectsGrabber.AudioTimeSyncControlleObj.GetField<AudioSource, AudioTimeSyncController>("_audioSource");
                foreach (Saber saber in Resources.FindObjectsOfTypeAll<Saber>())
                {
                    if (saber.saberType == SaberType.SaberA)
                        ObjectsGrabber.LeftSaber = saber;
                    else
                        ObjectsGrabber.RightSaber = saber;
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("[SHEEP_COMMAND_ERROR] : " + ex.Message);
            }
        }
    }
}
