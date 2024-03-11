using CP_SDK;
using SheepIntroSkip.Core;
using UnityEngine;

namespace SheepIntroSkip
{
    internal class SkipModule : ModuleBase<SkipModule>
    {
        public override EIModuleBaseType Type => EIModuleBaseType.External;

        public override string Name => "Sheep Intro Skip";

        public override string Description => "Allows you to skip useless parts of maps";

        public override bool UseChatFeatures => false;

        public override bool IsEnabled { get => true; set { } }

        public override EIModuleBaseActivationType ActivationType => EIModuleBaseActivationType.OnMenuSceneLoaded;

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