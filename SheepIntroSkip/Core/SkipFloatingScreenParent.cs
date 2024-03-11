using BeatSaberMarkupLanguage;
using HMUI;
using UnityEngine;

namespace SheepIntroSkip.Core
{
    internal class SkipFloatingScreenParent : MonoBehaviour
    {
        public static SkipFloatingScreenParent Instance;
        private BeatSaberMarkupLanguage.FloatingScreen.FloatingScreen m_FloatingScreen;

        public void Awake()
        {
            Instance = this;
            m_FloatingScreen = BeatSaberMarkupLanguage.FloatingScreen.FloatingScreen.CreateFloatingScreen(new Vector2(90f, 30f), false, new Vector3(0.0f, -0.48f, 2.38f), Quaternion.Euler(new Vector3(51.43f, 0.0f, 0.0f)));
            m_FloatingScreen.SetRootViewController(BeatSaberUI.CreateViewController<TextViewController>(), ViewController.AnimationType.None);
            m_FloatingScreen.transform.SetParent(transform);
        }
    }
}
