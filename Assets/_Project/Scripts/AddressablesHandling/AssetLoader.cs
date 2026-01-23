using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.AddressablesHandling
{
    public class AssetLoader : IAssetLoader
    {
        private GameObject _cachedObject;
        private GameObject _remoteObject;

        public async UniTask<T> LoadAsset<T>(string assetID)
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(assetID);
            _cachedObject = await handle.Task.AsUniTask();

            if (_cachedObject.TryGetComponent(out T asset) == false)
            {
                throw new NullReferenceException($"Object of type {typeof(T)} is null on attempt to load it from addressables.");
            }

            return asset;
        }

        //public void LoadRemoteAsset(string assetID)
        //{
        //    Addressables.InstantiateAsync(assetID).Completed += OnLoadDone;
        //}

        //public void OnLoadDone(AsyncOperationHandle<GameObject> handle)
        //{
        //    _remoteObject = handle.Result;
        //}

        //public void Unload()
        //{
        //    Addressables.Release(_remoteObject);
        //}

        public void UnloadAsset()
        {
            if (_cachedObject == null)
            {
                return;
            }

            //Addressables.ReleaseInstance(_cachedObject);
            Addressables.Release(_cachedObject);
            _cachedObject = null;
        }
    }
}