using _Project.Scripts.Bootstrap.Advertising;
using _Project.Scripts.Bootstrap.Authentication;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Bootstrap.Firebase;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Bootstrap
{
    public class EntryPoint : MonoBehaviour
    {
        private DataPersistenceHandler _dataPersistence;
        private FirebaseSetup _firebaseSetup;
        private FirebaseRemoteConfigFetcher _configFetcher;
        private AdsInitialization _adsInitialization;
        private AuthInitialization _authInitialization;

        private SceneSwitcher _sceneSwitcher;

        [Inject]
        private void Construct(FirebaseSetup firebaseSetup,
            FirebaseRemoteConfigFetcher remoteConfigFetcher,
            DataPersistenceHandler dataPersistence,
            AdsInitialization adsInitialization,
            AuthInitialization authInitialization,
            SceneSwitcher sceneSwitcher
            )
        {
            _firebaseSetup = firebaseSetup;
            _configFetcher = remoteConfigFetcher;
            _dataPersistence = dataPersistence;
            _adsInitialization = adsInitialization;
            _authInitialization = authInitialization;
            _sceneSwitcher = sceneSwitcher;
        }

        async void Start()
        {
            await UniTask.WhenAll(
                _firebaseSetup.InitializeFirebase(),
                _configFetcher.FetchData(),
                _authInitialization.InitializeAuthentication()
                );

            await _adsInitialization.InitializeAds();
            await _dataPersistence.LoadPlayerData();
            //await _adsInitialization.InitializeAds();
            //await _authInitialization.InitializeAuthentication();

            //_sceneSwitcher.LoadMenu();
        }
    }
}