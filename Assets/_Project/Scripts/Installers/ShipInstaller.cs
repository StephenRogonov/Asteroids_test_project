using _Project.Scripts.Bootstrap.Analytics;
using _Project.Scripts.GameFlow;
using _Project.Scripts.PlayerWeapons;
using _Project.Scripts.PlayerWeapons.Configs;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class ShipInstaller : MonoInstaller
    {
        [SerializeField] private ShipLaserConfig _shipShootingLaserConfig;
        [SerializeField] private ShipSpawnPosition _shipSpawnPosition;

        public override void InstallBindings()
        {
            Container.Bind<ShipLaserConfig>().FromInstance(_shipShootingLaserConfig).AsSingle();
            Container.Bind<ShipSpawnPosition>().FromInstance(_shipSpawnPosition).AsSingle();
            Container.Bind<MissilesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeaponTrigger>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AnalyticsService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameSessionData>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameOverPresenter>().AsSingle();
        }
    }
}