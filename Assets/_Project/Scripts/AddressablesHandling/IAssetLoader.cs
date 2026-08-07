using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.AddressablesHandling
{
    public interface IAssetLoader
    {
        public int CachedAssetsCount { get; set; }
        public UniTask<T> LoadPrefabByID<T>(string assetID) where T : Component;
        public UniTask<T> LoadAssetByReference<T>(AssetReferenceT<T> reference) where T : Object;
        public UniTask<T> LoadAssetByID<T>(string assetID) where T : Object;
        public void Unload(string assetID);
        public void Unload(AssetReference reference);
    }
}