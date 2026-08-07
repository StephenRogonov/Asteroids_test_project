using Cysharp.Threading.Tasks;
using System;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

namespace _Project.Scripts.InAppPurchasing
{
    public class IAPPresenter : IDetailedStoreListener, IDisposable, IInitializable
    {
        public IStoreController StoreController { get; private set; }
        public IExtensionProvider ExtensionProvider { get; private set; }
        private PurchaseApplier _purchaseApplier;
        private ResourceRequest _request;

        private Action<bool> OnPurchaseCompleted;

        public async void Initialize()
        {
            InitializationOptions options = new InitializationOptions()
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                .SetEnvironmentName("test");
#else
                .SetEnvironmentName("production");
#endif

            await UnityServices.InitializeAsync().AsUniTask();
            _request = Resources.LoadAsync<TextAsset>("IAPProductCatalog");
            _request.completed += HandleIAPCatalogLoaded;
        }

        public void SetPurchaseApplier(PurchaseApplier purchaseApplier)
        {
            _purchaseApplier = purchaseApplier;
        }

        private void HandleIAPCatalogLoaded(AsyncOperation operation)
        {
            ResourceRequest request = operation as ResourceRequest;

            Debug.Log($"Loaded Asset: {request.asset}");
            ProductCatalog catalog = JsonUtility.FromJson<ProductCatalog>((request.asset as TextAsset).text);
            Debug.Log($"Loaded catalog with {catalog.allProducts.Count} items");

            StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.StandardUser;
            StandardPurchasingModule.Instance().useFakeStoreAlways = true;

#if UNITY_ANDROID
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(
                StandardPurchasingModule.Instance(AppStore.GooglePlay)
                );
#elif UNITY_IOS
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(
                StandardPurchasingModule.Instance(AppStore.AppleAppStore)
                );
#else
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(
                StandardPurchasingModule.Instance(AppStore.NotSpecified)
                );
#endif

            foreach (ProductCatalogItem item in catalog.allProducts)
            {
                builder.AddProduct(item.id, item.type);
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            StoreController = controller;
            ExtensionProvider = extensions;
        }

        public void HandlePurchase(Product product, Action<bool> onPurchaseCompleted)
        {
            OnPurchaseCompleted = onPurchaseCompleted;
            StoreController.InitiatePurchase(product);
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"Error initializing IAP because of {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"Error initializing IAP because of {error}. {message}.");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"Failed to purchase {product.definition.id} because of {failureReason}.");
            OnPurchaseCompleted?.Invoke(false);
            OnPurchaseCompleted = null;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            Debug.Log($"Successfully purchased {purchaseEvent.purchasedProduct.definition.id}.");
            OnPurchaseCompleted?.Invoke(true);
            OnPurchaseCompleted = null;

            _purchaseApplier.ApplyPurchase(purchaseEvent.purchasedProduct);

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.LogError($"Unable to purchase product {product.definition.id}. {failureDescription.message}.");
            OnPurchaseCompleted?.Invoke(false);
            OnPurchaseCompleted = null;
        }

        public void Dispose()
        {
            _request.completed -= HandleIAPCatalogLoaded;
        }
    }
}