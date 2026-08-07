using _Project.Scripts.DataPersistence;
using System;

namespace _Project.Scripts.Bootstrap.LoadOptions
{
    public class LoadOptionsModel
    {
        private DataPersistenceHandler _dataPersistenceHandler;

        public event Action<string, string> ShowView;

        public LoadOptionsModel(DataPersistenceHandler dataPersistenceHandler)
        {
            _dataPersistenceHandler = dataPersistenceHandler;
        }

        public void TriggerLoadOptions(DateTime local, DateTime cloud)
        {
            string localTime;
            string cloudTime;

            if (local > cloud)
            {
                localTime = local.ToUniversalTime().ToString("dd MMM yyyy HH:mm:ss") + " (newer)";
                cloudTime = cloud.ToUniversalTime().ToString("dd MMM yyyy HH:mm:ss");
            }
            else
            {
                localTime = local.ToUniversalTime().ToString("dd MMM yyyy HH:mm:ss");
                cloudTime = cloud.ToUniversalTime().ToString("dd MMM yyyy HH:mm:ss") + " (newer)";
            }

                ShowView?.Invoke(localTime, cloudTime);
        }

        public void UseSelectedLoadOption(bool useCloud)
        {
            _dataPersistenceHandler.UseCloudData(useCloud);
        }
    }
}
