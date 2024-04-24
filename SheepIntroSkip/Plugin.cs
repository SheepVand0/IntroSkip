using HarmonyLib;
using IPA;
using SheepIntroSkip.Core;
using System.Collections.Generic;
using UnityEngine;

namespace SheepIntroSkip
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal HarmonyLib.Harmony HarmonyInstance = new HarmonyLib.Harmony("sheepvand.bs.introskip");

        internal static Plugin Instance { get; private set; }

        internal static IPA.Logging.Logger Log { get; private set; }

        [IPA.Init]
        public void Init(IPA.Logging.Logger logger)
        {
            Plugin.Instance = this;
            Plugin.Log = logger;
            this.HarmonyInstance.PatchAll();
            new GameObject("SheepJumpController").AddComponent<SongJumpController>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
        }

        [OnExit]
        public void OnApplicationExit() => this.HarmonyInstance.UnpatchSelf();
    }
}
