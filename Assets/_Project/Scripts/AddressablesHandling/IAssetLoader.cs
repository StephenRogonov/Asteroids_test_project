using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.AddressablesHandling
{
    public interface IAssetLoader
    {
        public UniTask<T> LoadAsset<T>(string assetID) where T : Component;
        public void Unload<T>(T asset) where T : Component;
    }
}