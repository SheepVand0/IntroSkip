using CP_SDK_BS.Game;
using IPA.Utilities;
using SheepIntroSkip.Config;
using SheepIntroSkip.Harmony;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SheepIntroSkip.Core
{
    internal class SongJumpController : MonoBehaviour
    {
        public static SongJumpController Instance;

        /// <summary>
        /// I forgot what this variable is useful for, but it's here
        /// </summary>
        internal bool Waiting = false;

        /// <summary>
        /// Waiting for the user to skip
        /// </summary>
        internal bool WaitingForSkip = false;

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////

        public void Awake()
        {
            Instance = this;
            Logic.OnLevelEnded += p_LevelCompletion => Waiting = false;
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////

        public async void SetCanSkip(float p_EndTime, TextViewController.EValueType anyInformation)
        {
            await Task.Delay(500);

            if (Waiting)
                return;

            SkipFloatingScreenParent.Instance.Show(anyInformation);

            Waiting = true;
            await Task.Delay(ISConfig.Instance.PressDuration);

            WaitingForSkip = true;

            bool l_Continue = false;

            await WaitUtils.Wait(() =>
            {
                if (!Waiting)
                {
                    WaitingForSkip = false;
                    return true;
                }

                if (!WaitingForSkip || 
                !Input.GetKeyDown(KeyCode.J) && (double)Input.GetAxis("TriggerLeftHand") < 0.800000011920929 && 
                (double)Input.GetAxis("TriggerRightHand") < 0.800000011920929)
                    return false;

                l_Continue = true;
                return true;
            }, 1);

            Waiting = false;
            WaitingForSkip = false;
            SkipFloatingScreenParent.Instance.Hide();
            if (!l_Continue)
                return;

            ObjectsGrabber.GrabObjects();
            NoteCutSoundEffectManagerFix.m_Disabled = false;

            AudioSource l_AudioSource = ObjectsGrabber.GameSongControllerObj.GetField<AudioTimeSyncController, GameSongController>("_audioTimeSyncController").GetField<AudioSource, AudioTimeSyncController>("_audioSource");

            l_AudioSource.time = p_EndTime;
            await Task.Delay(500);

            NoteCutSoundEffectManagerFix.m_Disabled = true;
        }
    }
}
