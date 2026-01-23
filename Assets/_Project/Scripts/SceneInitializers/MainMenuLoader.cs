using _Project.Scripts.AddressablesHandling;
using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.InAppPurchasing;
using _Project.Scripts.Menu;
using UnityEngine;
using Zenject;

public class MainMenuLoader : MonoBehaviour
{
    private MainMenu _mainMenu;
    private IAPPresenter _iAPPresenter;
    private DataPersistenceHandler _dataPersistenceHandler;
    private ShopItemModel _shopItemModel;
    private PurchasingUI _purchasingUI;
    private SceneSwitcher _sceneSwitcher;

    private IAssetLoader _assetLoader;
    private IInstantiator _instantiator;

    [Inject]
    private void Construct(
        IInstantiator instantiator,
        IAssetLoader assetLoader,
        IAPPresenter iAPPresenter,
        DataPersistenceHandler dataPersistenceHandler,
        ShopItemModel shopItemModel,
        SceneSwitcher sceneSwitcher
        )
    {
        _instantiator = instantiator;
        _assetLoader = assetLoader;
        _iAPPresenter = iAPPresenter;
        _dataPersistenceHandler = dataPersistenceHandler;
        _shopItemModel = shopItemModel;
        _sceneSwitcher = sceneSwitcher;
    }

    private async void Start()
    {
        _purchasingUI = _instantiator.InstantiatePrefabForComponent<PurchasingUI>(
            await _assetLoader.LoadAsset<PurchasingUI>(AssetsIDs.NO_ADS_MENU));
        _assetLoader.UnloadAsset();
        
        _purchasingUI.Init(_iAPPresenter);
        _shopItemModel.Init(_purchasingUI);
        
        _mainMenu = _instantiator.InstantiatePrefabForComponent<MainMenu>(
            await _assetLoader.LoadAsset<MainMenu>(AssetsIDs.MAIN_MENU));
        _assetLoader.UnloadAsset();
        
        _mainMenu.Init(_dataPersistenceHandler, _shopItemModel, _purchasingUI, _sceneSwitcher);
    }
}
