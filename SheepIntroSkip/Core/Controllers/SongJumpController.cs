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
        //internal bool Waiting = false;

        /// <summary>
        /// Waiting for the user to skip
        /// </summary>
        internal bool WaitingForSkip = false;

        /// <summary>
        /// The time you will go to if you skip
        /// </summary>
        internal float NextTime { get; private set; } = -1;

        internal float CannotSkipAnymoreTime { get; private set; } = -1;

        /// <summary>
        /// This could help you
        /// </summary>
        TextViewController.EValueType AnyInformation = TextViewController.EValueType.Nothing;

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////

        public void Awake()
        {
            Instance = this;
            Logic.OnLevelEnded += p_LevelCompletion => SetCannotSkip();
        }

        ////////////////////////////////////////////////
        ///////////////////////////////////////////////

        protected float CurrentSkipTime;
        protected float LastTime;

        public void Update()
        {
            ManualUpdate();
            LastTime = Time.realtimeSinceStartup;
        }

        private void ManualUpdate()
        {
            if (!WaitingForSkip) return;

            if (ObjectsGrabber.GameAudioSource.time >= CannotSkipAnymoreTime)
            {
                SetCannotSkip();
                return;
            }

            if (Input.GetKey(KeyCode.J) || Input.GetAxis("TriggerLeftHand") > 0.80f || Input.GetAxis("TriggerRightHand") > 0.80f)
            {
                if (CurrentSkipTime < ISConfig.Instance.PressDuration)
                {
                    CurrentSkipTime += (Time.realtimeSinceStartup - LastTime);
                }
                else
                {
                    Skip(NextTime);
                    SetCannotSkip();
                    CurrentSkipTime = 0;
                }

                return;
            }

            CurrentSkipTime = 0;
        }

        public void SetCanSkip(float time, TextViewController.EValueType anyInformation)
        {
            WaitingForSkip = true;
            NextTime = time;
            AnyInformation = anyInformation;

            ObjectsGrabber.GrabObjects();
            CannotSkipAnymoreTime = time - ObjectsGrabber.ObjectSpawnController.beatmapObjectSpawnMovementData.jumpDuration;

            SkipFloatingScreenParent.Instance.Show(anyInformation);
        }

        public void SetCannotSkip()
        {
            WaitingForSkip = false;
            SkipFloatingScreenParent.Instance.Hide();
        }

        public void Skip(float time)
        {
            AudioSource l_AudioSource = ObjectsGrabber.GameSongControllerObj
                .GetField<AudioTimeSyncController, GameSongController>("_audioTimeSyncController")
                .GetField<AudioSource, AudioTimeSyncController>("_audioSource");
            l_AudioSource.time = time;
        }

        /*public async void SetCanSkip(float p_EndTime, TextViewController.EValueType anyInformation)
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
                    l_Continue = false;
                    return true;
                }

                if (!WaitingForSkip || 
                !Input.GetKeyDown(KeyCode.J) && (double)Input.GetAxis("TriggerLeftHand") < 0.800000011920929 && 
                (double)Input.GetAxis("TriggerRightHand") < 0.800000011920929)
                    return false;

                l_Continue = true;
                return true;
            }, 1);

            SkipFloatingScreenParent.Instance.Hide();

            Waiting = false;
            WaitingForSkip = false;
            if (!l_Continue)
                return;

            ObjectsGrabber.GrabObjects();
            NoteCutSoundEffectManagerFix.m_Disabled = false;

            AudioSource l_AudioSource = ObjectsGrabber.GameSongControllerObj.GetField<AudioTimeSyncController, GameSongController>("_audioTimeSyncController").GetField<AudioSource, AudioTimeSyncController>("_audioSource");

            l_AudioSource.time = p_EndTime;
            await Task.Delay(500);

            NoteCutSoundEffectManagerFix.m_Disabled = true;
        }*/
    }
}
