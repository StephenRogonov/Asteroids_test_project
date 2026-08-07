using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Common;
using _Project.Scripts.Sounds;
using Cysharp.Threading.Tasks;
using System;

namespace _Project.Scripts.GameFlow
{
    public class GameStateLoader
    {
        private SceneSwitcher _sceneSwitcher;
        private IAssetLoader _assetLoader;
        private SoundFactory _soundFactory;
        private LoadingOverlay _loadingOverlay;

        public event Action GameSceneExited;

        public GameStateLoader(
            SceneSwitcher sceneSwitcher,
            IAssetLoader assetLoader,
            SoundFactory soundFactory
            )
        {
            _sceneSwitcher = sceneSwitcher;
            _assetLoader = assetLoader;
            _soundFactory = soundFactory;
        }

        public void Init(LoadingOverlay loadingOverlay)
        {
            _loadingOverlay = loadingOverlay;
        }

        public async void ExitToMainMenu()
        {
            await WaitRunningProcessesToFinish();
            _sceneSwitcher.LoadMenu();
        }

        public async void ReloadGameScene()
        {
            await WaitRunningProcessesToFinish();
            _sceneSwitcher.LoadGame();
        }

        private async UniTask WaitRunningProcessesToFinish()
        {
            _loadingOverlay.EnableObject();
            GameSceneExited?.Invoke();

            while (_assetLoader.CachedAssetsCount != 0 || _soundFactory.ActiveAudioSourcesCount != 0)
            {
                await UniTask.Yield();
            }
        }

    }
}