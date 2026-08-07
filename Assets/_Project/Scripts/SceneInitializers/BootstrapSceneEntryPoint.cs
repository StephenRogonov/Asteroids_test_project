using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Bootstrap.LoadOptions;
using _Project.Scripts.DataPersistence;
using Cysharp.Threading.Tasks;
using System;
using Zenject;

namespace _Project.Scripts.SceneInitializers
{
    public class BootstrapSceneEntryPoint : IInitializable
    {
        private LoadOptionsView _loadOptionsView;
        private LoadOptionsPresenter _loadOptionsPresenter;
        private LoadOptionsModel _loadOptionsModel;
        private DataPersistenceHandler _dataPersistenceHandler;

        private IAssetLoader _assetLoader;
        private IInstantiator _instantiator;

        public BootstrapSceneEntryPoint(
            IInstantiator instantiator,
            IAssetLoader assetLoader,
            DataPersistenceHandler dataPersistenceHandler
            )
        {
            _instantiator = instantiator;
            _assetLoader = assetLoader;
            _dataPersistenceHandler = dataPersistenceHandler;
        }

        public void Initialize()
        {
            _dataPersistenceHandler.SetBootstrapEntryPoint(this);
        }

        public async UniTask LoadSaveChoiseMenu(DateTime local, DateTime cloud)
        {
            _loadOptionsModel = new LoadOptionsModel(_dataPersistenceHandler);
            _loadOptionsPresenter = new LoadOptionsPresenter(_loadOptionsModel);
            _loadOptionsView = _instantiator.InstantiatePrefabForComponent<LoadOptionsView>(
                await _assetLoader.LoadPrefabByID<LoadOptionsView>(AssetsIDs.LOAD_OPTIONS_MENU));
            _loadOptionsView.Init(_assetLoader);
            _loadOptionsPresenter.Init(_loadOptionsView);
            _loadOptionsModel.TriggerLoadOptions(local, cloud);
        }
    }
}
