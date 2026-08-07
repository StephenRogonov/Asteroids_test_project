using _Project.Scripts.Obstacles;
using _Project.Scripts.SceneInitializers;
using _Project.Scripts.Sounds;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class SpawnerInstaller : MonoInstaller
    {
        [SerializeField] private ObstaclesSpawner _spawner;

        public override void InstallBindings()
        {
            Container.Bind<ObstaclesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ObstaclesSpawner>().FromInstance(_spawner).AsSingle();
            Container.BindInterfacesAndSelfTo<GameSceneAudioCatalog>().AsSingle().NonLazy();
            Container.Bind<SoundFactory>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameSceneEntryPoint>().AsSingle();
        }
    }
}