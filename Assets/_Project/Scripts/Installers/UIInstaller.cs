using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles.Score;
using _Project.Scripts.UI;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<HudModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<LeaderboardPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStateLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<PausePresenter>().AsSingle();
        }
    }
}