using UnityEngine.Purchasing;

namespace _Project.Scripts.InAppPurchasing
{
    public class ShopItemModel
    {
        private ProductCollection _products;
        private PurchasingUI _purchasingUI;
        private IAPPresenter _IAPPresenter;

        public void Init(PurchasingUI purchasingUI, IAPPresenter IAPPresenter)
        {
            _purchasingUI = purchasingUI;
            _IAPPresenter = IAPPresenter;
        }

        public void SetupProductPurchasePopup(string productID)
        {
            _products = _IAPPresenter.StoreController.products;
            _purchasingUI.SetupPurchasePopup(_products.WithID(productID));
        }
    }
}
