using _Project.Scripts.Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace _Project.Scripts.InAppPurchasing
{
    public class PurchasingUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private TMP_Text _resultText;
        [SerializeField] private Button _purchaseButton;
        [SerializeField] private Button _backgroundButton;
        [SerializeField] private Button _continueButton;
        [SerializeField] private GameObject _loadingOverlay;
        [SerializeField] private GameObject _purchaseResultPanel;
        [SerializeField] private GameObject _productInfoPanel;
        private IAPPresenter _iAPpresenter;
        private SoundFactory _soundFactory;

        private Product _product;

        public void Init(IAPPresenter iAPPresenter, SoundFactory soundFactory)
        {
            _iAPpresenter = iAPPresenter;
            _soundFactory = soundFactory;
        }

        private void OnEnable()
        {
            _purchaseButton.onClick.AddListener(Purchase);
            _backgroundButton.onClick.AddListener(ClosePopup);
            _continueButton.onClick.AddListener(ClosePopup);
        }

        private void OnDisable()
        {
            _purchaseButton.onClick.RemoveAllListeners();
            _backgroundButton.onClick.RemoveAllListeners();
            _continueButton.onClick.RemoveAllListeners();
        }

        public void EnableObject()
        {
            gameObject.SetActive(true);
        }

        private void DisableObject()
        {
            gameObject.SetActive(false);
        }

        public void SetupPurchasePopup(Product product)
        {
            _priceText.text = $"{product.metadata.localizedPriceString}";
            _product = product;
        }

        public void Purchase()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            _backgroundButton.enabled = false;
            _purchaseButton.enabled = false;
            _loadingOverlay.SetActive(true);
            _iAPpresenter.HandlePurchase(_product, HandlePurchaseCompleted);
        }

        private void HandlePurchaseCompleted(bool result)
        {
            if (result == true)
            {
                _resultText.text = "Purchase Success";
            }
            else if (result == false)
            {
                _resultText.text = "Purchase Failed";
            }

            _purchaseResultPanel.SetActive(true);
            _productInfoPanel.SetActive(false);
            _backgroundButton.enabled = true;
            _purchaseButton.enabled = true;
            _loadingOverlay.SetActive(false);
        }

        private void ClosePopup()
        {
            _soundFactory.PlaySound(AudioID.ClickSound);
            _productInfoPanel.SetActive(true);
            _purchaseResultPanel.SetActive(false);
            DisableObject();
        }
    }
}