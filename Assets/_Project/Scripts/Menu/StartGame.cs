using _Project.Scripts.Common;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Menu
{
    public class StartGame : MonoBehaviour
    {
        private Button _startButton;
        private SceneSwitcher _sceneSwitcher;
        private MainMenuLoader _mainMenuLoader;

        public void Init(SceneSwitcher sceneSwitcher, MainMenuLoader mainMenuLoader)
        {
            _sceneSwitcher = sceneSwitcher;
            _mainMenuLoader = mainMenuLoader;

            _startButton = GetComponent<Button>();
            _startButton.onClick.AddListener(_mainMenuLoader.UnloadMenuAssets);
            _startButton.onClick.AddListener(_sceneSwitcher.LoadGame);
        }

        private void OnDisable()
        {
            _startButton.onClick.RemoveAllListeners();
        }
    }
}