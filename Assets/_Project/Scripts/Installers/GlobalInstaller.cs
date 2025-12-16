using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.Advertising;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GlobalInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalAssetLoader>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SceneSwitcher>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FileDataHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CloudDataHandler>().AsSingle().NonLazy();
            Container.Bind<GameConfig>().AsSingle().NonLazy();
            Container.Bind<DataPersistenceHandler>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<Interstitial>().AsSingle();
            Container.BindInterfacesAndSelfTo<Rewarded>().AsSingle();
        }
    }
}