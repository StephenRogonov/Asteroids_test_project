using _Project.Scripts.Bootstrap;
using _Project.Scripts.Bootstrap.Advertising;
using _Project.Scripts.Bootstrap.Authentication;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Bootstrap.Firebase;
using _Project.Scripts.Bootstrap.LoadOptions;
using _Project.Scripts.SceneInitializers;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private EntryPoint _entryPoint;

        public override void InstallBindings()
        {
            Container.Bind<FirebaseSetup>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<FirebaseRemoteConfigFetcher>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AdsInitialization>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<AuthInitialization>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<BootstrapSceneEntryPoint>().AsSingle().NonLazy();
            Container.Bind<EntryPoint>().FromInstance(_entryPoint).AsSingle();
        }
    }
}