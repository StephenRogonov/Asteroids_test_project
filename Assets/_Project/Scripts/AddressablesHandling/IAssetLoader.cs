using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.AddressablesHandling
{
    public interface IAssetLoader
    {
        //public void LoadRemoteAsset(string asssetID);
        public UniTask<T> LoadAsset<T>(string assetID);
        public void UnloadAsset();
    }
}