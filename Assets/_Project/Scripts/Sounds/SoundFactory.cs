using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.Configs;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using System.Collections.Generic;
using System.Diagnostics;
using Zenject;

namespace _Project.Scripts.Sounds
{
    public class SoundFactory
    {
        private GameConfig _gameConfig;
        private Pool<AudioPlayer> _audioSourcesPool;
        private IAudioCatalog _audioCatalog;
        private IAssetLoader _assetLoader;
        private IInstantiator _instantiator;
        private Dictionary<AudioID, AudioPlayer> _registeredAudios = new();

        public int ActiveAudioSourcesCount { get; set; }

        public SoundFactory(
            DataPersistenceHandler dataPersistenceHandler,
            IAudioCatalog catalog,
            IAssetLoader assetLoader,
            IInstantiator instantiator
            )
        {
            _gameConfig = dataPersistenceHandler.GameConfig;
            _audioCatalog = catalog;
            _assetLoader = assetLoader;
            _instantiator = instantiator;

            CreateSourcesPool();
        }

        private async void CreateSourcesPool()
        {
             AudioPlayer audioPlayerPrefab = await _assetLoader.LoadPrefabByID<AudioPlayer>(AssetsIDs.AUDIO_PLAYER);
            _audioSourcesPool = _instantiator.Instantiate<Pool<AudioPlayer>>(new object[]
            {
                audioPlayerPrefab, _gameConfig.AudioSourcesPoolInitialSize
            });
        }

        public void PlayBackgroundMusic()
        {
            AudioPlayer audioSource = _audioSourcesPool.Get();
            audioSource.SetParameters(_audioCatalog.GetAudioData(AudioID.BackgroundMusic));
            audioSource.EnableObject();
            audioSource.Play();
        }

        public void PlayRegisteredSound(AudioID id)
        {
            AudioPlayer audioSource = _audioSourcesPool.Get();
            ActiveAudioSourcesCount++;
            audioSource.Finished -= ReturnToPool;
            audioSource.Finished += ReturnToPool;
            audioSource.SetParameters(_audioCatalog.GetAudioData(id));
            _registeredAudios.Add(id, audioSource);
            audioSource.EnableObject();
            audioSource.Play();
        }

        public void StopRegisteredSound(AudioID id)
        {
            if (_registeredAudios.TryGetValue(id, out AudioPlayer audioSource) == false)
            {
                return;
            }
            
            audioSource.StopPlayback();
            _registeredAudios.Remove(id);
        }

        public void PlaySound(AudioID id)
        {
            AudioPlayer audioSource = _audioSourcesPool.Get();
            ActiveAudioSourcesCount++;
            audioSource.Finished -= ReturnToPool;
            audioSource.Finished += ReturnToPool;
            audioSource.SetParameters(_audioCatalog.GetAudioData(id));
            audioSource.EnableObject();
            audioSource.PlayOnce();
        }

        private void ReturnToPool(AudioPlayer source)
        {
            _audioSourcesPool.Return(source);
            ActiveAudioSourcesCount--;
        }

        public void UnloadAssets()
        {
            _audioCatalog.ReleaseLoadedAssets();
            _assetLoader.Unload(AssetsIDs.AUDIO_PLAYER);
        }
    }
}