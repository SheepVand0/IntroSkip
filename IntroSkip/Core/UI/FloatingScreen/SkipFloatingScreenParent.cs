using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.FloatingScreen;
using HMUI;
using UnityEngine;

namespace SheepIntroSkip.Core
{
    internal class SkipFloatingScreenParent : MonoBehaviour
    {
        public static SkipFloatingScreenParent Instance;
        
        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        private FloatingScreen m_FloatingScreen;

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public TextViewController TextView;

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////
        
        public void Awake()
        {
            Instance = this;
            m_FloatingScreen = BeatSaberMarkupLanguage.FloatingScreen.FloatingScreen.CreateFloatingScreen(new Vector2(90f, 30f), false, new Vector3(0.0f, -0.48f, 2.38f), Quaternion.Euler(new Vector3(51.43f, 0.0f, 0.0f)));
            m_FloatingScreen.SetRootViewController(TextView = BeatSaberUI.CreateViewController<TextViewController>(), ViewController.AnimationType.None);
            m_FloatingScreen.transform.SetParent(transform);
        }

        public void Show(TextViewController.EValueType anyInformation)
        {
            gameObject.SetActive(true);
            TextView.SetUsefulInfoTextValue(anyInformation);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
