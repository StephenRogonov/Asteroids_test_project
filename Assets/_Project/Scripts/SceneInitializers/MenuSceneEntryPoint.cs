using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.InAppPurchasing;
using _Project.Scripts.Menu;
using _Project.Scripts.Sounds;
using Cysharp.Threading.Tasks;
using Zenject;

public class MenuSceneEntryPoint : IInitializable
{
    private MainMenu _mainMenu;
    private IAPPresenter _IAPPresenter;
    private DataPersistenceHandler _dataPersistenceHandler;
    private ShopItemModel _shopItemModel;
    private PurchasingUI _purchasingUI;
    private SceneSwitcher _sceneSwitcher;
    private SoundFactory _soundFactory;
    private LoadingOverlay _loadingOverlay;

    private IAssetLoader _assetLoader;
    private IAudioCatalog _audioCatalog;
    private IInstantiator _instantiator;

    public MenuSceneEntryPoint(
        IInstantiator instantiator,
        IAudioCatalog audioCatalog,
        IAssetLoader assetLoader,
        IAPPresenter iAPPresenter,
        DataPersistenceHandler dataPersistenceHandler,
        ShopItemModel shopItemModel,
        SceneSwitcher sceneSwitcher,
        SoundFactory soundFactory
        )
    {
        _instantiator = instantiator;
        _audioCatalog = audioCatalog;
        _assetLoader = assetLoader;
        _IAPPresenter = iAPPresenter;
        _dataPersistenceHandler = dataPersistenceHandler;
        _shopItemModel = shopItemModel;
        _sceneSwitcher = sceneSwitcher;
        _soundFactory = soundFactory;
    }

    public async void Initialize()
    {
        await InstantiateAddressables();

        _purchasingUI.Init(_IAPPresenter, _soundFactory);
        _shopItemModel.Init(_purchasingUI, _IAPPresenter);
        _mainMenu.Init(_dataPersistenceHandler, _shopItemModel, _purchasingUI, _sceneSwitcher, this, _soundFactory);
        _soundFactory.PlayBackgroundMusic();
        _loadingOverlay.DisableObject();
        _assetLoader.Unload(AssetsIDs.LOADING_OVERLAY);
    }

    private async UniTask InstantiateAddressables()
    {
        _loadingOverlay = _instantiator.InstantiatePrefabForComponent<LoadingOverlay>(
            await _assetLoader.LoadPrefabByID<LoadingOverlay>(AssetsIDs.LOADING_OVERLAY));
        _loadingOverlay.EnableObject();

        _purchasingUI = _instantiator.InstantiatePrefabForComponent<PurchasingUI>(
            await _assetLoader.LoadPrefabByID<PurchasingUI>(AssetsIDs.NO_ADS_MENU));

        _mainMenu = _instantiator.InstantiatePrefabForComponent<MainMenu>(
            await _assetLoader.LoadPrefabByID<MainMenu>(AssetsIDs.MAIN_MENU));

        await _audioCatalog.LoadAssets();
    }

    public void UnloadMenuAssets()
    {
        _soundFactory.UnloadAssets();
        _assetLoader.Unload(AssetsIDs.NO_ADS_MENU);
        _assetLoader.Unload(AssetsIDs.MAIN_MENU);
    }
}
