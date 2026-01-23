using _Project.Scripts.DataPersistence;
using System;
using Zenject;

namespace _Project.Scripts.Bootstrap.LoadOptions
{
    public class LoadOptionsModel : IInitializable
    {
        private DataPersistenceHandler _dataPersistenceHandler;

        public event Action<string, string> ShowView;

        public LoadOptionsModel(DataPersistenceHandler dataPersistenceHandler)
        {
            _dataPersistenceHandler = dataPersistenceHandler;
        }

        public void TriggerLoadOptions(DateTime local, DateTime cloud)
        {
            string localTime = local.ToUniversalTime().ToString("dd MMM yyyy HH:mm:ss");
            string cloudTime = cloud.ToUniversalTime().ToString("dd MMM yyyy HH:mm:ss");

            ShowView?.Invoke(localTime, cloudTime);
        }

        public void UseSelectedLoadOption(bool useCloud)
        {
            _dataPersistenceHandler.UseCloudData(useCloud);
        }

        public void Initialize()
        {
            _dataPersistenceHandler.SetLoadOptionsModel(this);
        }
    }
}
