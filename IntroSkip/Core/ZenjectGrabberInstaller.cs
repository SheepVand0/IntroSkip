using Zenject;

namespace SheepIntroSkip.Core
{
    internal class ZenjectGrabberInstaller : Installer<ZenjectGrabberInstaller>
    {
        public override void InstallBindings() => this.Container.Bind<ZenjectGrabber>().AsSingle().NonLazy();
    }
}
