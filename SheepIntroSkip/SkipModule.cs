using CP_SDK;
using CP_SDK.UI;
using SheepIntroSkip.Core;
using SheepIntroSkip.UI;
using UnityEngine;

namespace SheepIntroSkip
{
    internal class SkipModule : ModuleBase<SkipModule>
    {
        public override EIModuleBaseType Type => EIModuleBaseType.Integrated;

        public override string Name => "Sheep Intro Skip";

        public override string Description => "Allows you to skip useless parts of maps";

        public override bool UseChatFeatures => false;

        public override bool IsEnabled { get => true; set { } }

        public override EIModuleBaseActivationType ActivationType => EIModuleBaseActivationType.OnMenuSceneLoaded;

        internal static SettingsView Settings;

        protected override (IViewController, IViewController, IViewController) GetSettingsViewControllersImplementation()
        {
            if( Settings == null)
            {
                Settings = UISystem.CreateViewController<SettingsView>();
            }
            return (Settings, null, null);
        }

        protected override void OnDisable()
        {
            
        }

        protected override void OnEnable()
        {
            Object.DontDestroyOnLoad(new GameObject("SheepSkipFloatingScreen").AddComponent<SkipFloatingScreenParent>().gameObject);
            SkipFloatingScreenParent.Instance.gameObject.SetActive(false);
        }
    }
}