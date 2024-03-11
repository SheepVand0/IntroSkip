using HarmonyLib;
using SheepIntroSkip.Core;
using System.Linq;
using System.Threading.Tasks;

namespace SheepIntroSkip.Harmony
{
    /*[HarmonyPatch(typeof(EnvironmentSceneSetup), "InstallBindings")]
    internal class OnGameStarted
    {
        public static async void Postfix()
        {
            if (!Plugin.s_SkipabbleTimes.Any())
                return;
            Plugin.SkippableTime l_First = Plugin.s_SkipabbleTimes.First<Plugin.SkippableTime>();
            if ((double)l_First.Time != 0.0)
                return;
            await Task.Delay(500);
            SongJumpController.Instance.SetCanSkip(l_First.ToTime);
            Plugin.s_SkipabbleTimes.RemoveAt(0);
        }
    }*/
}