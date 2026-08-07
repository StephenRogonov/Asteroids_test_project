using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Common;
using _Project.Scripts.Sounds;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Menu
{
    public class StartGame : MonoBehaviour
    {
        private Button _startButton;
        private SceneSwitcher _sceneSwitcher;
        private MenuSceneEntryPoint _mainMenuLoader;
        private IAssetLoader _assetLoader;
        private SoundFactory _soundFactory;

        [Inject]
        private void Construct(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public void Init(
            SceneSwitcher sceneSwitcher, 
            MenuSceneEntryPoint mainMenuLoader, 
            SoundFactory soundFactory
            )
        {
            _sceneSwitcher = sceneSwitcher;
            _mainMenuLoader = mainMenuLoader;
            _soundFactory = soundFactory;

            _startButton = GetComponent<Button>();
            _startButton.onClick.AddListener(LoadGameScene);
        }

        private async void LoadGameScene()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            _mainMenuLoader.UnloadMenuAssets();

            while (_assetLoader.CachedAssetsCount != 0 || _soundFactory.ActiveAudioSourcesCount !=0)
            {
                await UniTask.Yield();
            }

            _sceneSwitcher.LoadGame();
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveAllListeners();
        }
    }
}