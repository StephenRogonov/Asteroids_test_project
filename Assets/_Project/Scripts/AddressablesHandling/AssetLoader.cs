using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _Project.Scripts.AddressablesHandling
{
    public class AssetLoader : IAssetLoader
    {
        private Dictionary<string, AsyncOperationHandle> _cachedIDHandles = new();
        private Dictionary<AssetReference, AsyncOperationHandle> _cachedReferenceHandles = new();

        public int CachedAssetsCount { get; set; }

        public async UniTask<T> LoadPrefabByID<T>(string assetID) where T : Component
        {
            T asset;
            GameObject resultAsset;
            AsyncOperationHandle handle;

            handle = Addressables.LoadAssetAsync<GameObject>(assetID);
            _cachedIDHandles.Add(assetID, handle);
            CachedAssetsCount++;
            await handle.Task.AsUniTask();

            resultAsset = (GameObject)handle.Result;

            if (resultAsset.TryGetComponent(out asset) == false)
            {
                throw new NullReferenceException($"Object of type {typeof(T)} is null on attempt to load it from addressables.");
            }

            return asset;
        }

        public async UniTask<T> LoadAssetByReference<T>(AssetReferenceT<T> reference) where T : UnityEngine.Object
        {
            AsyncOperationHandle handle;
            T resultAsset;

            handle = Addressables.LoadAssetAsync<T>(reference);
            _cachedReferenceHandles.Add(reference, handle);
            CachedAssetsCount++;
            await handle.Task.AsUniTask();
            resultAsset = (T)handle.Result;

            return resultAsset;
        }

        public async UniTask<T> LoadAssetByID<T>(string assetID) where T : UnityEngine.Object
        {
            AsyncOperationHandle handle;
            T resultAsset;

            handle = Addressables.LoadAssetAsync<T>(assetID);
            _cachedIDHandles.Add(assetID, handle);
            CachedAssetsCount++;
            await handle.Task.AsUniTask();
            resultAsset = (T)handle.Result;

            return resultAsset;
        }

        public void Unload(string assetID)
        {
            if (_cachedIDHandles.ContainsKey(assetID))
            {
                Addressables.Release(_cachedIDHandles[assetID]);
                _cachedIDHandles.Remove(assetID);
                CachedAssetsCount--;
            }
        }

        public void Unload(AssetReference reference)
        {
            if (_cachedReferenceHandles.ContainsKey(reference))
            {
                Addressables.Release(_cachedReferenceHandles[reference]);
                _cachedReferenceHandles.Remove(reference);
                CachedAssetsCount--;
            }
        }
    }
}