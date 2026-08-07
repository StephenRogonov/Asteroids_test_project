using _Project.Scripts.InAppPurchasing;
using _Project.Scripts.Sounds;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PurchaseApplier>().AsSingle().NonLazy();
            Container.Bind<ShopItemModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuAudioCatalog>().AsSingle().NonLazy();
            Container.Bind<SoundFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MenuSceneEntryPoint>().AsSingle();
        }
    }
}