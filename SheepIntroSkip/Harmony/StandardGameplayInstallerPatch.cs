using HarmonyLib;
using System;
using System.Reflection;
using Zenject;

namespace SheepIntroSkip.Core
{
    [HarmonyPatch(typeof(StandardGameplayInstaller), "InstallBindings")]
    public static class StandardGameplayInstallerPatch
    {
        private static readonly PropertyInfo s_ContainerPropertyInfo = typeof(MonoInstallerBase).GetProperty("Container", BindingFlags.Instance | BindingFlags.NonPublic);

        private static void Postfix(StandardGameplayInstaller __instance)
        {
            try
            {
                Installer<ZenjectGrabberInstaller>.Install(__instance.GetContainer());
            }
            catch (Exception ex)
            {
                Plugin.Log.Error("Error during binding Leaderboard (GuildSaberLeaderboard)");
                Plugin.Log.Error(string.Format("Here the stacktrace : {0}", (object)ex));
            }
        }

        private static DiContainer GetContainer(this MonoInstallerBase p_MonoInstallerBase) => (DiContainer)StandardGameplayInstallerPatch.s_ContainerPropertyInfo.GetValue((object)p_MonoInstallerBase);
    }
}
