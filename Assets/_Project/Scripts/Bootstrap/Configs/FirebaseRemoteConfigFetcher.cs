using _Project.Scripts.DataPersistence;
using Cysharp.Threading.Tasks;
using Firebase.RemoteConfig;
using System;
using UnityEngine;

namespace _Project.Scripts.Bootstrap.Configs
{
    public class FirebaseRemoteConfigFetcher
    {
        private DataPersistenceHandler _dataPersistenceHandler;

        public FirebaseRemoteConfigFetcher(DataPersistenceHandler dataPersistenceHandler)
        {
            _dataPersistenceHandler = dataPersistenceHandler;
        }

        public async UniTask FetchData()
        {
#if UNITY_EDITOR
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).AsUniTask();
#else
            await FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.FromHours(24)).AsUniTask();
#endif
            await FetchComplete();
        }

        private async UniTask FetchComplete()
        {
            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;

            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            await remoteConfig.ActivateAsync();

            _dataPersistenceHandler.SetRemoteGameConfig(remoteConfig.GetValue("gameConfigs").StringValue);
            Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
        }
    }
}