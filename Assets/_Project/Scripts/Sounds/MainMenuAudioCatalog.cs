using _Project.Scripts.AddressablesHandling;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Sounds
{
    public class MainMenuAudioCatalog : IAudioCatalog
    {
        private IAssetLoader _assetLoader;
        private AudioCatalogSO _audioCatalog;
        private Dictionary<AudioID, AudioClipDataSO> _loadedAudioData = new();

        public MainMenuAudioCatalog(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async UniTask LoadAssets()
        {   
            _audioCatalog = await _assetLoader.LoadAssetByID<AudioCatalogSO>(AssetsIDs.AUDIO_CATALOG_MAIN);

            foreach (AssetReferenceT<AudioClipDataSO> reference in _audioCatalog.ClipData)
            {
                AudioClipDataSO clipData = await _assetLoader.LoadAssetByReference<AudioClipDataSO>(reference);
                clipData.LoadedClip = await _assetLoader.LoadAssetByReference<AudioClip>(clipData.ClipReference);
                _loadedAudioData.Add(clipData.NameID, clipData);
            }
        }

        public AudioClipDataSO GetAudioData(AudioID id)
        {
            return _loadedAudioData[id];
        }

        public void ReleaseLoadedAssets()
        {
            foreach (var item in _loadedAudioData)
            {
                _assetLoader.Unload(item.Value.ClipReference);
            }

            foreach (AssetReferenceT<AudioClipDataSO> reference in _audioCatalog.ClipData)
            {
                _assetLoader.Unload(reference);
            }

            _assetLoader.Unload(AssetsIDs.AUDIO_CATALOG_MAIN);
        }
    }
}
