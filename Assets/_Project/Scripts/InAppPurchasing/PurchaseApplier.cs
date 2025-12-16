using _Project.Scripts.DataPersistence;
using System;
using UnityEngine;
using UnityEngine.Purchasing;

namespace _Project.Scripts.InAppPurchasing
{
    public class PurchaseApplier : IDataPersistence, IDisposable
    {
        private ProductCollection _products;
        private DataPersistenceHandler _dataPersistenceHandler;
        private bool _noAdsPurchased;

        public PurchaseApplier(DataPersistenceHandler dataPersistenceHandler)
        {
            _dataPersistenceHandler = dataPersistenceHandler;

            _dataPersistenceHandler.AddDataObject(this);
        }

        public void SetProductsCollection(ProductCollection productCollection)
        {
            _products = productCollection;
        }

        public void ApplyPurchase(Product product)
        {
            if (product == _products.WithID(ProductsIDs.NO_ADS))
            {
                ApplyNoAds();
            }
            else
            {
                Debug.LogError("Unable to apply Purchased product.");
            }
        }

        private async void ApplyNoAds()
        {
            _noAdsPurchased = true;
            await _dataPersistenceHandler.SavePlayerData();
        }

        public void SaveData(PlayerData data)
        {
            data.NoAdsPurchased = _noAdsPurchased;
        }

        public void Dispose()
        {
            _dataPersistenceHandler.RemoveDataObject(this);
        }
    }
}
