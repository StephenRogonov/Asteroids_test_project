using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles.Score;
using _Project.Scripts.UI;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private GameLoader _gameLoader;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HudModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseModel>().AsSingle().Lazy();
            Container.BindInterfacesAndSelfTo<PausePresenter>().AsSingle().NonLazy();
            Container.Bind<GameLoader>().FromInstance(_gameLoader).AsSingle().NonLazy();
        }
    }
}