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

        private ILocalAssetLoader _assetLoader;

        [Inject]
        private void Construct(
            ILocalAssetLoader assetLoader,
            LoadOptionsPresenter loadOptionsPresenter
            )
        {
            _assetLoader = assetLoader;
            _loadOptionsPresenter = loadOptionsPresenter;
        }

        private async void Start()
        {
            _loadOptionsUI = await _assetLoader.InstantiateAsset<LoadOptionsView>(LocalAssetsIDs.LOAD_OPTIONS_MENU);
            _loadOptionsPresenter.Init(_loadOptionsUI);
        }
    }
}
