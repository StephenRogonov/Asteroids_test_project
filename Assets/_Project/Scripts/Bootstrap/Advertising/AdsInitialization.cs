using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.DataPersistence;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Advertisements;

namespace _Project.Scripts.Bootstrap.Advertising
{
    public class AdsInitialization : IUnityAdsInitializationListener
    {
        private DataPersistenceHandler _dataPersistenceHandler;
        private GameConfig _gameConfig;
        private string _androidGameId;
        private string _iOSGameId;
        private string _gameId;
        private bool _testMode;

        public AdsInitialization(DataPersistenceHandler dataPersistenceHandler)
        {
            _dataPersistenceHandler = dataPersistenceHandler;

#if (UNITY_IOS && UNITY_ANDROID)
            _testMode = false;
#elif UNITY_EDITOR
            _testMode = true;
#endif
        }

        public async UniTask InitializeAds()
        {
            _gameConfig = _dataPersistenceHandler.GameConfig;
            _androidGameId = _gameConfig.AndroidGameId;
            _iOSGameId = _gameConfig.IosGameId;

#if UNITY_IOS
            _gameId = _iOSGameId;
#elif (UNITY_ANDROID || UNITY_EDITOR)
            _gameId = _androidGameId;
#endif
            
            if (!Advertisement.isInitialized && Advertisement.isSupported)
            {
                Advertisement.Initialize(_gameId, _testMode, this);
            }
        }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads initialization complete.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
        }
    }
}