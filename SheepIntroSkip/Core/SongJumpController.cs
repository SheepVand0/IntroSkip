using BeatSaberPlus.SDK.Game;
using IPA.Utilities;
using SheepIntroSkip.Harmony;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SheepIntroSkip.Core
{
    internal class SongJumpController : MonoBehaviour
    {
        public static SongJumpController Instance;
        internal bool m_Waiting = false;
        internal bool m_WaitingForSkip = false;

        public void Awake()
        {
            SongJumpController.Instance = this;
            Logic.OnLevelEnded += (Action<LevelCompletionData>)(p_LevelCompletion => this.m_Waiting = false);
        }

        public async void SetCanSkip(float p_EndTime)
        {
            
            if (StartedFromPractice.s_StartedFromPractice)
                return;

            await Task.Delay(500);
            Plugin.Log.Info(string.Format("Can Skip to {0} seconds", p_EndTime));
            if (m_Waiting)
                return;

            SkipFloatingScreenParent.Instance.gameObject.SetActive(true);

            m_Waiting = true;
            await Task.Delay(700);
            m_WaitingForSkip = true;

            bool l_Continue = false;
            bool l_SkipWaitUntil = false;

            await WaitUtils.Wait(() =>
            {
                if (l_SkipWaitUntil)
                    return true;

                if (!m_Waiting)
                {
                    m_WaitingForSkip = false;
                    l_SkipWaitUntil = true;
                    return false;
                }
                if (!m_WaitingForSkip || 
                !Input.GetKeyDown(KeyCode.J) && (double)Input.GetAxis("TriggerLeftHand") < 0.800000011920929 && (double)Input.GetAxis("TriggerRightHand") < 0.800000011920929)
                    return false;

                l_Continue = true;
                return true;
            }, 1);

            m_Waiting = false;
            m_WaitingForSkip = false;
            SkipFloatingScreenParent.Instance.gameObject.SetActive(false);
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
