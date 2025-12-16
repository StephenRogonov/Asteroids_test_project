using _Project.Scripts.GameFlow;
using _Project.Scripts.Obstacles.Score;
using _Project.Scripts.Player;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PauseSwitcher>().AsSingle();
            Container.Bind<ScoreCounter>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerControls>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<Camera>().FromInstance(Camera.main).AsSingle();
        }
    }
}