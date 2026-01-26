using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.LoadOptions;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.SceneInitializers
{
    public class BootstrapLoader : MonoBehaviour
    {
        private LoadOptionsView _loadOptionsUI;
        private LoadOptionsPresenter _loadOptionsPresenter;

        private IAssetLoader _assetLoader;
        private IInstantiator _instantiator;

        [Inject]
        private void Construct(
            IInstantiator instantiator,
            IAssetLoader assetLoader,
            LoadOptionsPresenter loadOptionsPresenter
            )
        {
            _instantiator = instantiator;
            _assetLoader = assetLoader;
            _loadOptionsPresenter = loadOptionsPresenter;
        }

        private async void Start()
        {
            _loadOptionsUI = _instantiator.InstantiatePrefabForComponent<LoadOptionsView>(
                await _assetLoader.LoadAsset<LoadOptionsView>(AssetsIDs.LOAD_OPTIONS_MENU));
            _loadOptionsUI.Init(_assetLoader);
            _loadOptionsPresenter.Init(_loadOptionsUI);
        }
    }
}
