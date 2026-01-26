using _Project.Scripts.Common;
using _Project.Scripts.DataPersistence;
using _Project.Scripts.InAppPurchasing;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private StartGame _startGame;
        [SerializeField] private Button _noAdsButton;

        private DataPersistenceHandler _dataPersistenceHandler;
        private PlayerData _playerData;
        private ShopItemModel _shopItemModel;
        private PurchasingUI _purchasingUI;

        public void Init(
            DataPersistenceHandler dataPersistenceHandler,
            ShopItemModel shopItemModel,
            PurchasingUI purchasingUI,
            SceneSwitcher sceneSwitcher,
            MainMenuLoader mainMenuLoader
            )
        {
            _dataPersistenceHandler = dataPersistenceHandler;
            _playerData = _dataPersistenceHandler.PlayerData;
            _shopItemModel = shopItemModel;
            _startGame.Init(sceneSwitcher, mainMenuLoader);

            if (_playerData.NoAdsPurchased == true)
            {
                _noAdsButton.gameObject.SetActive(false);
                return;
            }

            _purchasingUI = purchasingUI;

            _dataPersistenceHandler.PlayerDataChanged += UpdateUI;
            _noAdsButton?.onClick.AddListener(ActivateNoAdsPopup);
        }

        private void OnDisable()
        {
            _dataPersistenceHandler.PlayerDataChanged -= UpdateUI;
            _noAdsButton?.onClick.RemoveAllListeners();
        }

        private void UpdateUI()
        {
            if (_playerData.NoAdsPurchased == true)
            {
                _noAdsButton.gameObject.SetActive(false);
            }
        }

        private void ActivateNoAdsPopup()
        {
            _shopItemModel.SetupProductPurchasePopup(ProductsIDs.NO_ADS);
            _purchasingUI.EnableObject();
        }
    }
}