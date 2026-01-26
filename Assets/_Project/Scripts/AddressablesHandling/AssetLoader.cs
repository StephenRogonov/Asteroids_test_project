using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.AddressablesHandling
{
    public class AssetLoader : IAssetLoader
    {
        private GameObject _cachedObject;
        private List<GameObject> _assets = new();

        public async UniTask<T> LoadAsset<T>(string assetID) where T : Component
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(assetID);
            _cachedObject = await handle.Task.AsUniTask();
            _assets.Add(_cachedObject);

            if (_cachedObject.TryGetComponent(out T asset) == false)
            {
                throw new NullReferenceException($"Object of type {typeof(T)} is null on attempt to load it from addressables.");
            }

            return asset;
        }

        public void Unload<T>(T asset) where T : Component
        {
            foreach (GameObject item in _assets)
            {
                if (item.GetComponent<T>() != null)
                {
                    Addressables.Release(item);
                    //Debug.Log("!!!-Released-!!!");
                    _assets.Remove(item);
                    return;
                }
            }
        }
    }
}